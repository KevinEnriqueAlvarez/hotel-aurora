using Hotel.Core.Datos;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Datos;

/// <summary>Ida y vuelta de cada entidad por JSON, incluidos <c>private set</c> y <see cref="Pago"/> polimórfico.</summary>
public sealed class PersistenciaEntidadesTests : IDisposable
{
    private readonly CarpetaTemporal _carpeta = new();

    public void Dispose() => _carpeta.Dispose();

    private T IdaYVuelta<T>(T entidad) where T : EntidadBase
    {
        var ruta = _carpeta.Archivo(typeof(T).Name + ".json");
        new RepositorioJson<T>(ruta).Agregar(entidad);
        return new RepositorioJson<T>(ruta).ObtenerPorId(entidad.Id)!;
    }

    [Fact]
    public void Usuario_IdaYVuelta_ConservaHashYVerificaContrasena()
    {
        var u = new Usuario
        {
            Nombres = "Ana", Apellidos = "López", NumeroDocumento = "1234567890123", Telefono = "55512345",
            Correo = "ana@hotel.gt", NombreUsuario = "ana", Rol = RolUsuario.Administrador
        };
        u.EstablecerContrasena("Clave123");

        var leido = IdaYVuelta(u);

        Assert.Equal(u.HashContrasena, leido.HashContrasena);
        Assert.Equal(u.Sal, leido.Sal);
        Assert.Equal(RolUsuario.Administrador, leido.Rol);
        Assert.True(leido.VerificarContrasena("Clave123"));
    }

    [Fact]
    public void Habitacion_IdaYVuelta_ConservaEstadoPrivado()
    {
        var h = new Habitacion { Numero = "301", Piso = 3, Tipo = TipoHabitacion.Familiar, Capacidad = 5, TarifaNoche = 950m };
        h.CambiarEstado(EstadoHabitacion.Mantenimiento);

        var leido = IdaYVuelta(h);

        Assert.Equal(EstadoHabitacion.Mantenimiento, leido.Estado);
        Assert.Equal(TipoHabitacion.Familiar, leido.Tipo);
        Assert.Equal(950m, leido.TarifaNoche);
    }

    [Fact]
    public void Huesped_IdaYVuelta_ConservaTodosLosCampos()
    {
        var h = new Huesped
        {
            Nombres = "José", Apellidos = "Muñoz", TipoDocumento = TipoDocumento.Pasaporte, NumeroDocumento = "P1234567",
            Telefono = "+50255512345", Correo = "jose@mail.com", Nacionalidad = "Mexicana",
            FechaNacimiento = new DateTime(1985, 2, 3), Nit = "6K", Notas = "Prefiere piso alto"
        };

        var leido = IdaYVuelta(h);

        Assert.Equal("Muñoz", leido.Apellidos);
        Assert.Equal(TipoDocumento.Pasaporte, leido.TipoDocumento);
        Assert.Equal("6K", leido.Nit);
        Assert.Equal(new DateTime(1985, 2, 3), leido.FechaNacimiento);
    }

    [Fact]
    public void ServicioAdicional_IdaYVuelta_ConservaCategoriaYPrecio()
    {
        var leido = IdaYVuelta(new ServicioAdicional { Nombre = "Lavandería", Categoria = CategoriaServicio.Lavanderia, Precio = 45.50m });
        Assert.Equal(CategoriaServicio.Lavanderia, leido.Categoria);
        Assert.Equal(45.50m, leido.Precio);
    }

    [Fact]
    public void Reserva_IdaYVuelta_ConservaEstadoCargosYFechas()
    {
        var entrada = new DateTime(2026, 3, 10);
        var r = Reserva.Crear(2, 3, entrada, entrada.AddDays(3), 2, 480m);
        r.Codigo = "RES-000001";
        r.Confirmar();
        r.RegistrarCheckIn(entrada.AddHours(14));
        r.AgregarCargo(new CargoAdicional { ServicioId = 4, Descripcion = "Desayuno", Cantidad = 2, PrecioUnitario = 60m, Fecha = entrada });

        var leido = IdaYVuelta(r);

        Assert.Equal(EstadoReserva.EnCurso, leido.Estado);
        Assert.Equal(entrada.AddHours(14), leido.FechaCheckIn);
        Assert.Equal(3, leido.HabitacionId);
        Assert.Equal(480m, leido.TarifaPactada);
        Assert.Equal(3, leido.Noches);
        Assert.Single(leido.Cargos);
        Assert.Equal(120m, leido.TotalCargos);
    }

    [Fact]
    public void Reserva_Cancelada_ConservaMotivo()
    {
        var r = Reserva.Crear(1, 1, new DateTime(2026, 5, 1), new DateTime(2026, 5, 3), 1, 300m);
        r.Cancelar("Viaje cancelado");
        Assert.Equal("Viaje cancelado", IdaYVuelta(r).MotivoCancelacion);
    }

