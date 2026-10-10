namespace Hotel.UI.Tema;

/// <summary>Fuentes de la aplicación (Segoe UI y Segoe MDL2 Assets para íconos).</summary>
/// <remarks>RNF-05; sección 10.2: tipografía</remarks>
public static class Tipografia
{
    private const string Familia = "Segoe UI";
    private const string FamiliaIconos = "Segoe MDL2 Assets";

    /// <summary>Texto general: Segoe UI 10.</summary>
    public static readonly Font Texto = new(Familia, 10f);
    /// <summary>Etiquetas: Segoe UI Semibold 11.</summary>
    public static readonly Font Etiqueta = new(Familia + " Semibold", 11f);
    /// <summary>Etiquetas pequeñas: Segoe UI Semibold 9.</summary>
    public static readonly Font EtiquetaPequena = new(Familia + " Semibold", 9f);
    /// <summary>Título de tarjeta: 16.</summary>
    public static readonly Font TituloTarjeta = new(Familia + " Semibold", 16f);
    /// <summary>Título de vista: 20.</summary>
    public static readonly Font TituloVista = new(Familia + " Semibold", 20f);
    /// <summary>Valores destacados: 22 Bold.</summary>
    public static readonly Font Valor = new(Familia, 22f, FontStyle.Bold);
    /// <summary>Ayudas y notas: 9.</summary>
    public static readonly Font Pequena = new(Familia, 9f);

    /// <summary>Fuente de íconos del tamaño indicado.</summary>
    public static Font Icono(float tamano = 12f) => new(FamiliaIconos, tamano);
}
