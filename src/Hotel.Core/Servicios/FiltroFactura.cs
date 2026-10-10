using Hotel.Core.Dominio;

namespace Hotel.Core.Servicios;

/// <summary>Criterios del historial de facturación (fecha de emisión, estado, huésped/NIT/número).</summary>
/// <remarks>RF-25</remarks>
public record FiltroFactura(
    DateTime? Desde = null,
    DateTime? Hasta = null,
    EstadoFactura? Estado = null,
    string? Texto = null);
