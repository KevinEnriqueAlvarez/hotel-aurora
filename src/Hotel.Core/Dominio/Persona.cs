using System.Text.Json.Serialization;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Datos personales comunes a huéspedes y usuarios.</summary>
/// <remarks>RF-02, RF-08; validaciones de la sección 8</remarks>
public abstract class Persona : EntidadBase
{
    /// <summary>Nombres.</summary>
    public string Nombres { get; set; } = "";
    /// <summary>Apellidos.</summary>
    public string Apellidos { get; set; } = "";
    /// <summary>Tipo de documento.</summary>
    public TipoDocumento TipoDocumento { get; set; } = TipoDocumento.Dpi;
    /// <summary>Número de documento (DPI o pasaporte).</summary>
    public string NumeroDocumento { get; set; } = "";
    /// <summary>Teléfono.</summary>
    public string Telefono { get; set; } = "";
    /// <summary>Correo electrónico.</summary>
    public string Correo { get; set; } = "";

    /// <summary>Nombres y apellidos juntos.</summary>
    [JsonIgnore]
    public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

    /// <inheritdoc />
    /// <remarks>RF-08, RF-02</remarks>
    public override IEnumerable<string> Validar()
    {
        if (!Validador.EsNombreValido(Nombres)) yield return "Los nombres deben tener entre 2 y 60 letras.";
        if (!Validador.EsNombreValido(Apellidos)) yield return "Los apellidos deben tener entre 2 y 60 letras.";
        if (!Validador.EsDocumentoValido(TipoDocumento, NumeroDocumento))
            yield return TipoDocumento == TipoDocumento.Dpi
                ? "El DPI debe tener 13 dígitos."
                : "El pasaporte debe tener entre 6 y 20 caracteres alfanuméricos.";
        if (!Validador.EsTelefonoValido(Telefono)) yield return "El teléfono debe tener entre 8 y 15 dígitos.";
        if (!Validador.EsCorreoValido(Correo)) yield return "El correo electrónico no es válido.";
    }
}
