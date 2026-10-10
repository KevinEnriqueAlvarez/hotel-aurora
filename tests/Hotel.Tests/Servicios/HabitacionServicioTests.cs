using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Tests.Servicios;

/// <summary>CRUD, filtros y estados de <see cref="HabitacionServicio"/> (RF-03 a RF-07, RN-01, RN-10, RN-14).</summary>
public sealed class HabitacionServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    [Fact]
    public void Registrar_Valida_QuedaDisponibleYConId()
    {
        var h = _e.Habitacion("101");
        Assert.True(h.Id > 0);
        Assert.Equal(EstadoHabitacion.Disponible, h.Estado);
    }

    [Fact]
    public void Registrar_NumeroDuplicado_Rechaza()
    {
        _e.Habitacion("101");
        var r = _e.S.Habitaciones.Registrar(new Habitacion { Numero = "101", Capacidad = 2, TarifaNoche = 300m });
        Assert.False(r.Exito);
        Assert.Contains("Ya existe", r.Mensaje);
    }

    [Fact]
    public void Registrar_DatosInvalidos_DevuelveTodosLosErrores()
    {
        var r = _e.S.Habitaciones.Registrar(new Habitacion { Numero = "", Capacidad = 0, TarifaNoche = 0m });
        Assert.False(r.Exito);
        Assert.True(r.Errores.Count >= 3);
    }

    [Fact]
    public void Registrar_Recepcionista_RechazaPorPermiso()
    {
        _e.IniciarComoRecepcionista();
        var r = _e.S.Habitaciones.Registrar(new Habitacion { Numero = "999", Capacidad = 2, TarifaNoche = 300m });
        Assert.False(r.Exito);
    }

    [Fact]
    public void Editar_CambiaDatosSinTocarElEstado()
    {
        var h = _e.Habitacion("101");
        _e.S.Habitaciones.CambiarEstado(h.Id, EstadoHabitacion.Mantenimiento);
        var copia = _e.S.Habitaciones.ObtenerPorId(h.Id)!;
        copia.TarifaNoche = 650m;
        copia.Descripcion = "Renovada";

        Assert.True(_e.S.Habitaciones.Editar(copia).Exito);

        var guardada = _e.S.Habitaciones.ObtenerPorId(h.Id)!;
        Assert.Equal(650m, guardada.TarifaNoche);
        Assert.Equal(EstadoHabitacion.Mantenimiento, guardada.Estado);
    }

    [Fact]
    public void Editar_NumeroDeOtraHabitacion_Rechaza()
    {
        _e.Habitacion("101");
        var b = _e.Habitacion("102");
        b.Numero = "101";
        Assert.False(_e.S.Habitaciones.Editar(b).Exito);
    }

    [Fact]
    public void Editar_CopiaNoGuardada_NoAlteraElRepositorio()
    {
        var h = _e.Habitacion("101");
        var copia = _e.S.Habitaciones.ObtenerPorId(h.Id)!;
        copia.Numero = "XYZ";
        Assert.Equal("101", _e.S.Habitaciones.ObtenerPorId(h.Id)!.Numero);
    }

    [Fact]
    public void Eliminar_SinReservas_EliminaFisicamente()
    {
        var h = _e.Habitacion();
        var r = _e.S.Habitaciones.Eliminar(h.Id);
        Assert.True(r.Exito);
        Assert.Null(_e.S.Habitaciones.ObtenerPorId(h.Id));
    }

    [Fact]
    public void Eliminar_ConReservas_Desactiva()
    {
        var h = _e.Habitacion();
        _e.Reserva(_e.Huesped(), h, 5, 2);

        var r = _e.S.Habitaciones.Eliminar(h.Id);

        Assert.True(r.Exito);
        Assert.Contains("desactivó", r.Mensaje);
        Assert.False(_e.S.Habitaciones.ObtenerPorId(h.Id)!.Activo);
        Assert.Empty(_e.S.Habitaciones.Buscar());
        Assert.Single(_e.S.Habitaciones.Buscar(new FiltroHabitacion(IncluirInactivas: true)));
    }

    [Fact]
    public void Eliminar_HabitacionOcupada_Rechaza()
    {
        var reserva = _e.EstadiaEnCurso();
        Assert.False(_e.S.Habitaciones.Eliminar(reserva.HabitacionId).Exito);
    }

    [Fact]
    public void Buscar_PorTipoEstadoPisoYTexto_Filtra()
    {
        _e.Habitacion("101", tipo: TipoHabitacion.Sencilla, piso: 1);
        _e.Habitacion("201", tipo: TipoHabitacion.Doble, piso: 2);
        var suite = _e.Habitacion("301", tipo: TipoHabitacion.Suite, piso: 3);
        _e.S.Habitaciones.CambiarEstado(suite.Id, EstadoHabitacion.Mantenimiento);

        Assert.Single(_e.S.Habitaciones.Buscar(new FiltroHabitacion(Tipo: TipoHabitacion.Doble)));
        Assert.Single(_e.S.Habitaciones.Buscar(new FiltroHabitacion(Estado: EstadoHabitacion.Mantenimiento)));
        Assert.Single(_e.S.Habitaciones.Buscar(new FiltroHabitacion(Piso: 1)));
        Assert.Single(_e.S.Habitaciones.Buscar(new FiltroHabitacion(Texto: "30")));
        Assert.Equal(3, _e.S.Habitaciones.Buscar().Count);
    }

    [Fact]
    public void CambiarEstado_AMantenimientoYDeVuelta_Funciona()
    {
        var h = _e.Habitacion();
        Assert.True(_e.S.Habitaciones.CambiarEstado(h.Id, EstadoHabitacion.Mantenimiento).Exito);
        Assert.True(_e.S.Habitaciones.CambiarEstado(h.Id, EstadoHabitacion.Disponible).Exito);
    }

    [Theory]
    [InlineData(EstadoHabitacion.Ocupada)]
    [InlineData(EstadoHabitacion.Limpieza)]
    public void CambiarEstado_ManualAOcupadaOLimpieza_Rechaza(EstadoHabitacion destino)
    {
        var h = _e.Habitacion();
        Assert.False(_e.S.Habitaciones.CambiarEstado(h.Id, destino).Exito);
    }

    [Fact]
    public void CambiarEstado_HabitacionOcupada_Rechaza()
    {
        var reserva = _e.EstadiaEnCurso();
        Assert.False(_e.S.Habitaciones.CambiarEstado(reserva.HabitacionId, EstadoHabitacion.Mantenimiento).Exito);
    }

    [Fact]
    public void Liberar_HabitacionDisponible_Rechaza()
    {
        var h = _e.Habitacion();
        Assert.False(_e.S.Habitaciones.Liberar(h.Id).Exito);
    }

    [Fact]
    public void Liberar_Recepcionista_Puede()
    {
        var reserva = _e.EstadiaEnCurso();
        _e.S.Estadias.CheckOut(reserva.Id);
        _e.IniciarComoRecepcionista();
        Assert.True(_e.S.Habitaciones.Liberar(reserva.HabitacionId).Exito);
    }

    [Fact]
    public void ReservadasHoy_ReservaConfirmadaQueCubreHoy_SeCalcula()
    {
        var h = _e.Habitacion();
        _e.Reserva(_e.Huesped(), h, 0, 2);
        Assert.Contains(h.Id, _e.S.Habitaciones.ReservadasHoy());
        Assert.Equal(EstadoHabitacion.Disponible, _e.S.Habitaciones.ObtenerPorId(h.Id)!.Estado);
    }
}
