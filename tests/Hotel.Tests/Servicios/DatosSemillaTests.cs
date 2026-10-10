using System.Diagnostics;
using Hotel.Core.Comun;
using Hotel.Core.Datos;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Tests.Servicios;

/// <summary>Semilla de datos coherente y rendimiento (RNF-02, RNF-04) e indicadores (E1).</summary>
public sealed class DatosSemillaTests : IDisposable
{
    private readonly CarpetaTemporal _carpeta = new();
    private readonly DateTime _ahora = new(2026, 10, 10, 15, 30, 0);

    public void Dispose() => _carpeta.Dispose();

    private ContextoDatos Sembrar()
    {
        var ctx = new ContextoDatos(_carpeta.Ruta);
        Assert.True(DatosSemilla.SembrarSiVacio(ctx, _ahora));
        return ctx;
    }

    [Fact]
    public void Sembrar_CreaUsuariosHabitacionesHuespedesYServicios()
    {
        var ctx = Sembrar();
        var usuarios = ctx.Usuarios.ObtenerTodos();

        Assert.Equal(2, usuarios.Count);
        Assert.True(usuarios.Single(u => u.NombreUsuario == "admin").VerificarContrasena("Admin123*"));
        Assert.True(usuarios.Single(u => u.NombreUsuario == "recepcion").VerificarContrasena("Recepcion123*"));
        Assert.Equal(20, ctx.Habitaciones.ObtenerTodos().Count);
        Assert.Equal(4, ctx.Habitaciones.ObtenerTodos().Select(h => h.Piso).Distinct().Count());
        Assert.Equal(4, ctx.Habitaciones.ObtenerTodos().Select(h => h.Tipo).Distinct().Count());
        Assert.All(ctx.Habitaciones.ObtenerTodos(), h => Assert.InRange(h.TarifaNoche, 350m, 1500m));
        Assert.True(ctx.Huespedes.ObtenerTodos().Count >= 15);
        Assert.Contains(ctx.Huespedes.ObtenerTodos(), h => h.TipoDocumento == TipoDocumento.Pasaporte);
        Assert.Equal(10, ctx.Servicios.ObtenerTodos().Count);
    }

    [Fact]
    public void Sembrar_GeneraAlrededorDe40ReservasEnTodosLosEstados()
    {
        var ctx = Sembrar();
        var reservas = ctx.Reservas.ObtenerTodos(true);

        Assert.InRange(reservas.Count, 35, 45);
        foreach (var estado in Enum.GetValues<EstadoReserva>())
            Assert.Contains(reservas, r => r.Estado == estado);
        Assert.InRange(reservas.Min(r => r.FechaEntrada), _ahora.AddMonths(-8), _ahora.AddMonths(-6));
        Assert.True(reservas.Max(r => r.FechaEntrada) > _ahora.Date);
    }

    [Fact]
    public void Sembrar_FacturasConPagosParcialesYUnaAnulada()
    {
        var facturas = Sembrar().Facturas.ObtenerTodos(true);

        Assert.True(facturas.Count >= 20);
        Assert.Contains(facturas, f => f.Estado == EstadoFactura.Anulada);
        Assert.Contains(facturas, f => f.Estado == EstadoFactura.Pagada);
        Assert.Contains(facturas, f => f.Estado == EstadoFactura.Emitida && f.TotalPagado > 0 && f.Saldo > 0);
        Assert.All(facturas, f => Assert.True(f.TotalPagado <= f.Total));
        Assert.All(facturas.Where(f => f.Estado == EstadoFactura.Pagada), f => Assert.Equal(0m, f.Saldo));
        Assert.Equal(facturas.Count, facturas.Select(f => f.Numero).Distinct().Count());
    }

    [Fact]
    public void Sembrar_EstadosDeHabitacionCoherentesConLasReservas()
    {
        var ctx = Sembrar();
        var enCasa = ctx.Reservas.Buscar(r => r.Estado == EstadoReserva.EnCurso).Select(r => r.HabitacionId).ToHashSet();
        var ocupadas = ctx.Habitaciones.Buscar(h => h.Estado == EstadoHabitacion.Ocupada).Select(h => h.Id).ToHashSet();

        Assert.True(enCasa.Count > 0);
        Assert.True(enCasa.SetEquals(ocupadas));
        Assert.Contains(ctx.Habitaciones.ObtenerTodos(), h => h.Estado == EstadoHabitacion.Limpieza);
        Assert.Contains(ctx.Habitaciones.ObtenerTodos(), h => h.Estado == EstadoHabitacion.Mantenimiento);
    }

