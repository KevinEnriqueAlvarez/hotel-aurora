using System.Drawing.Drawing2D;

namespace Hotel.UI.Tema;

/// <summary>Utilidades de dibujo GDI+ compartidas por los controles.</summary>
/// <remarks>Sección 10.3</remarks>
public static class GraficosUtil
{
    /// <summary>Radio de esquinas estándar (14 px).</summary>
    public const int RadioEsquinas = 14;

    /// <summary>Trazo rectangular con esquinas redondeadas.</summary>
    public static GraphicsPath Redondeado(RectangleF r, float radio)
    {
        var d = Math.Min(radio * 2, Math.Min(r.Width, r.Height));
        var path = new GraphicsPath();
        if (d <= 0)
        {
            path.AddRectangle(r);
            return path;
        }
        path.AddArc(r.X, r.Y, d, d, 180, 90);
        path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        path.CloseFigure();
        return path;
    }

    /// <summary>Activa suavizado de formas y texto.</summary>
    public static void Suavizar(Graphics g)
    {
        g.SmoothingMode = SmoothingMode.AntiAlias;
        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
    }

    /// <summary>Escala un valor en píxeles a la densidad de pantalla del control.</summary>
    public static int Px(Control control, int pixeles) => (int)Math.Round(pixeles * control.DeviceDpi / 96.0);

    /// <summary>Dibuja texto centrado en un rectángulo.</summary>
    public static void TextoCentrado(Graphics g, string texto, Font fuente, Color color, RectangleF area)
    {
        using var formato = new StringFormat
        {
            Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center,
            Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap
        };
        using var pincel = new SolidBrush(color);
        g.DrawString(texto, fuente, pincel, area, formato);
    }

    /// <summary>Dibuja texto alineado con elipsis.</summary>
    public static void Texto(Graphics g, string texto, Font fuente, Color color, RectangleF area,
        StringAlignment horizontal = StringAlignment.Near, StringAlignment vertical = StringAlignment.Center)
    {
        using var formato = new StringFormat
        {
            Alignment = horizontal, LineAlignment = vertical,
            Trimming = StringTrimming.EllipsisCharacter, FormatFlags = StringFormatFlags.NoWrap
        };
        using var pincel = new SolidBrush(color);
        g.DrawString(texto, fuente, pincel, area, formato);
    }
}
