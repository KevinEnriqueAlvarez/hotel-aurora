using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Check-in, cargos adicionales y check-out con generación de factura.</summary>
/// <remarks>RF-19, RF-20, RF-21, RF-22, RN-07..RN-09, RN-15</remarks>
public class EstadiaServicio : ServicioBase
{
    private readonly IRepositorio<Reserva> _reservas;
    private readonly IRepositorio<Habitacion> _habitaciones;
    private readonly IRepositorio<Huesped> _huespedes;
    private readonly IRepositorio<ServicioAdicional> _servicios;
    private readonly IRepositorio<Factura> _facturas;
    private readonly IReloj _reloj;
    private readonly ConfiguracionHotel _config;

    /// <summary>Crea el servicio.</summary>
    public EstadiaServicio(IRepositorio<Reserva> reservas, IRepositorio<Habitacion> habitaciones,
        IRepositorio<Huesped> huespedes, IRepositorio<ServicioAdicional> servicios,
        IRepositorio<Factura> facturas, IReloj reloj, ConfiguracionHotel config)
    {
        _reservas = reservas;
        _habitaciones = habitaciones;
        _huespedes = huespedes;
        _servicios = servicios;
        _facturas = facturas;
        _reloj = reloj;
        _config = config;
    }

    /// <summary>Catálogo de servicios activos para registrar cargos.</summary>
    /// <remarks>RF-20</remarks>
    public IReadOnlyList<ServicioAdicional> ObtenerServicios() =>
        _servicios.ObtenerTodos().OrderBy(s => s.Categoria).ThenBy(s => s.Nombre).ToList();

    /// <summary>
    /// Check-in: reserva Confirmada con hoy en [entrada, salida), habitación Disponible (propia o reasignada).
    /// </summary>
    /// <remarks>RF-19, RN-07, RN-10</remarks>
    public Resultado CheckIn(int reservaId, int? habitacionId = null) => Ejecutar(() =>
    {
        var reserva = _reservas.ObtenerPorId(reservaId) ?? throw new ReglaNegocioException("La reserva no existe.");
        if (reserva.Estado != EstadoReserva.Confirmada)
            throw new ReglaNegocioException("Solo se puede hacer check-in de reservas Confirmadas.");
        var hoy = _reloj.Hoy;
        if (hoy < reserva.FechaEntrada || hoy >= reserva.FechaSalida)
            throw new ReglaNegocioException(
                $"El check-in solo puede hacerse entre el {reserva.FechaEntrada:dd/MM/yyyy} y antes del {reserva.FechaSalida:dd/MM/yyyy}.");

        var destinoId = habitacionId ?? reserva.HabitacionId;
        var habitacion = _habitaciones.ObtenerPorId(destinoId) ?? throw new ReglaNegocioException("La habitación no existe.");
        if (!habitacion.Activo || habitacion.Estado != EstadoHabitacion.Disponible)
            throw new ReglaNegocioException(
                $"La habitación {habitacion.Numero} no está disponible (estado: {habitacion.Estado}).");
        if (habitacion.Capacidad < reserva.CantidadPersonas)
            throw new ReglaNegocioException(
                $"La habitación {habitacion.Numero} admite máximo {habitacion.Capacidad} personas.");

        if (destinoId != reserva.HabitacionId)
        {
            ExigirSinConflicto(destinoId, reserva);
            reserva.ReasignarHabitacion(destinoId, habitacion.TarifaNoche);
        }

        habitacion.CambiarEstado(EstadoHabitacion.Ocupada);
        reserva.RegistrarCheckIn(_reloj.Ahora);
        _reservas.Actualizar(reserva);
        _habitaciones.Actualizar(habitacion);
    }, "Check-in realizado.");

    /// <summary>Agrega un cargo del catálogo a una reserva En curso.</summary>
    /// <remarks>RF-20, RN-08</remarks>
    public Resultado AgregarCargo(int reservaId, int servicioId, int cantidad) => Ejecutar(() =>
    {
        var reserva = _reservas.ObtenerPorId(reservaId) ?? throw new ReglaNegocioException("La reserva no existe.");
        var servicio = _servicios.ObtenerPorId(servicioId);
        if (servicio is null || !servicio.Activo) throw new ReglaNegocioException("El servicio no existe.");
        reserva.AgregarCargo(new CargoAdicional
        {
            ServicioId = servicio.Id,
            Descripcion = servicio.Nombre,
            Cantidad = cantidad,
            PrecioUnitario = servicio.Precio,
            Fecha = _reloj.Ahora
        });
        _reservas.Actualizar(reserva);
    }, "Cargo registrado.");

    /// <summary>Calcula la factura que resultaría hoy, sin guardar nada (estado de cuenta).</summary>
    /// <remarks>RF-21, RF-22, RN-11</remarks>
    public Resultado<Factura> PrevisualizarFactura(int reservaId, decimal porcentajeDescuento) => Ejecutar(() =>
    {
        var (reserva, habitacion, huesped) = CargarEstadia(reservaId);
        return Factura.Generar(reserva, habitacion, huesped, _config, porcentajeDescuento, _reloj.Ahora);
    });

    /// <summary>
    /// Check-out: genera la factura, finaliza la reserva y deja la habitación en Limpieza.
    /// </summary>
    /// <remarks>RF-21, RF-22, RN-09, RN-10, RN-11, RN-15</remarks>
    public Resultado<Factura> CheckOut(int reservaId, decimal porcentajeDescuento = 0m) => Ejecutar(() =>
    {
        var (reserva, habitacion, huesped) = CargarEstadia(reservaId);
        var ahora = _reloj.Ahora;

        reserva.RegistrarCheckOut(ahora);
        var factura = Factura.Generar(reserva, habitacion, huesped, _config, porcentajeDescuento, ahora);
        habitacion.CambiarEstado(EstadoHabitacion.Limpieza);
        factura.Numero = Constantes.FormatearCorrelativo(Constantes.PrefijoFactura,
            SiguienteId(_facturas.ObtenerTodos(incluirInactivos: true)));

        _facturas.Agregar(factura);
        _reservas.Actualizar(reserva);
        _habitaciones.Actualizar(habitacion);
        return factura;
    }, "Check-out realizado y factura generada.");

    private (Reserva, Habitacion, Huesped) CargarEstadia(int reservaId)
    {
        var reserva = _reservas.ObtenerPorId(reservaId) ?? throw new ReglaNegocioException("La reserva no existe.");
        if (reserva.Estado != EstadoReserva.EnCurso)
            throw new ReglaNegocioException("Solo se puede hacer check-out de reservas en curso.");
        var habitacion = _habitaciones.ObtenerPorId(reserva.HabitacionId)
            ?? throw new ReglaNegocioException("La habitación de la reserva no existe.");
        var huesped = _huespedes.ObtenerPorId(reserva.HuespedId)
            ?? throw new ReglaNegocioException("El huésped de la reserva no existe.");
        return (reserva, habitacion, huesped);
    }

    private void ExigirSinConflicto(int habitacionId, Reserva reserva)
    {
        var hayChoque = _reservas.Buscar(r => r.HabitacionId == habitacionId && r.Id != reserva.Id
            && r.SeSolapaCon(reserva.FechaEntrada, reserva.FechaSalida)).Any();
        if (hayChoque)
            throw new ReglaNegocioException("La habitación elegida tiene otra reserva que se cruza con esta estadía.");
    }
}
