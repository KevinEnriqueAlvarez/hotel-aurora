# 06 · Plan de pruebas

Ejecución automatizada: `dotnet test` → **255 pruebas, 255 superadas, 0 con error**.

## (a) Pruebas automatizadas (xUnit)

Convención de nombres: `Metodo_Escenario_ResultadoEsperado`. Se usan `RelojFijo` y carpetas temporales (se eliminan con `IDisposable`).

| Carpeta | Clase de prueba | Pruebas | Qué verifica | RF / RN |
|---|---|---:|---|---|
| Dominio | `ValidadorTests` | 40 | DPI, pasaporte, NIT (verificador numérico y K), correo, teléfono, nombres, contraseña | RF-02, RF-08 |
| Dominio | `ReservaTests` | 27 | Transiciones válidas e inválidas, noches, cargos, `SeSolapaCon` con casos borde, noches cobradas | RF-14..RF-21, RN-03, RN-05, RN-08, RN-09 |
| Dominio | `HabitacionTests` | 8 | Transiciones de estado y validación | RF-03, RF-07, RN-10 |
| Dominio | `HuespedTests` | 7 | Mayoría de edad, NIT, documento, pasaporte | RF-08 |
| Dominio | `FacturaTests` | 15 | RN-11 con y sin descuento, redondeo, pagos parciales y totales, pago mayor al saldo, anulación bloquea pagos | RF-22..RF-24, RN-11..RN-13 |
| Dominio | `PagoTests` | 9 | Polimorfismo: validación y detalle de efectivo, tarjeta y transferencia | RF-23, RN-12 |
| Dominio | `UsuarioTests` | 7 | Hash PBKDF2, sal distinta, verificación, política de contraseña | RF-01, RF-02, RNF-06 |
| Datos | `RepositorioJsonTests` | 15 | Id autoincremental, persistencia al reabrir, aislamiento de copias, archivo dañado con respaldo y aviso, escritura atómica, tildes y enums como texto | RNF-01..RNF-03 |
| Datos | `PersistenciaEntidadesTests` | 10 | Ida y vuelta de cada entidad, `private set`, `Pago` polimórfico, tarjeta sin número completo | RNF-01, RNF-02 |
| Servicios | `AutenticacionServicioTests` | 6 | Inicio y cierre de sesión, credenciales inválidas, rol | RF-01 |
| Servicios | `UsuarioServicioTests` | 12 | Registro, duplicados, permisos, desactivar, último administrador, restablecer contraseña | RF-02, RN-01 |
| Servicios | `HabitacionServicioTests` | 18 | CRUD, número único, eliminar vs desactivar, filtros, estados, «reservada hoy», permisos | RF-03..RF-07, RN-01, RN-10, RN-14 |
| Servicios | `HuespedServicioTests` | 12 | Registro y validaciones, documento único, eliminar vs desactivar, búsqueda, historial | RF-08..RF-12, RN-01, RN-14 |
| Servicios | `ReservaServicioTests` | 27 | Rechaza solapamiento, capacidad, fecha pasada, exceso de noches, mantenimiento; modificar excluyéndose a sí misma; disponibilidad; correlativos | RF-13..RF-18, RN-02..RN-06, RN-15 |
| Servicios | `EstadiaServicioTests` | 20 | Check-in cambia estados y reasigna; cargos; check-out genera la factura correcta y deja la habitación en Limpieza | RF-19..RF-22, RN-07..RN-09, RN-11 |
| Servicios | `FacturacionServicioTests` | 11 | Pagos parciales/totales, pago mayor al saldo, anulación por rol, resumen del periodo, filtros | RF-23..RF-25, RN-12, RN-13 |
| Servicios | `DatosSemillaTests` | 11 | Semilla coherente (20 habitaciones, ~40 reservas en todos los estados, facturas parciales y anulada), idempotencia, persistencia, consultas < 1 s, indicadores y series | RNF-02, RNF-04, E1 |

## (b) Casos de prueba manuales

