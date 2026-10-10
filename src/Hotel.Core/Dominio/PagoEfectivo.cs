using System.Text.Json.Serialization;
using Hotel.Core.Comun;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Dominio;

/// <summary>Pago en efectivo; calcula el cambio.</summary>
/// <remarks>RF-23, RN-12</remarks>
public class PagoEfectivo : Pago
{
    /// <summary>Monto entregado por el cliente.</summary>
    public decimal MontoRecibido { get; set; }

    /// <summary>Cambio a devolver.</summary>
    [JsonIgnore] public decimal Cambio => Math.Max(0, MontoRecibido - Monto);

    /// <inheritdoc />
    [JsonIgnore] public override string Metodo => "Efectivo";

    /// <inheritdoc />
    public override string Detalle() =>
        $"Recibido {Constantes.FormatearMoneda(MontoRecibido)}, cambio {Constantes.FormatearMoneda(Cambio)}";

    /// <inheritdoc />
    public override IEnumerable<string> Validar()
    {
        foreach (var e in base.Validar()) yield return e;
        if (MontoRecibido < Monto) yield return "El monto recibido no puede ser menor que el monto a pagar.";
    }
}
