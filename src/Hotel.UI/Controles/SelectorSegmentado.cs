using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Selector tipo pestañas: el segmento activo se pinta en azul claro.</summary>
/// <remarks>E1; sección 10.1</remarks>
[ToolboxItem(true)]
[DefaultEvent("IndiceCambiado")]
public class SelectorSegmentado : ControlDibujado
{
    private string[] _opciones = ["Opción 1", "Opción 2"];
    private int _indice;
    private int _hover = -1;

    /// <summary>Crea el selector.</summary>
    public SelectorSegmentado()
    {
        Size = new Size(300, 36);
        BackColor = Color.Transparent;
        Cursor = Cursors.Hand;
    }

    /// <summary>Se produce al elegir otro segmento.</summary>
    public event EventHandler? IndiceCambiado;

    /// <summary>Textos de los segmentos.</summary>
    [Category("Data")]
    public string[] Opciones
    {
        get => _opciones;
        set { _opciones = value ?? []; Invalidate(); }
    }

    /// <summary>Segmento activo.</summary>
    [Category("Data"), DefaultValue(0)]
    public int IndiceSeleccionado
    {
        get => _indice;
        set
        {
            if (value == _indice) return;
            _indice = value;
            Invalidate();
            IndiceCambiado?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc />
    protected override void OnMouseMove(MouseEventArgs e)
    {
        var i = IndiceEn(e.Location);
        if (i != _hover) { _hover = i; Invalidate(); }
        base.OnMouseMove(e);
    }

    /// <inheritdoc />
    protected override void OnMouseLeave(EventArgs e) { _hover = -1; Invalidate(); base.OnMouseLeave(e); }

    /// <inheritdoc />
    protected override void OnMouseClick(MouseEventArgs e)
    {
        var i = IndiceEn(e.Location);
        if (i >= 0) IndiceSeleccionado = i;
        base.OnMouseClick(e);
    }

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        if (_opciones.Length == 0) return;
        var marco = new RectangleF(0.5f, 0.5f, Width - 2, Height - 2);
        using (var ruta = GraficosUtil.Redondeado(marco, 10))
        {
            using var fondo = new SolidBrush(Paleta.Superficie);
            using var borde = new Pen(Paleta.Borde);
            g.FillPath(fondo, ruta);
            g.DrawPath(borde, ruta);
        }
        var ancho = marco.Width / _opciones.Length;
        for (var i = 0; i < _opciones.Length; i++)
        {
            var seg = new RectangleF(marco.X + i * ancho + 3, marco.Y + 3, ancho - 6, marco.Height - 6);
            var activo = i == _indice;
            if (activo)
            {
                using var ruta = GraficosUtil.Redondeado(seg, 8);
                using var pincel = new SolidBrush(Paleta.AzulPestana);
                g.FillPath(pincel, ruta);
            }
            var color = activo ? Color.White : (i == _hover ? Paleta.Marino : Paleta.TextoSecundario);
            GraficosUtil.TextoCentrado(g, _opciones[i], Tipografia.EtiquetaPequena, color, seg);
        }
    }

    private int IndiceEn(Point p)
    {
        if (_opciones.Length == 0 || p.X < 0 || p.X >= Width) return -1;
        return Math.Min(_opciones.Length - 1, p.X * _opciones.Length / Math.Max(1, Width));
    }
}