Credenciales: `admin / Admin123*` (Administrador) y `recepcion / Recepcion123*` (Recepcionista). Los datos de prueba se generan al primer arranque. Las columnas «Resultado obtenido» y «Evidencia» se completan al ejecutar cada caso.

| ID | RF | Precondición | Pasos | Datos de entrada | Resultado esperado | Resultado obtenido | Evidencia |
|---|---|---|---|---|---|---|---|
| CP-01 | RF-01 | App abierta en el login | Escribir credenciales y pulsar Ingresar (o Enter) | admin / Admin123* | Se abre la ventana principal con el menú completo, incluido «Usuarios» | | |
| CP-02 | RF-01 | App abierta en el login | Iniciar sesión como recepcionista | recepcion / Recepcion123* | El menú **no** muestra «Usuarios» | | |
| CP-03 | RF-01 | Login abierto | Ingresar con contraseña incorrecta | admin / mala | Mensaje en línea «Usuario o contraseña incorrectos»; no se abre la sesión | | |
| CP-04 | RF-01 | Sesión iniciada | Pulsar «Cerrar sesión» y confirmar | — | Pide confirmación y vuelve al login | | |
| CP-05 | RF-02 | Sesión de administrador | Usuarios → Nuevo → completar y Guardar | usuario `maria`, rol Recepcionista, clave `Clave123` | El usuario aparece en la lista como Activo | | |
| CP-06 | RF-02 | Existe el usuario `maria` | Registrar otro usuario con el mismo nombre | usuario `MARIA` | Error «Ya existe el usuario» | | |
| CP-07 | RF-02 | Existe `maria` | Editar rol; Restablecer contraseña; Desactivar | clave nueva `Nueva456` | Cambios guardados; usuario desactivado no puede iniciar sesión | | |
| CP-08 | RF-03 | Administrador | Habitaciones → Nuevo → Guardar | N.º 501, piso 5, Doble, cap. 2, Q 600 | La habitación aparece Disponible | | |
| CP-09 | RF-03 | Existe la habitación 501 | Registrar otra con N.º 501 | 501 | Error «Ya existe la habitación 501» | | |
| CP-10 | RF-04 | Habitación 501 | Editar tarifa | Q 650 | Tarifa actualizada; el estado no cambia | | |
| CP-11 | RF-05 | Habitación 501 sin reservas | Eliminar y confirmar | — | Se elimina físicamente | | |
| CP-12 | RF-05 | Habitación con historial (101) | Eliminar y confirmar | — | Mensaje «se desactivó»; aparece solo con «Incluir inactivas» | | |
| CP-13 | RF-06 | Datos de semilla | Filtrar por tipo, estado, piso y texto | Tipo Suite; Estado Limpieza; Piso 2; texto «jardín» | La tabla muestra solo las coincidencias y el contador se actualiza | | |
| CP-14 | RF-07 | Habitación Disponible y otra en Limpieza | Mantenimiento → quitar; Liberar | — | Disponible ↔ Mantenimiento; Limpieza → Disponible | | |
| CP-15 | RF-08 | Sesión iniciada | Huéspedes → Nuevo → completar | Nombres, DPI 13 dígitos, correo, nacimiento 1990 | Huésped en la lista con NIT «CF» | | |
| CP-16 | RF-08 | Formulario de huésped abierto | Probar datos inválidos | DPI `123`; nacimiento hace 17 años; NIT `10000004` | Cada campo muestra su error en tiempo real y no deja guardar | | |
| CP-17 | RF-09 | Huésped existente | Editar teléfono | 44411122 | Cambio visible en la tabla | | |
| CP-18 | RF-10 | Huésped sin reservas y otro con reservas | Eliminar a ambos | — | El primero se elimina; el segundo se desactiva | | |
| CP-19 | RF-11 | Datos de semilla | Escribir en el buscador | «Alvarado» y un DPI | Filtra al escribir por nombre o documento | | |
| CP-20 | RF-12 | Huésped con estadías | Ver detalle → pestaña «Historial de estadías» | — | Muestra fechas, habitación y total facturado | | |
| CP-21 | RF-13 | Datos de semilla | Reservas → Nuevo → fechas, personas y tipo → Buscar disponibilidad | Entrada hoy+3, salida hoy+5, 2 personas | Lista solo habitaciones libres con total estimado | | |
| CP-22 | RF-14 | Disponibilidad consultada | Elegir habitación y huésped; «Guardar pendiente» y «Guardar y confirmar» | — | Se crean `RES-######` Pendiente y Confirmada | | |
| CP-23 | RF-14 | Habitación con reserva del 15 al 17 | Reservar la misma habitación del 16 al 18 | — | Error por solapamiento. Del 17 al 19 **sí** se permite | | |
| CP-24 | RF-14 | Formulario de reserva | Habitación de capacidad 2 con 4 personas; entrada pasada; habitación en mantenimiento | — | Se rechaza con el mensaje de la regla (RN-02/RN-04) | | |
| CP-25 | RF-15 | Reserva Pendiente | Editar y cambiar fechas, habitación o personas | +1 noche | Se guarda revalidando; sigue sin chocar consigo misma | | |
| CP-26 | RF-16 | Reserva Pendiente | Confirmar | — | Estado Confirmada | | |
| CP-27 | RF-17 | Reserva Pendiente/Confirmada | Cancelar sin motivo y luego con motivo | «Cambio de planes» | Sin motivo no deja continuar; con motivo queda Cancelada | | |
| CP-28 | RF-18 | Datos de semilla | Filtrar por estado y rango de fechas | Estado EnCurso; desde/hasta | Lista filtrada y contador | | |
| CP-29 | RF-19 | Reserva Confirmada que entra hoy | Recepción → Realizar check-in; elegir otra habitación | — | Reserva En curso, habitación Ocupada; la tarifa se actualiza si cambió | | |
| CP-30 | RF-20 | Huésped en casa | Agregar cargo | Desayuno × 2 | El cargo aparece en el estado de cuenta; con cantidad 0 se rechaza | | |
| CP-31 | RF-21 | Huésped en casa | Check-out y confirmar | — | Reserva Finalizada, habitación en «Por limpiar» y se abre la factura | | |
| CP-32 | RF-22 | Check-out en curso | Aplicar descuento del 10 % antes de confirmar | 10 % | Totales según RN-11; descuento mayor al 30 % lo limita el control | | |
| CP-33 | RF-23 | Factura Emitida | Registrar pago parcial en efectivo y luego el saldo con tarjeta | Parcial Q 500; luego saldo | Saldo disminuye; con saldo 0 queda Pagada | | |
| CP-34 | RF-23 | Factura con saldo Q 100 | Intentar pagar más del saldo | Q 150 | Error «El monto excede el saldo» | | |
| CP-35 | RF-24 | Sesión de administrador | Anular factura con motivo | «Datos incorrectos» | Queda Anulada, sigue en el historial y no admite pagos | | |
| CP-36 | RF-24 | Sesión de recepcionista | Abrir Facturación | — | No aparece el botón Anular | | |
| CP-37 | RF-25 | Datos de semilla | Filtrar por fechas y estado | Estado Emitida | Historial filtrado y totales del periodo (facturado, cobrado, por cobrar) | | |
| CP-38 | RF-26 | Factura abierta | «Vista previa» e «Imprimir» | — | Se muestra la vista previa con líneas, totales, pagos y marca ANULADA si aplica | | |
| CP-39 | RNF-02, RNF-03, RNF-09 | App cerrada | Dañar `Datos/habitaciones.json` y abrir la app; revisar `errores.log` | texto inválido | La app abre, avisa del archivo dañado y conserva una copia `.danado-*` | | |
| CP-40 | RNF-05 | Pantalla 1366×768 | Maximizar y reducir la ventana | — | La interfaz es usable y respeta la paleta | | |
| CP-41 | E1 | Datos de semilla | Abrir Inicio y cambiar el selector Ingresos / Reservas / Ocupación | — | KPI, dona y barras de cobro; el gráfico cambia de serie | | |
