using System.Text.Json.Serialization;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Consumo registrado durante una estadía (objeto de valor).</summary>
/// <remarks>RF-20, RN-08</remarks>
public class CargoAdicional
{
    /// <summary>Servicio del catálogo que originó el cargo.</summary>
    public int ServicioId { get; init; }
    /// <summary>Descripción copiada del servicio.</summary>
    public string Descripcion { get; init; } = "";
    /// <summary>Cantidad (mayor que cero).</summary>
    public int Cantidad { get; init; }
    /// <summary>Precio unitario vigente al registrar.</summary>
    public decimal PrecioUnitario { get; init; }
    /// <summary>Fecha del cargo.</summary>
    public DateTime Fecha { get; init; }

    /// <summary>Cantidad × precio unitario.</summary>
    [JsonIgnore]
    public decimal Subtotal => Constantes.Redondear(Cantidad * PrecioUnitario);
}
