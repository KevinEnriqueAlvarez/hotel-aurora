using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>CRUD, filtros y estados de habitación. Los cambios estructurales son del Administrador.</summary>
/// <remarks>RF-03..RF-07, RN-01, RN-10, RN-14</remarks>
public class HabitacionServicio : ServicioBase
{
    private readonly IRepositorio<Habitacion> _habitaciones;
    private readonly IRepositorio<Reserva> _reservas;
    private readonly AutenticacionServicio _auth;
    private readonly IReloj _reloj;

    /// <summary>Crea el servicio.</summary>
    public HabitacionServicio(IRepositorio<Habitacion> habitaciones, IRepositorio<Reserva> reservas,
        AutenticacionServicio auth, IReloj reloj)
    {
        _habitaciones = habitaciones;
        _reservas = reservas;
        _auth = auth;
        _reloj = reloj;
    }

    /// <summary>Busca una habitación por Id (incluye inactivas).</summary>
    public Habitacion? ObtenerPorId(int id) => _habitaciones.ObtenerPorId(id);

    /// <summary>Registra una habitación nueva en estado Disponible.</summary>
    /// <remarks>RF-03, RN-01</remarks>
    public Resultado<Habitacion> Registrar(Habitacion habitacion) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("registrar habitaciones");
        ExigirValido(habitacion.Validar());
        ExigirNumeroUnico(habitacion);
        habitacion.Numero = habitacion.Numero.Trim();
        habitacion.FechaCreacion = _reloj.Ahora;
        habitacion.Activo = true;
        _habitaciones.Agregar(habitacion);
        return habitacion;
    }, "Habitación registrada.");

    /// <summary>Edita los datos de la habitación; el estado no cambia por esta vía.</summary>
    /// <remarks>RF-04, RN-01</remarks>
    public Resultado Editar(Habitacion datos) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("editar habitaciones");
        var existente = _habitaciones.ObtenerPorId(datos.Id) ?? throw new ReglaNegocioException("La habitación no existe.");
        existente.Numero = datos.Numero?.Trim() ?? "";
        existente.Piso = datos.Piso;
        existente.Tipo = datos.Tipo;
        existente.Capacidad = datos.Capacidad;
        existente.TarifaNoche = datos.TarifaNoche;
        existente.Descripcion = datos.Descripcion;
        ExigirValido(existente.Validar());
        ExigirNumeroUnico(existente);
        _habitaciones.Actualizar(existente);
    }, "Habitación actualizada.");

    /// <summary>Elimina la habitación si no tiene reservas; si tiene historial, la desactiva.</summary>
    /// <remarks>RF-05, RN-14</remarks>
    public Resultado Eliminar(int id)
    {
        try
        {
            _auth.ExigirAdministrador("eliminar habitaciones");
            var habitacion = _habitaciones.ObtenerPorId(id) ?? throw new ReglaNegocioException("La habitación no existe.");
            if (habitacion.Estado == EstadoHabitacion.Ocupada)
                throw new ReglaNegocioException("No se puede eliminar ni desactivar una habitación ocupada.");
            if (_reservas.Buscar(r => r.HabitacionId == id).Any())
            {
                habitacion.Activo = false;
                _habitaciones.Actualizar(habitacion);
                return Resultado.Ok("La habitación tiene historial de reservas: se desactivó en lugar de eliminarse.");
            }
            _habitaciones.Eliminar(id);
            return Resultado.Ok("Habitación eliminada.");
        }
        catch (ReglaNegocioException ex)
        {
            return Resultado.Fallo(ex.Errores);
        }
    }

    /// <summary>Consulta habitaciones aplicando los filtros, ordenadas por número.</summary>
    /// <remarks>RF-06</remarks>
    public IReadOnlyList<Habitacion> Buscar(FiltroHabitacion? filtro = null)
    {
        filtro ??= new FiltroHabitacion();
        var texto = filtro.Texto?.Trim();
        return _habitaciones.ObtenerTodos(filtro.IncluirInactivas)
            .Where(h => filtro.Tipo is null || h.Tipo == filtro.Tipo)
            .Where(h => filtro.Estado is null || h.Estado == filtro.Estado)
            .Where(h => filtro.Piso is null || h.Piso == filtro.Piso)
            .Where(h => string.IsNullOrEmpty(texto)
                || h.Numero.Contains(texto, StringComparison.OrdinalIgnoreCase)
                || h.Descripcion.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .OrderBy(h => h.Piso).ThenBy(h => h.Numero, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Alterna Disponible ↔ Mantenimiento (Ocupada solo cambia por check-in/out).</summary>
    /// <remarks>RF-07, RN-10</remarks>
    public Resultado CambiarEstado(int id, EstadoHabitacion nuevo) => Ejecutar(() =>
    {
        _auth.ExigirAdministrador("cambiar el estado de una habitación");
        if (nuevo is not (EstadoHabitacion.Disponible or EstadoHabitacion.Mantenimiento))
            throw new ReglaNegocioException("Manualmente solo se puede pasar a Disponible o Mantenimiento.");
        var habitacion = _habitaciones.ObtenerPorId(id) ?? throw new ReglaNegocioException("La habitación no existe.");
        habitacion.CambiarEstado(nuevo);
        _habitaciones.Actualizar(habitacion);
    }, "Estado actualizado.");

    /// <summary>Limpieza → Disponible.</summary>
    /// <remarks>RF-07, RN-10</remarks>
    public Resultado Liberar(int id) => Ejecutar(() =>
    {
        var habitacion = _habitaciones.ObtenerPorId(id) ?? throw new ReglaNegocioException("La habitación no existe.");
        if (habitacion.Estado != EstadoHabitacion.Limpieza)
            throw new ReglaNegocioException("Solo se pueden liberar habitaciones en limpieza.");
        habitacion.CambiarEstado(EstadoHabitacion.Disponible);
        _habitaciones.Actualizar(habitacion);
    }, "Habitación liberada.");

    /// <summary>Ids de habitaciones con una reserva Pendiente o Confirmada que cubre hoy («Reservada hoy»).</summary>
    /// <remarks>RN-10</remarks>
    public ISet<int> ReservadasHoy()
    {
        var hoy = _reloj.Hoy;
        return _reservas
            .Buscar(r => r.Estado is EstadoReserva.Pendiente or EstadoReserva.Confirmada
                         && r.FechaEntrada <= hoy && hoy < r.FechaSalida)
            .Select(r => r.HabitacionId)
            .ToHashSet();
    }

    private void ExigirNumeroUnico(Habitacion habitacion)
    {
        var repetida = _habitaciones.Buscar(h => h.Id != habitacion.Id &&
            string.Equals(h.Numero, habitacion.Numero.Trim(), StringComparison.OrdinalIgnoreCase)).Any();
        if (repetida) throw new ReglaNegocioException($"Ya existe la habitación {habitacion.Numero}.");
    }
}
