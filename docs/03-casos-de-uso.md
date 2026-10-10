# 03 · Casos de uso

## Actores

| Actor | Descripción |
|---|---|
| Recepcionista | Gestiona huéspedes, reservas, check-in/check-out, cargos adicionales y pagos. |
| Administrador | Todo lo del recepcionista + habitaciones, usuarios y anulación de facturas. |
| Huésped (secundario) | No usa el sistema; es el sujeto de reservas, estadías y facturas. |

Diagrama: `docs/uml/casos-de-uso.puml`.

---

## CU-01 Iniciar sesión
- **Actor:** Recepcionista, Administrador.
- **Precondición:** el usuario existe y está activo.
- **Flujo principal:** 1) Abre la aplicación. 2) Escribe usuario y contraseña. 3) Pulsa Ingresar. 4) El sistema verifica el hash y abre la ventana principal con el menú del rol.
- **Alternos:** A1) Credenciales inválidas → error en línea, no se abre sesión. A2) Usuario inactivo → mensaje de acceso denegado. A3) Cerrar sesión → confirmación y regreso al login.
- **Postcondición:** hay un usuario actual en sesión.
- **RF:** RF-01. **RN:** RNF-06.

## CU-02 Gestionar habitaciones
- **Actor:** Administrador (consulta: ambos).
- **Precondición:** sesión iniciada.
- **Flujo principal:** 1) Abre Habitaciones. 2) Filtra por tipo, estado, piso o texto. 3) Registra o edita una habitación con número, piso, tipo, capacidad, tarifa y descripción. 4) El sistema valida y guarda.
- **Alternos:** A1) Número duplicado → error (RN-01). A2) Eliminar con reservas → se desactiva (RN-14). A3) Pasar a Mantenimiento / liberar desde Limpieza (RN-10). A4) Habitación ocupada → no se puede desactivar ni pasar a mantenimiento.
- **Postcondición:** `habitaciones.json` actualizado.
- **RF:** RF-03, RF-04, RF-05, RF-06, RF-07.

## CU-03 Gestionar huéspedes
- **Actor:** Recepcionista, Administrador.
- **Precondición:** sesión iniciada.
- **Flujo principal:** 1) Abre Huéspedes. 2) Busca por nombre o documento. 3) Registra o edita datos personales, documento y NIT. 4) El sistema valida y guarda. 5) Consulta el detalle con historial de estadías.
- **Alternos:** A1) Documento duplicado (RN-01). A2) NIT o DPI inválidos. A3) Menor de edad. A4) Eliminar con reservas → desactivar (RN-14).
- **Postcondición:** `huespedes.json` actualizado.
- **RF:** RF-08, RF-09, RF-10, RF-11, RF-12.

## CU-04 Crear reserva
- **Actor:** Recepcionista.
- **Precondición:** existen habitaciones activas.
- **Flujo principal:** 1) Indica fechas, personas y tipo. 2) El sistema lista habitaciones disponibles con total estimado. 3) Selecciona habitación. 4) Busca o registra al huésped. 5) Guarda como Pendiente o Confirmada. 6) El sistema asigna el código `RES-######` y copia la tarifa.
- **Alternos:** A1) Fecha pasada o salida ≤ entrada (RN-02). A2) Personas > capacidad. A3) Exceso de noches. A4) Solapamiento (RN-03). A5) Habitación en mantenimiento (RN-04).
- **Postcondición:** reserva en `reservas.json`.
- **RF:** RF-13, RF-14, RF-18. **Incluye:** CU-03 (nuevo huésped).

## CU-05 Modificar / cancelar reserva
- **Actor:** Recepcionista.
- **Precondición:** reserva Pendiente o Confirmada.
- **Flujo principal:** 1) Selecciona la reserva. 2) Modifica fechas, habitación o personas, o la confirma, o la cancela. 3) El sistema revalida (excluyéndola a sí misma) y guarda.
- **Alternos:** A1) Reserva en otro estado → rechazo (RN-05/06). A2) Cancelar sin motivo → rechazo. A3) Nuevas fechas solapadas.
- **Postcondición:** reserva actualizada.
- **RF:** RF-15, RF-16, RF-17.

