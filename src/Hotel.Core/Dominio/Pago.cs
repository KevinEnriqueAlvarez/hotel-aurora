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
