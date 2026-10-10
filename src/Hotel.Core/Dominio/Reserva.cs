using System.Text.Json.Serialization;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Reserva de una habitación por un huésped, con su ciclo de vida y cargos.</summary>
/// <remarks>RF-14..RF-21, RN-02, RN-03, RN-05, RN-08, RN-09</remarks>
public class Reserva : EntidadBase
{
    private List<CargoAdicional> _cargos = [];

    /// <summary>Código correlativo <c>RES-000001</c> (RN-15).</summary>
    public string Codigo { get; set; } = "";
    /// <summary>Huésped titular.</summary>
    public int HuespedId { get; set; }
    /// <summary>Habitación asignada.</summary>
    [JsonInclude] public int HabitacionId { get; private set; }
    /// <summary>Fecha de entrada (sin hora).</summary>
    [JsonInclude] public DateTime FechaEntrada { get; private set; }
    /// <summary>Fecha de salida (sin hora).</summary>
    [JsonInclude] public DateTime FechaSalida { get; private set; }
    /// <summary>Cantidad de personas.</summary>
    [JsonInclude] public int CantidadPersonas { get; private set; }
    /// <summary>Tarifa por noche copiada de la habitación al reservar.</summary>
    [JsonInclude] public decimal TarifaPactada { get; private set; }

    /// <summary>Estado actual; cambia solo con los métodos de transición.</summary>
    [JsonInclude] public EstadoReserva Estado { get; private set; } = EstadoReserva.Pendiente;
    /// <summary>Motivo de la cancelación.</summary>
    [JsonInclude] public string? MotivoCancelacion { get; private set; }
    /// <summary>Momento del check-in.</summary>
    [JsonInclude] public DateTime? FechaCheckIn { get; private set; }
    /// <summary>Momento del check-out.</summary>
    [JsonInclude] public DateTime? FechaCheckOut { get; private set; }

    /// <summary>Cargos adicionales; se modifican solo con <see cref="AgregarCargo"/>.</summary>
    [JsonInclude]
    public IReadOnlyList<CargoAdicional> Cargos
    {
        get => _cargos;
        private set => _cargos = value?.ToList() ?? [];
    }

    /// <summary>Noches reservadas.</summary>
    [JsonIgnore] public int Noches => Math.Max(0, (FechaSalida.Date - FechaEntrada.Date).Days);
    /// <summary>Noches × tarifa pactada.</summary>
    [JsonIgnore] public decimal TotalAlojamiento => Noches * TarifaPactada;
    /// <summary>Suma de los cargos adicionales.</summary>
    [JsonIgnore] public decimal TotalCargos => _cargos.Sum(c => c.Subtotal);
    /// <summary>True si bloquea la habitación (Pendiente, Confirmada o EnCurso).</summary>
    [JsonIgnore] public bool EstaActiva => Estado is EstadoReserva.Pendiente or EstadoReserva.Confirmada or EstadoReserva.EnCurso;

    /// <summary>Crea la reserva con sus datos iniciales.</summary>
    /// <remarks>RF-14</remarks>
    public static Reserva Crear(int huespedId, int habitacionId, DateTime entrada, DateTime salida, int personas, decimal tarifa) =>
        new()
        {
            HuespedId = huespedId,
            HabitacionId = habitacionId,
            FechaEntrada = entrada.Date,
            FechaSalida = salida.Date,
            CantidadPersonas = personas,
            TarifaPactada = tarifa
        };

    /// <summary>Pendiente → Confirmada.</summary>
    /// <remarks>RF-16, RN-05</remarks>
    public void Confirmar() => Transicionar(EstadoReserva.Confirmada, EstadoReserva.Pendiente);

    /// <summary>Pendiente/Confirmada → Cancelada, con motivo obligatorio.</summary>
    /// <remarks>RF-17, RN-05</remarks>
    public void Cancelar(string motivo)
    {
        if (string.IsNullOrWhiteSpace(motivo)) throw new ReglaNegocioException("El motivo de cancelación es obligatorio.");
        Transicionar(EstadoReserva.Cancelada, EstadoReserva.Pendiente, EstadoReserva.Confirmada);
        MotivoCancelacion = motivo.Trim();
    }

