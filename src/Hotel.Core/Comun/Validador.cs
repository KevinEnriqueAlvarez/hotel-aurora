using System.Net.Mail;
using System.Text.RegularExpressions;
using Hotel.Core.Dominio;

namespace Hotel.Core.Comun;

/// <summary>Validaciones de formato reutilizables.</summary>
/// <remarks>RF-02, RF-08 (sección 8: validaciones)</remarks>
public static partial class Validador
{
    /// <summary>Mensaje estándar de contraseña inválida.</summary>
    public const string MensajeContrasena = "La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.";

    [GeneratedRegex(@"^\d{13}$")] private static partial Regex RegexDpi();
    [GeneratedRegex(@"^[A-Za-z0-9]{6,20}$")] private static partial Regex RegexPasaporte();
    [GeneratedRegex(@"^\+?\d{8,15}$")] private static partial Regex RegexTelefono();
    [GeneratedRegex(@"^[\p{L}]+( [\p{L}]+)*$")] private static partial Regex RegexNombre();
    [GeneratedRegex(@"^\d+[0-9K]$")] private static partial Regex RegexNit();

    /// <summary>DPI: 13 dígitos.</summary>
    public static bool EsDpiValido(string? valor) => valor is not null && RegexDpi().IsMatch(valor);

    /// <summary>Pasaporte: 6–20 caracteres alfanuméricos.</summary>
    public static bool EsPasaporteValido(string? valor) => valor is not null && RegexPasaporte().IsMatch(valor);

    /// <summary>Valida el documento según su tipo.</summary>
    public static bool EsDocumentoValido(TipoDocumento tipo, string? valor) =>
        tipo == TipoDocumento.Dpi ? EsDpiValido(valor) : EsPasaporteValido(valor);

    /// <summary>NIT: «CF» o dígitos + verificador (0–9 o K), módulo 11.</summary>
    public static bool EsNitValido(string? valor)
    {
        var nit = Normalizar(valor);
        if (nit == Constantes.NitConsumidorFinal) return true;
        if (nit.Length < 2 || !RegexNit().IsMatch(nit)) return false;
        return CalcularVerificadorNit(nit[..^1]) == nit[^1];
    }

    /// <summary>Calcula el dígito verificador de un NIT sin verificador.</summary>
    public static char CalcularVerificadorNit(string digitos)
    {
        var factor = digitos.Length + 1;
        var suma = 0;
        foreach (var c in digitos)
            suma += (c - '0') * factor--;
        var r = (11 - suma % 11) % 11;
        return r == 10 ? 'K' : (char)('0' + r);
    }

    /// <summary>Quita guiones y espacios y pasa a mayúsculas.</summary>
    public static string Normalizar(string? nit) =>
        (nit ?? "").Replace("-", "").Replace(" ", "").Trim().ToUpperInvariant();

    /// <summary>Correo con <c>MailAddress.TryCreate</c>.</summary>
    public static bool EsCorreoValido(string? valor) =>
        !string.IsNullOrWhiteSpace(valor) && MailAddress.TryCreate(valor.Trim(), out var m) && m.Address == valor.Trim();

    /// <summary>Teléfono: 8–15 dígitos, «+» inicial opcional.</summary>
    public static bool EsTelefonoValido(string? valor) => valor is not null && RegexTelefono().IsMatch(valor);

    /// <summary>Nombres: 2–60 caracteres, letras y espacios.</summary>
    public static bool EsNombreValido(string? valor) =>
        valor is { Length: >= 2 and <= 60 } && RegexNombre().IsMatch(valor);

    /// <summary>Contraseña: mínimo 8 caracteres, una mayúscula y un número.</summary>
    public static bool EsContrasenaValida(string? valor) =>
        valor is { Length: >= 8 } && valor.Any(char.IsUpper) && valor.Any(char.IsDigit);
}
