using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.UI.Formularios;

/// <summary>Check-in con asignación o reasignación de habitación.</summary>
/// <remarks>RF-19, RN-07; CU-06</remarks>
public partial class FormCheckIn : FormularioBase
{
    private readonly ServiciosHotel? _s;
    private readonly Reserva? _reserva;
    private IReadOnlyList<Habitacion> _opciones = [];

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public FormCheckIn() => InitializeComponent();

    /// <summary>Crea el diálogo para una reserva Confirmada.</summary>
    public FormCheckIn(ServiciosHotel servicios, Reserva reserva) : this()
    {
        _s = servicios;
        _reserva = reserva;
        Text = $"Check-in — {reserva.Codigo}";
        TextoGuardar = "Realizar check-in";
        var huesped = servicios.Huespedes.ObtenerPorId(reserva.HuespedId);
        lblInfo.Text = $"Huésped: {huesped?.NombreCompleto}\n" +
                       $"Estadía: {reserva.FechaEntrada:dd/MM/yyyy} al {reserva.FechaSalida:dd/MM/yyyy} · {reserva.CantidadPersonas} persona(s)";

        _opciones = servicios.Reservas
            .BuscarDisponibles(reserva.FechaEntrada, reserva.FechaSalida, reserva.CantidadPersonas, excluirReservaId: reserva.Id)
            .Where(h => h.Estado == EstadoHabitacion.Disponible)
            .OrderBy(h => h.Id == reserva.HabitacionId ? 0 : 1).ThenBy(h => h.Numero)
            .ToList();
        cmbHabitacion.DataSource = _opciones
            .Select(h => $"{h.Numero} · {h.Tipo} · cap. {h.Capacidad} · {Constantes.FormatearMoneda(h.TarifaNoche)}"
                         + (h.Id == reserva.HabitacionId ? "  (reservada)" : ""))
            .ToList();
        if (_opciones.Count == 0)
            lblAviso.Text = "No hay habitaciones disponibles ahora (¿en limpieza?). Libere una antes del check-in.";
    }

    /// <inheritdoc />
    protected override bool Guardar()
    {
        if (_s is null || _reserva is null) return false;
        if (cmbHabitacion.SelectedIndex < 0 || _opciones.Count == 0)
        {
            Mensajes.Advertencia(this, "No hay una habitación disponible para asignar.");
            return false;
        }
        var elegida = _opciones[cmbHabitacion.SelectedIndex];
        var r = _s.Estadias.CheckIn(_reserva.Id, elegida.Id == _reserva.HabitacionId ? null : elegida.Id);
        if (!Procesar(r)) return false;
        Mensajes.Exito(this, $"Check-in realizado en la habitación {elegida.Numero}.");
        return true;
    }
}
