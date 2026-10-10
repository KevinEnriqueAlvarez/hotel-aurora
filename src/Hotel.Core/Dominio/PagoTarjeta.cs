using System.Text.Json.Serialization;
using Hotel.Core.Comun;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Dominio;

/// <summary>Pago con tarjeta; nunca guarda el número completo.</summary>
/// <remarks>RF-23, RN-12</remarks>
public class PagoTarjeta : Pago
{
    /// <summary>Últimos cuatro dígitos de la tarjeta.</summary>
    public string UltimosCuatroDigitos { get; set; } = "";
    /// <summary>Número de autorización.</summary>
    public string NumeroAutorizacion { get; set; } = "";

    /// <inheritdoc />
    [JsonIgnore] public override string Metodo => "Tarjeta";

    /// <inheritdoc />
    public override string Detalle() => $"Tarjeta **** {UltimosCuatroDigitos}, autorización {NumeroAutorizacion}";

    /// <inheritdoc />
    public override IEnumerable<string> Validar()
    {
        foreach (var e in base.Validar()) yield return e;
        if (UltimosCuatroDigitos is not { Length: 4 } || !UltimosCuatroDigitos.All(char.IsDigit))
            yield return "Indique exactamente los 4 últimos dígitos de la tarjeta.";
        if (string.IsNullOrWhiteSpace(NumeroAutorizacion)) yield return "El número de autorización es obligatorio.";
    }
}
