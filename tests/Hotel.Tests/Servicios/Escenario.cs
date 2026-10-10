using Hotel.Core.Comun;
using Hotel.Core.Datos;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Tests.Servicios;

/// <summary>Entorno de prueba: carpeta temporal, reloj fijo, servicios y sesión de administrador.</summary>
public sealed class Escenario : IDisposable
{
    private readonly CarpetaTemporal _carpeta = new();
    private int _contadorDocumento = 1;

    /// <summary>Hoy para las pruebas: 10/03/2026.</summary>
    public static readonly DateTime Hoy = new(2026, 3, 10);

    public Escenario()
    {
        Reloj = new RelojFijo(Hoy.AddHours(9));
        Contexto = new ContextoDatos(_carpeta.Ruta);
        S = new ServiciosHotel(Contexto, Reloj);
        S.Usuarios.Registrar(Admin(), "Admin123*");
        S.Autenticacion.IniciarSesion("admin", "Admin123*");
    }

    public RelojFijo Reloj { get; }
    public ContextoDatos Contexto { get; }
    public ServiciosHotel S { get; }

    public void Dispose() => _carpeta.Dispose();

    private static Usuario Admin() => Persona("Admin", "Hotel", "admin", RolUsuario.Administrador, "1000000000001");

    public static Usuario Persona(string nombres, string apellidos, string usuario, RolUsuario rol, string dpi) => new()
    {
        Nombres = nombres, Apellidos = apellidos, NombreUsuario = usuario, Rol = rol, NumeroDocumento = dpi,
        TipoDocumento = TipoDocumento.Dpi, Telefono = "55512345", Correo = $"{usuario}@hotel.gt"
    };

    /// <summary>Registra una habitación vía servicio.</summary>
    public Habitacion Habitacion(string numero = "101", int capacidad = 2, decimal tarifa = 500m,
        TipoHabitacion tipo = TipoHabitacion.Doble, int piso = 1)
    {
        var r = S.Habitaciones.Registrar(new Habitacion
            { Numero = numero, Piso = piso, Tipo = tipo, Capacidad = capacidad, TarifaNoche = tarifa, Descripcion = "Prueba" });
        Assert.True(r.Exito, r.Mensaje);
        return r.Valor!;
    }

    /// <summary>Registra un huésped con documento distinto en cada llamada.</summary>
    public Huesped Huesped(string nombres = "Ana", string apellidos = "López", string nit = "CF")
    {
        var r = S.Huespedes.Registrar(new Huesped
        {
            Nombres = nombres, Apellidos = apellidos, TipoDocumento = TipoDocumento.Dpi,
            NumeroDocumento = $"{2000000000000 + _contadorDocumento++}", Telefono = "55512345",
            Correo = "huesped@correo.com", FechaNacimiento = new DateTime(1990, 1, 1), Nit = nit
        });
        Assert.True(r.Exito, r.Mensaje);
        return r.Valor!;
    }

    /// <summary>Crea una reserva y exige éxito.</summary>
    public Reserva Reserva(Huesped huesped, Habitacion habitacion, int diasDesdeHoy, int noches, int personas = 2, bool confirmar = true)
    {
        var entrada = Reloj.Ahora.Date.AddDays(diasDesdeHoy);
        var r = S.Reservas.Crear(huesped.Id, habitacion.Id, entrada, entrada.AddDays(noches), personas, confirmar);
        Assert.True(r.Exito, r.Mensaje);
        return r.Valor!;
    }

    /// <summary>Agrega un servicio al catálogo.</summary>
    public ServicioAdicional Servicio(string nombre = "Desayuno", decimal precio = 60m)
    {
        var s = new ServicioAdicional { Nombre = nombre, Categoria = CategoriaServicio.Restaurante, Precio = precio };
        Contexto.Servicios.Agregar(s);
        return s;
    }

    /// <summary>Reserva confirmada con llegada hoy y check-in realizado.</summary>
    public Reserva EstadiaEnCurso(int noches = 3, decimal tarifa = 500m)
    {
        var h = Habitacion(numero: "E" + Contexto.Habitaciones.ObtenerTodos(true).Count, tarifa: tarifa);
        var r = Reserva(Huesped(), h, 0, noches);
        var ci = S.Estadias.CheckIn(r.Id);
        Assert.True(ci.Exito, ci.Mensaje);
        return S.Reservas.ObtenerPorId(r.Id)!;
    }

    /// <summary>Cambia de sesión a un recepcionista (lo registra si no existe).</summary>
    public void IniciarComoRecepcionista()
    {
        if (S.Usuarios.ObtenerTodos().All(u => u.NombreUsuario != "recep"))
            Assert.True(S.Usuarios.Registrar(Persona("Rosa", "Recepción", "recep", RolUsuario.Recepcionista, "1000000000002"), "Recep123*").Exito);
        S.Autenticacion.CerrarSesion();
        Assert.True(S.Autenticacion.IniciarSesion("recep", "Recep123*").Exito);
    }
}
