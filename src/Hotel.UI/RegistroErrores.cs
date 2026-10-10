namespace Hotel.UI;

/// <summary>Registra errores inesperados en <c>Datos/errores.log</c> sin mostrar trazas al usuario.</summary>
/// <remarks>RNF-09</remarks>
public static class RegistroErrores
{
    private static string _carpeta = Path.Combine(AppContext.BaseDirectory, "Datos");

    /// <summary>Carpeta donde se escribe <c>errores.log</c>.</summary>
    public static void Configurar(string carpeta) => _carpeta = carpeta;

    /// <summary>Agrega la excepción al registro; nunca lanza.</summary>
    public static void Registrar(Exception excepcion, string origen)
    {
        try
        {
            Directory.CreateDirectory(_carpeta);
            var linea = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ({origen}) {excepcion}{Environment.NewLine}";
            File.AppendAllText(Path.Combine(_carpeta, "errores.log"), linea);
        }
        catch (IOException) { }
        catch (UnauthorizedAccessException) { }
    }
}
