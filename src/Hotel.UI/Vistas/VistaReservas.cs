using System.ComponentModel;
using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;
using Hotel.UI.Controles;
using Hotel.UI.Formularios;

namespace Hotel.UI.Vistas;

/// <summary>Listado de reservas con filtros por estado y fechas, y acciones del ciclo de vida.</summary>
/// <remarks>RF-14..RF-18; CU-04, CU-05, CU-11</remarks>
public class VistaReservas : VistaListadoBase
{
    private const string Todos = "Todos";

    private sealed record Fila(
        int Id,
        [property: DisplayName("Código")] string Codigo,
        [property: DisplayName("Huésped")] string Huesped,
        [property: DisplayName("Habitación")] string Habitacion,
        [property: DisplayName("Entrada")] string Entrada,
        [property: DisplayName("Salida")] string Salida,
        [property: DisplayName("Noches")] int Noches,
        [property: DisplayName("Personas")] int Personas,
        [property: DisplayName("Alojamiento")] string Total,
        [property: DisplayName("Estado")] string Estado) : IFilaConId;

    private readonly ServiciosHotel? _s;
    private readonly ComboBox _cmbEstado = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 140 };
    private readonly DateTimePicker _dtpDesde = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 130 };
    private readonly DateTimePicker _dtpHasta = new() { Format = DateTimePickerFormat.Short, ShowCheckBox = true, Checked = false, Width = 130 };
    private readonly BotonModerno _btnConfirmar = new() { Text = "Confirmar", Icono = "", Variante = VarianteBoton.Secundario, Width = 140, Height = 40 };
    private readonly BotonModerno _btnCancelar = new() { Text = "Cancelar reserva", Icono = "", Variante = VarianteBoton.Peligro, Width = 190, Height = 40 };

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaReservas() { }

    /// <summary>Crea la vista con los servicios de la aplicación.</summary>
    public VistaReservas(ServiciosHotel servicios)
    {
        _s = servicios;
        AyudaBusqueda = "Código, huésped o documento...";
        PermiteEliminar = false;
        TextoEliminar = "Cancelar";

        _cmbEstado.Items.Add(Todos);
        _cmbEstado.Items.AddRange(Enum.GetNames<EstadoReserva>());
        _cmbEstado.SelectedIndex = 0;
        AgregarFiltro("Estado", _cmbEstado);
        AgregarFiltro("Entrada desde", _dtpDesde);
        AgregarFiltro("hasta", _dtpHasta);

        _btnConfirmar.Click += (_, _) => Confirmar();
        _btnCancelar.Click += (_, _) => Cancelar();
        AgregarAccion(_btnConfirmar);
        AgregarAccion(_btnCancelar);

        _cmbEstado.SelectedIndexChanged += (_, _) => CargarDatos();
        _dtpDesde.ValueChanged += (_, _) => CargarDatos();
        _dtpHasta.ValueChanged += (_, _) => CargarDatos();
    }

    /// <inheritdoc />
    public override string TituloVista => "Reservas";

    /// <inheritdoc />
    protected override void CargarDatos()
    {
        if (_s is null) return;
        var huespedes = _s.Huespedes.ObtenerTodos().ToDictionary(h => h.Id);
        var habitaciones = _s.Habitaciones.Buscar(new FiltroHabitacion(IncluirInactivas: true)).ToDictionary(h => h.Id);
        var texto = TextoBusqueda;

        var filtro = new FiltroReserva(
            Estado: _cmbEstado.SelectedIndex > 0 ? Enum.Parse<EstadoReserva>((string)_cmbEstado.SelectedItem!) : null,
            Desde: _dtpDesde.Checked ? _dtpDesde.Value.Date : null,
            Hasta: _dtpHasta.Checked ? _dtpHasta.Value.Date : null);

        var filas = _s.Reservas.Buscar(filtro)
            .Where(r => string.IsNullOrEmpty(texto) || Coincide(r, huespedes, texto))
            .Select(r => new Fila(
                r.Id, r.Codigo,
                huespedes.TryGetValue(r.HuespedId, out var h) ? h.NombreCompleto : "—",
                habitaciones.TryGetValue(r.HabitacionId, out var hab) ? hab.Numero : "—",
                r.FechaEntrada.ToString(Constantes.FormatoFecha), r.FechaSalida.ToString(Constantes.FormatoFecha),
                r.Noches, r.CantidadPersonas, Constantes.FormatearMoneda(r.TotalAlojamiento), r.Estado.ToString()))
            .ToList();
        MostrarFilas(filas);
    }

    private static bool Coincide(Reserva r, Dictionary<int, Huesped> huespedes, string texto)
    {
        if (r.Codigo.Contains(texto, StringComparison.OrdinalIgnoreCase)) return true;
        return huespedes.TryGetValue(r.HuespedId, out var h)
               && (h.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase)
                   || h.NumeroDocumento.Contains(texto, StringComparison.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    protected override void AlCambiarSeleccion()
    {
        var estado = FilaSeleccionada<Fila>()?.Estado;
        var modificable = estado is nameof(EstadoReserva.Pendiente) or nameof(EstadoReserva.Confirmada);
        BotonEditar.Enabled = modificable;
        _btnConfirmar.Enabled = estado == nameof(EstadoReserva.Pendiente);
        _btnCancelar.Enabled = modificable;
    }

    /// <inheritdoc />
    protected override void Nuevo()
    {
        if (_s is null) return;
        using var form = new FormReserva(_s);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Editar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var reserva = _s.Reservas.ObtenerPorId(fila.Id);
        if (reserva is null || reserva.Estado is not (EstadoReserva.Pendiente or EstadoReserva.Confirmada))
        {
            Mensajes.Advertencia(FindForm(), "Solo se pueden modificar reservas Pendientes o Confirmadas.");
            return;
        }
        using var form = new FormReserva(_s, reserva);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    private void Confirmar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        if (!Mensajes.Confirmar(FindForm(), $"¿Confirmar la reserva {fila.Codigo}?", "Confirmar reserva")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Reservas.Confirmar(fila.Id));
        CargarDatos();
    }

    private void Cancelar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        using var form = new FormEntradaTexto($"Cancelar reserva {fila.Codigo}",
            "Escriba el motivo de la cancelación (obligatorio):", textoGuardar: "Cancelar reserva");
        if (form.ShowDialog(FindForm()) != DialogResult.OK) return;
        if (!Mensajes.Confirmar(FindForm(), $"¿Cancelar definitivamente la reserva {fila.Codigo}?", "Cancelar reserva",
                peligro: true, textoAceptar: "Cancelar reserva")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Reservas.Cancelar(fila.Id, form.Valor));
        CargarDatos();
    }

    /// <inheritdoc />
    protected override void VerDetalle()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var r = _s.Reservas.ObtenerPorId(fila.Id);
        if (r is null) return;
        var lineas = new List<string>
        {
            $"Reserva {r.Codigo} — {r.Estado}",
            $"Huésped: {fila.Huesped}   ·   Habitación: {fila.Habitacion}",
            $"Estadía: {fila.Entrada} → {fila.Salida} ({r.Noches} noche(s)), {r.CantidadPersonas} persona(s)",
            $"Tarifa pactada: {Constantes.FormatearMoneda(r.TarifaPactada)} por noche",
            $"Alojamiento: {Constantes.FormatearMoneda(r.TotalAlojamiento)}   ·   Cargos: {Constantes.FormatearMoneda(r.TotalCargos)}"
        };
        if (r.FechaCheckIn is not null) lineas.Add($"Check-in: {r.FechaCheckIn:dd/MM/yyyy HH:mm}");
        if (r.FechaCheckOut is not null) lineas.Add($"Check-out: {r.FechaCheckOut:dd/MM/yyyy HH:mm}");
        foreach (var c in r.Cargos)
            lineas.Add($"   • {c.Fecha:dd/MM} {c.Descripcion} × {c.Cantidad} = {Constantes.FormatearMoneda(c.Subtotal)}");
        if (!string.IsNullOrEmpty(r.MotivoCancelacion)) lineas.Add($"Motivo de cancelación: {r.MotivoCancelacion}");
        Mensajes.Informacion(FindForm(), string.Join(Environment.NewLine, lineas), "Detalle de reserva");
    }
}