## CU-06 Realizar check-in
- **Actor:** Recepcionista.
- **Precondición:** reserva Confirmada con llegada hoy.
- **Flujo principal:** 1) Abre Recepción → Llegadas de hoy. 2) Selecciona la reserva y, si lo desea, otra habitación. 3) El sistema valida y marca reserva EnCurso y habitación Ocupada.
- **Alternos:** A1) Fuera del rango [entrada, salida). A2) Habitación no Disponible (en limpieza). A3) Capacidad insuficiente en la reasignada.
- **Postcondición:** reserva EnCurso; habitación Ocupada.
- **RF:** RF-19. **RN:** RN-07.

## CU-07 Registrar cargo adicional
- **Actor:** Recepcionista.
- **Precondición:** reserva EnCurso.
- **Flujo principal:** 1) Selecciona un huésped en casa. 2) Elige un servicio del catálogo y la cantidad. 3) El sistema agrega el cargo con el precio vigente.
- **Alternos:** A1) Cantidad ≤ 0. A2) Reserva no EnCurso.
- **Postcondición:** el cargo queda en la reserva.
- **RF:** RF-20. **RN:** RN-08.

## CU-08 Realizar check-out
- **Actor:** Recepcionista.
- **Precondición:** reserva EnCurso.
- **Flujo principal:** 1) Selecciona el huésped en casa. 2) Revisa el estado de cuenta y aplica descuento opcional. 3) Confirma. 4) El sistema genera la factura, finaliza la reserva y deja la habitación en Limpieza. 5) Muestra la factura.
- **Alternos:** A1) Descuento mayor al máximo. A2) Estancia menor a un día → se cobra 1 noche.
- **Postcondición:** factura `FAC-######` emitida; reserva Finalizada; habitación en Limpieza.
- **RF:** RF-21, RF-22. **RN:** RN-09, RN-11. **Incluye:** CU-09 (opcional).

## CU-09 Registrar pago
- **Actor:** Recepcionista.
- **Precondición:** factura Emitida con saldo.
- **Flujo principal:** 1) Abre la factura. 2) Elige método (efectivo, tarjeta, transferencia). 3) Ingresa los datos. 4) El sistema valida, registra el pago y actualiza el saldo; con saldo 0 la factura queda Pagada.
- **Alternos:** A1) Monto > saldo o ≤ 0. A2) Efectivo recibido < monto. A3) Tarjeta sin 4 dígitos/autorización. A4) Factura anulada.
- **Postcondición:** pago registrado en la factura.
- **RF:** RF-23. **RN:** RN-12.

## CU-10 Anular factura
- **Actor:** Administrador.
- **Precondición:** factura no anulada.
- **Flujo principal:** 1) Abre la factura. 2) Pulsa Anular y escribe el motivo. 3) Confirma. 4) El sistema marca la factura Anulada (no se elimina).
- **Alternos:** A1) Recepcionista → permiso denegado. A2) Motivo vacío.
- **Postcondición:** factura Anulada, sin posibilidad de pagos.
- **RF:** RF-24. **RN:** RN-13.

## CU-11 Consultar historiales
- **Actor:** Recepcionista, Administrador.
- **Precondición:** sesión iniciada.
- **Flujo principal:** 1) Consulta historial de estadías de un huésped, listado de reservas con filtros o historial de facturación con totales del periodo. 2) Opcionalmente abre vista previa o imprime la factura.
- **Alternos:** A1) Sin resultados → lista vacía.
- **Postcondición:** ninguna (solo lectura).
- **RF:** RF-12, RF-18, RF-25, RF-26.

## CU-12 Gestionar usuarios
- **Actor:** Administrador.
- **Precondición:** sesión de Administrador.
- **Flujo principal:** 1) Abre Usuarios. 2) Registra, edita, desactiva o restablece la contraseña. 3) El sistema valida y guarda el hash.
- **Alternos:** A1) Nombre de usuario duplicado (RN-01). A2) Contraseña débil. A3) Desactivarse a sí mismo → rechazo.
- **Postcondición:** `usuarios.json` actualizado.
- **RF:** RF-02.
