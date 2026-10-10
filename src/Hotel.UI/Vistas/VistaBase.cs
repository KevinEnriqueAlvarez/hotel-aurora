using Hotel.UI.Tema;

namespace Hotel.UI.Vistas;

/// <summary>Base de las vistas que se cargan dentro de la ventana principal.</summary>
/// <remarks>Sección 10.4: «sin abrir ventanas nuevas»</remarks>
public class VistaBase : UserControl
{
    /// <summary>Crea la vista con el fondo de la aplicación.</summary>
    public VistaBase()
    {
        BackColor = Paleta.Fondo;
        Font = Tipografia.Texto;
        DoubleBuffered = true;
    }

    /// <summary>Título que muestra el encabezado de la ventana principal.</summary>
    public virtual string TituloVista => "";

    /// <summary>Vuelve a cargar los datos desde los servicios.</summary>
    public virtual void Refrescar() { }
}
