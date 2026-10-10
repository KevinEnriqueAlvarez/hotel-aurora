using Hotel.Core.Datos;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Raíz de composición: crea todos los servicios sobre un <see cref="ContextoDatos"/> y un reloj.</summary>
/// <remarks>RNF-07</remarks>
public sealed class ServiciosHotel
{
    /// <summary>Compone los servicios inyectando repositorios, reloj y configuración.</summary>
    public ServiciosHotel(ContextoDatos contexto, IReloj reloj)
    {
        Autenticacion = new AutenticacionServicio(contexto.Usuarios);
        Usuarios = new UsuarioServicio(contexto.Usuarios, Autenticacion, reloj);
        Habitaciones = new HabitacionServicio(contexto.Habitaciones, contexto.Reservas, Autenticacion, reloj);
        Huespedes = new HuespedServicio(contexto.Huespedes, contexto.Reservas, contexto.Facturas, contexto.Habitaciones, reloj);
        Reservas = new ReservaServicio(contexto.Reservas, contexto.Habitaciones, contexto.Huespedes, reloj, contexto.Configuracion);
        Estadias = new EstadiaServicio(contexto.Reservas, contexto.Habitaciones, contexto.Huespedes,
            contexto.Servicios, contexto.Facturas, reloj, contexto.Configuracion);
        Facturacion = new FacturacionServicio(contexto.Facturas, Autenticacion, reloj);
        Reportes = new ReporteServicio(contexto.Reservas, contexto.Habitaciones, contexto.Facturas, reloj);
        Configuracion = contexto.Configuracion;
        Reloj = reloj;
    }

    /// <summary>Sesión y permisos.</summary>
    public AutenticacionServicio Autenticacion { get; }
    /// <summary>Gestión de usuarios.</summary>
    public UsuarioServicio Usuarios { get; }
    /// <summary>Gestión de habitaciones.</summary>
    public HabitacionServicio Habitaciones { get; }
    /// <summary>Gestión de huéspedes.</summary>
    public HuespedServicio Huespedes { get; }
    /// <summary>Gestión de reservas.</summary>
    public ReservaServicio Reservas { get; }
    /// <summary>Check-in, cargos y check-out.</summary>
    public EstadiaServicio Estadias { get; }
    /// <summary>Facturación y pagos.</summary>
    public FacturacionServicio Facturacion { get; }
    /// <summary>Indicadores del tablero.</summary>
    public ReporteServicio Reportes { get; }
    /// <summary>Configuración del hotel.</summary>
    public Dominio.ConfiguracionHotel Configuracion { get; }
    /// <summary>Reloj usado por los servicios.</summary>
    public IReloj Reloj { get; }
}
