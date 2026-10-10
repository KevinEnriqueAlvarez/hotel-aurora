using System.Text.Json.Serialization;
using Hotel.Core.Comun;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Dominio;

/// <summary>Pago por transferencia bancaria.</summary>
/// <remarks>RF-23, RN-12</remarks>
public class PagoTransferencia : Pago
{
    /// <summary>Banco emisor.</summary>
    public string Banco { get; set; } = "";
    /// <summary>Número de referencia.</summary>
    public string NumeroReferencia { get; set; } = "";

    /// <inheritdoc />
    [JsonIgnore] public override string Metodo => "Transferencia";

    /// <inheritdoc />
    public override string Detalle() => $"{Banco}, referencia {NumeroReferencia}";

    /// <inheritdoc />
    public override IEnumerable<string> Validar()
    {
        foreach (var e in base.Validar()) yield return e;
        if (string.IsNullOrWhiteSpace(Banco)) yield return "El banco es obligatorio.";
        if (string.IsNullOrWhiteSpace(NumeroReferencia)) yield return "El número de referencia es obligatorio.";
    }
}
