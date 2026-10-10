using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Transiciones de estado de <see cref="Habitacion"/> (RN-10) y su validación.</summary>
public class HabitacionTests
{
    private static Habitacion Nueva() =>
        new() { Numero = "101", Piso = 1, Tipo = TipoHabitacion.Doble, Capacidad = 2, TarifaNoche = 500m };

    [Fact]
    public void CambiarEstado_DisponibleAMantenimientoYVuelta_Permitido()
    {
        var h = Nueva();
        h.CambiarEstado(EstadoHabitacion.Mantenimiento);
        h.CambiarEstado(EstadoHabitacion.Disponible);
        Assert.Equal(EstadoHabitacion.Disponible, h.Estado);
    }

    [Fact]
    public void CambiarEstado_CicloDeEstadia_Permitido()
    {
        var h = Nueva();
        h.CambiarEstado(EstadoHabitacion.Ocupada);
        h.CambiarEstado(EstadoHabitacion.Limpieza);
        h.CambiarEstado(EstadoHabitacion.Disponible);
        Assert.Equal(EstadoHabitacion.Disponible, h.Estado);
    }

    [Theory]
    [InlineData(EstadoHabitacion.Limpieza)]
    [InlineData(EstadoHabitacion.Disponible)]
    public void CambiarEstado_DisponibleATransicionInvalida_Lanza(EstadoHabitacion destino) =>
        Assert.Throws<ReglaNegocioException>(() => Nueva().CambiarEstado(destino));

    [Fact]
    public void CambiarEstado_OcupadaAMantenimiento_Lanza()
    {
        var h = Nueva();
        h.CambiarEstado(EstadoHabitacion.Ocupada);
        Assert.Throws<ReglaNegocioException>(() => h.CambiarEstado(EstadoHabitacion.Mantenimiento));
    }

    [Fact]
    public void CambiarEstado_LimpiezaAMantenimiento_Lanza()
    {
        var h = Nueva();
        h.CambiarEstado(EstadoHabitacion.Ocupada);
        h.CambiarEstado(EstadoHabitacion.Limpieza);
        Assert.Throws<ReglaNegocioException>(() => h.CambiarEstado(EstadoHabitacion.Mantenimiento));
    }

    [Fact]
    public void Validar_DatosCorrectos_SinErrores() => Assert.Empty(Nueva().Validar());

    [Fact]
    public void Validar_TarifaCeroYSinNumero_DevuelveErrores()
    {
        var h = Nueva();
        h.Numero = "";
        h.TarifaNoche = 0;
        Assert.Equal(2, h.Validar().Count());
    }
}
