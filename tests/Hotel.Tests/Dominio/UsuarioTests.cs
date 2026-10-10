using Hotel.Core.Comun;
using Hotel.Core.Dominio;

namespace Hotel.Tests.Dominio;

/// <summary>Hash PBKDF2 y verificación de contraseña de <see cref="Usuario"/> (RNF-06).</summary>
public class UsuarioTests
{
    [Fact]
    public void VerificarContrasena_Correcta_DevuelveTrue()
    {
        var u = new Usuario();
        u.EstablecerContrasena("Admin123*");
        Assert.True(u.VerificarContrasena("Admin123*"));
    }

    [Fact]
    public void VerificarContrasena_Incorrecta_DevuelveFalse()
    {
        var u = new Usuario();
        u.EstablecerContrasena("Admin123*");
        Assert.False(u.VerificarContrasena("admin123*"));
    }

    [Fact]
    public void VerificarContrasena_SinContrasenaEstablecida_DevuelveFalse() =>
        Assert.False(new Usuario().VerificarContrasena("Admin123*"));

    [Fact]
    public void EstablecerContrasena_NoGuardaTextoPlano()
    {
        var u = new Usuario();
        u.EstablecerContrasena("Admin123*");
        Assert.DoesNotContain("Admin123", u.HashContrasena);
        Assert.NotEmpty(u.Sal);
    }

    [Fact]
    public void EstablecerContrasena_MismaClaveDosUsuarios_HashDistintoPorSal()
    {
        var a = new Usuario();
        var b = new Usuario();
        a.EstablecerContrasena("Admin123*");
        b.EstablecerContrasena("Admin123*");
        Assert.NotEqual(a.HashContrasena, b.HashContrasena);
        Assert.NotEqual(a.Sal, b.Sal);
    }

    [Fact]
    public void EstablecerContrasena_Debil_LanzaReglaNegocio() =>
        Assert.Throws<ReglaNegocioException>(() => new Usuario().EstablecerContrasena("abc"));

    [Fact]
    public void Validar_UsuarioCompleto_SinErrores()
    {
        var u = new Usuario
        {
            Nombres = "Ana", Apellidos = "López", NumeroDocumento = "1234567890123",
            Telefono = "55512345", Correo = "ana@hotel.gt", NombreUsuario = "ana.lopez"
        };
        u.EstablecerContrasena("Clave123");
        Assert.Empty(u.Validar());
    }
}
