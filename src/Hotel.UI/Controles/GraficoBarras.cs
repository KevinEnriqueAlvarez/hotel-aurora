using System.ComponentModel;
using System.Drawing.Drawing2D;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Barra de datos del gráfico (la destacada lleva degradado y valor encima).</summary>
/// <remarks>E1</remarks>
public record PuntoBarra(string Etiqueta, decimal Valor, bool Destacada);

/// <summary>Gráfico de barras: marino por defecto y degradado Teal → TealClaro para las destacadas.</summary>
/// <remarks>E1; sección 11</remarks>
[ToolboxItem(true)]
public class GraficoBarras : ControlDibujado
{
    private IReadOnlyList<PuntoBarra> _datos = [];

    /// <summary>Crea el gráfico.</summary>
    public GraficoBarras()
    {
        Size = new Size(600, 260);
        BackColor = Color.Transparent;
    }

    /// <summary>Puntos a graficar.</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public IReadOnlyList<PuntoBarra> Datos
    {
        get => _datos;
        set { _datos = value ?? []; Invalidate(); }
    }

    /// <summary>Formato de los valores del eje y de las etiquetas (por defecto abreviado: 4.1k).</summary>
    [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Func<decimal, string> FormatoValor { get; set; } = FormatoCorto;

    /// <summary>Abrevia miles y millones.</summary>
    public static string FormatoCorto(decimal v) => v switch
    {
        >= 1_000_000m => $"{v / 1_000_000m:0.#}M",
        >= 1_000m => $"{v / 1_000m:0.#}k",
        _ => $"{v:0.#}"
    };

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        const int izquierda = 46, derecha = 8, arriba = 26, abajo = 40;
        var area = new RectangleF(izquierda, arriba, Width - izquierda - derecha, Height - arriba - abajo);
        if (area.Width <= 0 || area.Height <= 0) return;

        var maximo = _datos.Count == 0 ? 0 : _datos.Max(d => d.Valor);
        var paso = PasoAgradable(maximo / 4m);
        var tope = Math.Max(paso * 4, paso);
        DibujarEjeY(g, area, paso, tope);

        if (_datos.Count == 0) return;
        var ranura = area.Width / _datos.Count;
        var anchoBarra = Math.Min(ranura * 0.46f, 34f);
        for (var i = 0; i < _datos.Count; i++)
        {
            var d = _datos[i];
            var altura = tope == 0 ? 0 : (float)(d.Valor / tope) * area.Height;
            var x = area.X + i * ranura + (ranura - anchoBarra) / 2;
            var barra = new RectangleF(x, area.Bottom - altura, anchoBarra, Math.Max(altura, 0));
            DibujarBarra(g, barra, d.Destacada);
            if (d.Destacada && d.Valor > 0)
                GraficosUtil.TextoCentrado(g, "Q" + FormatoValor(d.Valor), Tipografia.EtiquetaPequena, Paleta.Marino,
                    new RectangleF(x - 14, barra.Top - 22, anchoBarra + 28, 20));
            DibujarEtiquetaX(g, d, new RectangleF(area.X + i * ranura + 2, area.Bottom + 10, ranura - 4, 22));
        }
    }

    private void DibujarEjeY(Graphics g, RectangleF area, decimal paso, decimal tope)
    {
        using var rejilla = new Pen(Paleta.Borde) { DashStyle = DashStyle.Dash };
        for (var i = 0; i <= 4; i++)
        {
            var y = area.Bottom - area.Height * i / 4f;
            g.DrawLine(rejilla, area.X, y, area.Right, y);
            GraficosUtil.Texto(g, FormatoValor(paso * i), Tipografia.Pequena, Paleta.TextoSecundario,
                new RectangleF(0, y - 10, area.X - 6, 20), StringAlignment.Far);
        }
    }

    private static void DibujarBarra(Graphics g, RectangleF barra, bool destacada)
    {
        if (barra.Height < 1) return;
        var radio = Math.Min(7f, barra.Width / 2);
        using var ruta = new GraphicsPath();
        ruta.AddArc(barra.X, barra.Y, radio * 2, radio * 2, 180, 90);
        ruta.AddArc(barra.Right - radio * 2, barra.Y, radio * 2, radio * 2, 270, 90);
        ruta.AddLine(barra.Right, barra.Bottom, barra.X, barra.Bottom);
        ruta.CloseFigure();
        if (destacada)
        {
            using var degradado = new LinearGradientBrush(
                new RectangleF(barra.X, barra.Y - 1, barra.Width, barra.Height + 2), Paleta.TealClaro, Paleta.Teal, 90f);
            g.FillPath(degradado, ruta);
        }
        else
        {
            using var pincel = new SolidBrush(Paleta.Marino);
            g.FillPath(pincel, ruta);
        }
    }

    private static void DibujarEtiquetaX(Graphics g, PuntoBarra d, RectangleF caja)
    {
        var ancho = Math.Min(caja.Width, 46f);
        var rect = new RectangleF(caja.X + (caja.Width - ancho) / 2, caja.Y, ancho, caja.Height);
        using var ruta = GraficosUtil.Redondeado(rect, 6);
        using var relleno = new SolidBrush(d.Destacada ? Paleta.AzulPestana : Paleta.Superficie);
        using var borde = new Pen(d.Destacada ? Paleta.AzulPestana : Paleta.Borde);
        g.FillPath(relleno, ruta);
        g.DrawPath(borde, ruta);
        GraficosUtil.TextoCentrado(g, d.Etiqueta, Tipografia.Pequena, d.Destacada ? Color.White : Paleta.TextoSecundario, rect);
    }

    private static decimal PasoAgradable(decimal bruto)
    {
        if (bruto <= 0) return 1;
        var exponente = (decimal)Math.Pow(10, Math.Floor(Math.Log10((double)bruto)));
        var fraccion = bruto / exponente;
        var nice = fraccion <= 1 ? 1 : fraccion <= 2 ? 2 : fraccion <= 2.5m ? 2.5m : fraccion <= 5 ? 5 : 10;
        return nice * exponente;
    }
}
