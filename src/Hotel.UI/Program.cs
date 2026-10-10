using Hotel.Core.Comun;
using Hotel.Core.Datos;
using Hotel.Core.Servicios;
using Hotel.UI.Formularios;

namespace Hotel.UI;

/// <summary>Punto de entrada y raíz de composición: crea el contexto de datos y los servicios.</summary>
/// <remarks>RNF-02, RNF-07, RNF-09</remarks>
static class Program
{
    private const string MensajeInesperado =
        "Ocurrió un problema inesperado. Sus datos están a salvo. Si el error continúa, informe al administrador.";

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) => ManejarError(e.Exception, "ThreadException");
        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
            ManejarError(e.ExceptionObject as Exception ?? new InvalidOperationException("Error desconocido"), "UnhandledException");

        try
        {
            Ejecutar();
        }
        catch (Exception ex)
        {
            ManejarError(ex, "Main");
        }
    }

    private static void Ejecutar()
    {
        var carpeta = Path.Combine(AppContext.BaseDirectory, "Datos");
        RegistroErrores.Configurar(carpeta);

        var contexto = new ContextoDatos(carpeta);
        DatosSemilla.SembrarSiVacio(contexto, DateTime.Now);
        foreach (var aviso in contexto.Avisos) Mensajes.Advertencia(null, aviso, "Archivo dañado");

        var servicios = new ServiciosHotel(contexto, new RelojSistema());
        while (true)
        {
            using (var login = new FormLogin(servicios))
                if (login.ShowDialog() != DialogResult.OK) return;

            using var principal = new FormPrincipal(servicios);
            if (principal.ShowDialog() != DialogResult.Retry) return;   // Retry = cerrar sesión
        }
    }

    private static void ManejarError(Exception excepcion, string origen)
    {
        RegistroErrores.Registrar(excepcion, origen);
        try { Mensajes.Error(null, MensajeInesperado, "Algo salió mal"); }
        catch { MessageBox.Show(MensajeInesperado, "Hotel Aurora", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }
}
