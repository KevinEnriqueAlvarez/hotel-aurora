using Hotel.Core.Comun;
using Hotel.Core.Datos;
using Hotel.Core.Dominio;
using Hotel.Core.Servicios;

namespace Hotel.Tests.Servicios;

/// <summary>Gestión de usuarios (RF-02, RN-01) y sesión (RF-01).</summary>
public sealed class UsuarioServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    private static Usuario Nuevo(string usuario = "maria", RolUsuario rol = RolUsuario.Recepcionista, string dpi = "3000000000001") =>
        Escenario.Persona("María", "Gómez", usuario, rol, dpi);

    [Fact]
    public void Registrar_PrimerUsuarioSinSesion_Permitido()
    {
        using var carpeta = new CarpetaTemporal();
        var s = new ServiciosHotel(new ContextoDatos(carpeta.Ruta), new RelojFijo(Escenario.Hoy));
        Assert.True(s.Usuarios.Registrar(Nuevo("admin", RolUsuario.Administrador), "Admin123*").Exito);
    }

    [Fact]
    public void Registrar_SegundoUsuarioSinSesion_Rechaza()
    {
        using var carpeta = new CarpetaTemporal();
        var s = new ServiciosHotel(new ContextoDatos(carpeta.Ruta), new RelojFijo(Escenario.Hoy));
        s.Usuarios.Registrar(Nuevo("admin", RolUsuario.Administrador), "Admin123*");
        Assert.False(s.Usuarios.Registrar(Nuevo("otro"), "Clave123").Exito);
    }

    [Fact]
    public void Registrar_Administrador_GuardaHashNoTextoPlano()
    {
        var r = _e.S.Usuarios.Registrar(Nuevo(), "Clave123");
        Assert.True(r.Exito, r.Mensaje);
        Assert.True(r.Valor!.VerificarContrasena("Clave123"));
        Assert.DoesNotContain("Clave123", File.ReadAllText(Path.Combine(_e.Contexto.Carpeta, "usuarios.json")));
    }

    [Fact]
    public void Registrar_NombreDeUsuarioDuplicado_RechazaSinImportarMayusculas()
    {
        _e.S.Usuarios.Registrar(Nuevo("maria"), "Clave123");
        var r = _e.S.Usuarios.Registrar(Nuevo("MARIA", dpi: "3000000000002"), "Clave123");
        Assert.False(r.Exito);
    }

    [Fact]
    public void Registrar_ContrasenaDebil_Rechaza() =>
        Assert.False(_e.S.Usuarios.Registrar(Nuevo(), "abc").Exito);

    [Fact]
    public void Registrar_Recepcionista_RechazaPorPermiso()
    {
        _e.IniciarComoRecepcionista();
        Assert.False(_e.S.Usuarios.Registrar(Nuevo(), "Clave123").Exito);
    }

    [Fact]
    public void Editar_CambiaRolYConservaContrasena()
    {
        var u = _e.S.Usuarios.Registrar(Nuevo(), "Clave123").Valor!;
        u.Rol = RolUsuario.Administrador;
        u.Telefono = "44455566";

        Assert.True(_e.S.Usuarios.Editar(u).Exito);

        var guardado = _e.S.Usuarios.ObtenerPorId(u.Id)!;
        Assert.Equal(RolUsuario.Administrador, guardado.Rol);
        Assert.True(guardado.VerificarContrasena("Clave123"));
    }

    [Fact]
    public void Desactivar_Usuario_NoPuedeIniciarSesion()
    {
        var u = _e.S.Usuarios.Registrar(Nuevo(), "Clave123").Valor!;
        Assert.True(_e.S.Usuarios.Desactivar(u.Id).Exito);
        Assert.False(_e.S.Autenticacion.IniciarSesion("maria", "Clave123").Exito);
    }

    [Fact]
    public void Desactivar_SiMismo_Rechaza() =>
        Assert.False(_e.S.Usuarios.Desactivar(_e.S.Autenticacion.UsuarioActual!.Id).Exito);

    [Fact]
    public void Desactivar_UltimoAdministrador_Rechaza()
    {
        var otro = _e.S.Usuarios.Registrar(Nuevo("otro", RolUsuario.Recepcionista), "Clave123").Valor!;
        var admin = _e.S.Autenticacion.UsuarioActual!;
        otro.Rol = RolUsuario.Administrador;
        _e.S.Usuarios.Editar(otro);
        _e.S.Autenticacion.CerrarSesion();
        _e.S.Autenticacion.IniciarSesion("otro", "Clave123");
        _e.S.Usuarios.Desactivar(admin.Id);          // queda un solo admin activo: "otro"
        Assert.False(_e.S.Usuarios.Editar(new Usuario
        {
            Id = otro.Id, Nombres = otro.Nombres, Apellidos = otro.Apellidos, NumeroDocumento = otro.NumeroDocumento,
            Telefono = otro.Telefono, Correo = otro.Correo, NombreUsuario = otro.NombreUsuario,
            Rol = RolUsuario.Recepcionista
        }).Exito);
    }

    [Fact]
    public void RestablecerContrasena_NuevaClaveFunciona_YLaAnteriorNo()
    {
        var u = _e.S.Usuarios.Registrar(Nuevo(), "Clave123").Valor!;
        Assert.True(_e.S.Usuarios.RestablecerContrasena(u.Id, "Nueva456").Exito);
        Assert.True(_e.S.Autenticacion.IniciarSesion("maria", "Nueva456").Exito);
        Assert.False(_e.S.Autenticacion.IniciarSesion("maria", "Clave123").Exito);
    }

    [Fact]
    public void RestablecerContrasena_Debil_Rechaza()
    {
        var u = _e.S.Usuarios.Registrar(Nuevo(), "Clave123").Valor!;
        Assert.False(_e.S.Usuarios.RestablecerContrasena(u.Id, "corta").Exito);
    }
}
