using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Barra de progreso con etiqueta y porcentaje (p. ej. «Cobrado» vs «Pendiente»).</summary>
/// <remarks>E1; sección 11</remarks>
[ToolboxItem(true)]
public class BarraProgreso : ControlDibujado
{
    private string _etiqueta = "Progreso";
    private decimal _valor;
    private string _textoValor = "";
    private Color _colorBarra = Paleta.Teal;

    /// <summary>Crea la barra.</summary>
    public BarraProgreso()
    {
        Size = new Size(300, 44);
        BackColor = Color.Transparent;
    }

    /// <summary>Etiqueta a la izquierda.</summary>
    [Category("Appearance")] public string Etiqueta { get => _etiqueta; set { _etiqueta = value; Invalidate(); } }

    /// <summary>Porcentaje de llenado (0–100).</summary>
    [Category("Appearance"), DefaultValue(typeof(decimal), "0")]
    public decimal Valor
    {
        get => _valor;
        set { _valor = Math.Clamp(value, 0m, 100m); Invalidate(); }
    }

    /// <summary>Texto a la derecha; si está vacío se muestra el porcentaje.</summary>
    [Category("Appearance"), DefaultValue("")] public string TextoValor { get => _textoValor; set { _textoValor = value; Invalidate(); } }

    /// <summary>Color del relleno.</summary>
    [Category("Appearance")] public Color ColorBarra { get => _colorBarra; set { _colorBarra = value; Invalidate(); } }

    /// <inheritdoc />
    protected override void Dibujar(Graphics g)
    {
        var texto = string.IsNullOrEmpty(_textoValor) ? $"{_valor:0}%" : _textoValor;
        GraficosUtil.Texto(g, _etiqueta, Tipografia.EtiquetaPequena, Paleta.TextoSecundario, new RectangleF(0, 0, Width * 0.6f, 22));
        GraficosUtil.Texto(g, texto, Tipografia.EtiquetaPequena, Paleta.Marino,
            new RectangleF(Width * 0.4f, 0, Width * 0.6f, 22), StringAlignment.Far);

        var pista = new RectangleF(0, 28, Width - 1, 8);
        using (var ruta = GraficosUtil.Redondeado(pista, 4))
        using (var fondo = new SolidBrush(Paleta.Borde))
            g.FillPath(fondo, ruta);

        var ancho = pista.Width * (float)_valor / 100f;
        if (ancho < 1) return;
        var relleno = new RectangleF(pista.X, pista.Y, Math.Max(ancho, 8), pista.Height);
        using var rutaRelleno = GraficosUtil.Redondeado(relleno, 4);
        using var pincel = new SolidBrush(_colorBarra);
        g.FillPath(pincel, rutaRelleno);
    }
}
