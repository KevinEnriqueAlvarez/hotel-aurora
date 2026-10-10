using Hotel.Core.Comun;

namespace Hotel.UI.Formularios;

/// <summary>Ayudante estático para mostrar mensajes temáticos.</summary>
/// <remarks>Sección 10.6; confirmaciones de RF-05, RF-10, RF-17, RF-21, RF-24</remarks>
public static class Mensajes
{
    /// <summary>Mensaje informativo.</summary>
    public static void Informacion(IWin32Window? propietario, string texto, string titulo = "Información") =>
        Mostrar(propietario, TipoMensaje.Informacion, titulo, texto);

    /// <summary>Mensaje de éxito.</summary>
    public static void Exito(IWin32Window? propietario, string texto, string titulo = "Listo") =>
        Mostrar(propietario, TipoMensaje.Exito, titulo, texto);

    /// <summary>Advertencia.</summary>
    public static void Advertencia(IWin32Window? propietario, string texto, string titulo = "Atención") =>
        Mostrar(propietario, TipoMensaje.Advertencia, titulo, texto);

    /// <summary>Error amigable.</summary>
    public static void Error(IWin32Window? propietario, string texto, string titulo = "No se pudo completar") =>
        Mostrar(propietario, TipoMensaje.Error, titulo, texto);

    /// <summary>Pide confirmación; true si el usuario acepta.</summary>
    public static bool Confirmar(IWin32Window? propietario, string texto, string titulo = "Confirmación",
        bool peligro = false, string textoAceptar = "Confirmar")
    {
        using var dialogo = new DialogoMensaje(TipoMensaje.Confirmacion, titulo, texto, peligro, textoAceptar);
        return dialogo.ShowDialog(propietario) == DialogResult.OK;
    }

    /// <summary>Muestra el resultado de un servicio: éxito con mensaje o el error devuelto.</summary>
    /// <returns>El mismo <see cref="Resultado.Exito"/>.</returns>
    public static bool MostrarResultado(IWin32Window? propietario, Resultado resultado, bool avisarExito = true)
    {
        if (!resultado.Exito) Error(propietario, resultado.Mensaje);
        else if (avisarExito && !string.IsNullOrWhiteSpace(resultado.Mensaje)) Exito(propietario, resultado.Mensaje);
        return resultado.Exito;
    }

    private static void Mostrar(IWin32Window? propietario, TipoMensaje tipo, string titulo, string texto)
    {
        using var dialogo = new DialogoMensaje(tipo, titulo, texto);
        dialogo.ShowDialog(propietario);
    }
}
