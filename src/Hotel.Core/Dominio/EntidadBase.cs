using Hotel.Core.Interfaces;

namespace Hotel.Core.Dominio;

/// <summary>Base de las entidades persistentes: identidad, auditoría mínima y validación.</summary>
/// <remarks>RNF-07; herencia y polimorfismo de <see cref="Validar"/></remarks>
public abstract class EntidadBase : IValidable
{
    /// <summary>Identificador único (asignado por el repositorio).</summary>
    public int Id { get; set; }

    /// <summary>Fecha de creación.</summary>
    public DateTime FechaCreacion { get; set; }

    /// <summary>False cuando la entidad fue desactivada (RN-14).</summary>
    public bool Activo { get; set; } = true;

    /// <inheritdoc />
    public abstract IEnumerable<string> Validar();
}
