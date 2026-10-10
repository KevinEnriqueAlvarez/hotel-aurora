using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.UI.Formularios;

/// <summary>Registro de un pago; los campos cambian según el método elegido.</summary>
/// <remarks>RF-23, RN-12; CU-09</remarks>
public partial class FormPago : FormularioBase
{
    private static readonly string[] Metodos = ["Efectivo", "Tarjeta", "Transferencia"];

    private readonly ServiciosHotel? _s;
    private readonly Factura? _factura;

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormPago()
    {
        InitializeComponent();
        cmbMetodo.DataSource = Metodos;
    }

    /// <summary>Crea el diálogo para la factura indicada.</summary>
    public FormPago(ServiciosHotel servicios, Factura factura) : this()
    {
        _s = servicios;
        _factura = factura;
        Text = $"Registrar pago — {factura.Numero}";
        TextoGuardar = "Registrar pago";
        lblSaldo.Text = $"Total {Constantes.FormatearMoneda(factura.Total)} · Pagado {Constantes.FormatearMoneda(factura.TotalPagado)} · " +
                        $"Saldo {Constantes.FormatearMoneda(factura.Saldo)}";
        numMonto.Maximum = Math.Max(0.01m, factura.Saldo);
        numMonto.Value = Math.Max(0.01m, factura.Saldo);
        numRecibido.Value = Math.Min(numRecibido.Maximum, Math.Ceiling(factura.Saldo));

        Validar(numMonto, () => numMonto.Value <= 0 || numMonto.Value > factura.Saldo ? "El monto debe ser mayor que cero y no exceder el saldo." : null);
        Validar(numRecibido, () => Metodo == "Efectivo" && numRecibido.Value < numMonto.Value ? "El monto recibido no puede ser menor al monto." : null);
        Validar(txtDigitos, () => Metodo == "Tarjeta" && !(txtDigitos.Text.Length == 4 && txtDigitos.Text.All(char.IsDigit)) ? "Escriba exactamente 4 dígitos." : null);
        Validar(txtAutorizacion, () => Metodo == "Tarjeta" && string.IsNullOrWhiteSpace(txtAutorizacion.Text) ? "La autorización es obligatoria." : null);
        Validar(txtBanco, () => Metodo == "Transferencia" && string.IsNullOrWhiteSpace(txtBanco.Text) ? "El banco es obligatorio." : null);
        Validar(txtReferencia, () => Metodo == "Transferencia" && string.IsNullOrWhiteSpace(txtReferencia.Text) ? "La referencia es obligatoria." : null);
        MostrarCampos();
    }

    private string Metodo => cmbMetodo.SelectedItem as string ?? Metodos[0];

    private void MostrarCampos()
    {
        pnlEfectivo.Visible = Metodo == "Efectivo";
        pnlTarjeta.Visible = Metodo == "Tarjeta";
        pnlTransferencia.Visible = Metodo == "Transferencia";
        lblCambio.Text = $"Cambio a devolver: {Constantes.FormatearMoneda(Math.Max(0, numRecibido.Value - numMonto.Value))}";
    }

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null || _factura is null) return false;
        Pago pago = Metodo switch
        {
            "Tarjeta" => new PagoTarjeta
            {
                Monto = numMonto.Value, UltimosCuatroDigitos = txtDigitos.Text.Trim(), NumeroAutorizacion = txtAutorizacion.Text.Trim()
            },
            "Transferencia" => new PagoTransferencia
            {
                Monto = numMonto.Value, Banco = txtBanco.Text.Trim(), NumeroReferencia = txtReferencia.Text.Trim()
            },
            _ => new PagoEfectivo { Monto = numMonto.Value, MontoRecibido = numRecibido.Value }
        };
        return Procesar(_s.Facturacion.RegistrarPago(_factura.Id, pago));
    }

    private void Metodo_Cambiado(object? sender, EventArgs e) => MostrarCampos();
}
