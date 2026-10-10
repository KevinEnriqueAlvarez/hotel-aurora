using Hotel.Core.Comun;
using Hotel.Core.Dominio;
using Hotel.Core.Interfaces;

namespace Hotel.Core.Servicios;

/// <summary>CRUD, búsqueda e historial de estadías de huéspedes.</summary>
/// <remarks>RF-08..RF-12, RN-01, RN-14</remarks>
public class HuespedServicio : ServicioBase
{
    private readonly IRepositorio<Huesped> _huespedes;
    private readonly IRepositorio<Reserva> _reservas;
    private readonly IRepositorio<Factura> _facturas;
    private readonly IRepositorio<Habitacion> _habitaciones;
    private readonly IReloj _reloj;

    /// <summary>Crea el servicio.</summary>
    public HuespedServicio(IRepositorio<Huesped> huespedes, IRepositorio<Reserva> reservas,
        IRepositorio<Factura> facturas, IRepositorio<Habitacion> habitaciones, IReloj reloj)
    {
        _huespedes = huespedes;
        _reservas = reservas;
        _facturas = facturas;
        _habitaciones = habitaciones;
        _reloj = reloj;
    }

    /// <summary>Busca un huésped por Id (incluye inactivos).</summary>
    public Huesped? ObtenerPorId(int id) => _huespedes.ObtenerPorId(id);

    /// <summary>Todos los huéspedes, incluidos los inactivos (para mostrar nombres en listados).</summary>
    public IReadOnlyList<Huesped> ObtenerTodos() => _huespedes.ObtenerTodos(incluirInactivos: true);

    /// <summary>Registra un huésped con documento único.</summary>
    /// <remarks>RF-08, RN-01</remarks>
    public Resultado<Huesped> Registrar(Huesped huesped) => Ejecutar(() =>
    {
        Normalizar(huesped);
        ExigirValido(huesped.Validar(_reloj.Hoy));
        ExigirDocumentoUnico(huesped);
        huesped.FechaCreacion = _reloj.Ahora;
        huesped.Activo = true;
        _huespedes.Agregar(huesped);
        return huesped;
    }, "Huésped registrado.");

    /// <summary>Edita los datos de un huésped.</summary>
    /// <remarks>RF-09, RN-01</remarks>
    public Resultado Editar(Huesped datos) => Ejecutar(() =>
    {
        var existente = _huespedes.ObtenerPorId(datos.Id) ?? throw new ReglaNegocioException("El huésped no existe.");
        Normalizar(datos);
        existente.Nombres = datos.Nombres;
        existente.Apellidos = datos.Apellidos;
        existente.TipoDocumento = datos.TipoDocumento;
        existente.NumeroDocumento = datos.NumeroDocumento;
        existente.Telefono = datos.Telefono;
        existente.Correo = datos.Correo;
        existente.Nacionalidad = datos.Nacionalidad;
        existente.FechaNacimiento = datos.FechaNacimiento;
        existente.Nit = datos.Nit;
        existente.Notas = datos.Notas;
        ExigirValido(existente.Validar(_reloj.Hoy));
        ExigirDocumentoUnico(existente);
        _huespedes.Actualizar(existente);
    }, "Huésped actualizado.");

    /// <summary>Elimina al huésped si no tiene reservas; si tiene historial, lo desactiva.</summary>
    /// <remarks>RF-10, RN-14</remarks>
    public Resultado Eliminar(int id)
    {
        var huesped = _huespedes.ObtenerPorId(id);
        if (huesped is null) return Resultado.Fallo("El huésped no existe.");
        if (_reservas.Buscar(r => r.HuespedId == id).Any())
        {
            huesped.Activo = false;
            _huespedes.Actualizar(huesped);
            return Resultado.Ok("El huésped tiene historial de reservas: se desactivó en lugar de eliminarse.");
        }
        _huespedes.Eliminar(id);
        return Resultado.Ok("Huésped eliminado.");
    }

    /// <summary>Busca activos por nombre o documento (cadena vacía = todos).</summary>
    /// <remarks>RF-11</remarks>
    public IReadOnlyList<Huesped> Buscar(string? texto, bool incluirInactivos = false)
    {
        var t = texto?.Trim();
        return _huespedes.ObtenerTodos(incluirInactivos)
            .Where(h => string.IsNullOrEmpty(t)
                || h.NombreCompleto.Contains(t, StringComparison.OrdinalIgnoreCase)
                || h.NumeroDocumento.Contains(t, StringComparison.OrdinalIgnoreCase))
            .OrderBy(h => h.Apellidos).ThenBy(h => h.Nombres)
            .ToList();
    }

    /// <summary>Estadías del huésped (reservas En curso o Finalizadas) con el total facturado.</summary>
    /// <remarks>RF-12</remarks>
    public IReadOnlyList<EstadiaHistorial> HistorialEstadias(int huespedId)
    {
        var habitaciones = _habitaciones.ObtenerTodos(incluirInactivos: true).ToDictionary(h => h.Id);
        var facturas = _facturas.Buscar(f => f.HuespedId == huespedId && f.Estado != EstadoFactura.Anulada)
            .GroupBy(f => f.ReservaId).ToDictionary(g => g.Key, g => g.Sum(f => f.Total));

        return _reservas
            .Buscar(r => r.HuespedId == huespedId && r.Estado is EstadoReserva.EnCurso or EstadoReserva.Finalizada)
            .OrderByDescending(r => r.FechaEntrada)
            .Select(r => new EstadiaHistorial(
                r.Codigo,
                (r.FechaCheckIn ?? r.FechaEntrada).Date,
                (r.FechaCheckOut ?? r.FechaSalida).Date,
                habitaciones.TryGetValue(r.HabitacionId, out var h) ? h.Numero : "—",
                r.Estado,
                facturas.GetValueOrDefault(r.Id)))
            .ToList();
    }

    private static void Normalizar(Huesped h)
    {
        h.Nombres = h.Nombres?.Trim() ?? "";
        h.Apellidos = h.Apellidos?.Trim() ?? "";
        h.NumeroDocumento = h.NumeroDocumento?.Trim().ToUpperInvariant() ?? "";
        h.Correo = h.Correo?.Trim() ?? "";
        h.Nit = string.IsNullOrWhiteSpace(h.Nit) ? Constantes.NitConsumidorFinal : Validador.Normalizar(h.Nit);
    }

    private void ExigirDocumentoUnico(Huesped huesped)
    {
        var repetido = _huespedes.Buscar(h => h.Id != huesped.Id &&
            string.Equals(h.NumeroDocumento, huesped.NumeroDocumento, StringComparison.OrdinalIgnoreCase)).Any();
        if (repetido) throw new ReglaNegocioException($"Ya existe un huésped con el documento {huesped.NumeroDocumento}.");
    }
}
