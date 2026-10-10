using Hotel.Core.Interfaces;

namespace Hotel.Core.Comun;

/// <summary>Reloj controlable para pruebas y para la semilla de datos.</summary>
/// <remarks>RN-02, RN-07</remarks>
public sealed class RelojFijo : IReloj
{
    /// <summary>Crea el reloj en la fecha indicada.</summary>
    public RelojFijo(DateTime ahora) => Ahora = ahora;

    /// <summary>Fecha y hora actuales; se puede modificar.</summary>
    public DateTime Ahora { get; set; }
}
