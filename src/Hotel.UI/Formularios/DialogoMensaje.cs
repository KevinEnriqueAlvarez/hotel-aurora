using Hotel.UI.Controles;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>Diálogo de mensajes con el tema de la aplicación.</summary>
/// <remarks>Sección 10.6; RNF-09</remarks>
public partial class DialogoMensaje : Form
{
    private TipoMensaje _tipo = TipoMensaje.Informacion;
    private Color _color = Paleta.Azul;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public DialogoMensaje()
    {
        InitializeComponent();
        BackColor = Paleta.Tarjeta;
        Font = Tipografia.Texto;
        lblTitulo.Font = Tipografia.TituloTarjeta;
        lblTitulo.ForeColor = Paleta.Marino;
        lblMensaje.ForeColor = Paleta.TextoPrincipal;
        lblIcono.Font = Tipografia.Icono(20f);
    }

    /// <summary>Crea el diálogo con el contenido indicado.</summary>
    /// <param name="tipo">Tipo (determina ícono y color).</param>
    /// <param name="titulo">Título.</param>
    /// <param name="mensaje">Texto del mensaje.</param>
    /// <param name="peligro">En confirmaciones, pinta el botón de aceptar como acción destructiva.</param>
    /// <param name="textoAceptar">Texto del botón de aceptar en confirmaciones.</param>
    public DialogoMensaje(TipoMensaje tipo, string titulo, string mensaje, bool peligro = false, string textoAceptar = "Confirmar")
        : this()
    {
        _tipo = tipo;
        Text = titulo;
        lblTitulo.Text = titulo;
        lblMensaje.Text = mensaje;
        (_color, lblIcono.Text) = tipo switch
        {
            TipoMensaje.Exito => (Paleta.Teal, ""),
            TipoMensaje.Advertencia => (Paleta.Advertencia, ""),
            TipoMensaje.Error => (Paleta.Error, ""),
            TipoMensaje.Confirmacion => (peligro ? Paleta.Error : Paleta.Azul, ""),
            _ => (Paleta.Azul, "")
        };
        lblIcono.ForeColor = Color.White;

        if (tipo == TipoMensaje.Confirmacion)
        {
            btnAceptar.Text = textoAceptar;
            btnAceptar.DialogResult = DialogResult.OK;
            btnAceptar.Variante = peligro ? VarianteBoton.Peligro : VarianteBoton.Primario;
            btnCancelar.Visible = true;
        }
        else
        {
            btnAceptar.Text = "Aceptar";
            btnCancelar.Visible = false;
            CancelButton = btnAceptar;
        }
        AjustarAltura();
    }

    /// <summary>Tipo de mensaje mostrado.</summary>
    public TipoMensaje Tipo => _tipo;

    /// <summary>Dibuja el borde y el círculo del ícono.</summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        GraficosUtil.Suavizar(e.Graphics);
        using var borde = new Pen(Paleta.Borde);
        e.Graphics.DrawRectangle(borde, 0, 0, Width - 1, Height - 1);
        using var relleno = new SolidBrush(_color);
        e.Graphics.FillEllipse(relleno, lblIcono.Bounds);
    }

    private void AjustarAltura()
    {
        var alto = Math.Max(lblIcono.Bottom, lblMensaje.Bottom) + 24;
        pnlBotones.Top = alto;
        ClientSize = new Size(ClientSize.Width, alto + pnlBotones.Height);
    }
}
