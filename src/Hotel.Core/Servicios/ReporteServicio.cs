using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Consultas agregadas para el tablero «Inicio» (solo lectura).</summary>
/// <remarks>E1</remarks>
public class ReporteServicio : ServicioBase
{
    /// <summary>Nombre del segmento «Reservada hoy» en <see cref="HabitacionesPorEstado"/>.</summary>
    public const string ReservadaHoy = "Reservada hoy";

    private readonly IRepositorio<Reserva> _reservas;
    private readonly IRepositorio<Habitacion> _habitaciones;
    private readonly IRepositorio<Factura> _facturas;
    private readonly IReloj _reloj;

    /// <summary>Crea el servicio.</summary>
    public ReporteServicio(IRepositorio<Reserva> reservas, IRepositorio<Habitacion> habitaciones,
        IRepositorio<Factura> facturas, IReloj reloj)
    {
        _reservas = reservas;
        _habitaciones = habitaciones;
        _facturas = facturas;
        _reloj = reloj;
    }

    /// <summary>KPI del día: ocupación, ingresos del mes, reservas activas, llegadas y saldo.</summary>
    public IndicadoresInicio Indicadores()
    {
        var hoy = _reloj.Hoy;
        var habitaciones = _habitaciones.ObtenerTodos();
        var ocupadas = habitaciones.Count(h => h.Estado == EstadoHabitacion.Ocupada);
        var vigentes = FacturasVigentes();
        return new IndicadoresInicio(
            HabitacionesTotal: habitaciones.Count,
            HabitacionesOcupadas: ocupadas,
            OcupacionHoy: habitaciones.Count == 0 ? 0 : Math.Round(100m * ocupadas / habitaciones.Count, 1),
            IngresosMes: vigentes.Where(f => EnMes(f.FechaEmision, hoy)).Sum(f => f.Total),
            ReservasActivas: _reservas.Buscar(r => r.EstaActiva).Count(),
            LlegadasHoy: _reservas.Buscar(r => r.Estado is EstadoReserva.Pendiente or EstadoReserva.Confirmada
                                               && r.FechaEntrada == hoy).Count(),
            Cobrado: vigentes.Sum(f => f.TotalPagado),
            SaldoPorCobrar: vigentes.Sum(f => f.Saldo));
    }

    /// <summary>Serie de los últimos <paramref name="meses"/> meses, el actual al final.</summary>
    public IReadOnlyList<PuntoMensual> SerieMensual(SerieReporte serie, int meses = 12)
    {
        var hoy = _reloj.Hoy;
        var inicio = new DateTime(hoy.Year, hoy.Month, 1).AddMonths(-(meses - 1));
        var facturas = FacturasVigentes();
        var reservas = _reservas.Buscar(_ => true).ToList();
        var habitaciones = _habitaciones.ObtenerTodos().Count;

        return Enumerable.Range(0, meses).Select(i =>
        {
            var mes = inicio.AddMonths(i);
            var valor = serie switch
            {
                SerieReporte.Ingresos => facturas.Where(f => EnMes(f.FechaEmision, mes)).Sum(f => f.Total),
                SerieReporte.Reservas => reservas.Count(r => r.Estado != EstadoReserva.Cancelada && EnMes(r.FechaEntrada, mes)),
                _ => Ocupacion(reservas, habitaciones, mes, hoy)
            };
            return new PuntoMensual(mes, Etiqueta(mes), valor);
        }).ToList();
    }

    /// <summary>Habitaciones activas por estado; «Reservada hoy» se calcula (RN-10).</summary>
    public IReadOnlyList<ConteoEstado> HabitacionesPorEstado()
    {
        var hoy = _reloj.Hoy;
        var reservadas = _reservas
            .Buscar(r => r.Estado is EstadoReserva.Pendiente or EstadoReserva.Confirmada
                         && r.FechaEntrada <= hoy && hoy < r.FechaSalida)
            .Select(r => r.HabitacionId).ToHashSet();
        var habitaciones = _habitaciones.ObtenerTodos();
        var reservadasHoy = habitaciones.Count(h => h.Estado == EstadoHabitacion.Disponible && reservadas.Contains(h.Id));
        int Contar(EstadoHabitacion e) => habitaciones.Count(h => h.Estado == e);
        return
        [
            new ConteoEstado("Disponible", Contar(EstadoHabitacion.Disponible) - reservadasHoy),
            new ConteoEstado("Ocupada", Contar(EstadoHabitacion.Ocupada)),
            new ConteoEstado(ReservadaHoy, reservadasHoy),
            new ConteoEstado("Limpieza", Contar(EstadoHabitacion.Limpieza)),
            new ConteoEstado("Mantenimiento", Contar(EstadoHabitacion.Mantenimiento))
        ];
    }

    private List<Factura> FacturasVigentes() =>
        _facturas.Buscar(f => f.Estado != EstadoFactura.Anulada).ToList();

    private static bool EnMes(DateTime fecha, DateTime mes) => fecha.Year == mes.Year && fecha.Month == mes.Month;

    private static string Etiqueta(DateTime mes) =>
        Constantes.Cultura.DateTimeFormat.GetMonthName(mes.Month)[..3].ToUpperInvariant();

    private static decimal Ocupacion(List<Reserva> reservas, int habitaciones, DateTime mes, DateTime hoy)
    {
        var inicioMes = new DateTime(mes.Year, mes.Month, 1);
        var finMes = inicioMes.AddMonths(1);
        var limite = hoy.AddDays(1);
        if (finMes > limite) finMes = limite;
        var dias = (finMes - inicioMes).Days;
        if (dias <= 0 || habitaciones == 0) return 0;

        var noches = reservas
            .Where(r => r.FechaCheckIn is not null)
            .Sum(r =>
            {
                var desde = r.FechaCheckIn!.Value.Date;
                var hasta = r.FechaCheckOut?.Date ?? limite;
                var a = desde > inicioMes ? desde : inicioMes;
                var b = hasta < finMes ? hasta : finMes;
                return Math.Max(0, (b - a).Days);
            });
        return Math.Round(100m * noches / (habitaciones * dias), 1);
    }
}
