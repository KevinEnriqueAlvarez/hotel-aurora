namespace Hotel.Core.Interfaces;

/// <summary>Contrato de los objetos que saben validarse a sí mismos.</summary>
/// <remarks>RF-03, RF-08, RF-14 (validación de datos)</remarks>
public interface IValidable
{
    /// <summary>Devuelve los mensajes de error; vacío si el objeto es válido.</summary>
    /// <remarks>RF-03, RF-08, RF-14</remarks>
    IEnumerable<string> Validar();
}
