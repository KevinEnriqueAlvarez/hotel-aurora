using Hotel.Core.Servicios;

namespace Hotel.UI.Vistas;

public class VistaInicio : VistaBase { public VistaInicio(ServiciosHotel s) { } public override string TituloVista => "Inicio"; }
public class VistaHabitaciones : VistaBase { public VistaHabitaciones(ServiciosHotel s) { } public override string TituloVista => "Habitaciones"; }
public class VistaHuespedes : VistaBase { public VistaHuespedes(ServiciosHotel s) { } public override string TituloVista => "Huéspedes"; }
public class VistaReservas : VistaBase { public VistaReservas(ServiciosHotel s) { } public override string TituloVista => "Reservas"; }
public class VistaRecepcion : VistaBase { public VistaRecepcion(ServiciosHotel s) { } public override string TituloVista => "Recepción"; }
public class VistaFacturacion : VistaBase { public VistaFacturacion(ServiciosHotel s) { } public override string TituloVista => "Facturación"; }
public class VistaUsuarios : VistaBase { public VistaUsuarios(ServiciosHotel s) { } public override string TituloVista => "Usuarios"; }
