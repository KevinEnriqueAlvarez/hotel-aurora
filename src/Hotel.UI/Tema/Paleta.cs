using Hotel.Core.Dominio;

namespace Hotel.UI.Tema;

/// <summary>Paleta de colores de la aplicación (estilo neumórfico suave).</summary>
/// <remarks>RNF-05; sección 10.2</remarks>
public static class Paleta
{
    /// <summary>Fondo de la aplicación.</summary>
    public static readonly Color Fondo = Hex("E6EBF1");
    /// <summary>Tarjetas y paneles.</summary>
    public static readonly Color Tarjeta = Hex("F4F7FA");
    /// <summary>Campos, tablas y chips.</summary>
    public static readonly Color Superficie = Color.White;
    /// <summary>Color base de la sombra difusa.</summary>
    public static readonly Color SombraBase = Hex("A3B1C6");
    /// <summary>Barra lateral, títulos y barras de gráfico.</summary>
    public static readonly Color Marino = Hex("2E3A50");
    /// <summary>Hover y botón activo de la barra lateral.</summary>
    public static readonly Color MarinoHover = Hex("3B4963");
    /// <summary>Acento principal.</summary>
    public static readonly Color Teal = Hex("1FB2B5");
    /// <summary>Degradado de barras destacadas y hover primario.</summary>
    public static readonly Color TealClaro = Hex("4FD1D3");
    /// <summary>Serie de gráfico.</summary>
    public static readonly Color Cian = Hex("38D3E0");
    /// <summary>Serie de gráfico e información.</summary>
    public static readonly Color Azul = Hex("3B8FD9");
    /// <summary>Pestaña o segmento activo.</summary>
    public static readonly Color AzulPestana = Hex("86B9EA");
    /// <summary>Serie de gráfico y texto terciario.</summary>
    public static readonly Color Pizarra = Hex("5A6E8C");
    /// <summary>Texto principal.</summary>
    public static readonly Color TextoPrincipal = Hex("1F2A3C");
    /// <summary>Etiquetas y ayudas.</summary>
    public static readonly Color TextoSecundario = Hex("7A8699");
    /// <summary>Bordes de campos.</summary>
    public static readonly Color Borde = Hex("D5DCE5");
    /// <summary>Fila seleccionada.</summary>
    public static readonly Color SeleccionTabla = Hex("D6F3F4");
    /// <summary>Advertencias.</summary>
    public static readonly Color Advertencia = Hex("F2A93B");
    /// <summary>Errores y acciones destructivas.</summary>
    public static readonly Color Error = Hex("E25C5C");

    /// <summary>Color de la sombra con la transparencia indicada.</summary>
    public static Color Sombra(int alfa) => Color.FromArgb(alfa, SombraBase);

    /// <summary>Color de la insignia («pill») para un estado de habitación, reserva o factura.</summary>
    /// <remarks>Sección 10.2: colores de estado</remarks>
    public static Color ColorDeEstado(string? estado) => estado switch
    {
        nameof(EstadoHabitacion.Disponible) or nameof(EstadoReserva.EnCurso) or nameof(EstadoFactura.Pagada) => Teal,
        nameof(EstadoHabitacion.Ocupada) => Marino,
        "Reservada hoy" or nameof(EstadoReserva.Confirmada) or nameof(EstadoFactura.Emitida) => Azul,
        nameof(EstadoHabitacion.Limpieza) or nameof(EstadoReserva.Finalizada) => Pizarra,
        nameof(EstadoHabitacion.Mantenimiento) or nameof(EstadoReserva.Pendiente) => Advertencia,
        nameof(EstadoReserva.Cancelada) or nameof(EstadoFactura.Anulada) => Error,
        _ => Pizarra
    };

    /// <summary>Texto legible de un estado (separa «EnCurso» en «En curso»).</summary>
    public static string TextoDeEstado(string estado) => estado == nameof(EstadoReserva.EnCurso) ? "En curso" : estado;

    /// <summary>Mezcla dos colores (0 = a, 1 = b).</summary>
    public static Color Mezclar(Color a, Color b, double t) => Color.FromArgb(
        (int)(a.R + (b.R - a.R) * t), (int)(a.G + (b.G - a.G) * t), (int)(a.B + (b.B - a.B) * t));

    /// <summary>Color de fondo visible detrás de un control (primer ancestro opaco).</summary>
    public static Color FondoDe(Control? control)
    {
        for (var c = control?.Parent; c is not null; c = c.Parent)
            if (c.BackColor.A == 255) return c.BackColor;
        return Fondo;
    }

    private static Color Hex(string rgb) => ColorTranslator.FromHtml("#" + rgb);
}
