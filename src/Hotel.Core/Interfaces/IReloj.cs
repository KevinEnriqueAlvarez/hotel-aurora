namespace Hotel.Core.Interfaces;

/// <summary>Abstrae la hora actual para poder fijarla en pruebas y semilla.</summary>
/// <remarks>RN-02, RN-07 (dependen de «hoy»)</remarks>
public interface IReloj
{
    /// <summary>Fecha y hora actuales.</summary>
    DateTime Ahora { get; }

    /// <summary>Fecha de hoy sin hora.</summary>
    DateTime Hoy => Ahora.Date;
}
