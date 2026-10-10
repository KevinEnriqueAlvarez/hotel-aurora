# 04 · Matriz de trazabilidad

Problema → Requisitos → UML → Clases → Código C# → Interfaz → Archivos → Pruebas.

| RF | CU | Clases de dominio | Servicio.Método | Vista / Formulario | Archivo JSON | Prueba(s) |
|---|---|---|---|---|---|---|
| RF-01 | CU-01 | Usuario | AutenticacionServicio.IniciarSesion / CerrarSesion | FormLogin, FormPrincipal | usuarios.json | AutenticacionServicioTests, UsuarioTests |
| RF-02 | CU-12 | Usuario, Persona | UsuarioServicio.Registrar / Editar / Desactivar / RestablecerContrasena | VistaUsuarios, FormUsuario | usuarios.json | UsuarioServicioTests, UsuarioTests |
| RF-03 | CU-02 | Habitacion | HabitacionServicio.Registrar | VistaHabitaciones, FormHabitacion | habitaciones.json | HabitacionServicioTests, HabitacionTests |
| RF-04 | CU-02 | Habitacion | HabitacionServicio.Editar | VistaHabitaciones, FormHabitacion | habitaciones.json | HabitacionServicioTests |
| RF-05 | CU-02 | Habitacion | HabitacionServicio.Eliminar | VistaHabitaciones | habitaciones.json | HabitacionServicioTests |
| RF-06 | CU-02 | Habitacion | HabitacionServicio.Buscar | VistaHabitaciones | habitaciones.json | HabitacionServicioTests |
| RF-07 | CU-02 | Habitacion | HabitacionServicio.CambiarEstado / Liberar | VistaHabitaciones, VistaRecepcion | habitaciones.json | HabitacionTests, HabitacionServicioTests |
| RF-08 | CU-03 | Huesped, Persona | HuespedServicio.Registrar | VistaHuespedes, FormHuesped | huespedes.json | HuespedServicioTests, HuespedTests, ValidadorTests |
| RF-09 | CU-03 | Huesped | HuespedServicio.Editar | VistaHuespedes, FormHuesped | huespedes.json | HuespedServicioTests |
| RF-10 | CU-03 | Huesped | HuespedServicio.Eliminar | VistaHuespedes | huespedes.json | HuespedServicioTests |
| RF-11 | CU-03 | Huesped | HuespedServicio.Buscar | VistaHuespedes | huespedes.json | HuespedServicioTests |
| RF-12 | CU-11 | Huesped, Reserva, Factura | HuespedServicio.HistorialEstadias | FormHuespedDetalle | huespedes.json, reservas.json, facturas.json | HuespedServicioTests |
| RF-13 | CU-04 | Habitacion, Reserva | ReservaServicio.BuscarDisponibles | FormReserva | habitaciones.json, reservas.json | ReservaServicioTests |
| RF-14 | CU-04 | Reserva | ReservaServicio.Crear | FormReserva | reservas.json | ReservaServicioTests, ReservaTests |
| RF-15 | CU-05 | Reserva | ReservaServicio.Modificar | FormReserva | reservas.json | ReservaServicioTests |
| RF-16 | CU-05 | Reserva | ReservaServicio.Confirmar | VistaReservas | reservas.json | ReservaTests, ReservaServicioTests |
| RF-17 | CU-05 | Reserva | ReservaServicio.Cancelar | VistaReservas, FormEntradaTexto | reservas.json | ReservaTests, ReservaServicioTests |
| RF-18 | CU-11 | Reserva | ReservaServicio.Buscar | VistaReservas | reservas.json | ReservaServicioTests |
| RF-19 | CU-06 | Reserva, Habitacion | EstadiaServicio.CheckIn | VistaRecepcion | reservas.json, habitaciones.json | EstadiaServicioTests |
| RF-20 | CU-07 | Reserva, CargoAdicional, ServicioAdicional | EstadiaServicio.AgregarCargo | VistaRecepcion, FormCargo | reservas.json, servicios.json | EstadiaServicioTests |
| RF-21 | CU-08 | Reserva, Factura, Habitacion | EstadiaServicio.CheckOut | VistaRecepcion, FormCheckOut | reservas.json, facturas.json, habitaciones.json | EstadiaServicioTests |
| RF-22 | CU-08 | Factura, LineaFactura, ConfiguracionHotel | Factura.Generar (vía EstadiaServicio.CheckOut) | FormCheckOut, FormFacturaDetalle | facturas.json | FacturaTests, EstadiaServicioTests |
| RF-23 | CU-09 | Pago, PagoEfectivo, PagoTarjeta, PagoTransferencia | FacturacionServicio.RegistrarPago | FormPago | facturas.json | FacturaTests, PagoTests, FacturacionServicioTests |
| RF-24 | CU-10 | Factura | FacturacionServicio.Anular | VistaFacturacion, FormEntradaTexto | facturas.json | FacturaTests, FacturacionServicioTests |
| RF-25 | CU-11 | Factura | FacturacionServicio.Buscar / ResumenPeriodo | VistaFacturacion | facturas.json | FacturacionServicioTests |
| RF-26 | CU-11 | Factura | — (ImpresorFactura sobre PrintDocument) | FormFacturaDetalle | facturas.json | Prueba manual CP-30 |

