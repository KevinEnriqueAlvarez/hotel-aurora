namespace Hotel.Core.Dominio;

/// <summary>Datos y parámetros del hotel que usan facturación y reservas.</summary>
/// <remarks>RN-02, RN-11, RF-22</remarks>
public class ConfiguracionHotel
{
    /// <summary>Nombre comercial.</summary>
    public string NombreHotel { get; set; } = "Hotel Aurora";
    /// <summary>NIT del hotel.</summary>
    public string Nit { get; set; } = "1234567-8";
    /// <summary>Dirección.</summary>
    public string Direccion { get; set; } = "5a Avenida 10-25, Zona 1, Ciudad de Guatemala";
    /// <summary>Teléfono.</summary>
    public string Telefono { get; set; } = "22345678";
    /// <summary>IVA en porcentaje (12).</summary>
    public decimal PorcentajeIva { get; set; } = 12m;
    /// <summary>Impuesto de turismo en porcentaje (10).</summary>
    public decimal PorcentajeTurismo { get; set; } = 10m;
    /// <summary>Máximo de noches por reserva (RN-02).</summary>
    public int MaxNochesPorReserva { get; set; } = 30;
    /// <summary>Descuento máximo permitido en porcentaje (RN-11).</summary>
    public decimal DescuentoMaximo { get; set; } = 30m;
}
