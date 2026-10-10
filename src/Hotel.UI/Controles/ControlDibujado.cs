using System.ComponentModel;
using Hotel.UI.Tema;

namespace Hotel.UI.Controles;

/// <summary>Base de los controles dibujados con GDI+: doble búfer y fondo del contenedor.</summary>
/// <remarks>Sección 10.3; RNF-05</remarks>
[DesignerCategory("Code")]
public abstract class ControlDibujado : Control
{
    /// <summary>Configura estilos de dibujo.</summary>
    protected ControlDibujado()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer
                 | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor, true);
        DoubleBuffered = true;
        Font = Tipografia.Texto;
    }

    /// <summary>Pinta el color del contenedor para que las esquinas redondeadas se vean limpias.</summary>
    protected override void OnPaintBackground(PaintEventArgs e) =>
        e.Graphics.Clear(BackColor.A == 255 ? BackColor : Paleta.FondoDe(this));

    /// <summary>Prepara el contexto gráfico y delega en <see cref="Dibujar"/>.</summary>
    protected sealed override void OnPaint(PaintEventArgs e)
    {
        GraficosUtil.Suavizar(e.Graphics);
        Dibujar(e.Graphics);
    }

    /// <summary>Dibuja el control.</summary>
    protected abstract void Dibujar(Graphics g);
}
