using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Botón de la barra lateral: ícono + texto; el activo lleva fondo MarinoHover e indicador teal.</summary>
/// <remarks>Sección 10.4</remarks>
[ToolboxItem(true)]
public class BotonNavegacion : ControlDibujado
{
    private bool _activo;
    private bool _hover;
    private string _icono = "";

    /// <summary>Crea el botón de navegación.</summary>
    public BotonNavegacion()
    {
        Size = new Size(220, 46);
        Cursor = Cursors.Hand;
        BackColor = Paleta.Marino;
        ForeColor = Color.White;
        SetStyle(ControlStyles.Selectable, true);
        TabStop = true;
    }

    /// <summary>Glifo de Segoe MDL2 Assets.</summary>
    [Category("Appearance")]
    public string Icono
    {
        get => _icono;
        set { _icono = value; Invalidate(); }
    }

    /// <summary>True si es la vista activa.</summary>
    [Category("Appearance"), DefaultValue(false)]
    public bool Activo
    {
        get => _activo;
        set { _activo = value; Invalidate(); }
    }

    /// <summary>Texto del botón.</summary>
    [Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [System.Diagnostics.CodeAnalysis.AllowNull]
    public override string Text
    {
        get => base.Text;
        set { base.Text = value; Invalidate(); }
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e) { _hover = false; Invalidate(); base.OnMouseLeave(e); }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Enter) { OnClick(EventArgs.Empty); e.Handled = true; }
        base.OnKeyDown(e);
    }

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        var fondo = _activo || _hover ? Paleta.MarinoHover : Paleta.Marino;
        using (var pincel = new SolidBrush(fondo)) g.FillRectangle(pincel, ClientRectangle);
        if (_activo)
        {
            using var indicador = new SolidBrush(Paleta.Teal);
            g.FillRectangle(indicador, 0, 6, 4, Height - 12);
        }
        var color = _activo ? Color.White : Color.FromArgb(210, 220, 235);
        using var iconos = Tipografia.Icono(13f);
        GraficosUtil.Texto(g, _icono, iconos, _activo ? Paleta.TealClaro : color, new RectangleF(22, 0, 30, Height), StringAlignment.Center);
        GraficosUtil.Texto(g, Text, Tipografia.Etiqueta, color, new RectangleF(60, 0, Width - 66, Height));
    }
}
