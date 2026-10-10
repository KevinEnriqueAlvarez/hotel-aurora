using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hotel.Core.Datos;

/// <summary>Opciones de serialización compartidas por todos los archivos JSON.</summary>
/// <remarks>RNF-01</remarks>
public static class JsonConfig
{
    /// <summary>Indentado, camelCase, enums como texto y tildes sin escapar.</summary>
    public static readonly JsonSerializerOptions Opciones = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        Converters = { new JsonStringEnumConverter() }
    };
}
