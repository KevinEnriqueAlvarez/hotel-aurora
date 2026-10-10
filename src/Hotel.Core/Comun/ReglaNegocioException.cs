namespace Hotel.Core.Comun;

/// <summary>Se lanza cuando se viola una regla de negocio; los servicios la convierten en <see cref="Resultado"/>.</summary>
/// <remarks>RN-02..RN-13, RNF-09</remarks>
public class ReglaNegocioException : Exception
{
    /// <summary>Crea la excepción con el mensaje amigable para el usuario.</summary>
    public ReglaNegocioException(string mensaje) : base(mensaje) { }
}
