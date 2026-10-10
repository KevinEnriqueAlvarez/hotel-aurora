using System.Text.Json;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Datos;

/// <summary>
/// Repositorio genérico: carga el archivo una vez en memoria y persiste cada cambio.
/// Entrega copias de las entidades para que una edición no cancelada no altere los datos.
/// </summary>
/// <remarks>RNF-01, RNF-02, RNF-03, RNF-04, RN-14</remarks>
public class RepositorioJson<T> : IRepositorio<T> where T : EntidadBase
{
    private readonly string _ruta;
    private readonly List<T> _items;

    /// <summary>Abre (o crea vacío) el repositorio del archivo indicado.</summary>
    public RepositorioJson(string ruta, Action<string>? alAvisar = null)
    {
        _ruta = ruta;
        _items = ArchivoJson.Leer<List<T>>(ruta, alAvisar) ?? [];
    }

    /// <inheritdoc />
    public IReadOnlyList<T> ObtenerTodos(bool incluirInactivos = false) =>
        _items.Where(e => incluirInactivos || e.Activo).Select(Clonar).ToList();

    /// <inheritdoc />
    public T? ObtenerPorId(int id)
    {
        var item = _items.FirstOrDefault(e => e.Id == id);
        return item is null ? null : Clonar(item);
    }

    /// <inheritdoc />
    public IEnumerable<T> Buscar(Func<T, bool> criterio) =>
        _items.Where(criterio).Select(Clonar).ToList();

    /// <inheritdoc />
    public void Agregar(T entidad)
    {
        entidad.Id = (_items.Count == 0 ? 0 : _items.Max(e => e.Id)) + 1;
        var copia = Clonar(entidad);
        _items.Add(copia);
        try { Persistir(); }
        catch { _items.Remove(copia); throw; }
    }

    /// <inheritdoc />
    public void Actualizar(T entidad)
    {
        var indice = _items.FindIndex(e => e.Id == entidad.Id);
        if (indice < 0) throw new KeyNotFoundException($"No existe {typeof(T).Name} con Id {entidad.Id}.");
        _items[indice] = Clonar(entidad);
        Persistir();
    }

    /// <inheritdoc />
    public void Eliminar(int id)
    {
        if (_items.RemoveAll(e => e.Id == id) > 0) Persistir();
    }

    private void Persistir() => ArchivoJson.Escribir(_ruta, _items);

    private static T Clonar(T entidad) =>
        JsonSerializer.Deserialize<T>(JsonSerializer.SerializeToUtf8Bytes(entidad, JsonConfig.Opciones), JsonConfig.Opciones)!;
}
