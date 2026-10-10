using Hotel.Core.Interfaces;

namespace Hotel.Core.Comun;

/// <summary>Reloj de producción: devuelve la hora del equipo.</summary>
/// <remarks>RN-02</remarks>
public sealed class RelojSistema : IReloj
{
    /// <inheritdoc />
    public DateTime Ahora => DateTime.Now;
}
