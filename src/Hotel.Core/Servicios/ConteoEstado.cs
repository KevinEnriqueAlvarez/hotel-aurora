namespace Hotel.Core.Servicios;

/// <summary>Cantidad de habitaciones en un estado (incluye «Reservada hoy»).</summary>
/// <remarks>E1, RN-10</remarks>
public record ConteoEstado(string Nombre, int Cantidad);
