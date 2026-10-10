using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Insignia tipo «pill» con el color del estado (habitación, reserva o factura).</summary>
/// <remarks>Sección 10.2 y 10.3; RF-06, RF-18, RF-25</remarks>
[ToolboxItem(true)]
public class InsigniaEstado : ControlDibujado
{
    private string _estado = "";

    /// <summary>Crea la insignia con altura estándar.</summary>
    public InsigniaEstado()
    {
        Size = new Size(110, 26);
        BackColor = Color.Transparent;
    }

    /// <summary>Nombre del estado (p. ej. «Disponible», «EnCurso», «Pagada»).</summary>
    [Category("Appearance"), DefaultValue("")]
    public string Estado
    {
        get => _estado;
        set { _estado = value ?? ""; Invalidate(); }
    }

    /// <inheritdoc />
    protected override void Dibujar(Graphics g) =>
        Pintar(g, new Rectangle(0, 0, Width - 1, Height - 1), _estado);

    /// <summary>Dibuja una insignia en el rectángulo dado; se reutiliza en las celdas de las tablas.</summary>
    public static void Pintar(Graphics g, Rectangle area, string estado)
    {
        if (string.IsNullOrEmpty(estado)) return;
        var color = Paleta.ColorDeEstado(estado);
        var texto = Paleta.TextoDeEstado(estado);
        using var fuente = new Font(Tipografia.EtiquetaPequena, FontStyle.Bold);
        var ancho = (int)g.MeasureString(texto, fuente).Width + 22;
        var pill = new Rectangle(area.X + 2, area.Y + Math.Max(0, (area.Height - 24) / 2), Math.Min(ancho, area.Width - 4), 24);
        using var ruta = GraficosUtil.Redondeado(pill, pill.Height / 2f);
        using var relleno = new SolidBrush(Color.FromArgb(32, color));
        g.FillPath(relleno, ruta);
        GraficosUtil.TextoCentrado(g, texto, fuente, color, pill);
    }
}
