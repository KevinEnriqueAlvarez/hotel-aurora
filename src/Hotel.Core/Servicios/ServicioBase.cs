using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.Core.Servicios;

/// <summary>Utilidades comunes: convierte <see cref="ReglaNegocioException"/> en <see cref="Resultado"/>.</summary>
/// <remarks>RNF-07, RNF-09</remarks>
public abstract class ServicioBase
{
    /// <summary>Ejecuta la acción y devuelve éxito, o fallo si se viola una regla de negocio.</summary>
    protected static Resultado Ejecutar(Action accion, string mensajeOk = "")
    {
        try
        {
            accion();
            return Resultado.Ok(mensajeOk);
        }
        catch (ReglaNegocioException ex)
        {
            return Resultado.Fallo(ex.Errores);
        }
    }

    /// <summary>Ejecuta la función y devuelve su valor, o fallo si se viola una regla de negocio.</summary>
    protected static Resultado<T> Ejecutar<T>(Func<T> funcion, string mensajeOk = "")
    {
        try
        {
            return Resultado<T>.Ok(funcion(), mensajeOk);
        }
        catch (ReglaNegocioException ex)
        {
            return Resultado<T>.Fallo(ex.Errores);
        }
    }

    /// <summary>Lanza <see cref="ReglaNegocioException"/> si hay errores de validación.</summary>
    protected static void ExigirValido(IEnumerable<string> errores)
    {
        var lista = errores.ToList();
        if (lista.Count > 0) throw new ReglaNegocioException(lista);
    }

    /// <summary>Siguiente Id que asignará el repositorio (máximo + 1); base de los correlativos (RN-15).</summary>
    protected static int SiguienteId(IEnumerable<EntidadBase> todas) =>
        (todas.Any() ? todas.Max(e => e.Id) : 0) + 1;
}
