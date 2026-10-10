using Hotel.Core.Dominio;

namespace Hotel.Core.Interfaces;

/// <summary>Acceso a una colección persistente de entidades.</summary>
/// <remarks>RNF-01, RNF-02, RNF-03, RNF-07</remarks>
public interface IRepositorio<T> where T : EntidadBase
{
    /// <summary>Lista las entidades; por defecto solo las activas.</summary>
    IReadOnlyList<T> ObtenerTodos(bool incluirInactivos = false);

    /// <summary>Busca por Id (incluye inactivas); null si no existe.</summary>
    T? ObtenerPorId(int id);

    /// <summary>Busca entre todas las entidades (activas e inactivas).</summary>
    IEnumerable<T> Buscar(Func<T, bool> criterio);

    /// <summary>Asigna el Id y persiste.</summary>
    void Agregar(T entidad);

    /// <summary>Reemplaza la entidad con el mismo Id y persiste.</summary>
    void Actualizar(T entidad);

    /// <summary>Elimina físicamente; el servicio decide si desactiva en su lugar (RN-14).</summary>
    void Eliminar(int id);
}
