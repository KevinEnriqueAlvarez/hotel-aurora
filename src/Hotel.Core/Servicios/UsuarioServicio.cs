using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>Gestión de usuarios (solo Administrador).</summary>
/// <remarks>RF-02, RN-01, RNF-06</remarks>
public class UsuarioServicio : ServicioBase
{
    private readonly IRepositorio<Usuario> _usuarios;
    private readonly AutenticacionServicio _auth;
    private readonly IReloj _reloj;

    /// <summary>Crea el servicio.</summary>
    public UsuarioServicio(IRepositorio<Usuario> usuarios, AutenticacionServicio auth, IReloj reloj)
    {
        _usuarios = usuarios;
        _auth = auth;
        _reloj = reloj;
    }

    /// <summary>Lista todos los usuarios, activos e inactivos.</summary>
    public IReadOnlyList<Usuario> ObtenerTodos() =>
        _usuarios.ObtenerTodos(incluirInactivos: true).OrderBy(u => u.NombreUsuario).ToList();

    /// <summary>Busca un usuario por Id.</summary>
    public Usuario? ObtenerPorId(int id) => _usuarios.ObtenerPorId(id);

    /// <summary>Registra un usuario con su contraseña; el primer usuario del sistema no requiere sesión.</summary>
    /// <remarks>RF-02, RN-01</remarks>
    public Resultado<Usuario> Registrar(Usuario usuario, string contrasena) => Ejecutar(() =>
    {
        if (_usuarios.ObtenerTodos(incluirInactivos: true).Count > 0) _auth.ExigirAdministrador("registrar usuarios");
        usuario.EstablecerContrasena(contrasena);
        ExigirValido(usuario.Validar());
        ExigirNombreUnico(usuario);
        usuario.FechaCreacion = _reloj.Ahora;
        usuario.Activo = true;
        _usuarios.Agregar(usuario);
        return usuario;
    }, "Usuario registrado.");

    /// <summary>Edita datos y rol; conserva la contraseña guardada.</summary>
    /// <remarks>RF-02, RN-01</remarks>
    public Resultado Editar(Usuario datos) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("editar usuarios");
        var existente = _usuarios.ObtenerPorId(datos.Id) ?? throw new ReglaNegocioException("El usuario no existe.");
        CopiarDatos(datos, existente);
        ExigirValido(existente.Validar());
        ExigirNombreUnico(existente);
        if (existente.Rol != RolUsuario.Administrador || !existente.Activo) ExigirOtroAdministrador(existente.Id);
        _usuarios.Actualizar(existente);
    }, "Usuario actualizado.");

    /// <summary>Desactiva un usuario (no se elimina).</summary>
    /// <remarks>RF-02</remarks>
    public Resultado Desactivar(int id) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("desactivar usuarios");
        var usuario = _usuarios.ObtenerPorId(id) ?? throw new ReglaNegocioException("El usuario no existe.");
        if (_auth.UsuarioActual?.Id == id) throw new ReglaNegocioException("No puede desactivar su propio usuario.");
        ExigirOtroAdministrador(id);
        usuario.Activo = false;
        _usuarios.Actualizar(usuario);
    }, "Usuario desactivado.");

    /// <summary>Asigna una contraseña nueva.</summary>
    /// <remarks>RF-02, RNF-06</remarks>
    public Resultado RestablecerContrasena(int id, string nueva) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("restablecer contraseñas");
        var usuario = _usuarios.ObtenerPorId(id) ?? throw new ReglaNegocioException("El usuario no existe.");
        usuario.EstablecerContrasena(nueva);
        _usuarios.Actualizar(usuario);
    }, "Contraseña restablecida.");

    private void ExigirNombreUnico(Usuario usuario)
    {
        var repetido = _usuarios.Buscar(u => u.Id != usuario.Id &&
            string.Equals(u.NombreUsuario, usuario.NombreUsuario, StringComparison.OrdinalIgnoreCase)).Any();
        if (repetido) throw new ReglaNegocioException($"Ya existe el usuario «{usuario.NombreUsuario}».");
    }

    private void ExigirOtroAdministrador(int idExcluido)
    {
        var hayOtro = _usuarios.Buscar(u => u.Id != idExcluido && u.Activo && u.Rol == RolUsuario.Administrador).Any();
        if (!hayOtro) throw new ReglaNegocioException("Debe quedar al menos un administrador activo.");
    }

    private static void CopiarDatos(Usuario origen, Usuario destino)
    {
        destino.Nombres = origen.Nombres;
        destino.Apellidos = origen.Apellidos;
        destino.TipoDocumento = origen.TipoDocumento;
        destino.NumeroDocumento = origen.NumeroDocumento;
        destino.Telefono = origen.Telefono;
        destino.Correo = origen.Correo;
        destino.NombreUsuario = origen.NombreUsuario;
        destino.Rol = origen.Rol;
        destino.Activo = origen.Activo;
    }
}
