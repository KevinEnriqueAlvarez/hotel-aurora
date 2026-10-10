using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Disponibilidad, creación, modificación, confirmación y cancelación de reservas.</summary>
/// <remarks>RF-13..RF-18, RN-02..RN-06, RN-15</remarks>
public class ReservaServicio : ServicioBase
{
    private readonly IRepositorio<Reserva> _reservas;
    private readonly IRepositorio<Habitacion> _habitaciones;
    private readonly IRepositorio<Huesped> _huespedes;
    private readonly IReloj _reloj;
    private readonly ConfiguracionHotel _config;

    /// <summary>Crea el servicio.</summary>
    public ReservaServicio(IRepositorio<Reserva> reservas, IRepositorio<Habitacion> habitaciones,
        IRepositorio<Huesped> huespedes, IReloj reloj, ConfiguracionHotel config)
    {
        _reservas = reservas;
        _habitaciones = habitaciones;
        _huespedes = huespedes;
        _reloj = reloj;
        _config = config;
    }

    /// <summary>Busca una reserva por Id.</summary>
    public Reserva? ObtenerPorId(int id) => _reservas.ObtenerPorId(id);

    /// <summary>
    /// Habitaciones activas, fuera de mantenimiento, con capacidad y sin reservas activas que se solapen.
    /// </summary>
    /// <remarks>RF-13, RN-03, RN-04</remarks>
    public IReadOnlyList<Habitacion> BuscarDisponibles(DateTime entrada, DateTime salida, int personas,
        TipoHabitacion? tipo = null, int? excluirReservaId = null)
    {
        if (salida.Date <= entrada.Date) return [];
        var ocupadas = _reservas
            .Buscar(r => r.Id != excluirReservaId && r.SeSolapaCon(entrada, salida))
            .Select(r => r.HabitacionId).ToHashSet();
        return _habitaciones.ObtenerTodos()
            .Where(h => h.Estado != EstadoHabitacion.Mantenimiento)
            .Where(h => h.Capacidad >= personas)
            .Where(h => tipo is null || h.Tipo == tipo)
            .Where(h => !ocupadas.Contains(h.Id))
            .OrderBy(h => h.TarifaNoche).ThenBy(h => h.Numero)
            .ToList();
    }

    /// <summary>Crea una reserva Pendiente o Confirmada validando RN-02 a RN-04.</summary>
    /// <remarks>RF-14, RN-02, RN-03, RN-04, RN-15</remarks>
    public Resultado<Reserva> Crear(int huespedId, int habitacionId, DateTime entrada, DateTime salida,
        int personas, bool confirmar = false) => Ejecutar(() =>
    {
        var huesped = _huespedes.ObtenerPorId(huespedId) ?? throw new ReglaNegocioException("El huésped no existe.");
        if (!huesped.Activo) throw new ReglaNegocioException("El huésped está desactivado.");
        var habitacion = ObtenerHabitacionReservable(habitacionId);
        ValidarFechasYCapacidad(entrada, salida, personas, habitacion);
        ExigirSinSolapamiento(habitacion.Id, entrada, salida, excluirReservaId: null);

        var reserva = Reserva.Crear(huespedId, habitacionId, entrada, salida, personas, habitacion.TarifaNoche);
        reserva.Codigo = Constantes.FormatearCorrelativo(Constantes.PrefijoReserva,
            SiguienteId(_reservas.ObtenerTodos(incluirInactivos: true)));
        reserva.FechaCreacion = _reloj.Ahora;
        if (confirmar) reserva.Confirmar();
        _reservas.Agregar(reserva);
        return reserva;
    }, "Reserva creada.");

    /// <summary>Modifica fechas, habitación o personas revalidando RN-02 a RN-04 sin contarse a sí misma.</summary>
    /// <remarks>RF-15, RN-06</remarks>
    public Resultado Modificar(int reservaId, int habitacionId, DateTime entrada, DateTime salida, int personas) =>
        Ejecutar(() =>
        {
            var reserva = ObtenerReserva(reservaId);
            if (reserva.Estado is not (EstadoReserva.Pendiente or EstadoReserva.Confirmada))
                throw new ReglaNegocioException("Solo se pueden modificar reservas Pendientes o Confirmadas.");
            var habitacion = ObtenerHabitacionReservable(habitacionId);
            ValidarFechasYCapacidad(entrada, salida, personas, habitacion);
            ExigirSinSolapamiento(habitacion.Id, entrada, salida, reservaId);

            if (habitacion.Id != reserva.HabitacionId)
                reserva.ReasignarHabitacion(habitacion.Id, habitacion.TarifaNoche);
            reserva.CambiarFechas(entrada, salida, personas);
            _reservas.Actualizar(reserva);
        }, "Reserva modificada.");

