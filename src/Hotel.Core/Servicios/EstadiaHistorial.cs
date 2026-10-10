using Hotel.Core.Dominio;

namespace Hotel.Core.Servicios;

/// <summary>Fila del historial de estadías de un huésped.</summary>
/// <remarks>RF-12</remarks>
public record EstadiaHistorial(
    string CodigoReserva,
    DateTime Entrada,
    DateTime Salida,
    string Habitacion,
    EstadoReserva Estado,
    decimal TotalFacturado);
