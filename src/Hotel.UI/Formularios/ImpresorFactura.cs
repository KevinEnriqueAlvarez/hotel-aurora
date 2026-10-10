using System.Drawing.Printing;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.UI.Formularios;

/// <summary>Documento de impresión de una factura (vista previa e impresión).</summary>
/// <remarks>RF-26; CU-11</remarks>
public sealed class ImpresorFactura : PrintDocument
{
    private readonly Factura _factura;
    private readonly ConfiguracionHotel _hotel;
    private readonly string _codigoReserva;

    /// <summary>Prepara el documento.</summary>
    public ImpresorFactura(Factura factura, ConfiguracionHotel hotel, string codigoReserva)
    {
        _factura = factura;
        _hotel = hotel;
        _codigoReserva = codigoReserva;
        DocumentName = $"Factura {factura.Numero}";
        DefaultPageSettings.Margins = new Margins(60, 60, 60, 60);
    }

    /// <summary>Dibuja la página (una sola página; las facturas del hotel son cortas).</summary>
    protected override void OnPrintPage(PrintPageEventArgs e)
    {
        base.OnPrintPage(e);
        var g = e.Graphics!;
        var margen = e.MarginBounds;
        using var titulo = new Font("Segoe UI", 20, FontStyle.Bold);
        using var negrita = new Font("Segoe UI", 10, FontStyle.Bold);
        using var normal = new Font("Segoe UI", 10);
        using var pequena = new Font("Segoe UI", 9);
        using var lapiz = new Pen(Color.Gray);
        float y = margen.Top;

        g.DrawString(_hotel.NombreHotel, titulo, Brushes.Black, margen.Left, y);
        y += 40;
        g.DrawString($"NIT {_hotel.Nit}  ·  {_hotel.Direccion}  ·  Tel. {_hotel.Telefono}", pequena, Brushes.DimGray, margen.Left, y);
        y += 36;
        g.DrawString($"FACTURA {_factura.Numero}", negrita, Brushes.Black, margen.Left, y);
        g.DrawString($"Fecha: {_factura.FechaEmision:dd/MM/yyyy HH:mm}", normal, Brushes.Black, margen.Right - 220, y);
        y += 24;
        g.DrawString($"Cliente: {_factura.NombreCliente}", normal, Brushes.Black, margen.Left, y);
        g.DrawString($"NIT: {_factura.NitCliente}", normal, Brushes.Black, margen.Right - 220, y);
        y += 22;
        g.DrawString($"Reserva: {_codigoReserva}", normal, Brushes.Black, margen.Left, y);
        y += 36;

        y = DibujarLineas(g, margen, y, negrita, normal, lapiz);
        y = DibujarTotales(g, margen, y + 12, negrita, normal);
        DibujarPagos(g, margen, y + 20, negrita, pequena);

        if (_factura.Estado == EstadoFactura.Anulada)
        {
            using var marca = new Font("Segoe UI", 64, FontStyle.Bold);
            using var pincel = new SolidBrush(Color.FromArgb(70, Color.Red));
            g.DrawString("ANULADA", marca, pincel, margen.Left + 60, margen.Top + 260);
        }
        e.HasMorePages = false;
    }

    private float DibujarLineas(Graphics g, Rectangle m, float y, Font negrita, Font normal, Pen lapiz)
    {
        var xCant = m.Right - 330f;
        var xPrecio = m.Right - 230f;
        var xSub = m.Right - 110f;
        g.DrawString("Concepto", negrita, Brushes.Black, m.Left, y);
        g.DrawString("Cant.", negrita, Brushes.Black, xCant, y);
        g.DrawString("Precio", negrita, Brushes.Black, xPrecio, y);
        g.DrawString("Subtotal", negrita, Brushes.Black, xSub, y);
        y += 24;
        g.DrawLine(lapiz, m.Left, y, m.Right, y);
        y += 6;
        foreach (var l in _factura.Lineas)
        {
            g.DrawString(l.Concepto, normal, Brushes.Black, new RectangleF(m.Left, y, xCant - m.Left - 8, 22));
            g.DrawString(l.Cantidad.ToString(), normal, Brushes.Black, xCant, y);
            g.DrawString(Constantes.FormatearMoneda(l.PrecioUnitario), normal, Brushes.Black, xPrecio, y);
            g.DrawString(Constantes.FormatearMoneda(l.Subtotal), normal, Brushes.Black, xSub, y);
            y += 24;
        }
        g.DrawLine(lapiz, m.Left, y, m.Right, y);
        return y;
    }

    private float DibujarTotales(Graphics g, Rectangle m, float y, Font negrita, Font normal)
    {
        void Fila(string texto, string valor, Font fuente)
        {
            g.DrawString(texto, fuente, Brushes.Black, m.Right - 330, y);
            g.DrawString(valor, fuente, Brushes.Black, m.Right - 110, y);
            y += 24;
        }
        Fila("Subtotal", Constantes.FormatearMoneda(_factura.Subtotal), normal);
        Fila($"Descuento ({_factura.PorcentajeDescuento:0.##}%)", "− " + Constantes.FormatearMoneda(_factura.Descuento), normal);
        Fila($"IVA ({_hotel.PorcentajeIva:0}%)", Constantes.FormatearMoneda(_factura.Iva), normal);
        Fila($"Impuesto de turismo ({_hotel.PorcentajeTurismo:0}%)", Constantes.FormatearMoneda(_factura.ImpuestoTurismo), normal);
        Fila("TOTAL", Constantes.FormatearMoneda(_factura.Total), negrita);
        Fila("Pagado", Constantes.FormatearMoneda(_factura.TotalPagado), normal);
        Fila("Saldo", Constantes.FormatearMoneda(_factura.Saldo), negrita);
        return y;
    }

    private void DibujarPagos(Graphics g, Rectangle m, float y, Font negrita, Font pequena)
    {
        if (_factura.Pagos.Count > 0)
        {
            g.DrawString("Pagos registrados", negrita, Brushes.Black, m.Left, y);
            y += 24;
            foreach (var p in _factura.Pagos)
            {
                g.DrawString($"{p.Fecha:dd/MM/yyyy}  {p.Metodo}  {Constantes.FormatearMoneda(p.Monto)}  — {p.Detalle()}",
                    pequena, Brushes.Black, m.Left, y);
                y += 20;
            }
        }
        if (_factura.Estado == EstadoFactura.Anulada)
            g.DrawString($"Factura anulada: {_factura.MotivoAnulacion}", pequena, Brushes.Red, m.Left, y + 10);
        g.DrawString("Comprobante interno — no es una factura electrónica FEL de la SAT.", pequena, Brushes.DimGray, m.Left, m.Bottom - 20);
    }
}
