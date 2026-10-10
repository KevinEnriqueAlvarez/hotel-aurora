# 02 · Requisitos

## Requisitos funcionales

| ID | Módulo | Requisito |
|---|---|---|
| RF-01 | Acceso | Iniciar y cerrar sesión con usuario y contraseña; el menú se adapta al rol. |
| RF-02 | Acceso | Gestionar usuarios (registrar, editar, desactivar, restablecer contraseña). Solo Administrador. |
| RF-03 | Habitaciones | Registrar habitación: número único, piso, tipo, capacidad, tarifa por noche, descripción. |
| RF-04 | Habitaciones | Editar habitación. |
| RF-05 | Habitaciones | Eliminar habitación sin reservas, o desactivarla si tiene historial. |
| RF-06 | Habitaciones | Consultar y filtrar por tipo, estado, piso y texto. |
| RF-07 | Habitaciones | Cambiar estado operativo (Disponible ↔ Mantenimiento) y liberar (Limpieza → Disponible). |
| RF-08 | Huéspedes | Registrar huésped con documento único (DPI o pasaporte), datos personales y NIT. |
| RF-09 | Huéspedes | Editar huésped. |
| RF-10 | Huéspedes | Eliminar huésped sin reservas, o desactivarlo si tiene historial. |
| RF-11 | Huéspedes | Buscar por nombre o documento. |
| RF-12 | Huéspedes | Consultar historial de estadías (fechas, habitación, total facturado). |
| RF-13 | Reservas | Consultar disponibilidad por rango de fechas, cantidad de personas y tipo. |
| RF-14 | Reservas | Crear reserva (Pendiente o Confirmada) validando reglas RN-02 a RN-04. |
| RF-15 | Reservas | Modificar fechas, habitación o personas de una reserva Pendiente/Confirmada. |
| RF-16 | Reservas | Confirmar reserva. |
| RF-17 | Reservas | Cancelar reserva con motivo obligatorio. |
| RF-18 | Reservas | Listar y filtrar reservas por estado, rango de fechas y huésped. |
| RF-19 | Check-in/out | Realizar check-in con asignación o reasignación de habitación. |
| RF-20 | Check-in/out | Registrar cargos adicionales (catálogo de servicios) durante la estadía. |
| RF-21 | Check-in/out | Realizar check-out: genera la factura y deja la habitación en Limpieza. |
| RF-22 | Facturación | Generar factura: alojamiento + servicios adicionales − descuento + impuestos. |
| RF-23 | Facturación | Registrar pagos (efectivo, tarjeta, transferencia), incluidos pagos parciales. |
| RF-24 | Facturación | Anular factura con motivo (solo Administrador). |
| RF-25 | Facturación | Consultar historial de facturación con filtros y totales del periodo. |
| RF-26 | Facturación | Vista previa e impresión de factura. |

## Requisitos no funcionales

| ID | Requisito |
|---|---|
| RNF-01 | Persistencia en archivos JSON legibles (indentados, enums como texto, tildes sin escapar). |
| RNF-02 | Los datos permanecen al cerrar y volver a abrir la aplicación. |
| RNF-03 | Escritura atómica; un archivo dañado no cierra la aplicación (se respalda y se avisa). |
| RNF-04 | Respuesta menor a 1 s con cientos de registros. |
| RNF-05 | Interfaz consistente con la paleta definida; usable a 1280×720. |
| RNF-06 | Contraseñas almacenadas con hash PBKDF2 (SHA-256, sal aleatoria, 100 000 iteraciones). |
| RNF-07 | Arquitectura en capas; formularios sin lógica de negocio. |
| RNF-08 | Código comentado en español con referencias a RF/RN. |
| RNF-09 | Ningún error inesperado muestra trazas técnicas: mensaje amigable y registro en `Datos/errores.log`. |

## Reglas de negocio

| ID | Regla |
|---|---|
| RN-01 | Únicos: número de habitación, documento de huésped, nombre de usuario. |
| RN-02 | Reserva: entrada ≥ hoy (al crear), salida > entrada, máximo `MaxNochesPorReserva` noches, personas ≤ capacidad. |
| RN-03 | Solapamiento: una reserva activa (Pendiente, Confirmada, EnCurso) bloquea la habitación si `entradaA < salidaB && entradaB < salidaA`. La salida el mismo día de la entrada de otra **no** se solapa. |
| RN-04 | Habitaciones en Mantenimiento o inactivas no se ofrecen para reservar. |
| RN-05 | Transiciones de reserva: Pendiente → Confirmada → EnCurso → Finalizada; Pendiente/Confirmada → Cancelada (motivo obligatorio). Cualquier otra lanza `ReglaNegocioException`. |
| RN-06 | Modificar solo reservas Pendiente/Confirmada, revalidando RN-02 a RN-04 y excluyéndose a sí misma. |
| RN-07 | Check-in: reserva Confirmada y hoy dentro de [entrada, salida); habitación Disponible. Puede reasignarse a otra habitación disponible con capacidad suficiente (se actualiza la tarifa pactada). Habitación → Ocupada. |
| RN-08 | Cargos adicionales solo en reservas EnCurso, cantidad > 0. |
| RN-09 | Check-out: solo EnCurso; noches cobradas = máx(1, días reales entre check-in y check-out); genera factura; reserva → Finalizada; habitación → Limpieza. |
| RN-10 | Estado de habitación: Ocupada solo cambia por check-in/check-out; Limpieza → Disponible (liberar); Disponible ↔ Mantenimiento (manual). «Reservada hoy» no se guarda: se calcula. |
| RN-11 | Factura: Subtotal = alojamiento + cargos; Descuento = Subtotal × %desc (0 – `DescuentoMaximo`); Base = Subtotal − Descuento; IVA = Base × %IVA; Impuesto de turismo = Alojamiento × (1 − %desc) × %Turismo; Total = Base + IVA + Turismo. Redondeo a 2 decimales (`AwayFromZero`). |
| RN-12 | Pagos: monto > 0 y ≤ saldo. Efectivo exige recibido ≥ monto (calcula cambio); tarjeta exige 4 dígitos y autorización; transferencia exige banco y referencia. Saldo 0 → Pagada. |
| RN-13 | Anulación: solo Administrador, motivo obligatorio; una factura anulada no admite pagos y nunca se elimina. |
| RN-14 | No se elimina físicamente un huésped o una habitación con reservas: se desactiva. |
| RN-15 | Correlativos: `RES-000001`, `FAC-000001`. |

### Validaciones de datos (`Validador`)

| Dato | Regla |
|---|---|
| DPI | 13 dígitos |
| Pasaporte | 6–20 caracteres alfanuméricos |
| NIT | `CF`, o dígitos + verificador (0–9 o K), módulo 11 |
| Correo | `MailAddress.TryCreate` |
| Teléfono | 8–15 dígitos, `+` inicial opcional |
| Nombres | 2–60 caracteres, letras (con tildes y ñ) y espacios |
| Contraseña | mínimo 8 caracteres, una mayúscula y un número |
| Huésped | mayor de 18 años |
