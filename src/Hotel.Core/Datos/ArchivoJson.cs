using System.Text.Json;

namespace Hotel.Core.Datos;

/// <summary>Lectura segura y escritura atómica de archivos JSON.</summary>
/// <remarks>RNF-02, RNF-03</remarks>
public static class ArchivoJson
{
    /// <summary>
    /// Lee el archivo; si no existe devuelve default y si está dañado lo respalda
    /// como <c>archivo.json.danado-yyyyMMddHHmmss</c>, avisa y devuelve default.
    /// </summary>
    /// <remarks>RNF-03</remarks>
    public static T? Leer<T>(string ruta, Action<string>? alAvisar = null)
    {
        if (!File.Exists(ruta)) return default;
        try
        {
            using var flujo = File.OpenRead(ruta);
            return JsonSerializer.Deserialize<T>(flujo, JsonConfig.Opciones);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException or InvalidOperationException)
        {
            var respaldo = $"{ruta}.danado-{DateTime.Now:yyyyMMddHHmmss}";
            File.Move(ruta, respaldo, overwrite: true);
            alAvisar?.Invoke($"El archivo «{Path.GetFileName(ruta)}» estaba dañado. Se guardó una copia como «{Path.GetFileName(respaldo)}» y se inició vacío.");
            return default;
        }
    }

    /// <summary>Escribe a un archivo temporal y lo mueve sobre el destino.</summary>
    /// <remarks>RNF-03</remarks>
    public static void Escribir<T>(string ruta, T contenido)
    {
        var temporal = ruta + ".tmp";
        using (var flujo = File.Create(temporal))
            JsonSerializer.Serialize(flujo, contenido, JsonConfig.Opciones);
        File.Move(temporal, ruta, overwrite: true);
    }
}