    [Fact]
    public void Factura_IdaYVuelta_ConservaLineasYPagosPolimorficos()
    {
        var fecha = new DateTime(2026, 3, 13);
        var reserva = Reserva.Crear(1, 1, fecha.AddDays(-3), fecha, 2, 500m);
        reserva.Id = 7;
        reserva.Confirmar();
        reserva.RegistrarCheckIn(fecha.AddDays(-3));
        reserva.AgregarCargo(new CargoAdicional { Descripcion = "Minibar", Cantidad = 3, PrecioUnitario = 25m });
        var hab = new Habitacion { Id = 1, Numero = "101", Tipo = TipoHabitacion.Doble };
        var hue = new Huesped { Id = 1, Nombres = "Ana", Apellidos = "López", Nit = "CF" };
        var f = Factura.Generar(reserva, hab, hue, new ConfiguracionHotel(), 5m, fecha);
        f.Numero = "FAC-000001";
        f.RegistrarPago(new PagoEfectivo { Monto = 100m, MontoRecibido = 200m, Fecha = fecha });
        f.RegistrarPago(new PagoTarjeta { Monto = 200m, UltimosCuatroDigitos = "4242", NumeroAutorizacion = "A9", Fecha = fecha });
        f.RegistrarPago(new PagoTransferencia { Monto = 300m, Banco = "BAC", NumeroReferencia = "REF1", Fecha = fecha });

        var leido = IdaYVuelta(f);

        Assert.Equal(f.Total, leido.Total);
        Assert.Equal(f.Iva, leido.Iva);
        Assert.Equal(2, leido.Lineas.Count);
        Assert.Equal(3, leido.Pagos.Count);
        Assert.IsType<PagoEfectivo>(leido.Pagos[0]);
        Assert.Equal(100m, ((PagoEfectivo)leido.Pagos[0]).Cambio);
        Assert.Equal("4242", ((PagoTarjeta)leido.Pagos[1]).UltimosCuatroDigitos);
        Assert.Equal("REF1", ((PagoTransferencia)leido.Pagos[2]).NumeroReferencia);
        Assert.Equal(f.Saldo, leido.Saldo);
        Assert.Equal(EstadoFactura.Emitida, leido.Estado);
    }

    [Fact]
    public void Factura_Anulada_ConservaEstadoYMotivo()
    {
        var fecha = new DateTime(2026, 3, 13);
        var reserva = Reserva.Crear(1, 1, fecha.AddDays(-1), fecha, 1, 500m);
        reserva.Confirmar();
        reserva.RegistrarCheckIn(fecha.AddDays(-1));
        var f = Factura.Generar(reserva, new Habitacion { Numero = "1" }, new Huesped { Nombres = "A", Apellidos = "B" },
            new ConfiguracionHotel(), 0m, fecha);
        f.Anular("Datos erróneos");

        var leido = IdaYVuelta(f);

        Assert.Equal(EstadoFactura.Anulada, leido.Estado);
        Assert.Equal("Datos erróneos", leido.MotivoAnulacion);
    }

    [Fact]
    public void PagoTarjeta_Json_NoContieneNumeroCompleto()
    {
        var f = Factura.Generar(
            CrearReservaEnCurso(), new Habitacion { Numero = "1" }, new Huesped { Nombres = "A", Apellidos = "B" },
            new ConfiguracionHotel(), 0m, new DateTime(2026, 3, 12));
        f.RegistrarPago(new PagoTarjeta { Monto = 10m, UltimosCuatroDigitos = "1111", NumeroAutorizacion = "X" });
        var ruta = _carpeta.Archivo("f.json");
        new RepositorioJson<Factura>(ruta).Agregar(f);

        var texto = File.ReadAllText(ruta);

        Assert.Contains("\"tipo\": \"tarjeta\"", texto);
        Assert.DoesNotContain("numeroTarjeta", texto);
    }

    private static Reserva CrearReservaEnCurso()
    {
        var r = Reserva.Crear(1, 1, new DateTime(2026, 3, 10), new DateTime(2026, 3, 12), 1, 500m);
        r.Confirmar();
        r.RegistrarCheckIn(new DateTime(2026, 3, 10));
        return r;
    }

    [Fact]
    public void ConfiguracionHotel_IdaYVuelta_ConservaParametros()
    {
        var ruta = _carpeta.Archivo("configuracion.json");
        ArchivoJson.Escribir(ruta, new ConfiguracionHotel { NombreHotel = "Aurora Ñ", PorcentajeIva = 12m, DescuentoMaximo = 25m });
        var leido = ArchivoJson.Leer<ConfiguracionHotel>(ruta)!;
        Assert.Equal("Aurora Ñ", leido.NombreHotel);
        Assert.Equal(25m, leido.DescuentoMaximo);
    }
}