    /// <summary>Pendiente → Confirmada.</summary>
    /// <remarks>RF-16, RN-05</remarks>
    public Resultado Confirmar(int reservaId) => Ejecutar(() =>
    {
        var reserva = ObtenerReserva(reservaId);
        reserva.Confirmar();
        _reservas.Actualizar(reserva);
    }, "Reserva confirmada.");

    /// <summary>Cancela con motivo obligatorio.</summary>
    /// <remarks>RF-17, RN-05</remarks>
    public Resultado Cancelar(int reservaId, string motivo) => Ejecutar(() =>
    {
        var reserva = ObtenerReserva(reservaId);
        reserva.Cancelar(motivo);
        _reservas.Actualizar(reserva);
    }, "Reserva cancelada.");

    /// <summary>Lista reservas por estado, rango de fechas de entrada y huésped, de la más reciente a la más antigua.</summary>
    /// <remarks>RF-18</remarks>
    public IReadOnlyList<Reserva> Buscar(FiltroReserva? filtro = null)
    {
        filtro ??= new FiltroReserva();
        var texto = filtro.Texto?.Trim();
        return _reservas.Buscar(r =>
                (filtro.Estado is null || r.Estado == filtro.Estado)
                && (filtro.Desde is null || r.FechaEntrada >= filtro.Desde.Value.Date)
                && (filtro.Hasta is null || r.FechaEntrada <= filtro.Hasta.Value.Date)
                && (filtro.HuespedId is null || r.HuespedId == filtro.HuespedId)
                && (string.IsNullOrEmpty(texto) || r.Codigo.Contains(texto, StringComparison.OrdinalIgnoreCase)))
            .OrderByDescending(r => r.FechaEntrada).ThenByDescending(r => r.Id)
            .ToList();
    }

    /// <summary>Reservas Confirmadas cuya fecha de llegada es hoy o ya pasó y aún no vencen (listas para check-in).</summary>
    /// <remarks>RF-19, RN-07</remarks>
    public IReadOnlyList<Reserva> LlegadasDelDia()
    {
        var hoy = _reloj.Hoy;
        return _reservas
            .Buscar(r => r.Estado == EstadoReserva.Confirmada && r.FechaEntrada <= hoy && hoy < r.FechaSalida)
            .OrderBy(r => r.FechaEntrada).ThenBy(r => r.Codigo).ToList();
    }

    /// <summary>Reservas En curso.</summary>
    /// <remarks>RF-20, RF-21</remarks>
    public IReadOnlyList<Reserva> HuespedesEnCasa() =>
        _reservas.Buscar(r => r.Estado == EstadoReserva.EnCurso).OrderBy(r => r.FechaSalida).ThenBy(r => r.Codigo).ToList();

    private Reserva ObtenerReserva(int id) =>
        _reservas.ObtenerPorId(id) ?? throw new ReglaNegocioException("La reserva no existe.");

    private Habitacion ObtenerHabitacionReservable(int habitacionId)
    {
        var habitacion = _habitaciones.ObtenerPorId(habitacionId)
            ?? throw new ReglaNegocioException("La habitación no existe.");
        if (!habitacion.Activo || habitacion.Estado == EstadoHabitacion.Mantenimiento)
            throw new ReglaNegocioException($"La habitación {habitacion.Numero} no está disponible para reservar (mantenimiento o inactiva).");
        return habitacion;
    }

    private void ValidarFechasYCapacidad(DateTime entrada, DateTime salida, int personas, Habitacion habitacion)
    {
        if (entrada.Date < _reloj.Hoy) throw new ReglaNegocioException("La fecha de entrada no puede ser anterior a hoy.");
        if (salida.Date <= entrada.Date) throw new ReglaNegocioException("La fecha de salida debe ser posterior a la de entrada.");
        var noches = (salida.Date - entrada.Date).Days;
        if (noches > _config.MaxNochesPorReserva)
            throw new ReglaNegocioException($"La reserva no puede exceder {_config.MaxNochesPorReserva} noches.");
        if (personas < 1) throw new ReglaNegocioException("La cantidad de personas debe ser al menos 1.");
        if (personas > habitacion.Capacidad)
            throw new ReglaNegocioException($"La habitación {habitacion.Numero} admite máximo {habitacion.Capacidad} personas.");
    }

    private void ExigirSinSolapamiento(int habitacionId, DateTime entrada, DateTime salida, int? excluirReservaId)
    {
        var choque = _reservas
            .Buscar(r => r.HabitacionId == habitacionId && r.Id != excluirReservaId && r.SeSolapaCon(entrada, salida))
            .FirstOrDefault();
        if (choque is not null)
            throw new ReglaNegocioException(
                $"La habitación ya está reservada del {choque.FechaEntrada:dd/MM/yyyy} al {choque.FechaSalida:dd/MM/yyyy} ({choque.Codigo}).");
    }
}