    [Fact]
    public void Sembrar_NingunaHabitacionTieneReservasActivasSolapadas()
    {
        var activas = Sembrar().Reservas.ObtenerTodos(true).Where(r => r.EstaActiva).ToList();
        foreach (var grupo in activas.GroupBy(r => r.HabitacionId))
            foreach (var a in grupo)
                Assert.DoesNotContain(grupo, b => b.Id != a.Id && b.SeSolapaCon(a.FechaEntrada, a.FechaSalida));
    }

    [Fact]
    public void Sembrar_CodigosYNumerosCorrelativos()
    {
        var ctx = Sembrar();
        var codigos = ctx.Reservas.ObtenerTodos(true).OrderBy(r => r.Id).Select(r => r.Codigo).ToList();
        Assert.Equal("RES-000001", codigos[0]);
        Assert.Equal(Constantes.FormatearCorrelativo("RES-", codigos.Count), codigos[^1]);
    }

    [Fact]
    public void Sembrar_YaConDatos_NoHaceNada()
    {
        var ctx = Sembrar();
        var antes = ctx.Reservas.ObtenerTodos(true).Count;
        Assert.False(DatosSemilla.SembrarSiVacio(ctx, _ahora));
        Assert.Equal(antes, ctx.Reservas.ObtenerTodos(true).Count);
    }

    [Fact]
    public void Sembrar_PersisteYSePuedeReabrir()
    {
        var antes = Sembrar().Reservas.ObtenerTodos(true).Count;
        var reabierto = new ContextoDatos(_carpeta.Ruta);
        Assert.Equal(antes, reabierto.Reservas.ObtenerTodos(true).Count);
        Assert.Empty(reabierto.Avisos);
    }

    [Fact]
    public void Consultas_ConDatosDeSemilla_RespondenEnMenosDeUnSegundo()
    {
        var ctx = Sembrar();
        var s = new ServiciosHotel(ctx, new RelojFijo(_ahora));
        var reloj = Stopwatch.StartNew();

        _ = s.Reservas.Buscar();
        _ = s.Facturacion.Buscar();
        _ = s.Habitaciones.Buscar();
        _ = s.Reportes.Indicadores();
        _ = s.Reportes.SerieMensual(SerieReporte.Ocupacion);
        _ = s.Reservas.BuscarDisponibles(_ahora.Date.AddDays(3), _ahora.Date.AddDays(6), 2);

        Assert.True(reloj.ElapsedMilliseconds < 1000, $"Tardó {reloj.ElapsedMilliseconds} ms");
    }

    [Fact]
    public void Indicadores_ConDatosDeSemilla_SonCoherentes()
    {
        var ctx = Sembrar();
        var s = new ServiciosHotel(ctx, new RelojFijo(_ahora));

        var k = s.Reportes.Indicadores();
        var estados = s.Reportes.HabitacionesPorEstado();

        Assert.Equal(20, k.HabitacionesTotal);
        Assert.True(k.HabitacionesOcupadas > 0);
        Assert.InRange(k.OcupacionHoy, 1m, 100m);
        Assert.True(k.ReservasActivas > 0);
        Assert.True(k.LlegadasHoy >= 2);
        Assert.True(k.Cobrado > 0);
        Assert.True(k.SaldoPorCobrar > 0);
        Assert.Equal(20, estados.Sum(e => e.Cantidad));
    }

    [Fact]
    public void SerieMensual_DoceMesesConUltimosMesesConIngresos()
    {
        var s = new ServiciosHotel(Sembrar(), new RelojFijo(_ahora));

        var ingresos = s.Reportes.SerieMensual(SerieReporte.Ingresos);
        var reservas = s.Reportes.SerieMensual(SerieReporte.Reservas);
        var ocupacion = s.Reportes.SerieMensual(SerieReporte.Ocupacion);

        Assert.Equal(12, ingresos.Count);
        Assert.Equal("OCT", ingresos[^1].Etiqueta);
        Assert.True(ingresos.Take(3).All(p => p.Valor == 0m));     // antes de la historia sembrada
        Assert.True(ingresos.Skip(6).All(p => p.Valor > 0m), string.Join(", ", ingresos.Select(p => p.Etiqueta + "=" + p.Valor)));
        Assert.True(reservas.Sum(p => p.Valor) > 30);
        Assert.All(ocupacion, p => Assert.InRange(p.Valor, 0m, 100m));
    }
}
