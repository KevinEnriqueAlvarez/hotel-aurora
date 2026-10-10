using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Tests.Servicios;

/// <summary>Reglas RN-02 a RN-06 y RN-15 de <see cref="ReservaServicio"/>.</summary>
public sealed class ReservaServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    private Resultado<Reserva> Crear(Huesped h, Habitacion hab, int dias, int noches, int personas = 2, bool confirmar = false)
    {
        var entrada = Escenario.Hoy.AddDays(dias);
        return _e.S.Reservas.Crear(h.Id, hab.Id, entrada, entrada.AddDays(noches), personas, confirmar);
    }

    [Fact]
    public void Crear_DatosValidos_AsignaCodigoTarifaYEstadoPendiente()
    {
        var hab = _e.Habitacion(tarifa: 480m);
        var r = Crear(_e.Huesped(), hab, 1, 3);

        Assert.True(r.Exito, r.Mensaje);
        Assert.Equal("RES-000001", r.Valor!.Codigo);
        Assert.Equal(480m, r.Valor.TarifaPactada);
        Assert.Equal(EstadoReserva.Pendiente, r.Valor.Estado);
    }

    [Fact]
    public void Crear_Confirmada_QuedaConfirmada()
    {
        var r = Crear(_e.Huesped(), _e.Habitacion(), 1, 2, confirmar: true);
        Assert.Equal(EstadoReserva.Confirmada, r.Valor!.Estado);
    }

    [Fact]
    public void Crear_DosReservas_CorrelativoIncrementa()
    {
        var hab = _e.Habitacion();
        var h = _e.Huesped();
        Crear(h, hab, 1, 1);
        var segunda = Crear(h, hab, 5, 1);
        Assert.Equal("RES-000002", segunda.Valor!.Codigo);
    }

    [Fact]
    public void Crear_FechaDeEntradaPasada_Rechaza()
    {
        var r = Crear(_e.Huesped(), _e.Habitacion(), -1, 2);
        Assert.False(r.Exito);
        Assert.Contains("anterior a hoy", r.Mensaje);
    }

    [Fact]
    public void Crear_EntradaHoy_Acepta() => Assert.True(Crear(_e.Huesped(), _e.Habitacion(), 0, 1).Exito);

    [Fact]
    public void Crear_SalidaIgualAEntrada_Rechaza() =>
        Assert.False(Crear(_e.Huesped(), _e.Habitacion(), 1, 0).Exito);

    [Fact]
    public void Crear_MasNochesQueElMaximo_Rechaza()
    {
        var r = Crear(_e.Huesped(), _e.Habitacion(), 1, 31);
        Assert.False(r.Exito);
        Assert.Contains("30 noches", r.Mensaje);
    }

    [Fact]
    public void Crear_ExactamenteElMaximoDeNoches_Acepta() =>
        Assert.True(Crear(_e.Huesped(), _e.Habitacion(), 1, 30).Exito);

    [Fact]
    public void Crear_PersonasExcedenCapacidad_Rechaza()
    {
        var r = Crear(_e.Huesped(), _e.Habitacion(capacidad: 2), 1, 2, personas: 3);
        Assert.False(r.Exito);
        Assert.Contains("máximo 2", r.Mensaje);
    }

    [Fact]
    public void Crear_HabitacionEnMantenimiento_Rechaza()
    {
        var hab = _e.Habitacion();
        Assert.True(_e.S.Habitaciones.CambiarEstado(hab.Id, EstadoHabitacion.Mantenimiento).Exito);
        Assert.False(Crear(_e.Huesped(), hab, 1, 2).Exito);
    }

    [Fact]
    public void Crear_HabitacionInactiva_Rechaza()
    {
        var hab = _e.Habitacion();
        var h = _e.Huesped();
        Crear(h, hab, 40, 1); // le da historial: se desactiva en lugar de eliminar
        _e.S.Habitaciones.Eliminar(hab.Id);
        Assert.False(Crear(h, hab, 1, 2).Exito);
    }

    [Fact]
    public void Crear_FechasSolapadas_Rechaza()
    {
        var hab = _e.Habitacion();
        var h = _e.Huesped();
        Assert.True(Crear(h, hab, 5, 4).Exito);                 // 15 al 19
        var r = Crear(_e.Huesped(), hab, 7, 3);                 // 17 al 20
        Assert.False(r.Exito);
        Assert.Contains("ya está reservada", r.Mensaje);
    }

    [Fact]
    public void Crear_SalidaIgualAEntradaDeOtra_NoSeSolapa()
    {
        var hab = _e.Habitacion();
        Assert.True(Crear(_e.Huesped(), hab, 5, 2).Exito);      // 15 al 17
        Assert.True(Crear(_e.Huesped(), hab, 7, 2).Exito);      // 17 al 19
        Assert.True(Crear(_e.Huesped(), hab, 3, 2).Exito);      // 13 al 15
    }

    [Fact]
    public void Crear_ReservaCanceladaNoBloqueaLaHabitacion()
    {
        var hab = _e.Habitacion();
        var primera = Crear(_e.Huesped(), hab, 5, 3).Valor!;
        Assert.True(_e.S.Reservas.Cancelar(primera.Id, "Cambio de planes").Exito);
        Assert.True(Crear(_e.Huesped(), hab, 5, 3).Exito);
    }

    [Fact]
    public void Crear_HuespedInexistente_Rechaza()
    {
        var hab = _e.Habitacion();
        var r = _e.S.Reservas.Crear(999, hab.Id, Escenario.Hoy.AddDays(1), Escenario.Hoy.AddDays(2), 1);
        Assert.False(r.Exito);
    }

    [Fact]
    public void Modificar_MismasFechas_SeExcluyeASiMisma()
    {
        var hab = _e.Habitacion();
        var reserva = Crear(_e.Huesped(), hab, 5, 3, confirmar: true).Valor!;
        var entrada = reserva.FechaEntrada;

        var r = _e.S.Reservas.Modificar(reserva.Id, hab.Id, entrada, entrada.AddDays(4), 2);

        Assert.True(r.Exito, r.Mensaje);
        Assert.Equal(4, _e.S.Reservas.ObtenerPorId(reserva.Id)!.Noches);
    }

    [Fact]
    public void Modificar_FechasSolapadasConOtraReserva_Rechaza()
    {
        var hab = _e.Habitacion();
        Crear(_e.Huesped(), hab, 10, 3);
        var reserva = Crear(_e.Huesped(), hab, 5, 2).Valor!;

        var r = _e.S.Reservas.Modificar(reserva.Id, hab.Id, Escenario.Hoy.AddDays(8), Escenario.Hoy.AddDays(11), 2);

        Assert.False(r.Exito);
    }

    [Fact]
    public void Modificar_OtraHabitacion_ActualizaTarifaPactada()
    {
        var barata = _e.Habitacion("101", tarifa: 400m);
        var cara = _e.Habitacion("102", tarifa: 900m);
        var reserva = Crear(_e.Huesped(), barata, 5, 2).Valor!;

        var r = _e.S.Reservas.Modificar(reserva.Id, cara.Id, reserva.FechaEntrada, reserva.FechaSalida, 2);

        Assert.True(r.Exito, r.Mensaje);
        var guardada = _e.S.Reservas.ObtenerPorId(reserva.Id)!;
        Assert.Equal(cara.Id, guardada.HabitacionId);
        Assert.Equal(900m, guardada.TarifaPactada);
    }

    [Fact]
    public void Modificar_ReservaEnCurso_Rechaza()
    {
        var enCurso = _e.EstadiaEnCurso();
        var r = _e.S.Reservas.Modificar(enCurso.Id, enCurso.HabitacionId, enCurso.FechaEntrada, enCurso.FechaSalida.AddDays(1), 2);
        Assert.False(r.Exito);
    }

    [Fact]
    public void Modificar_PersonasExcedenCapacidad_Rechaza()
    {
        var hab = _e.Habitacion(capacidad: 2);
        var reserva = Crear(_e.Huesped(), hab, 5, 2).Valor!;
        Assert.False(_e.S.Reservas.Modificar(reserva.Id, hab.Id, reserva.FechaEntrada, reserva.FechaSalida, 4).Exito);
    }

    [Fact]
    public void Confirmar_Pendiente_Confirma_YSegundaVezFalla()
    {
        var reserva = Crear(_e.Huesped(), _e.Habitacion(), 2, 1).Valor!;
        Assert.True(_e.S.Reservas.Confirmar(reserva.Id).Exito);
        Assert.False(_e.S.Reservas.Confirmar(reserva.Id).Exito);
    }

    [Fact]
    public void Cancelar_SinMotivo_Rechaza()
    {
        var reserva = Crear(_e.Huesped(), _e.Habitacion(), 2, 1).Valor!;
        Assert.False(_e.S.Reservas.Cancelar(reserva.Id, "").Exito);
        Assert.Equal(EstadoReserva.Pendiente, _e.S.Reservas.ObtenerPorId(reserva.Id)!.Estado);
    }

    [Fact]
    public void Cancelar_ConMotivo_GuardaMotivo()
    {
        var reserva = Crear(_e.Huesped(), _e.Habitacion(), 2, 1).Valor!;
        Assert.True(_e.S.Reservas.Cancelar(reserva.Id, "Enfermedad").Exito);
        Assert.Equal("Enfermedad", _e.S.Reservas.ObtenerPorId(reserva.Id)!.MotivoCancelacion);
    }

    [Fact]
    public void BuscarDisponibles_FiltraSolapadasCapacidadMantenimientoYTipo()
    {
        var libre = _e.Habitacion("101", capacidad: 2, tarifa: 500m);
        var ocupada = _e.Habitacion("102", capacidad: 2);
        var chica = _e.Habitacion("103", capacidad: 1);
        var mant = _e.Habitacion("104", capacidad: 2);
        var suite = _e.Habitacion("105", capacidad: 3, tipo: TipoHabitacion.Suite, tarifa: 1000m);
        Crear(_e.Huesped(), ocupada, 4, 5);
        _e.S.Habitaciones.CambiarEstado(mant.Id, EstadoHabitacion.Mantenimiento);

        var todas = _e.S.Reservas.BuscarDisponibles(Escenario.Hoy.AddDays(5), Escenario.Hoy.AddDays(7), 2);
        var soloSuites = _e.S.Reservas.BuscarDisponibles(Escenario.Hoy.AddDays(5), Escenario.Hoy.AddDays(7), 2, TipoHabitacion.Suite);

        Assert.Equal([libre.Id, suite.Id], todas.Select(h => h.Id).ToArray());
        Assert.DoesNotContain(todas, h => h.Id == chica.Id);
        Assert.Equal([suite.Id], soloSuites.Select(h => h.Id).ToArray());
    }

    [Fact]
    public void BuscarDisponibles_ExcluyendoReserva_LiberaSuPropiaHabitacion()
    {
        var hab = _e.Habitacion();
        var reserva = Crear(_e.Huesped(), hab, 5, 3).Valor!;
        var sin = _e.S.Reservas.BuscarDisponibles(reserva.FechaEntrada, reserva.FechaSalida, 2);
        var con = _e.S.Reservas.BuscarDisponibles(reserva.FechaEntrada, reserva.FechaSalida, 2, excluirReservaId: reserva.Id);
        Assert.Empty(sin);
        Assert.Single(con);
    }

    [Fact]
    public void Buscar_PorEstadoYRangoDeFechas_Filtra()
    {
        var hab = _e.Habitacion();
        var h = _e.Huesped();
        Crear(h, hab, 2, 1, confirmar: true);
        Crear(h, hab, 10, 1);
        Crear(h, hab, 20, 1, confirmar: true);

        var confirmadas = _e.S.Reservas.Buscar(new FiltroReserva(Estado: EstadoReserva.Confirmada));
        var rango = _e.S.Reservas.Buscar(new FiltroReserva(Desde: Escenario.Hoy.AddDays(5), Hasta: Escenario.Hoy.AddDays(15)));
        var delHuesped = _e.S.Reservas.Buscar(new FiltroReserva(HuespedId: h.Id));

        Assert.Equal(2, confirmadas.Count);
        Assert.Single(rango);
        Assert.Equal(3, delHuesped.Count);
    }

    [Fact]
    public void LlegadasDelDia_SoloConfirmadasQueEntranHoy()
    {
        var hab = _e.Habitacion();
        var hab2 = _e.Habitacion("102");
        var h = _e.Huesped();
        Crear(h, hab, 0, 2, confirmar: true);
        Crear(h, hab2, 0, 2, confirmar: false);
        Crear(h, _e.Habitacion("103"), 3, 2, confirmar: true);

        Assert.Single(_e.S.Reservas.LlegadasDelDia());
    }
}
