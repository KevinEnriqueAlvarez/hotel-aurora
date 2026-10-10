using Hotel.Core.Dominio;

namespace Hotel.Tests.Servicios;

/// <summary>Inicio y cierre de sesión (RF-01).</summary>
public sealed class AutenticacionServicioTests : IDisposable
{
    private readonly Escenario _e = new();

    public void Dispose() => _e.Dispose();

    [Fact]
    public void IniciarSesion_CredencialesCorrectas_DejaUsuarioActual()
    {
        _e.S.Autenticacion.CerrarSesion();
        var r = _e.S.Autenticacion.IniciarSesion("admin", "Admin123*");
        Assert.True(r.Exito);
        Assert.Equal("admin", _e.S.Autenticacion.UsuarioActual!.NombreUsuario);
        Assert.True(_e.S.Autenticacion.EsAdministrador);
    }

    [Fact]
    public void IniciarSesion_UsuarioEnOtraCapitalizacion_Funciona()
    {
        _e.S.Autenticacion.CerrarSesion();
        Assert.True(_e.S.Autenticacion.IniciarSesion("ADMIN", "Admin123*").Exito);
    }

    [Fact]
    public void IniciarSesion_ContrasenaIncorrecta_Falla()
    {
        _e.S.Autenticacion.CerrarSesion();
        var r = _e.S.Autenticacion.IniciarSesion("admin", "mala");
        Assert.False(r.Exito);
        Assert.Null(_e.S.Autenticacion.UsuarioActual);
    }

    [Fact]
    public void IniciarSesion_UsuarioInexistente_FallaConMensajeGenerico()
    {
        var r = _e.S.Autenticacion.IniciarSesion("nadie", "Admin123*");
        Assert.False(r.Exito);
        Assert.Equal("Usuario o contraseña incorrectos.", r.Mensaje);
    }

    [Fact]
    public void CerrarSesion_LimpiaElUsuarioActual()
    {
        _e.S.Autenticacion.CerrarSesion();
        Assert.Null(_e.S.Autenticacion.UsuarioActual);
        Assert.False(_e.S.Autenticacion.EsAdministrador);
    }

    [Fact]
    public void IniciarSesion_Recepcionista_NoEsAdministrador()
    {
        _e.IniciarComoRecepcionista();
        Assert.Equal(RolUsuario.Recepcionista, _e.S.Autenticacion.UsuarioActual!.Rol);
        Assert.False(_e.S.Autenticacion.EsAdministrador);
    }
}
