using System.Text.Json.Serialization;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Línea de detalle de una factura (objeto de valor).</summary>
/// <remarks>RF-22</remarks>
public class LineaFactura
{
    /// <summary>Concepto facturado.</summary>
    public string Concepto { get; init; } = "";
    /// <summary>Cantidad.</summary>
    public int Cantidad { get; init; }
    /// <summary>Precio unitario.</summary>
    public decimal PrecioUnitario { get; init; }

    /// <summary>Cantidad × precio unitario.</summary>
    [JsonIgnore]
    public decimal Subtotal => Constantes.Redondear(Cantidad * PrecioUnitario);
}
