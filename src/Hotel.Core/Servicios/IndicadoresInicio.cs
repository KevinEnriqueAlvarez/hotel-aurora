namespace Hotel.Core.Servicios;

/// <summary>Indicadores del tablero de inicio.</summary>
/// <remarks>E1</remarks>
public record IndicadoresInicio(
    int HabitacionesTotal,
    int HabitacionesOcupadas,
    decimal OcupacionHoy,
    decimal IngresosMes,
    int ReservasActivas,
    int LlegadasHoy,
    decimal Cobrado,
    decimal SaldoPorCobrar);
