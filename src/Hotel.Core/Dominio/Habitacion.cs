using System.Text.Json.Serialization;
using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Habitación del hotel con su estado operativo.</summary>
/// <remarks>RF-03, RF-04, RF-07, RN-01, RN-10</remarks>
public class Habitacion : EntidadBase
{
    private const int CapacidadMaxima = 12;
    private const int NumeroMaxCaracteres = 10;

    /// <summary>Número visible, único (RN-01).</summary>
    public string Numero { get; set; } = "";
    /// <summary>Piso.</summary>
    public int Piso { get; set; } = 1;
    /// <summary>Tipo de habitación.</summary>
    public TipoHabitacion Tipo { get; set; } = TipoHabitacion.Sencilla;
    /// <summary>Cantidad máxima de personas.</summary>
    public int Capacidad { get; set; } = 1;
    /// <summary>Tarifa por noche en quetzales.</summary>
    public decimal TarifaNoche { get; set; }
    /// <summary>Descripción.</summary>
    public string Descripcion { get; set; } = "";

    /// <summary>Estado operativo; cambia solo con <see cref="CambiarEstado"/>.</summary>
    [JsonInclude] public EstadoHabitacion Estado { get; private set; } = EstadoHabitacion.Disponible;

    /// <summary>Cambia el estado validando la transición permitida.</summary>
    /// <remarks>RF-07, RN-10</remarks>
    /// <exception cref="ReglaNegocioException">Transición no permitida.</exception>
    public void CambiarEstado(EstadoHabitacion nuevo)
    {
        if (!TransicionPermitida(Estado, nuevo))
            throw new ReglaNegocioException($"No se puede pasar la habitación {Numero} de {Estado} a {nuevo}.");
        Estado = nuevo;
    }

    /// <inheritdoc />
    /// <remarks>RF-03</remarks>
    public override IEnumerable<string> Validar()
    {
        if (string.IsNullOrWhiteSpace(Numero) || Numero.Length > NumeroMaxCaracteres)
            yield return "El número de habitación es obligatorio (máximo 10 caracteres).";
        if (Piso < 1) yield return "El piso debe ser 1 o mayor.";
        if (Capacidad < 1 || Capacidad > CapacidadMaxima) yield return $"La capacidad debe estar entre 1 y {CapacidadMaxima}.";
        if (TarifaNoche <= 0) yield return "La tarifa por noche debe ser mayor que cero.";
    }

    private static bool TransicionPermitida(EstadoHabitacion de, EstadoHabitacion a) => (de, a) switch
    {
        (EstadoHabitacion.Disponible, EstadoHabitacion.Ocupada) => true,       // check-in
        (EstadoHabitacion.Ocupada, EstadoHabitacion.Limpieza) => true,         // check-out
        (EstadoHabitacion.Limpieza, EstadoHabitacion.Disponible) => true,      // liberar
        (EstadoHabitacion.Disponible, EstadoHabitacion.Mantenimiento) => true, // manual
        (EstadoHabitacion.Mantenimiento, EstadoHabitacion.Disponible) => true, // manual
        _ => false
    };
}