## Reglas de negocio

| RN | Clase / método que la implementa | Prueba(s) |
|---|---|---|
| RN-01 | HabitacionServicio, HuespedServicio, UsuarioServicio (unicidad) | HabitacionServicioTests, HuespedServicioTests, UsuarioServicioTests |
| RN-02 | ReservaServicio.ValidarReglas, Reserva.CambiarFechas | ReservaServicioTests |
| RN-03 | Reserva.SeSolapaCon, ReservaServicio | ReservaTests, ReservaServicioTests |
| RN-04 | ReservaServicio.BuscarDisponibles / Crear | ReservaServicioTests |
| RN-05 | Reserva.Confirmar / Cancelar / RegistrarCheckIn / RegistrarCheckOut | ReservaTests |
| RN-06 | ReservaServicio.Modificar | ReservaServicioTests |
| RN-07 | EstadiaServicio.CheckIn | EstadiaServicioTests |
| RN-08 | Reserva.AgregarCargo, EstadiaServicio.AgregarCargo | ReservaTests, EstadiaServicioTests |
| RN-09 | EstadiaServicio.CheckOut, Reserva.NochesCobradas | EstadiaServicioTests, ReservaTests |
| RN-10 | Habitacion.CambiarEstado | HabitacionTests |
| RN-11 | Factura.Generar | FacturaTests |
| RN-12 | Factura.RegistrarPago, Pago.Validar | FacturaTests, PagoTests |
| RN-13 | Factura.Anular, FacturacionServicio.Anular | FacturaTests, FacturacionServicioTests |
| RN-14 | HabitacionServicio.Eliminar, HuespedServicio.Eliminar | HabitacionServicioTests, HuespedServicioTests |
| RN-15 | Constantes.FormatearCorrelativo | ReservaServicioTests, EstadiaServicioTests |

## RNF

| RNF | Implementación | Prueba(s) |
|---|---|---|
| RNF-01 | JsonConfig | RepositorioJsonTests |
| RNF-02 | RepositorioJson (persistencia inmediata) | RepositorioJsonTests |
| RNF-03 | RepositorioJson (escritura atómica y respaldo) | RepositorioJsonTests |
| RNF-04 | RepositorioJson en memoria | DatosSemillaTests |
| RNF-05 | Tema/Paleta, FormPrincipal | Manual |
| RNF-06 | Usuario.EstablecerContrasena | UsuarioTests |
| RNF-07 | Estructura UI → Core | Revisión de código |
| RNF-08 | Comentarios XML | Revisión de código |
| RNF-09 | Program.cs (manejador global), RegistroErrores | Manual |
