using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Transiciones, noches y solapamiento de <see cref="Reserva"/> (RN-03, RN-05, RN-08, RN-09).</summary>
public class ReservaTests
{
    private static readonly DateTime Entrada = new(2026, 3, 10);
    private static readonly DateTime Salida = new(2026, 3, 13);

    private static Reserva Nueva() => Reserva.Crear(1, 1, Entrada, Salida, 2, 500m);

    private static Reserva EnCurso()
    {
        var r = Nueva();
        r.Confirmar();
        r.RegistrarCheckIn(Entrada);
        return r;
    }

    [Fact]
    public void Noches_TresDias_DevuelveTres() => Assert.Equal(3, Nueva().Noches);

    [Fact]
    public void TotalAlojamiento_TresNochesA500_Devuelve1500() => Assert.Equal(1500m, Nueva().TotalAlojamiento);

    [Fact]
    public void Confirmar_Pendiente_PasaAConfirmada()
    {
        var r = Nueva();
        r.Confirmar();
        Assert.Equal(EstadoReserva.Confirmada, r.Estado);
    }

    [Fact]
    public void Confirmar_YaConfirmada_LanzaReglaNegocio()
    {
        var r = Nueva();
        r.Confirmar();
        Assert.Throws<ReglaNegocioException>(r.Confirmar);
    }

    [Fact]
    public void Cancelar_SinMotivo_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => Nueva().Cancelar("  "));

    [Fact]
    public void Cancelar_Pendiente_GuardaMotivo()
    {
        var r = Nueva();
        r.Cancelar("Cambio de planes");
        Assert.Equal(EstadoReserva.Cancelada, r.Estado);
        Assert.Equal("Cambio de planes", r.MotivoCancelacion);
    }

    [Fact]
    public void Cancelar_EnCurso_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => EnCurso().Cancelar("x"));

    [Fact]
    public void RegistrarCheckIn_Pendiente_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => Nueva().RegistrarCheckIn(Entrada));

    [Fact]
    public void RegistrarCheckOut_EnCurso_PasaAFinalizada()
    {
        var r = EnCurso();
        r.RegistrarCheckOut(Salida);
        Assert.Equal(EstadoReserva.Finalizada, r.Estado);
        Assert.Equal(Salida, r.FechaCheckOut);
    }

    [Fact]
    public void RegistrarCheckOut_Confirmada_LanzaReglaNegocio()
    {
        var r = Nueva();
        r.Confirmar();
        Assert.Throws<ReglaNegocioException>(() => r.RegistrarCheckOut(Salida));
    }

    [Fact]
    public void AgregarCargo_EnCurso_SumaAlTotal()
    {
        var r = EnCurso();
        r.AgregarCargo(new CargoAdicional { Descripcion = "Desayuno", Cantidad = 2, PrecioUnitario = 50m });
        Assert.Single(r.Cargos);
        Assert.Equal(100m, r.TotalCargos);
    }

    [Fact]
    public void AgregarCargo_ReservaPendiente_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() =>
            Nueva().AgregarCargo(new CargoAdicional { Cantidad = 1, PrecioUnitario = 10m }));

    [Fact]
    public void AgregarCargo_CantidadCero_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() =>
            EnCurso().AgregarCargo(new CargoAdicional { Cantidad = 0, PrecioUnitario = 10m }));

    [Fact]
    public void CambiarFechas_SalidaAnteriorAEntrada_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => Nueva().CambiarFechas(Salida, Entrada, 2));

    [Fact]
    public void CambiarFechas_ReservaEnCurso_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => EnCurso().CambiarFechas(Entrada, Salida.AddDays(1), 2));

    [Fact]
    public void ReasignarHabitacion_Pendiente_ActualizaTarifa()
    {
        var r = Nueva();
        r.ReasignarHabitacion(7, 900m);
        Assert.Equal(7, r.HabitacionId);
        Assert.Equal(900m, r.TarifaPactada);
    }

    [Theory]
    [InlineData(10, 13, true)]   // idéntico
    [InlineData(8, 11, true)]    // cruza el inicio
    [InlineData(12, 15, true)]   // cruza el final
    [InlineData(11, 12, true)]   // contenido
    [InlineData(13, 15, false)]  // entrada = salida de la existente: no se solapa
    [InlineData(7, 10, false)]   // salida = entrada de la existente: no se solapa
    [InlineData(1, 5, false)]
    public void SeSolapaCon_Rangos_AplicaRN03(int dia1, int dia2, bool esperado)
    {
        var r = Nueva();
        Assert.Equal(esperado, r.SeSolapaCon(new DateTime(2026, 3, dia1), new DateTime(2026, 3, dia2)));
    }

    [Fact]
    public void SeSolapaCon_ReservaCancelada_NoBloquea()
    {
        var r = Nueva();
        r.Cancelar("x");
        Assert.False(r.SeSolapaCon(Entrada, Salida));
    }

    [Theory]
    [InlineData(0, 1)]   // mismo día: mínimo una noche
    [InlineData(1, 1)]
    [InlineData(3, 3)]
    public void NochesCobradas_DiasReales_AplicaMinimoUno(int dias, int esperado) =>
        Assert.Equal(esperado, EnCurso().NochesCobradas(Entrada.AddDays(dias)));
}
