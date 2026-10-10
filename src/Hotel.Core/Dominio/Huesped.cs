using Hotel.Core.Comun;

namespace Hotel.Core.Dominio;

/// <summary>Huésped del hotel: sujeto de reservas, estadías y facturas.</summary>
/// <remarks>RF-08, RF-09, RF-10, RN-01, RN-14</remarks>
public class Huesped : Persona
{
    /// <summary>Nacionalidad.</summary>
    public string Nacionalidad { get; set; } = "Guatemalteca";
    /// <summary>Fecha de nacimiento.</summary>
    public DateTime FechaNacimiento { get; set; }
    /// <summary>NIT para facturar («CF» por defecto).</summary>
    public string Nit { get; set; } = Constantes.NitConsumidorFinal;
    /// <summary>Notas libres.</summary>
    public string Notas { get; set; } = "";

    /// <inheritdoc />
    public override IEnumerable<string> Validar() => Validar(DateTime.Today);

    /// <summary>Valida usando <paramref name="hoy"/> para calcular la mayoría de edad.</summary>
    /// <remarks>RF-08</remarks>
    public IEnumerable<string> Validar(DateTime hoy)
    {
        foreach (var error in base.Validar()) yield return error;
        if (FechaNacimiento == default || FechaNacimiento.Date > hoy.AddYears(-Constantes.EdadMinima).Date)
            yield return $"El huésped debe ser mayor de {Constantes.EdadMinima} años.";
        if (!Validador.EsNitValido(Nit)) yield return "El NIT no es válido (use CF o un NIT con verificador correcto).";
    }
}
