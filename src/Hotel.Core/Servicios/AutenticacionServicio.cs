using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Inicio y cierre de sesión; conserva el usuario actual y los permisos por rol.</summary>
/// <remarks>RF-01, RN-13, RNF-06</remarks>
public class AutenticacionServicio : ServicioBase
{
    private readonly IRepositorio<Usuario> _usuarios;

    /// <summary>Crea el servicio.</summary>
    public AutenticacionServicio(IRepositorio<Usuario> usuarios) => _usuarios = usuarios;

    /// <summary>Usuario con sesión iniciada, o null.</summary>
    public Usuario? UsuarioActual { get; private set; }

    /// <summary>True si la sesión actual es de un Administrador.</summary>
    public bool EsAdministrador => UsuarioActual?.Rol == RolUsuario.Administrador;

    /// <summary>Verifica las credenciales e inicia la sesión.</summary>
    /// <remarks>RF-01</remarks>
    public Resultado<Usuario> IniciarSesion(string nombreUsuario, string contrasena)
    {
        var usuario = _usuarios.Buscar(u =>
            string.Equals(u.NombreUsuario, nombreUsuario?.Trim(), StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        if (usuario is null || !usuario.VerificarContrasena(contrasena ?? ""))
            return Resultado<Usuario>.Fallo("Usuario o contraseña incorrectos.");
        if (!usuario.Activo)
            return Resultado<Usuario>.Fallo("El usuario está desactivado. Contacte al administrador.");
        UsuarioActual = usuario;
        return Resultado<Usuario>.Ok(usuario, $"Bienvenido, {usuario.Nombres}.");
    }

    /// <summary>Cierra la sesión actual.</summary>
    /// <remarks>RF-01</remarks>
    public void CerrarSesion() => UsuarioActual = null;

    /// <summary>Lanza <see cref="ReglaNegocioException"/> si el usuario actual no es Administrador.</summary>
    /// <remarks>RF-02, RF-24, RN-13</remarks>
    public void ExigirAdministrador(string accion)
    {
        if (!EsAdministrador)
            throw new ReglaNegocioException($"Solo el Administrador puede {accion}.");
    }
}
