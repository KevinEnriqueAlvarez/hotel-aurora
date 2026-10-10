using Hotel.Core.Dominio;

namespace Hotel.Core.Servicios;

/// <summary>Criterios para consultar habitaciones.</summary>
/// <remarks>RF-06</remarks>
public record FiltroHabitacion(
    TipoHabitacion? Tipo = null,
    EstadoHabitacion? Estado = null,
    int? Piso = null,
    string? Texto = null,
    bool IncluirInactivas = false);
