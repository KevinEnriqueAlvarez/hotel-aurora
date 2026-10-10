using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Controles;
using Hotel.UI.Formularios;

namespace Hotel.UI.Vistas;

/// <summary>Historial de facturación con filtros y totales del periodo.</summary>
/// <remarks>RF-22..RF-26; CU-09, CU-10, CU-11</remarks>
public class VistaFacturacion : VistaListadoBase
{
    private const string Todos = "Todos";

    private sealed record Fila(
        int Id,
        [property: DisplayName("Factura")] string Numero,
        [property: DisplayName("Fecha")] string Fecha,
        [property: DisplayName("Cliente")] string Cliente,
        [property: DisplayName("NIT")] string Nit,
        [property: DisplayName("Total")] string Total,
        [property: DisplayName("Pagado")] string Pagado,
        [property: DisplayName("Saldo")] string Saldo,
        [property: DisplayName("Estado")] string Estado) : IFilaConId;

    private readonly ServiciosHotel? _s;
    private readonly ComboBox _cmbEstado = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 130 };
    private readonly DateTimePicker _dtpDesde = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 130 };
    private readonly DateTimePicker _dtpHasta = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 130 };
    private readonly BotonModerno _btnPago = new() { Text = "Registrar pago", Icono = "", Variante = VarianteBoton.Secundario, Width = 180, Height = 40 };
    private readonly BotonModerno _btnImprimir = new() { Text = "Vista previa", Icono = "", Variante = VarianteBoton.Secundario, Width = 160, Height = 40 };

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaFacturacion() { }

    /// <summary>Crea la vista con los servicios de la aplicación.</summary>
    public VistaFacturacion(ServiciosHotel servicios)
    {
        _s = servicios;
        AyudaBusqueda = "Número, cliente o NIT...";
        PermiteNuevo = false;
        PermiteEditar = false;
        PermiteEliminar = servicios.Autenticacion.EsAdministrador;
        TextoEliminar = "Anular";
        PermiteDetalle = true;

        _cmbEstado.Items.Add(Todos);
        _cmbEstado.Items.AddRange(Enum.GetNames<EstadoFactura>());
        _cmbEstado.SelectedIndex = 0;
        AgregarFiltro("Estado", _cmbEstado);
        AgregarFiltro("Desde", _dtpDesde);
        AgregarFiltro("Hasta", _dtpHasta);

        _btnPago.Click += (_, _) => RegistrarPago();
        _btnImprimir.Click += (_, _) => VerDetalle();
        AgregarAccion(_btnPago);
        AgregarAccion(_btnImprimir);

        _cmbEstado.SelectedIndexChanged += (_, _) => CargarDatos();
        _dtpDesde.ValueChanged += (_, _) => CargarDatos();
        _dtpHasta.ValueChanged += (_, _) => CargarDatos();
    }

    /// <inheritdoc />
    public override string TituloVista => "Facturación";

    /// <inheritdoc />
    protected override void CargarDatos()
    {
        if (_s is null) return;
        var filtro = new FiltroFactura(
            Desde: _dtpDesde.Checked ? _dtpDesde.Value.Date : null,
            Hasta: _dtpHasta.Checked ? _dtpHasta.Value.Date : null,
            Estado: _cmbEstado.SelectedIndex > 0 ? Enum.Parse<EstadoFactura>((string)_cmbEstado.SelectedItem!) : null,
            Texto: TextoBusqueda);

        var facturas = _s.Facturacion.Buscar(filtro);
        MostrarFilas(facturas.Select(f => new Fila(f.Id, f.Numero, f.FechaEmision.ToString(Constantes.FormatoFecha),
            f.NombreCliente, f.NitCliente, Constantes.FormatearMoneda(f.Total), Constantes.FormatearMoneda(f.TotalPagado),
            Constantes.FormatearMoneda(f.Saldo), f.Estado.ToString())).ToList());

        var vigentes = facturas.Where(f => f.Estado != EstadoFactura.Anulada).ToList();
        EstablecerResumen($"{facturas.Count} factura(s)   ·   Facturado {Constantes.FormatearMoneda(vigentes.Sum(f => f.Total))}   ·   " +
                          $"Cobrado {Constantes.FormatearMoneda(vigentes.Sum(f => f.TotalPagado))}   ·   " +
                          $"Por cobrar {Constantes.FormatearMoneda(vigentes.Sum(f => f.Saldo))}");
    }

    /// <inheritdoc />
    protected override void AlCambiarSeleccion()
    {
        var estado = FilaSeleccionada<Fila>()?.Estado;
        _btnPago.Enabled = estado == nameof(EstadoFactura.Emitida);
    }

    /// <inheritdoc />
    protected override void VerDetalle()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        using var form = new FormFacturaDetalle(_s, fila.Id);
        form.ShowDialog(FindForm());
        CargarDatos();
    }

    /// <inheritdoc />
    protected override void Editar() => VerDetalle();

    /// <inheritdoc />
    protected override void Eliminar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        using var form = new FormEntradaTexto($"Anular factura {fila.Numero}",
            "Escriba el motivo de la anulación (obligatorio):", textoGuardar: "Anular factura");
        if (form.ShowDialog(FindForm()) != DialogResult.OK) return;
        if (!Mensajes.Confirmar(FindForm(), $"¿Anular la factura {fila.Numero}? Esta acción no se puede deshacer.",
                "Anular factura", peligro: true, textoAceptar: "Anular")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Facturacion.Anular(fila.Id, form.Valor));
        CargarDatos();
    }

    private void RegistrarPago()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var factura = _s.Facturacion.ObtenerPorId(fila.Id);
        if (factura is null) return;
        using var form = new FormPago(_s, factura);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }
}
