namespace Hotel.Core.Datos;

/// <summary>Genera datos de prueba coherentes usando los servicios de negocio.</summary>
/// <remarks>RNF-02; sección 9 «DatosSemilla»</remarks>
public static class DatosSemilla
{
    /// <summary>Si no hay usuarios, siembra usuarios, habitaciones, huéspedes, servicios y ~40 reservas.</summary>
    /// <param name="contexto">Contexto de datos destino.</param>
    /// <param name="ahora">Momento actual; la historia se genera hacia atrás desde aquí.</param>
    /// <returns>True si se sembró.</returns>
    public static bool SembrarSiVacio(ContextoDatos contexto, DateTime ahora)
    {
        if (contexto.Usuarios.ObtenerTodos(incluirInactivos: true).Count > 0) return false;
        new SembradorDatos(contexto, ahora).Ejecutar();
        return true;
    }
}
