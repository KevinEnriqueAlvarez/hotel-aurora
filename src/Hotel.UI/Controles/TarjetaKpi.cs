using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Tarjeta de indicador: ícono, título, valor destacado y subtítulo.</summary>
/// <remarks>E1; sección 11</remarks>
[ToolboxItem(true)]
public class TarjetaKpi : ControlDibujado
{
    private string _titulo = "Indicador";
    private string _valor = "0";
    private string _subtitulo = "";
    private string _icono = "";
    private Color _acento = Paleta.Teal;

    /// <summary>Crea la tarjeta.</summary>
    public TarjetaKpi()
    {
        Size = new Size(250, 120);
        BackColor = Color.Transparent;
    }

    /// <summary>Título (etiqueta).</summary>
    [Category("Appearance")] public string Titulo { get => _titulo; set { _titulo = value; Invalidate(); } }
    /// <summary>Valor destacado.</summary>
    [Category("Appearance")] public string Valor { get => _valor; set { _valor = value; Invalidate(); } }
    /// <summary>Texto secundario.</summary>
    [Category("Appearance"), DefaultValue("")] public string Subtitulo { get => _subtitulo; set { _subtitulo = value; Invalidate(); } }
    /// <summary>Glifo de Segoe MDL2 Assets.</summary>
    [Category("Appearance")] public string Icono { get => _icono; set { _icono = value; Invalidate(); } }
    /// <summary>Color del ícono.</summary>
    [Category("Appearance")] public Color Acento { get => _acento; set { _acento = value; Invalidate(); } }

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        PanelTarjeta.DibujarTarjeta(g, new Rectangle(0, 0, Width, Height), Paleta.Tarjeta);
        var x = PanelTarjeta.MargenSombra + 16;
        var circulo = new Rectangle(x, PanelTarjeta.MargenSombra + 14, 40, 40);
        using (var fondo = new SolidBrush(Color.FromArgb(36, _acento))) g.FillEllipse(fondo, circulo);
        using (var fuente = Tipografia.Icono(14f))
            GraficosUtil.TextoCentrado(g, _icono, fuente, _acento, circulo);

        var textoX = circulo.Right + 12;
        var ancho = Width - textoX - PanelTarjeta.MargenSombra - 8;
        GraficosUtil.Texto(g, _titulo, Tipografia.EtiquetaPequena, Paleta.TextoSecundario,
            new RectangleF(textoX, circulo.Y - 2, ancho, 24));
        GraficosUtil.Texto(g, _valor, Tipografia.Valor, Paleta.Marino,
            new RectangleF(textoX, circulo.Y + 20, ancho, 40));
        GraficosUtil.Texto(g, _subtitulo, Tipografia.Pequena, Paleta.TextoSecundario,
            new RectangleF(x, Height - PanelTarjeta.MargenSombra - 38, Width - x - 20, 24));
    }
}
