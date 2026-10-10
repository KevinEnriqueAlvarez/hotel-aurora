using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Variante visual de <see cref="BotonModerno"/>.</summary>
public enum VarianteBoton { Primario, Secundario, Peligro, Fantasma }

/// <summary>Botón con esquinas redondeadas, estados hover/presionado, cursor de mano e ícono opcional.</summary>
/// <remarks>Sección 10.3</remarks>
[ToolboxItem(true)]
[DefaultEvent("Click")]
public class BotonModerno : ControlDibujado, IButtonControl
{
    private VarianteBoton _variante = VarianteBoton.Primario;
    private string _icono = "";
    private bool _hover;
    private bool _presionado;
    private bool _porDefecto;

    /// <summary>Crea el botón con cursor de mano y tamaño estándar.</summary>
    public BotonModerno()
    {
        Cursor = Cursors.Hand;
        Size = new Size(120, 38);
        SetStyle(ControlStyles.Selectable, true);
        TabStop = true;
        BackColor = Color.Transparent;
    }

    /// <summary>Estilo del botón.</summary>
    [Category("Appearance"), DefaultValue(VarianteBoton.Primario)]
    public VarianteBoton Variante
    {
        get => _variante;
        set { _variante = value; Invalidate(); }
    }

    /// <summary>Glifo de Segoe MDL2 Assets (por ejemplo «»).</summary>
    [Category("Appearance"), DefaultValue("")]
    public string Icono
    {
        get => _icono;
        set { _icono = value; Invalidate(); }
    }

    /// <summary>Texto del botón.</summary>
    [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    [System.Diagnostics.CodeAnalysis.AllowNull]
    public override string Text
    {
        get => base.Text;
        set { base.Text = value; Invalidate(); }
    }

    /// <inheritdoc />
    [Category("Behavior"), DefaultValue(DialogResult.None)]
    public DialogResult DialogResult { get; set; }

    /// <inheritdoc />
    public void NotifyDefault(bool value)
    {
        _porDefecto = value;
        Invalidate();
    }

    /// <inheritdoc />
    public void PerformClick()
    {
        if (Enabled && Visible) OnClick(EventArgs.Empty);
    }

    /// <summary>Cierra el diálogo padre si el botón tiene <see cref="DialogResult"/>.</summary>
    protected override void OnClick(EventArgs e)
    {
        base.OnClick(e);
        if (DialogResult != DialogResult.None && FindForm() is { Modal: true } formulario)
            formulario.DialogResult = DialogResult;
    }

    /// <inheritdoc />
    protected override void OnMouseEnter(EventArgs e) { _hover = true; Invalidate(); base.OnMouseEnter(e); }
    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e) { _hover = false; _presionado = false; Invalidate(); base.OnMouseLeave(e); }
    /// <inheritdoc />
    protected override void OnMouseDown(MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left) { _presionado = true; Focus(); Invalidate(); }
        base.OnMouseDown(e);
    }
    /// <inheritdoc />
    protected override void OnMouseUp(MouseEventArgs e) { _presionado = false; Invalidate(); base.OnMouseUp(e); }
    /// <inheritdoc />
    protected override void OnEnabledChanged(EventArgs e) { Invalidate(); base.OnEnabledChanged(e); }
    /// <inheritdoc />
    protected override void OnGotFocus(EventArgs e) { Invalidate(); base.OnGotFocus(e); }
    /// <inheritdoc />
    protected override void OnLostFocus(EventArgs e) { Invalidate(); base.OnLostFocus(e); }

    /// <inheritdoc />
    protected override void OnKeyDown(KeyEventArgs e)
    {
        if (e.KeyCode is Keys.Space or Keys.Enter) { PerformClick(); e.Handled = true; }
        base.OnKeyDown(e);
    }

    /// <inheritdoc />
    protected override bool IsInputKey(Keys keyData) => keyData is Keys.Space or Keys.Enter || base.IsInputKey(keyData);

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        var area = new RectangleF(0.5f, 0.5f, Width - 2, Height - 2);
        var (relleno, texto, borde) = Colores();
        if (!Enabled)
        {
            relleno = Paleta.Mezclar(relleno, Paleta.Fondo, 0.55);
            texto = Paleta.TextoSecundario;
        }

        using (var ruta = GraficosUtil.Redondeado(area, GraficosUtil.Px(this, 10)))
        {
            using var pincel = new SolidBrush(relleno);
            g.FillPath(pincel, ruta);
            if (borde is { } colorBorde)
            {
                using var lapiz = new Pen(colorBorde);
                g.DrawPath(lapiz, ruta);
            }
            if ((Focused && ShowFocusCues) || _porDefecto && _variante != VarianteBoton.Fantasma)
            {
                using var foco = new Pen(Color.FromArgb(Focused ? 160 : 60, Paleta.AzulPestana), 2);
                g.DrawPath(foco, ruta);
            }
        }
        DibujarContenido(g, texto);
    }

    private void DibujarContenido(Graphics g, Color color)
    {
        using var fuenteIcono = Tipografia.Icono(11f);
        var fuente = Font;
        var conIcono = !string.IsNullOrEmpty(_icono);
        var anchoIcono = conIcono ? g.MeasureString(_icono, fuenteIcono).Width : 0f;
        var anchoTexto = string.IsNullOrEmpty(Text) ? 0f : g.MeasureString(Text, fuente).Width;
        var separacion = conIcono && anchoTexto > 0 ? 8f : 0f;
        var total = anchoIcono + separacion + anchoTexto;
        var x = Math.Max(4f, (Width - total) / 2f);

        if (conIcono)
        {
            GraficosUtil.Texto(g, _icono, fuenteIcono, color, new RectangleF(x, 0, anchoIcono + 4, Height), StringAlignment.Near);
            x += anchoIcono + separacion;
        }
        if (anchoTexto > 0)
            GraficosUtil.Texto(g, Text, fuente, color, new RectangleF(x, 0, Width - x - 2, Height), StringAlignment.Near);
    }

    private (Color relleno, Color texto, Color? borde) Colores()
    {
        var oscurecer = _presionado ? 0.18 : 0;
        switch (_variante)
        {
            case VarianteBoton.Primario:
                var teal = _hover ? Paleta.TealClaro : Paleta.Teal;
                return (Paleta.Mezclar(teal, Color.Black, oscurecer), Color.White, null);
            case VarianteBoton.Peligro:
                var rojo = _hover ? Paleta.Mezclar(Paleta.Error, Color.White, 0.15) : Paleta.Error;
                return (Paleta.Mezclar(rojo, Color.Black, oscurecer), Color.White, null);
            case VarianteBoton.Secundario:
                var fondo = _presionado ? Paleta.Borde : _hover ? Paleta.SeleccionTabla : Paleta.Superficie;
                return (fondo, Paleta.TextoPrincipal, Paleta.Borde);
            default:
                var fantasma = _presionado ? Paleta.Borde : _hover ? Paleta.Tarjeta : Paleta.FondoDe(this);
                return (fantasma, Paleta.Marino, null);
        }
    }
}
