using System.ComponentModel;
using System.Drawing.Drawing2D;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Panel con esquinas redondeadas y sombra difusa dibujada dentro de sus límites.</summary>
/// <remarks>Sección 10.3; RNF-05</remarks>
[ToolboxItem(true)]
[DesignerCategory("Code")]
public class PanelTarjeta : Panel
{
    /// <summary>Margen interno reservado para la sombra.</summary>
    public const int MargenSombra = 6;
    private const int CapasSombra = 6;
    private const int AlfaPorCapa = 8;

    /// <summary>Crea la tarjeta con el relleno y el padding estándar (6 px de sombra + 16 px).</summary>
    public PanelTarjeta()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.ResizeRedraw, true);
        DoubleBuffered = true;
        Padding = new Padding(MargenSombra + 16);
        BackColor = Paleta.Tarjeta;
    }

    /// <summary>Color de relleno de la tarjeta.</summary>
    [Category("Appearance"), DefaultValue(typeof(Color), "244, 247, 250")]
    public Color ColorTarjeta
    {
        get => BackColor;
        set => BackColor = value;
    }

    /// <summary>Radio de las esquinas.</summary>
    [Category("Appearance"), DefaultValue(GraficosUtil.RadioEsquinas)]
    public int Radio { get; set; } = GraficosUtil.RadioEsquinas;

    /// <summary>Dibuja el fondo del contenedor, la sombra y la tarjeta.</summary>
    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(Paleta.FondoDe(this));
        GraficosUtil.Suavizar(g);
        DibujarTarjeta(g, new Rectangle(0, 0, Width, Height), BackColor, Radio);
    }

    /// <summary>Evita el parpadeo: el fondo se pinta en <see cref="OnPaint"/>.</summary>
    protected override void OnPaintBackground(PaintEventArgs e) { }

    /// <summary>Dibuja una tarjeta con sombra difusa contenida en <paramref name="limites"/>.</summary>
    public static void DibujarTarjeta(Graphics g, Rectangle limites, Color relleno, int radio = GraficosUtil.RadioEsquinas)
    {
        var tarjeta = new Rectangle(limites.X + MargenSombra, limites.Y + MargenSombra,
            limites.Width - 2 * MargenSombra, limites.Height - 2 * MargenSombra - 2);
        if (tarjeta.Width <= 0 || tarjeta.Height <= 0) return;

        for (var i = CapasSombra; i >= 1; i--)
        {
            var capa = Rectangle.Inflate(tarjeta, i, i);
            capa.Offset(0, 1);
            using var ruta = GraficosUtil.Redondeado(capa, radio + i);
            using var pincel = new SolidBrush(Paleta.Sombra(AlfaPorCapa));
            g.FillPath(pincel, ruta);
        }
        using var rutaTarjeta = GraficosUtil.Redondeado(tarjeta, radio);
        using var relleno2 = new SolidBrush(relleno);
        g.FillPath(relleno2, rutaTarjeta);
        using var borde = new Pen(Color.FromArgb(120, Color.White));
        g.DrawPath(borde, rutaTarjeta);
    }
}
