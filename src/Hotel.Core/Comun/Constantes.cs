using System.Globalization;

namespace Hotel.Core.Comun;

/// <summary>Constantes y formatos compartidos (sin números mágicos).</summary>
/// <remarks>RN-11, RN-15, RNF-06</remarks>
public static class Constantes
{
    /// <summary>Cultura de Guatemala (moneda en quetzales, fechas dd/MM/yyyy).</summary>
    public static readonly CultureInfo Cultura = CrearCultura();

    /// <summary>Formato de fecha de la aplicación.</summary>
    public const string FormatoFecha = "dd/MM/yyyy";
    /// <summary>Prefijo del correlativo de reservas (RN-15).</summary>
    public const string PrefijoReserva = "RES-";
    /// <summary>Prefijo del correlativo de facturas (RN-15).</summary>
    public const string PrefijoFactura = "FAC-";
    /// <summary>Dígitos del correlativo (RN-15).</summary>
    public const int DigitosCorrelativo = 6;
    /// <summary>Iteraciones PBKDF2 (RNF-06).</summary>
    public const int IteracionesPbkdf2 = 100_000;
    /// <summary>Tamaño de la sal en bytes (RNF-06).</summary>
    public const int TamanoSal = 16;
    /// <summary>Tamaño del hash en bytes (RNF-06).</summary>
    public const int TamanoHash = 32;
    /// <summary>Edad mínima de un huésped (RF-08).</summary>
    public const int EdadMinima = 18;
    /// <summary>NIT de consumidor final.</summary>
    public const string NitConsumidorFinal = "CF";
    /// <summary>Decimales de los montos (RN-11).</summary>
    public const int DecimalesMoneda = 2;

    /// <summary>Genera un correlativo, p. ej. <c>RES-000012</c> (RN-15).</summary>
    public static string FormatearCorrelativo(string prefijo, int numero) =>
        prefijo + numero.ToString("D" + DigitosCorrelativo, CultureInfo.InvariantCulture);

    /// <summary>Da formato de quetzales: <c>Q 1,250.00</c>.</summary>
    public static string FormatearMoneda(decimal monto) => "Q " + monto.ToString("N2", Cultura);

    /// <summary>Redondea a 2 decimales con <see cref="MidpointRounding.AwayFromZero"/> (RN-11).</summary>
    public static decimal Redondear(decimal monto) =>
        Math.Round(monto, DecimalesMoneda, MidpointRounding.AwayFromZero);

    private static CultureInfo CrearCultura()
    {
        try { return new CultureInfo("es-GT"); }
        catch (CultureNotFoundException) { return CultureInfo.InvariantCulture; }
    }
}
