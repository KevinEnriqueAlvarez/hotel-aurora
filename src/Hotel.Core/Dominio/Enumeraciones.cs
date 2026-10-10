namespace Hotel.Core.Dominio;

/// <summary>Tipo de documento de identificación (RF-08).</summary>
public enum TipoDocumento { Dpi, Pasaporte }

/// <summary>Rol de un usuario del sistema (RF-01, RF-02).</summary>
public enum RolUsuario { Administrador, Recepcionista }

/// <summary>Tipo de habitación (RF-03).</summary>
public enum TipoHabitacion { Sencilla, Doble, Suite, Familiar }

/// <summary>Estado operativo guardado de la habitación (RN-10).</summary>
public enum EstadoHabitacion { Disponible, Ocupada, Limpieza, Mantenimiento }

/// <summary>Estado del ciclo de vida de una reserva (RN-05).</summary>
public enum EstadoReserva { Pendiente, Confirmada, EnCurso, Finalizada, Cancelada }

/// <summary>Categoría de un servicio adicional (RF-20).</summary>
public enum CategoriaServicio { Restaurante, Minibar, Lavanderia, Spa, Transporte, Otro }

/// <summary>Estado de una factura (RN-12, RN-13).</summary>
public enum EstadoFactura { Emitida, Pagada, Anulada }
