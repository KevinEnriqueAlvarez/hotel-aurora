using Hotel.Core.Comun;

namespace Hotel.UI.Formularios;

/// <summary>Diálogo que pide un texto: motivo de cancelación/anulación o contraseña nueva.</summary>
/// <remarks>RF-02, RF-17, RF-24; CU-05, CU-10</remarks>
public partial class FormEntradaTexto : FormularioBase
{
    private readonly bool _esContrasena;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormEntradaTexto() => InitializeComponent();

    /// <summary>Crea el diálogo.</summary>
    /// <param name="titulo">Título de la ventana.</param>
    /// <param name="etiqueta">Texto que describe lo que se pide.</param>
    /// <param name="esContrasena">Oculta los caracteres y exige una contraseña válida.</param>
    /// <param name="textoGuardar">Texto del botón de aceptar.</param>
    public FormEntradaTexto(string titulo, string etiqueta, bool esContrasena = false, string textoGuardar = "Aceptar") : this()
    {
        _esContrasena = esContrasena;
        Text = titulo;
        lblEtiqueta.Text = etiqueta;
        TextoGuardar = textoGuardar;
        if (esContrasena)
        {
            txtValor.Multiline = false;
            txtValor.UseSystemPasswordChar = true;
            txtValor.Height = 28;
            Validar(txtValor, () => Validador.EsContrasenaValida(txtValor.Text) ? null : Validador.MensajeContrasena);
        }
        else
        {
            Validar(txtValor, () => string.IsNullOrWhiteSpace(txtValor.Text) ? "El motivo es obligatorio." : null);
        }
    }

    /// <summary>Texto escrito por el usuario.</summary>
    public string Valor => _esContrasena ? txtValor.Text : txtValor.Text.Trim();
}
