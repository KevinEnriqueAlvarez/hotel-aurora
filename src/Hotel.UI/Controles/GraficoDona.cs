using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Segmento de la dona.</summary>
/// <remarks>E1</remarks>
public record SegmentoDona(string Nombre, decimal Valor, Color Color);

/// <summary>Gráfico de dona con total al centro y leyenda con conteo y porcentaje.</summary>
/// <remarks>E1; sección 11</remarks>
[ToolboxItem(true)]
public class GraficoDona : ControlDibujado
{
    private IReadOnlyList<SegmentoDona> _segmentos = [];
    private string _textoCentral = "";
    private string _subtituloCentral = "";

    /// <summary>Crea el gráfico.</summary>
    public GraficoDona()
    {
        Size = new Size(520, 220);
        BackColor = Color.Transparent;
    }

    /// <summary>Segmentos a graficar.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<SegmentoDona> Segmentos
    {
        get => _segmentos;
        set { _segmentos = value ?? []; Invalidate(); }
    }

    /// <summary>Texto grande del centro (p. ej. el total).</summary>
    [Category("Appearance"), DefaultValue("")]
    public string TextoCentral
    {
        get => _textoCentral;
        set { _textoCentral = value; Invalidate(); }
    }

    /// <summary>Texto pequeño bajo el central.</summary>
    [Category("Appearance"), DefaultValue("")]
    public string SubtituloCentral
    {
        get => _subtituloCentral;
        set { _subtituloCentral = value; Invalidate(); }
    }

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        var lado = Math.Min(Height - 8, (int)(Width * 0.46));
        if (lado < 40) return;
        var dona = new Rectangle(4, (Height - lado) / 2, lado, lado);
        var grosor = lado * 0.2f;
        var total = _segmentos.Sum(s => s.Valor);

        DibujarAnillo(g, dona, grosor, total);
        GraficosUtil.TextoCentrado(g, _textoCentral, Tipografia.TituloTarjeta, Paleta.Marino,
            new RectangleF(dona.X, dona.Y + lado / 2f - 18, lado, 26));
        GraficosUtil.TextoCentrado(g, _subtituloCentral, Tipografia.Pequena, Paleta.TextoSecundario,
            new RectangleF(dona.X, dona.Y + lado / 2f + 6, lado, 20));
        DibujarLeyenda(g, new Rectangle(dona.Right + 24, 0, Width - dona.Right - 28, Height), total);
    }

    private void DibujarAnillo(Graphics g, Rectangle dona, float grosor, decimal total)
    {
        var radio = new RectangleF(dona.X + grosor / 2, dona.Y + grosor / 2, dona.Width - grosor, dona.Height - grosor);
        if (total <= 0)
        {
            using var vacio = new Pen(Paleta.Borde, grosor);
            g.DrawEllipse(vacio, radio);
            return;
        }
        var inicio = -90f;
        foreach (var s in _segmentos.Where(s => s.Valor > 0))
        {
            var barrido = (float)(s.Valor / total) * 360f;
            using var lapiz = new Pen(s.Color, grosor);
            var hueco = _segmentos.Count(x => x.Valor > 0) > 1 ? 2f : 0f;
            g.DrawArc(lapiz, radio, inicio + hueco / 2, Math.Max(0.5f, barrido - hueco));
            inicio += barrido;
        }
    }

    private void DibujarLeyenda(Graphics g, Rectangle area, decimal total)
    {
        if (area.Width < 120 || _segmentos.Count == 0) return;
        const int alto = 30;
        var y = area.Y + (area.Height - alto * _segmentos.Count) / 2f;
        foreach (var s in _segmentos)
        {
            using (var punto = new SolidBrush(s.Color)) g.FillEllipse(punto, area.X, y + alto / 2f - 5, 10, 10);
            var pct = total <= 0 ? 0 : s.Valor / total * 100m;
            var x = area.X + 20;
            GraficosUtil.Texto(g, s.Nombre, Tipografia.Texto, Paleta.TextoPrincipal, new RectangleF(x, y, area.Width - 120, alto));
            GraficosUtil.Texto(g, $"{s.Valor:0}", Tipografia.EtiquetaPequena, Paleta.Marino,
                new RectangleF(area.Right - 100, y, 36, alto), StringAlignment.Far);
            GraficosUtil.Texto(g, $"{pct:0}%", Tipografia.Pequena, Paleta.TextoSecundario,
                new RectangleF(area.Right - 60, y, 56, alto), StringAlignment.Far);
            y += alto;
        }
    }
}
