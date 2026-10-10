using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Tema;

namespace Hotel.UI.Formularios;

/// <summary>
/// Crea o modifica una reserva en cuatro secciones: fechas y personas, habitaciones disponibles,
/// huésped y resumen.
/// </summary>
/// <remarks>RF-13, RF-14, RF-15, RN-02..RN-06; CU-04, CU-05</remarks>
public partial class FormReserva : FormularioBase
{
    private sealed record FilaHabitacion(
        int Id,
        [property: DisplayName("Habitación")] string Numero,
        [property: DisplayName("Piso")] int Piso,
        [property: DisplayName("Tipo")] string Tipo,
        [property: DisplayName("Capacidad")] int Capacidad,
        [property: DisplayName("Tarifa por noche")] string Tarifa,
        [property: DisplayName("Total estimado")] string Total);

    private sealed record FilaHuesped(
        int Id,
        [property: DisplayName("Nombre")] string Nombre,
        [property: DisplayName("Documento")] string Documento,
        [property: DisplayName("Teléfono")] string Telefono);

    private readonly ServiciosHotel? _s;
    private readonly Reserva? _original;
    private IReadOnlyList<Habitacion> _disponibles = [];
    private int? _huespedId;
    private string _huespedNombre = "";

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormReserva()
    {
        InitializeComponent();
        cmbTipo.Items.Add("Cualquiera");
        cmbTipo.Items.AddRange(Enum.GetNames<TipoHabitacion>());
        cmbTipo.SelectedIndex = 0;
        Estilizador.AplicarTabla(tablaHabitaciones);
        Estilizador.AplicarTabla(tablaHuespedes);
        lblResumen.ForeColor = Paleta.Marino;
    }

    /// <summary>Crea el formulario para una reserva nueva (o la modifica si se indica <paramref name="existente"/>).</summary>
    public FormReserva(ServiciosHotel servicios, Reserva? existente = null) : this()
    {
        _s = servicios;
        _original = existente;
        var hoy = servicios.Reloj.Ahora.Date;
        dtpEntrada.MinDate = existente is null ? hoy : Min(hoy, existente.FechaEntrada);
        dtpSalida.MinDate = dtpEntrada.MinDate;
        dtpEntrada.Value = existente?.FechaEntrada ?? hoy;
        dtpSalida.Value = existente?.FechaSalida ?? hoy.AddDays(1);
        numPersonas.Maximum = 12;

        if (existente is null)
        {
            Text = "Nueva reserva";
            TextoGuardar = "Guardar pendiente";
            ActualizarHuespedes();
        }
        else
        {
            Text = $"Modificar reserva {existente.Codigo}";
            TextoGuardar = "Guardar cambios";
            numPersonas.Value = existente.CantidadPersonas;
            var huesped = servicios.Huespedes.ObtenerPorId(existente.HuespedId);
            _huespedId = existente.HuespedId;
            _huespedNombre = huesped?.NombreCompleto ?? "";
            txtBuscarHuesped.Enabled = btnNuevoHuesped.Enabled = tablaHuespedes.Enabled = false;
            txtBuscarHuesped.Text = _huespedNombre;
            btnConfirmar.Visible = false;
            BuscarDisponibilidad();
        }
    }

    private static DateTime Min(DateTime a, DateTime b) => a < b ? a : b;

    private int Noches => Math.Max(0, (dtpSalida.Value.Date - dtpEntrada.Value.Date).Days);

    private void BuscarDisponibilidad()
    {
        if (_s is null) return;
        if (Noches <= 0)
        {
            Mensajes.Advertencia(this, "La fecha de salida debe ser posterior a la de entrada.");
            return;
        }
        var tipo = cmbTipo.SelectedIndex > 0 ? Enum.Parse<TipoHabitacion>((string)cmbTipo.SelectedItem!) : (TipoHabitacion?)null;
        _disponibles = _s.Reservas.BuscarDisponibles(dtpEntrada.Value, dtpSalida.Value, (int)numPersonas.Value, tipo, _original?.Id);
        tablaHabitaciones.DataSource = _disponibles.Select(h => new FilaHabitacion(
            h.Id, h.Numero, h.Piso, h.Tipo.ToString(), h.Capacidad,
            Constantes.FormatearMoneda(h.TarifaNoche), Constantes.FormatearMoneda(h.TarifaNoche * Noches))).ToList();
        if (tablaHabitaciones.Columns["Id"] is { } id) id.Visible = false;

        lblDisponibles.Text = _disponibles.Count == 0
            ? "No hay habitaciones disponibles con esos criterios."
            : $"{_disponibles.Count} habitación(es) disponible(s) — {Noches} noche(s)";
        if (_original is not null)
            foreach (DataGridViewRow fila in tablaHabitaciones.Rows)
                if (fila.DataBoundItem is FilaHabitacion f && f.Id == _original.HabitacionId)
                    fila.Selected = true;
        ActualizarResumen();
    }

