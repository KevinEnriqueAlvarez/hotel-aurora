using Hotel.Core.Servicios;

namespace Hotel.UI.Vistas;

public class VistaInicio : VistaBase { public VistaInicio(ServiciosHotel s) { } public override string TituloVista => "Inicio"; }
public class VistaReservas : VistaBase { public VistaReservas(ServiciosHotel s) { } public override string TituloVista => "Reservas"; }
public class VistaRecepcion : VistaBase { public VistaRecepcion(ServiciosHotel s) { } public override string TituloVista => "Recepción"; }
public class VistaFacturacion : VistaBase { public VistaFacturacion(ServiciosHotel s) { } public override string TituloVista => "Facturación"; }
