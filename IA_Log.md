# IA_Log.md — Registro de Uso de Inteligencia Artificial

## Proyecto: Sistema de Control de Salidas y Pesaje
## Herramienta utilizada: Claude Code (Anthropic — claude-sonnet-4-6)
## Fecha: Junio 2026

---

## Resumen

Durante el desarrollo de esta prueba técnica utilicé **Claude Code** como asistente principal para optimizar el tiempo de producción y garantizar la calidad del código.

Mi rol: **diseñar, supervisar, validar y ajustar** cada sección generada. No se integró ningún bloque de código sin haberlo leído y comprendido primero.

---

## Bitácora de interacciones

### 1. Diseño del esquema SQL (DDL)

**Consulta a la IA:** Analizar los campos del archivo de datos de prueba y proponer el tipo de dato óptimo para cada columna en SQL Server.

**Resultado generado:**
- `NVARCHAR` para texto, `DECIMAL(10,2)` para pesos, `DATETIME2` para el timestamp.
- `UNIQUE CONSTRAINT` sobre `Folio_Despacho` para garantizar unicidad.
- Campos de pesaje como `NULL` para representar el estado "pendiente".

**Ajuste propio:** Verifiqué que `Peso_Bascula_Salida` admitiera negativos al no agregar `CHECK CONSTRAINT` positivo.

---

### 2. Stored Procedure transaccional

**Consulta a la IA:** Estructura con `BEGIN TRY / BEGIN TRAN / COMMIT / ROLLBACK` y validación de negocio.

**Resultado generado:** SP con manejo de transacciones y `THROW` para propagar el error a la capa de aplicación.

**Ajuste propio:** Agregué `AND Peso_Bascula_Salida IS NULL` en la validación para evitar reescribir registros ya procesados.

---

### 3. Arquitectura MVVM en .NET 10

**Consulta a la IA:** Scaffold completo de proyecto WPF con patrón MVVM: `BaseViewModel`, `RelayCommand`, separación por carpetas (Models, Views, ViewModels, Services, Helpers, Converters).

**Resultado generado:** Estructura base con `INotifyPropertyChanged`, `ICommand` y separación de responsabilidades.

**Ajuste propio:** Rediseñé la inyección del servicio para pasar `IDespachoService` directamente al `AuditoriaViewModel`, eliminando una primera versión que usaba Reflection innecesariamente.

---

### 4. Lógica de validación crítica del 3%

**Consulta a la IA:** Implementación de la regla de negocio: cálculo de diferencia porcentual y activación condicional del campo de justificación.

**Resultado generado:** Método `Recalcular()` en `AuditoriaViewModel` con cálculo en tiempo real al cambiar `PesoBasculaTexto`.

**Ajuste propio:** Validé que `PuedeAutorizar()` bloqueara el botón correctamente cuando hay alerta pero no hay justificación, y que `RaiseCanExecuteChanged()` se llamara en los momentos correctos.

---

### 5. Manejo de errores y logging

**Consulta a la IA:** Implementar `try-catch` asíncrono y escritura de errores en `log.txt`.

**Resultado generado:** Clase `Logger` estática con `File.AppendAllText`, con captura silenciosa del error de escritura para no romper la App si el disco está lleno.

---

### 6. Pipeline CI/CD

**Consulta a la IA:** Workflow básico de GitHub Actions para build y publicación de app WPF en Windows.

**Resultado generado:** `.github/workflows/ci.yml` con setup de .NET 10, restore, build Release, publish y upload de artefacto.

---

## Reflexión

El uso estratégico de IA redujo significativamente el tiempo de scaffolding inicial, permitiéndome enfocarme en la **lógica de negocio específica** del dominio (pesaje, diferencia porcentual, integridad transaccional).

Cada sección fue revisada, comprendida y donde fue necesario, corregida. Esta combinación de generación asistida y supervisión crítica es el flujo de trabajo que propongo como desarrollador.
