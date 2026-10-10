using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Datos;

/// <summary>Agrupa los repositorios y la configuración que viven en una carpeta de datos.</summary>
/// <remarks>RNF-01, RNF-02, RNF-03, RNF-07</remarks>
public class ContextoDatos
{
    private readonly List<string> _avisos = [];
    private readonly string _archivoConfiguracion;

    /// <summary>Abre la carpeta indicada (la crea si no existe).</summary>
    public ContextoDatos(string carpeta)
    {
        Carpeta = carpeta;
        Directory.CreateDirectory(carpeta);
        _archivoConfiguracion = Path.Combine(carpeta, "configuracion.json");

        Usuarios = Abrir<Usuario>("usuarios.json");
        Habitaciones = Abrir<Habitacion>("habitaciones.json");
        Huespedes = Abrir<Huesped>("huespedes.json");
        Servicios = Abrir<ServicioAdicional>("servicios.json");
        Reservas = Abrir<Reserva>("reservas.json");
        Facturas = Abrir<Factura>("facturas.json");

        var configuracion = ArchivoJson.Leer<ConfiguracionHotel>(_archivoConfiguracion, _avisos.Add);
        Configuracion = configuracion ?? new ConfiguracionHotel();
        if (configuracion is null) GuardarConfiguracion();
    }

    /// <summary>Carpeta donde viven los archivos JSON.</summary>
    public string Carpeta { get; }
    /// <summary>Usuarios del sistema.</summary>
    public IRepositorio<Usuario> Usuarios { get; }
    /// <summary>Habitaciones.</summary>
    public IRepositorio<Habitacion> Habitaciones { get; }
    /// <summary>Huéspedes.</summary>
    public IRepositorio<Huesped> Huespedes { get; }
    /// <summary>Catálogo de servicios adicionales.</summary>
    public IRepositorio<ServicioAdicional> Servicios { get; }
    /// <summary>Reservas.</summary>
    public IRepositorio<Reserva> Reservas { get; }
    /// <summary>Facturas.</summary>
    public IRepositorio<Factura> Facturas { get; }
    /// <summary>Datos y parámetros del hotel.</summary>
    public ConfiguracionHotel Configuracion { get; }
    /// <summary>Avisos de archivos dañados detectados al abrir (RNF-03).</summary>
    public IReadOnlyList<string> Avisos => _avisos;

    /// <summary>Persiste la configuración.</summary>
    public void GuardarConfiguracion() => ArchivoJson.Escribir(_archivoConfiguracion, Configuracion);

    private RepositorioJson<T> Abrir<T>(string archivo) where T : EntidadBase =>
        new(Path.Combine(Carpeta, archivo), _avisos.Add);
}
