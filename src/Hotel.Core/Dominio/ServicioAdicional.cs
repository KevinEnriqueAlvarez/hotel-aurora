namespace Hotel.Core.Dominio;

/// <summary>Servicio del catálogo que puede cargarse a una estadía.</summary>
/// <remarks>RF-20</remarks>
public class ServicioAdicional : EntidadBase
{
    /// <summary>Nombre del servicio.</summary>
    public string Nombre { get; set; } = "";
    /// <summary>Categoría.</summary>
    public CategoriaServicio Categoria { get; set; } = CategoriaServicio.Otro;
    /// <summary>Precio unitario en quetzales.</summary>
    public decimal Precio { get; set; }

    /// <inheritdoc />
    public override IEnumerable<string> Validar()
    {
        if (string.IsNullOrWhiteSpace(Nombre)) yield return "El nombre del servicio es obligatorio.";
        if (Precio <= 0) yield return "El precio debe ser mayor que cero.";
    }
}
