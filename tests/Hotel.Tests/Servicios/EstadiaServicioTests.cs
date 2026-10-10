using Hotel.Core.Dominio;

namespace Hotel.Tests.Servicios;

/// <summary>Check-in, cargos y check-out (RN-07 a RN-11).</summary>
public sealed class EstadiaServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    private EstadoHabitacion EstadoDe(int habitacionId) => _e.S.Habitaciones.ObtenerPorId(habitacionId)!.Estado;

    [Fact]
    public void CheckIn_ReservaConfirmada_ReservaEnCursoYHabitacionOcupada()
    {
        var hab = _e.Habitacion();
        var reserva = _e.Reserva(_e.Huesped(), hab, 0, 2);

        var r = _e.S.Estadias.CheckIn(reserva.Id);

        Assert.True(r.Exito, r.Mensaje);
        var guardada = _e.S.Reservas.ObtenerPorId(reserva.Id)!;
        Assert.Equal(EstadoReserva.EnCurso, guardada.Estado);
        Assert.Equal(_e.Reloj.Ahora, guardada.FechaCheckIn);
        Assert.Equal(EstadoHabitacion.Ocupada, EstadoDe(hab.Id));
    }

    [Fact]
    public void CheckIn_ReservaPendiente_Rechaza()
    {
        var reserva = _e.Reserva(_e.Huesped(), _e.Habitacion(), 0, 2, confirmar: false);
        Assert.False(_e.S.Estadias.CheckIn(reserva.Id).Exito);
    }

    [Fact]
    public void CheckIn_AntesDeLaFechaDeEntrada_Rechaza()
    {
        var reserva = _e.Reserva(_e.Huesped(), _e.Habitacion(), 2, 2);
        Assert.False(_e.S.Estadias.CheckIn(reserva.Id).Exito);
    }

    [Fact]
    public void CheckIn_EnElDiaDeSalida_Rechaza()
    {
        var reserva = _e.Reserva(_e.Huesped(), _e.Habitacion(), 0, 2);
        _e.Reloj.Ahora = Escenario.Hoy.AddDays(2).AddHours(10);
        Assert.False(_e.S.Estadias.CheckIn(reserva.Id).Exito);
    }

    [Fact]
    public void CheckIn_HabitacionEnLimpieza_Rechaza()
    {
        var hab = _e.Habitacion();
        var primera = _e.Reserva(_e.Huesped(), hab, 0, 1);
        _e.S.Estadias.CheckIn(primera.Id);
        _e.S.Estadias.CheckOut(primera.Id);
        var segunda = _e.Reserva(_e.Huesped(), hab, 0, 2);

        var r = _e.S.Estadias.CheckIn(segunda.Id);

        Assert.False(r.Exito);
        Assert.Contains("no está disponible", r.Mensaje);
    }

    [Fact]
    public void CheckIn_ReasignandoHabitacion_ActualizaTarifaYEstados()
    {
        var original = _e.Habitacion("101", tarifa: 400m);
        var nueva = _e.Habitacion("102", tarifa: 700m);
        var reserva = _e.Reserva(_e.Huesped(), original, 0, 2);

        var r = _e.S.Estadias.CheckIn(reserva.Id, nueva.Id);

        Assert.True(r.Exito, r.Mensaje);
        var guardada = _e.S.Reservas.ObtenerPorId(reserva.Id)!;
        Assert.Equal(nueva.Id, guardada.HabitacionId);
        Assert.Equal(700m, guardada.TarifaPactada);
        Assert.Equal(EstadoHabitacion.Ocupada, EstadoDe(nueva.Id));
        Assert.Equal(EstadoHabitacion.Disponible, EstadoDe(original.Id));
    }

    [Fact]
    public void CheckIn_ReasignandoAHabitacionPequena_Rechaza()
    {
        var original = _e.Habitacion("101", capacidad: 3);
        var chica = _e.Habitacion("102", capacidad: 1);
        var reserva = _e.Reserva(_e.Huesped(), original, 0, 2, personas: 2);
        Assert.False(_e.S.Estadias.CheckIn(reserva.Id, chica.Id).Exito);
    }

    [Fact]
    public void CheckIn_ReasignandoAHabitacionConReservaFutura_Rechaza()
    {
        var original = _e.Habitacion("101");
        var otra = _e.Habitacion("102");
        var reserva = _e.Reserva(_e.Huesped(), original, 0, 3);
        _e.Reserva(_e.Huesped(), otra, 1, 2);
        Assert.False(_e.S.Estadias.CheckIn(reserva.Id, otra.Id).Exito);
    }

    [Fact]
    public void AgregarCargo_ReservaEnCurso_RegistraConPrecioDelCatalogo()
    {
        var reserva = _e.EstadiaEnCurso();
        var servicio = _e.Servicio("Desayuno", 65m);

        var r = _e.S.Estadias.AgregarCargo(reserva.Id, servicio.Id, 2);

        Assert.True(r.Exito, r.Mensaje);
        var guardada = _e.S.Reservas.ObtenerPorId(reserva.Id)!;
        Assert.Single(guardada.Cargos);
        Assert.Equal(130m, guardada.TotalCargos);
        Assert.Equal("Desayuno", guardada.Cargos[0].Descripcion);
    }

    [Fact]
    public void AgregarCargo_CantidadCero_Rechaza()
    {
        var reserva = _e.EstadiaEnCurso();
        Assert.False(_e.S.Estadias.AgregarCargo(reserva.Id, _e.Servicio().Id, 0).Exito);
    }

    [Fact]
    public void AgregarCargo_ReservaConfirmada_Rechaza()
    {
        var reserva = _e.Reserva(_e.Huesped(), _e.Habitacion(), 0, 2);
        Assert.False(_e.S.Estadias.AgregarCargo(reserva.Id, _e.Servicio().Id, 1).Exito);
    }

    [Fact]
    public void CheckOut_TresNochesConCargo_GeneraFacturaYDejaHabitacionEnLimpieza()
    {
        var reserva = _e.EstadiaEnCurso(noches: 3, tarifa: 500m);
        _e.S.Estadias.AgregarCargo(reserva.Id, _e.Servicio("Spa", 100m).Id, 1);
        _e.Reloj.Ahora = Escenario.Hoy.AddDays(3).AddHours(11);

        var r = _e.S.Estadias.CheckOut(reserva.Id, 0m);

        Assert.True(r.Exito, r.Mensaje);
        var f = r.Valor!;
        Assert.Equal("FAC-000001", f.Numero);
        Assert.Equal(1600m, f.Subtotal);          // 3 × 500 + 100
        Assert.Equal(192m, f.Iva);                // 12 %
        Assert.Equal(150m, f.ImpuestoTurismo);    // 10 % de 1500
        Assert.Equal(1942m, f.Total);
        Assert.Equal(EstadoFactura.Emitida, f.Estado);
        Assert.Equal(EstadoReserva.Finalizada, _e.S.Reservas.ObtenerPorId(reserva.Id)!.Estado);
        Assert.Equal(EstadoHabitacion.Limpieza, EstadoDe(reserva.HabitacionId));
        Assert.NotNull(_e.S.Facturacion.ObtenerPorId(f.Id));
    }

    [Fact]
    public void CheckOut_MismoDiaDelCheckIn_CobraUnaNoche()
    {
        var reserva = _e.EstadiaEnCurso(noches: 2, tarifa: 400m);
        var r = _e.S.Estadias.CheckOut(reserva.Id);
        Assert.Equal(400m, r.Valor!.Lineas[0].Subtotal);
        Assert.Equal(1, r.Valor.Lineas[0].Cantidad);
    }

    [Fact]
    public void CheckOut_SalidaAnticipada_CobraLasNochesReales()
    {
        var reserva = _e.EstadiaEnCurso(noches: 5, tarifa: 400m);
        _e.Reloj.Ahora = Escenario.Hoy.AddDays(2).AddHours(10);
        var r = _e.S.Estadias.CheckOut(reserva.Id);
        Assert.Equal(2, r.Valor!.Lineas[0].Cantidad);
    }

    [Fact]
    public void CheckOut_ConDescuento_AplicaRN11()
    {
        var reserva = _e.EstadiaEnCurso(noches: 2, tarifa: 500m);
        _e.Reloj.Ahora = Escenario.Hoy.AddDays(2).AddHours(10);
        var f = _e.S.Estadias.CheckOut(reserva.Id, 10m).Valor!;
        // subtotal 1000, desc 100, base 900, IVA 108, turismo 1000×0.9×10% = 90
        Assert.Equal(100m, f.Descuento);
        Assert.Equal(108m, f.Iva);
        Assert.Equal(90m, f.ImpuestoTurismo);
        Assert.Equal(1098m, f.Total);
    }

    [Fact]
    public void CheckOut_DescuentoSobreElMaximo_RechazaYNoCambiaEstados()
    {
        var reserva = _e.EstadiaEnCurso();
        var r = _e.S.Estadias.CheckOut(reserva.Id, 50m);

        Assert.False(r.Exito);
        Assert.Equal(EstadoReserva.EnCurso, _e.S.Reservas.ObtenerPorId(reserva.Id)!.Estado);
        Assert.Equal(EstadoHabitacion.Ocupada, EstadoDe(reserva.HabitacionId));
        Assert.Empty(_e.S.Facturacion.Buscar());
    }

    [Fact]
    public void CheckOut_ReservaNoEnCurso_Rechaza()
    {
        var reserva = _e.Reserva(_e.Huesped(), _e.Habitacion(), 0, 2);
        Assert.False(_e.S.Estadias.CheckOut(reserva.Id).Exito);
    }

    [Fact]
    public void CheckOut_DosEstadias_CorrelativosDeFacturaIncrementan()
    {
        var a = _e.EstadiaEnCurso();
        var b = _e.EstadiaEnCurso();
        Assert.Equal("FAC-000001", _e.S.Estadias.CheckOut(a.Id).Valor!.Numero);
        Assert.Equal("FAC-000002", _e.S.Estadias.CheckOut(b.Id).Valor!.Numero);
    }

    [Fact]
    public void PrevisualizarFactura_NoPersisteNada()
    {
        var reserva = _e.EstadiaEnCurso();
        var r = _e.S.Estadias.PrevisualizarFactura(reserva.Id, 5m);
        Assert.True(r.Exito);
        Assert.Equal(EstadoReserva.EnCurso, _e.S.Reservas.ObtenerPorId(reserva.Id)!.Estado);
        Assert.Empty(_e.S.Facturacion.Buscar());
    }

    [Fact]
    public void Liberar_HabitacionTrasCheckOut_VuelveADisponible()
    {
        var reserva = _e.EstadiaEnCurso();
        _e.S.Estadias.CheckOut(reserva.Id);
        Assert.True(_e.S.Habitaciones.Liberar(reserva.HabitacionId).Exito);
        Assert.Equal(EstadoHabitacion.Disponible, EstadoDe(reserva.HabitacionId));
    }
}
