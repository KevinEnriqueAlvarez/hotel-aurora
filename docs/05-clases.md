# 05 · Descripción de clases

Diagrama completo: `docs/uml/clases.puml`.

## Comun
| Clase | Responsabilidad |
|---|---|
| `Resultado` / `Resultado<T>` | Respuesta uniforme de los servicios: `Exito`, `Mensaje`, `Errores`, `Valor`. |
| `ReglaNegocioException` | Señala una regla de negocio violada; los servicios la convierten en `Resultado.Fallo`. |
| `Validador` (estático) | Validaciones de DPI, pasaporte, NIT (módulo 11), correo, teléfono, nombres y contraseña. |
| `Constantes` | Cultura es-GT, formatos, prefijos de correlativo, iteraciones PBKDF2, mayoría de edad. |
| `RelojSistema` / `RelojFijo` | Implementaciones de `IReloj`: producción y pruebas/semilla. |

## Dominio
| Clase | Responsabilidad / atributos clave | Relaciones |
|---|---|---|
| `EntidadBase` (abstracta) | `Id`, `FechaCreacion`, `Activo`, `Validar()`. | Implementa `IValidable`. |
| `Persona` (abstracta) | Datos personales y validación común. | Hereda de `EntidadBase`. |
| `Huesped` | Nacionalidad, nacimiento, NIT. Valida mayoría de edad y NIT. | Hereda de `Persona`. |
| `Usuario` | Nombre de usuario, rol, hash+sal PBKDF2. | Hereda de `Persona`. |
| `Habitacion` | Número, piso, tipo, capacidad, tarifa; estado con transiciones validadas. | Hereda de `EntidadBase`. |
| `Reserva` | Ciclo de vida (RN-05), cargos, noches y solapamiento. | Referencia a `Huesped` y `Habitacion` por Id; compone `CargoAdicional`. |
| `ServicioAdicional` | Catálogo de servicios cobrables. | — |
| `CargoAdicional` | Objeto de valor: consumo registrado en una estadía. | Compuesto por `Reserva`. |
| `Factura` | Generación con RN-11, pagos (RN-12) y anulación (RN-13). | Compone `LineaFactura` y `Pago`. |
| `Pago` (abstracta) + `PagoEfectivo`, `PagoTarjeta`, `PagoTransferencia` | Polimorfismo: `Metodo`, `Detalle()`, `Validar()`. JSON polimórfico con discriminador `tipo`. | Compuestos por `Factura`. |
| `ConfiguracionHotel` | Datos del hotel, IVA, turismo, máximos. | Leída por servicios y `Factura`. |

## Datos
| Clase | Responsabilidad |
|---|---|
| `JsonConfig` | Opciones de `System.Text.Json` (indentado, camelCase, enums como texto, tildes sin escapar). |
| `RepositorioJson<T>` | Repositorio genérico en memoria con persistencia atómica, Id = máx + 1, respaldo de archivos dañados. Devuelve copias para aislar las ediciones. |
| `ContextoDatos` | Agrupa los repositorios y la configuración; recibe la carpeta de datos. |
| `DatosSemilla` | Genera datos de prueba usando los servicios y un `RelojFijo`. |

## Servicios
| Clase | Responsabilidad |
|---|---|
| `AutenticacionServicio` | Sesión y verificación de contraseña. |
| `UsuarioServicio` | CRUD de usuarios (solo Administrador). |
| `HabitacionServicio` | CRUD, filtros y estados de habitación. |
| `HuespedServicio` | CRUD, búsqueda e historial de estadías. |
| `ReservaServicio` | Disponibilidad, creación, modificación, confirmación y cancelación. |
| `EstadiaServicio` | Check-in, cargos y check-out con factura. |
| `FacturacionServicio` | Pagos, anulación, historial y resumen del periodo. |
| `ReporteServicio` | Indicadores y series para el tablero de inicio (E1). |
| `ServiciosHotel` | Raíz de composición: crea todos los servicios con repositorios, reloj y configuración. |
| `ServicioBase` | Convierte `ReglaNegocioException` en `Resultado` (patrón común de los servicios). |

## Interfaz (Hotel.UI)
| Clase | Responsabilidad |
|---|---|
| `Paleta`, `Tipografia`, `GraficosUtil`, `Estilizador` | Tema visual, dibujo GDI+ y aplicación a tablas y campos. |
| `ControlDibujado` | Base de los controles personalizados (doble búfer, fondo del contenedor). |
| `PanelTarjeta`, `BotonModerno`, `BotonNavegacion`, `InsigniaEstado`, `SelectorSegmentado` | Controles GDI+ base. |
| `GraficoBarras`, `GraficoDona`, `TarjetaKpi`, `BarraProgreso` | Controles del tablero (E1). |
| `FormularioBase`, `VistaListadoBase` | Herencia visual para diálogos y vistas de listado. |
| `DialogoMensaje`, `Mensajes` | Mensajes temáticos. |
| `FormLogin`, `FormPrincipal` | Acceso y navegación. |
| `VistaBase` → `VistaListadoBase` → `VistaHabitaciones`, `VistaHuespedes`, `VistaReservas`, `VistaFacturacion`, `VistaUsuarios` | Vistas de listado (herencia). |
| `VistaInicio`, `VistaRecepcion` | Vistas con diseño propio. |
| `FormHabitacion`, `FormHuesped`, `FormHuespedDetalle`, `FormUsuario`, `FormReserva`, `FormCheckIn`, `FormCargo`, `FormCheckOut`, `FormPago`, `FormFacturaDetalle`, `FormEntradaTexto` | Diálogos que heredan de `FormularioBase`. |
| `ImpresorFactura` | `PrintDocument` para la vista previa e impresión (RF-26). |

Diagrama de la interfaz: `docs/uml/clases-ui.puml`.
