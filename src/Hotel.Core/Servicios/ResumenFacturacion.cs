namespace Hotel.Core.Servicios;

/// <summary>Totales de facturación de un periodo (las anuladas no suman).</summary>
/// <remarks>RF-25</remarks>
public record ResumenFacturacion(
    int Cantidad,
    int CantidadAnuladas,
    decimal TotalFacturado,
    decimal TotalCobrado,
    decimal SaldoPendiente);
