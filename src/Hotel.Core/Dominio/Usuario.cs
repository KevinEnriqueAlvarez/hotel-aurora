using System.Security.Cryptography;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Usuario del sistema con contraseña protegida por PBKDF2.</summary>
/// <remarks>RF-01, RF-02, RN-01, RNF-06</remarks>
public class Usuario : Persona
{
    /// <summary>Nombre de usuario único (RN-01).</summary>
    public string NombreUsuario { get; set; } = "";

    /// <summary>Rol que determina el menú y los permisos.</summary>
    public RolUsuario Rol { get; set; } = RolUsuario.Recepcionista;

    /// <summary>Hash PBKDF2 (Base64).</summary>
    [JsonInclude] public string HashContrasena { get; private set; } = "";

    /// <summary>Sal aleatoria (Base64).</summary>
    [JsonInclude] public string Sal { get; private set; } = "";

    /// <summary>Guarda el hash de la contraseña con una sal nueva.</summary>
    /// <remarks>RNF-06</remarks>
    /// <exception cref="ReglaNegocioException">La contraseña no cumple la política.</exception>
    public void EstablecerContrasena(string texto)
    {
        if (!Validador.EsContrasenaValida(texto)) throw new ReglaNegocioException(Validador.MensajeContrasena);
        var sal = RandomNumberGenerator.GetBytes(Constantes.TamanoSal);
        Sal = Convert.ToBase64String(sal);
        HashContrasena = Convert.ToBase64String(CalcularHash(texto, sal));
    }

    /// <summary>Comprueba una contraseña contra el hash guardado.</summary>
    /// <remarks>RF-01, RNF-06</remarks>
    public bool VerificarContrasena(string texto)
    {
        if (string.IsNullOrEmpty(HashContrasena) || string.IsNullOrEmpty(Sal) || string.IsNullOrEmpty(texto))
            return false;
        var calculado = CalcularHash(texto, Convert.FromBase64String(Sal));
        return CryptographicOperations.FixedTimeEquals(calculado, Convert.FromBase64String(HashContrasena));
    }

    /// <inheritdoc />
    /// <remarks>RF-02, RN-01</remarks>
    public override IEnumerable<string> Validar()
    {
        foreach (var error in base.Validar()) yield return error;
        if (!Regex.IsMatch(NombreUsuario ?? "", @"^[A-Za-z0-9._]{3,30}$"))
            yield return "El nombre de usuario debe tener 3 a 30 letras, números, punto o guion bajo.";
        if (string.IsNullOrEmpty(HashContrasena)) yield return "La contraseña es obligatoria.";
    }

    private static byte[] CalcularHash(string texto, byte[] sal) =>
        Rfc2898DeriveBytes.Pbkdf2(texto, sal, Constantes.IteracionesPbkdf2, HashAlgorithmName.SHA256, Constantes.TamanoHash);
}
