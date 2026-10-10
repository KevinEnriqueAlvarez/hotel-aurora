namespace Hotel.Tests;

/// <summary>Carpeta temporal única que se elimina al terminar la prueba.</summary>
public sealed class CarpetaTemporal : IDisposable
{
    /// <summary>Ruta de la carpeta creada.</summary>
    public string Ruta { get; } = Path.Combine(Path.GetTempPath(), "hotel-aurora-tests", Guid.NewGuid().ToString("N"));

    /// <summary>Crea la carpeta.</summary>
    public CarpetaTemporal() => Directory.CreateDirectory(Ruta);

    /// <summary>Combina la ruta con un archivo.</summary>
    public string Archivo(string nombre) => Path.Combine(Ruta, nombre);

    /// <inheritdoc />
    public void Dispose()
    {
        try { Directory.Delete(Ruta, recursive: true); } catch (IOException) { }
    }
}