    private void ActualizarHuespedes()
    {
        if (_s is null) return;
        tablaHuespedes.DataSource = _s.Huespedes.Buscar(txtBuscarHuesped.Text).Take(50)
            .Select(h => new FilaHuesped(h.Id, h.NombreCompleto, $"{h.TipoDocumento}: {h.NumeroDocumento}", h.Telefono)).ToList();
        if (tablaHuespedes.Columns["Id"] is { } id) id.Visible = false;
    }

    private Habitacion? HabitacionSeleccionada =>
        tablaHabitaciones.CurrentRow?.DataBoundItem is FilaHabitacion f ? _disponibles.FirstOrDefault(h => h.Id == f.Id) : null;

    private void ActualizarResumen()
    {
        var hab = HabitacionSeleccionada;
        var partes = new List<string>
        {
            $"Fechas: {dtpEntrada.Value:dd/MM/yyyy} → {dtpSalida.Value:dd/MM/yyyy} ({Noches} noche(s)), {numPersonas.Value} persona(s)"
        };
        partes.Add(hab is null
            ? "Habitación: (seleccione una de la lista)"
            : $"Habitación: {hab.Numero} ({hab.Tipo}) — {Constantes.FormatearMoneda(hab.TarifaNoche)} × {Noches} = {Constantes.FormatearMoneda(hab.TarifaNoche * Noches)} (sin impuestos)");
        partes.Add(_huespedId is null ? "Huésped: (seleccione o registre uno)" : $"Huésped: {_huespedNombre}");
        lblResumen.Text = string.Join(Environment.NewLine, partes);
    }

    /// <inheritdoc />
    protected override bool Guardar() => GuardarReserva(confirmar: false);

    private bool GuardarReserva(bool confirmar)
    {
        if (_s is null) return false;
        var hab = HabitacionSeleccionada;
        if (hab is null)
        {
            Mensajes.Advertencia(this, "Busque disponibilidad y seleccione una habitación.");
            return false;
        }
        if (_huespedId is null)
        {
            Mensajes.Advertencia(this, "Seleccione un huésped o registre uno nuevo.");
            return false;
        }

        var entrada = dtpEntrada.Value.Date;
        var salida = dtpSalida.Value.Date;
        var personas = (int)numPersonas.Value;
        if (_original is not null)
            return Procesar(_s.Reservas.Modificar(_original.Id, hab.Id, entrada, salida, personas));

        var r = _s.Reservas.Crear(_huespedId.Value, hab.Id, entrada, salida, personas, confirmar);
        if (!Procesar(r)) return false;
        Mensajes.Exito(this, $"Reserva {r.Valor!.Codigo} creada ({(confirmar ? "Confirmada" : "Pendiente")}).");
        return true;
    }

    private void BtnBuscar_Click(object? sender, EventArgs e) => BuscarDisponibilidad();

    private void Criterio_Cambiado(object? sender, EventArgs e)
    {
        // Al cambiar fechas o personas, la lista anterior deja de ser válida.
        _disponibles = [];
        tablaHabitaciones.DataSource = null;
        lblDisponibles.Text = "Pulse «Buscar disponibilidad» para ver las habitaciones.";
        ActualizarResumen();
    }

    private void FechaEntrada_ValueChanged(object? sender, EventArgs e)
    {
        if (dtpSalida.Value.Date <= dtpEntrada.Value.Date) dtpSalida.Value = dtpEntrada.Value.Date.AddDays(1);
        Criterio_Cambiado(sender, e);
    }

    private void TablaHabitaciones_SelectionChanged(object? sender, EventArgs e) => ActualizarResumen();

    private void TxtBuscarHuesped_TextChanged(object? sender, EventArgs e)
    {
        if (_original is null) ActualizarHuespedes();
    }

    private void TablaHuespedes_SelectionChanged(object? sender, EventArgs e)
    {
        if (_original is not null || tablaHuespedes.CurrentRow?.DataBoundItem is not FilaHuesped f) return;
        _huespedId = f.Id;
        _huespedNombre = f.Nombre;
        ActualizarResumen();
    }

    private void BtnNuevoHuesped_Click(object? sender, EventArgs e)
    {
        if (_s is null) return;
        using var form = new FormHuesped(_s);
        if (form.ShowDialog(this) != DialogResult.OK || form.Resultado is null) return;
        _huespedId = form.Resultado.Id;
        _huespedNombre = form.Resultado.NombreCompleto;
        txtBuscarHuesped.Text = form.Resultado.NombreCompleto;
        ActualizarResumen();
    }

    private void BtnConfirmar_Click(object? sender, EventArgs e)
    {
        if (GuardarReserva(confirmar: true)) DialogResult = DialogResult.OK;
    }
}
