namespace Hotel.Core.Comun;

/// <summary>Respuesta uniforme de los servicios, sin excepciones hacia la interfaz.</summary>
/// <remarks>RNF-07, RNF-09</remarks>
public class Resultado
{
    /// <summary>Crea un resultado.</summary>
    protected Resultado(bool exito, string mensaje, IEnumerable<string>? errores)
    {
        Exito = exito;
        Mensaje = mensaje;
        Errores = (errores ?? []).ToList();
    }

    /// <summary>Indica si la operación se completó.</summary>
    public bool Exito { get; }

    /// <summary>Mensaje para el usuario.</summary>
    public string Mensaje { get; }

    /// <summary>Detalle de errores de validación.</summary>
    public IReadOnlyList<string> Errores { get; }

    /// <summary>Resultado exitoso.</summary>
    public static Resultado Ok(string mensaje = "") => new(true, mensaje, null);

    /// <summary>Resultado fallido con un mensaje.</summary>
    public static Resultado Fallo(string mensaje) => new(false, mensaje, [mensaje]);

    /// <summary>Resultado fallido con varios errores de validación.</summary>
    public static Resultado Fallo(IEnumerable<string> errores)
    {
        var lista = errores.ToList();
        return new Resultado(false, string.Join(Environment.NewLine, lista), lista);
    }
}

/// <summary>Resultado que además transporta un valor.</summary>
/// <remarks>RNF-07</remarks>
public sealed class Resultado<T> : Resultado
{
    private Resultado(bool exito, string mensaje, IEnumerable<string>? errores, T? valor)
        : base(exito, mensaje, errores) => Valor = valor;

    /// <summary>Valor devuelto cuando la operación tuvo éxito.</summary>
    public T? Valor { get; }

    /// <summary>Resultado exitoso con valor.</summary>
    public static Resultado<T> Ok(T valor, string mensaje = "") => new(true, mensaje, null, valor);

    /// <summary>Resultado fallido con un mensaje.</summary>
    public static new Resultado<T> Fallo(string mensaje) => new(false, mensaje, [mensaje], default);

    /// <summary>Resultado fallido con varios errores.</summary>
    public static new Resultado<T> Fallo(IEnumerable<string> errores)
    {
        var lista = errores.ToList();
        return new Resultado<T>(false, string.Join(Environment.NewLine, lista), lista, default);
    }
}
