using System.ComponentModel;
using Hotel.Core.Servicios;
using Hotel.UI.Controles;
using Hotel.UI.Formularios;

namespace Hotel.UI.Vistas;

/// <summary>Gestión de usuarios (solo Administrador).</summary>
/// <remarks>RF-02; CU-12</remarks>
public class VistaUsuarios : VistaListadoBase
{
    private sealed record Fila(
        int Id,
        [property: DisplayName("Usuario")] string Usuario,
        [property: DisplayName("Nombre")] string Nombre,
        [property: DisplayName("Rol")] string Rol,
        [property: DisplayName("Teléfono")] string Telefono,
        [property: DisplayName("Correo")] string Correo,
        [property: DisplayName("Estado")] string Estado) : IFilaConId;

    private readonly ServiciosHotel? _s;
    private readonly BotonModerno _btnClave = new() { Text = "Restablecer contraseña", Icono = "", Variante = VarianteBoton.Secundario, Width = 230, Height = 40 };

    /// <summary>Constructor para el Diseñador de Visual Studio.</summary>
    public VistaUsuarios() { }

    /// <summary>Crea la vista con los servicios de la aplicación.</summary>
    public VistaUsuarios(ServiciosHotel servicios)
    {
        _s = servicios;
        AyudaBusqueda = "Buscar usuario o nombre...";
        PermiteDetalle = false;
        TextoEliminar = "Desactivar";
        _btnClave.Click += (_, _) => RestablecerContrasena();
        AgregarAccion(_btnClave);
    }

    /// <inheritdoc />
    public override string TituloVista => "Usuarios";

    /// <inheritdoc />
    protected override void CargarDatos()
    {
        if (_s is null) return;
        var texto = TextoBusqueda;
        MostrarFilas(_s.Usuarios.ObtenerTodos()
            .Where(u => string.IsNullOrEmpty(texto)
                || u.NombreUsuario.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || u.NombreCompleto.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .Select(u => new Fila(u.Id, u.NombreUsuario, u.NombreCompleto, u.Rol.ToString(), u.Telefono, u.Correo,
                u.Activo ? "Activo" : "Inactivo"))
            .ToList());
    }

    /// <inheritdoc />
    protected override void AlCambiarSeleccion() => _btnClave.Enabled = FilaSeleccionada<Fila>() is not null;

    /// <inheritdoc />
    protected override void Nuevo()
    {
        if (_s is null) return;
        using var form = new FormUsuario(_s);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Editar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        var usuario = _s.Usuarios.ObtenerPorId(fila.Id);
        if (usuario is null) return;
        using var form = new FormUsuario(_s, usuario);
        if (form.ShowDialog(FindForm()) == DialogResult.OK) CargarDatos();
    }

    /// <inheritdoc />
    protected override void Eliminar()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        if (!Mensajes.Confirmar(FindForm(), $"¿Desactivar al usuario «{fila.Usuario}»? Ya no podrá iniciar sesión.",
                "Desactivar usuario", peligro: true, textoAceptar: "Desactivar")) return;
        Mensajes.MostrarResultado(FindForm(), _s.Usuarios.Desactivar(fila.Id));
        CargarDatos();
    }

    private void RestablecerContrasena()
    {
        if (_s is null || FilaSeleccionada<Fila>() is not { } fila) return;
        using var form = new FormEntradaTexto($"Restablecer contraseña — {fila.Usuario}",
            "Escriba la contraseña nueva (mínimo 8 caracteres, una mayúscula y un número):",
            esContrasena: true, textoGuardar: "Restablecer");
        if (form.ShowDialog(FindForm()) != DialogResult.OK) return;
        Mensajes.MostrarResultado(FindForm(), _s.Usuarios.RestablecerContrasena(fila.Id, form.Valor));
    }
}
