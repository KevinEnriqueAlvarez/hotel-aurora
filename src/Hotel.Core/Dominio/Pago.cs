using System.Text.Json.Serialization;
using Hotel.Core.Comun;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Dominio;

/// <summary>Pago abstracto; cada método de pago valida y describe sus propios datos.</summary>
/// <remarks>RF-23, RN-12; polimorfismo de <see cref="Validar"/> y <see cref="Detalle"/></remarks>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "tipo")]
[JsonDerivedType(typeof(PagoEfectivo), "efectivo")]
[JsonDerivedType(typeof(PagoTarjeta), "tarjeta")]
[JsonDerivedType(typeof(PagoTransferencia), "transferencia")]
public abstract class Pago : IValidable
{
    /// <summary>Monto aplicado a la factura.</summary>
    public decimal Monto { get; set; }
    /// <summary>Fecha del pago.</summary>
    public DateTime Fecha { get; set; }

    /// <summary>Nombre del método de pago.</summary>
    [JsonIgnore] public abstract string Metodo { get; }

    /// <summary>Texto descriptivo de los datos propios del pago.</summary>
    public abstract string Detalle();

    /// <inheritdoc />
    /// <remarks>RN-12</remarks>
    public virtual IEnumerable<string> Validar()
    {
        if (Monto <= 0) yield return "El monto del pago debe ser mayor que cero.";
    }
}
