using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>Detalle de factura: líneas, totales, pagos, saldo, pago, anulación e impresión.</summary>
/// <remarks>RF-22..RF-26; CU-09, CU-10, CU-11</remarks>
public partial class FormFacturaDetalle : FormularioBase
{
    private sealed record FilaLinea(
        [property: DisplayName("Concepto")] string Concepto,
        [property: DisplayName("Cant.")] int Cantidad,
        [property: DisplayName("Precio unitario")] string Precio,
        [property: DisplayName("Subtotal")] string Subtotal);

    private sealed record FilaPago(
        [property: DisplayName("Fecha")] string Fecha,
        [property: DisplayName("Método")] string Metodo,
        [property: DisplayName("Monto")] string Monto,
        [property: DisplayName("Detalle")] string Detalle);

    private readonly ServiciosHotel? _s;
    private readonly int _facturaId;
    private Factura? _factura;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormFacturaDetalle()
    {
        InitializeComponent();
        TextoGuardar = "Cerrar";
        MostrarCancelar = false;
        Estilizador.AplicarTabla(tablaLineas);
        Estilizador.AplicarTabla(tablaPagos);
    }

    /// <summary>Muestra la factura indicada.</summary>
    public FormFacturaDetalle(ServiciosHotel servicios, int facturaId) : this()
    {
        _s = servicios;
        _facturaId = facturaId;
        btnAnular.Visible = servicios.Autenticacion.EsAdministrador;
        Cargar();
    }

    private void Cargar()
    {
        if (_s is null) return;
        _factura = _s.Facturacion.ObtenerPorId(_facturaId);
        if (_factura is null) return;
        var f = _factura;
        var reserva = _s.Reservas.ObtenerPorId(f.ReservaId);

        Text = $"Factura {f.Numero}";
        lblEncabezado.Text = $"{f.Numero}   ·   {f.FechaEmision:dd/MM/yyyy HH:mm}   ·   Reserva {reserva?.Codigo}\n" +
                             $"Cliente: {f.NombreCliente}   ·   NIT: {f.NitCliente}";
        insignia.Estado = f.Estado.ToString();

        tablaLineas.DataSource = f.Lineas.Select(l => new FilaLinea(
            l.Concepto, l.Cantidad, Constantes.FormatearMoneda(l.PrecioUnitario), Constantes.FormatearMoneda(l.Subtotal))).ToList();
        tablaPagos.DataSource = f.Pagos.Select(p => new FilaPago(
            p.Fecha.ToString("dd/MM/yyyy HH:mm"), p.Metodo, Constantes.FormatearMoneda(p.Monto), p.Detalle())).ToList();

        var c = _s.Configuracion;
        lblTotales.Text =
            $"Subtotal: {Constantes.FormatearMoneda(f.Subtotal)}\n" +
            $"Descuento ({f.PorcentajeDescuento:0.##}%): − {Constantes.FormatearMoneda(f.Descuento)}\n" +
            $"IVA ({c.PorcentajeIva:0}%): {Constantes.FormatearMoneda(f.Iva)}\n" +
            $"Impuesto de turismo ({c.PorcentajeTurismo:0}%): {Constantes.FormatearMoneda(f.ImpuestoTurismo)}\n" +
            $"TOTAL: {Constantes.FormatearMoneda(f.Total)}";
        lblSaldo.Text = f.Estado == EstadoFactura.Anulada
            ? $"ANULADA — {f.MotivoAnulacion}"
            : $"Pagado: {Constantes.FormatearMoneda(f.TotalPagado)}   ·   Saldo: {Constantes.FormatearMoneda(f.Saldo)}";
        lblSaldo.ForeColor = f.Estado == EstadoFactura.Anulada ? Paleta.Error : f.Saldo > 0 ? Paleta.Advertencia : Paleta.Teal;

        btnPago.Enabled = f.Estado == EstadoFactura.Emitida && f.Saldo > 0;
        btnAnular.Enabled = f.Estado != EstadoFactura.Anulada;
    }

    private ImpresorFactura? CrearDocumento() =>
        _factura is null || _s is null ? null
            : new ImpresorFactura(_factura, _s.Configuracion, _s.Reservas.ObtenerPorId(_factura.ReservaId)?.Codigo ?? "—");

    private void BtnPago_Click(object? sender, EventArgs e)
    {
        if (_s is null || _factura is null) return;
        using var form = new FormPago(_s, _factura);
        if (form.ShowDialog(this) == DialogResult.OK) Cargar();
    }

    private void BtnAnular_Click(object? sender, EventArgs e)
    {
        if (_s is null || _factura is null) return;
        using var form = new FormEntradaTexto($"Anular factura {_factura.Numero}",
            "Escriba el motivo de la anulación (obligatorio):", textoGuardar: "Anular factura");
        if (form.ShowDialog(this) != DialogResult.OK) return;
        if (!Mensajes.Confirmar(this, $"¿Anular la factura {_factura.Numero}? Esta acción no se puede deshacer.",
                "Anular factura", peligro: true, textoAceptar: "Anular")) return;
        Mensajes.MostrarResultado(this, _s.Facturacion.Anular(_facturaId, form.Valor));
        Cargar();
    }

    private void BtnVistaPrevia_Click(object? sender, EventArgs e)
    {
        using var documento = CrearDocumento();
        if (documento is null) return;
        using var vista = new PrintPreviewDialog
        {
            Document = documento, Width = 900, Height = 760, StartPosition = FormStartPosition.CenterParent,
            Text = $"Vista previa — {_factura!.Numero}"
        };
        vista.ShowDialog(this);
    }

    private void BtnImprimir_Click(object? sender, EventArgs e)
    {
        using var documento = CrearDocumento();
        if (documento is null) return;
        using var dialogo = new PrintDialog { Document = documento };
        if (dialogo.ShowDialog(this) == DialogResult.OK) documento.Print();
    }
}