    /// <summary>Confirmada → EnCurso.</summary>
    /// <remarks>RF-19, RN-05, RN-07</remarks>
    public void RegistrarCheckIn(DateTime fecha)
    {
        Transicionar(EstadoReserva.EnCurso, EstadoReserva.Confirmada);
        FechaCheckIn = fecha;
    }

    /// <summary>EnCurso → Finalizada.</summary>
    /// <remarks>RF-21, RN-05, RN-09</remarks>
    public void RegistrarCheckOut(DateTime fecha)
    {
        Transicionar(EstadoReserva.Finalizada, EstadoReserva.EnCurso);
        FechaCheckOut = fecha;
    }

    /// <summary>Agrega un cargo; solo en reservas EnCurso y con cantidad mayor que cero.</summary>
    /// <remarks>RF-20, RN-08</remarks>
    public void AgregarCargo(CargoAdicional cargo)
    {
        if (Estado != EstadoReserva.EnCurso)
            throw new ReglaNegocioException("Solo se pueden registrar cargos en reservas en curso.");
        if (cargo.Cantidad <= 0) throw new ReglaNegocioException("La cantidad del cargo debe ser mayor que cero.");
        _cargos.Add(cargo);
    }

    /// <summary>Cambia fechas y personas de una reserva Pendiente o Confirmada.</summary>
    /// <remarks>RF-15, RN-02, RN-06</remarks>
    public void CambiarFechas(DateTime entrada, DateTime salida, int personas)
    {
        ExigirModificable();
        if (salida.Date <= entrada.Date) throw new ReglaNegocioException("La fecha de salida debe ser posterior a la de entrada.");
        if (personas < 1) throw new ReglaNegocioException("La cantidad de personas debe ser al menos 1.");
        FechaEntrada = entrada.Date;
        FechaSalida = salida.Date;
        CantidadPersonas = personas;
    }

    /// <summary>Asigna otra habitación y copia su tarifa.</summary>
    /// <remarks>RF-15, RF-19, RN-07</remarks>
    public void ReasignarHabitacion(int habitacionId, decimal tarifa)
    {
        ExigirModificable();
        HabitacionId = habitacionId;
        TarifaPactada = tarifa;
    }

    /// <summary>
    /// True si la reserva activa se cruza con el rango: <c>entradaA &lt; salidaB &amp;&amp; entradaB &lt; salidaA</c>.
    /// </summary>
    /// <remarks>RN-03</remarks>
    public bool SeSolapaCon(DateTime entrada, DateTime salida) =>
        EstaActiva && FechaEntrada.Date < salida.Date && entrada.Date < FechaSalida.Date;

    /// <summary>Noches cobradas: máx(1, días entre check-in y la salida real).</summary>
    /// <remarks>RN-09</remarks>
    public int NochesCobradas(DateTime fechaSalidaReal)
    {
        var inicio = (FechaCheckIn ?? FechaEntrada).Date;
        return Math.Max(1, (fechaSalidaReal.Date - inicio).Days);
    }

    /// <inheritdoc />
    public override IEnumerable<string> Validar()
    {
        if (HuespedId <= 0) yield return "Debe seleccionar un huésped.";
        if (HabitacionId <= 0) yield return "Debe seleccionar una habitación.";
        if (FechaSalida.Date <= FechaEntrada.Date) yield return "La fecha de salida debe ser posterior a la de entrada.";
        if (CantidadPersonas < 1) yield return "La cantidad de personas debe ser al menos 1.";
    }

    private void Transicionar(EstadoReserva destino, params EstadoReserva[] origenes)
    {
        if (!origenes.Contains(Estado))
            throw new ReglaNegocioException($"No se puede pasar una reserva {Estado} a {destino}.");
        Estado = destino;
    }

    private void ExigirModificable()
    {
        if (Estado is not (EstadoReserva.Pendiente or EstadoReserva.Confirmada))
            throw new ReglaNegocioException("Solo se pueden modificar reservas Pendientes o Confirmadas.");
    }
}
