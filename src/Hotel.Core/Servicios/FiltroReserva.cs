using Hotel.Core.Dominio;

namespace Hotel.Core.Servicios;

/// <summary>Criterios para listar reservas (la fecha filtra por fecha de entrada).</summary>
/// <remarks>RF-18</remarks>
public record FiltroReserva(
    EstadoReserva? Estado = null,
    DateTime? Desde = null,
    DateTime? Hasta = null,
    int? HuespedId = null,
    string? Texto = null);
