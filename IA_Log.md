# IA_Log.md — Registro de Uso de Inteligencia Artificial

## Proyecto: Sistema de Control de Salidas y Pesaje
## Empresa: ADN Energía — Prueba Técnica
## Fecha: Junio 2026

---

## Enfoque de trabajo

El diseño de la solución, la lógica de negocio y las decisiones de arquitectura
fueron **definidas y dirigidas por mí**. Utilicé herramientas de IA como apoyo
para **acelerar el código repetitivo, mantener una estructura
limpia y consistente, y resolver errores puntuales** durante el desarrollo.

Todo el código fue **revisado, comprendido y ajustado** por mí antes de
integrarse. La IA funcionó como un asistente de productividad, no como autor
de la solución.

---

## Áreas donde la IA aportó valor

### 1. Estructura y organización del proyecto (código limpio)
Apoyo para mantener una separación de carpetas consistente con el patrón MVVM
(Models, Views, ViewModels, Services, Helpers, Converters) y para escribir
clases base reutilizables (`BaseViewModel`) siguiendo
convenciones estándar de .NET. La decisión de usar MVVM y cómo distribuir las
responsabilidades fue mía.

### 2. Reducción de código repetitivo
Generación asistida del "plumbing" típico de WPF: implementación de
`INotifyPropertyChanged`, bindings, y los `Converters` de visibilidad. Esto me
permitió concentrar el tiempo en la **lógica de negocio del dominio**, que es lo
que realmente importa: pesaje, diferencia porcentual y control transaccional.

### 3. Resolución de errores
Apoyo para diagnosticar y corregir errores durante el desarrollo, por ejemplo:
- Ajuste de la configuración del tema visual (`ThemeMode`) tras un error de
  arranque, validando el valor correcto soportado por el framework.
- Revisión de la cadena async/await para asegurar que la UI no se congelara.

### 4. Validación de buenas prácticas
Consulté para contrastar criterios sobre tipos de datos en SQL Server, manejo de
transacciones (`BEGIN TRAN/COMMIT/ROLLBACK`) y captura de excepciones, pero la
implementación final y los ajustes al dominio los definí yo.

---

## Decisiones y ajustes propios (sin IA)

- Diseñé la regla de negocio del **3% de diferencia** y su validación obligatoria
  de justificación.
- Definí que `Peso_Bascula_Salida` debía **permitir valores negativos** (sin
  `CHECK CONSTRAINT`) según el requerimiento.
- Agregué la condición `AND Peso_Bascula_Salida IS NULL` en el Stored Procedure
  para evitar reprocesar registros ya autorizados.
- Decidí la composición de dependencias (inyectar el servicio en el ViewModel).

---

## Reflexión

El uso de IA me permitió **trabajar más rápido sin perder control sobre el
código**. Mi aporte estuvo en el diseño, la lógica del negocio y la validación
crítica de cada pieza generada. Considero que saber *dirigir* y *supervisar*
estas herramientas, comprendiendo cada línea, es una competencia clave del
desarrollo de software actual.
