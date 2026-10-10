using Hotel.Core.Dominio;

namespace Hotel.Tests.Servicios;

/// <summary>CRUD, búsqueda e historial de <c>HuespedServicio</c> (RF-08 a RF-12, RN-01, RN-14).</summary>
public sealed class HuespedServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    private Huesped Datos(string doc = "2456789012345") => new()
    {
        Nombres = "María", Apellidos = "Pérez", TipoDocumento = TipoDocumento.Dpi, NumeroDocumento = doc,
        Telefono = "55512345", Correo = "maria@correo.com", FechaNacimiento = new DateTime(1992, 6, 1)
    };

    [Fact]
    public void Registrar_Valido_AsignaIdYNitPorDefecto()
    {
        var r = _e.S.Huespedes.Registrar(Datos());
        Assert.True(r.Exito, r.Mensaje);
        Assert.True(r.Valor!.Id > 0);
        Assert.Equal("CF", r.Valor.Nit);
    }

    [Fact]
    public void Registrar_DocumentoDuplicado_Rechaza()
    {
        _e.S.Huespedes.Registrar(Datos());
        var r = _e.S.Huespedes.Registrar(Datos());
        Assert.False(r.Exito);
        Assert.Contains("documento", r.Mensaje);
    }

    [Fact]
    public void Registrar_Menor_Rechaza()
    {
        var h = Datos();
        h.FechaNacimiento = Escenario.Hoy.AddYears(-17);
        Assert.False(_e.S.Huespedes.Registrar(h).Exito);
    }

    [Fact]
    public void Registrar_NitInvalido_Rechaza()
    {
        var h = Datos();
        h.Nit = "10000004";
        Assert.False(_e.S.Huespedes.Registrar(h).Exito);
    }

    [Fact]
    public void Registrar_NitConVerificadorK_Acepta()
    {
        var h = Datos();
        h.Nit = "6-k";
        var r = _e.S.Huespedes.Registrar(h);
        Assert.True(r.Exito, r.Mensaje);
        Assert.Equal("6K", r.Valor!.Nit);
    }

    [Fact]
    public void Editar_CambiaDatos_YConservaElResto()
    {
        var h = _e.S.Huespedes.Registrar(Datos()).Valor!;
        h.Telefono = "44411122";
        Assert.True(_e.S.Huespedes.Editar(h).Exito);
        Assert.Equal("44411122", _e.S.Huespedes.ObtenerPorId(h.Id)!.Telefono);
    }

    [Fact]
    public void Editar_DocumentoDeOtroHuesped_Rechaza()
    {
        _e.S.Huespedes.Registrar(Datos("1111111111111"));
        var otro = _e.S.Huespedes.Registrar(Datos("2222222222222")).Valor!;
        otro.NumeroDocumento = "1111111111111";
        Assert.False(_e.S.Huespedes.Editar(otro).Exito);
    }

    [Fact]
    public void Eliminar_SinReservas_EliminaFisicamente()
    {
        var h = _e.Huesped();
        Assert.True(_e.S.Huespedes.Eliminar(h.Id).Exito);
        Assert.Null(_e.S.Huespedes.ObtenerPorId(h.Id));
    }

    [Fact]
    public void Eliminar_ConReservas_Desactiva()
    {
        var h = _e.Huesped();
        _e.Reserva(h, _e.Habitacion(), 3, 2);

        var r = _e.S.Huespedes.Eliminar(h.Id);

        Assert.True(r.Exito);
        Assert.False(_e.S.Huespedes.ObtenerPorId(h.Id)!.Activo);
        Assert.Empty(_e.S.Huespedes.Buscar(""));
    }

    [Fact]
    public void Buscar_PorNombreODocumento_Filtra()
    {
        _e.Huesped("Carlos", "Morales");
        _e.Huesped("Lucía", "Barrios");
        var porNombre = _e.S.Huespedes.Buscar("lucía");
        var doc = _e.S.Huespedes.ObtenerTodos()[0].NumeroDocumento;

        Assert.Single(porNombre);
        Assert.Single(_e.S.Huespedes.Buscar(doc));
        Assert.Equal(2, _e.S.Huespedes.Buscar("").Count);
        Assert.Empty(_e.S.Huespedes.Buscar("zzz"));
    }

    [Fact]
    public void Reservar_HuespedDesactivado_Rechaza()
    {
        var h = _e.Huesped();
        var hab = _e.Habitacion();
        _e.Reserva(h, hab, 20, 1);
        _e.S.Huespedes.Eliminar(h.Id);
        var r = _e.S.Reservas.Crear(h.Id, hab.Id, Escenario.Hoy.AddDays(1), Escenario.Hoy.AddDays(2), 1);
        Assert.False(r.Exito);
    }

    [Fact]
    public void HistorialEstadias_IncluyeFinalizadasConTotalFacturado()
    {
        var reserva = _e.EstadiaEnCurso(noches: 2, tarifa: 500m);
        _e.Reloj.Ahora = Escenario.Hoy.AddDays(2).AddHours(10);
        var factura = _e.S.Estadias.CheckOut(reserva.Id).Valor!;
        var otra = _e.Reserva(_e.S.Huespedes.ObtenerPorId(reserva.HuespedId)!, _e.Habitacion("999"), 5, 1);

        var historial = _e.S.Huespedes.HistorialEstadias(reserva.HuespedId);

        Assert.Single(historial);          // la reserva futura no es estadía
        Assert.Equal(factura.Total, historial[0].TotalFacturado);
        Assert.Equal(EstadoReserva.Finalizada, historial[0].Estado);
        Assert.NotEqual(otra.Codigo, historial[0].CodigoReserva);
    }
}
