-- ============================================================
-- Script_BD.sql
-- Sistema de Control de Salidas y Pesaje
-- Empresa: Transportes del Norte
-- ============================================================

USE master;
GO

-- Crea la base de datos si no existe
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'TransportesNorte')
BEGIN
    CREATE DATABASE TransportesNorte;
END
GO

USE TransportesNorte;
GO

-- ============================================================
-- TABLA: Despachos_Salida
-- Registra cada camion que entra al CEDIS y su pesaje de salida.
-- Los campos de pesaje (Peso_Bascula_Salida, Peso_Neto_Real,
-- Justificacion_Diferencia, Fecha_Hora_Salida) quedan NULL
-- hasta que el operador autoriza la salida desde la App.
-- ============================================================
IF OBJECT_ID('dbo.Despachos_Salida', 'U') IS NOT NULL
    DROP TABLE dbo.Despachos_Salida;
GO

CREATE TABLE dbo.Despachos_Salida
(
    ID                       INT            IDENTITY(1,1)  NOT NULL,
    Folio_Despacho           NVARCHAR(20)                  NOT NULL,
    Centro_Operativo         NVARCHAR(100)                 NOT NULL,
    Placa_Tracto             NVARCHAR(20)                  NOT NULL,
    Nombre_Conductor         NVARCHAR(150)                 NOT NULL,
    Peso_Tara                DECIMAL(10, 2)                NOT NULL,
    Peso_Teorico_ERP         DECIMAL(10, 2)                NOT NULL,
    Peso_Bascula_Salida      DECIMAL(10, 2)                NULL,   -- Permite negativos (sin CHECK CONSTRAINT)
    Peso_Neto_Real           DECIMAL(10, 2)                NULL,   -- Calculado: Bascula - Tara
    Justificacion_Diferencia NVARCHAR(500)                 NULL,   -- Requerido solo si diferencia > 3%
    Fecha_Hora_Salida        DATETIME2                     NULL,   -- Se llena al autorizar salida

    CONSTRAINT PK_Despachos_Salida  PRIMARY KEY (ID),
    CONSTRAINT UQ_Folio_Despacho    UNIQUE      (Folio_Despacho)
);
GO

-- ============================================================
-- DATOS DE PRUEBA
-- 30 registros pendientes de pesaje (columnas de salida en NULL)
-- ============================================================
INSERT INTO dbo.Despachos_Salida
    (Folio_Despacho, Centro_Operativo, Placa_Tracto, Nombre_Conductor, Peso_Tara, Peso_Teorico_ERP)
VALUES
    ('SAL-2025-001', 'CEDIS Norte',     '58-AK-9F', 'Juan Pérez',                14.50, 22.00),
    ('SAL-2025-002', 'CEDIS Norte',     '89-LPO-2', 'Roberto Díaz',              15.20, 18.50),
    ('SAL-2025-003', 'Planta Sur',      '12-TY-88', 'María González',            16.00, 25.00),
    ('SAL-2025-004', 'Planta Sur',      '77-BN-1C', 'Carlos Ruiz',               14.80, 20.00),
    ('SAL-2025-005', 'CEDIS Occidente', '44-WE-5R', 'Luis Hernández',            15.50, 21.50),
    ('SAL-2025-006', 'CDMX Oriente',   '99-ZZ-1A', 'Ana Torres',                14.20, 19.00),
    ('SAL-2025-007', 'Puerto Veracruz', '33-QA-2S', 'Pedro Infante',             15.80, 28.00),
    ('SAL-2025-008', 'CEDIS Norte',     '11-SX-4D', 'Sofía Vergara',             14.60, 22.50),
    ('SAL-2025-009', 'Planta Sur',      '66-DC-5F', 'Jorge Campos',              15.10, 17.00),
    ('SAL-2025-010', 'CEDIS Occidente', '22-FV-6G', 'Lucía Méndez',             14.90, 20.50),
    ('SAL-2025-011', 'CDMX Oriente',   '88-GB-7H', 'Miguel Hidalgo',            15.30, 23.00),
    ('SAL-2025-012', 'Puerto Veracruz', '44-HN-8J', 'Frida Kahlo',               16.10, 29.00),
    ('SAL-2025-013', 'CEDIS Norte',     '55-JM-9K', 'Diego Rivera',              14.70, 21.00),
    ('SAL-2025-014', 'Planta Sur',      '99-KK-0L', 'Salma Hayek',               15.00, 19.50),
    ('SAL-2025-015', 'CEDIS Occidente', '12-LP-1Q', 'Guillermo del Toro',        15.40, 24.00),
    ('SAL-2025-016', 'CDMX Oriente',   '34-OI-2W', 'Alfonso Cuarón',            14.55, 20.00),
    ('SAL-2025-017', 'Puerto Veracruz', '56-UY-3E', 'Alejandro G. Iñárritu',    15.90, 27.50),
    ('SAL-2025-018', 'CEDIS Norte',     '78-TR-4R', 'Cantinflas',                14.65, 21.80),
    ('SAL-2025-019', 'Planta Sur',      '90-EW-5T', 'Chespirito',                15.25, 18.20),
    ('SAL-2025-020', 'CEDIS Occidente', '23-RQ-6Y', 'El Santo',                  14.85, 22.20),
    ('SAL-2025-021', 'CDMX Oriente',   '45-AF-7U', 'Blue Demon',                15.35, 23.50),
    ('SAL-2025-022', 'Puerto Veracruz', '67-SG-8I', 'Hugo Sánchez',              16.05, 28.50),
    ('SAL-2025-023', 'CEDIS Norte',     '89-DH-9O', 'Julio César Chávez',        14.75, 21.20),
    ('SAL-2025-024', 'Planta Sur',      '10-FJ-0P', 'Lorena Ochoa',              15.05, 19.80),
    ('SAL-2025-025', 'CEDIS Occidente', '32-GK-1A', 'Rafael Márquez',            15.45, 24.50),
    ('SAL-2025-026', 'CDMX Oriente',   '54-HL-2S', 'Ana Guevara',               14.60, 20.20),
    ('SAL-2025-027', 'Puerto Veracruz', '76-LZ-3D', 'Sor Juana',                 15.95, 27.80),
    ('SAL-2025-028', 'CEDIS Norte',     '98-XC-4F', 'Benito Juárez',             14.70, 22.00),
    ('SAL-2025-029', 'Planta Sur',      '21-VB-5G', 'Pancho Villa',              15.30, 18.70),
    ('SAL-2025-030', 'CEDIS Occidente', '43-NM-6H', 'Emiliano Zapata',           14.90, 22.50);
GO

-- ============================================================
-- STORED PROCEDURE: sp_AutorizarSalida
--
-- Recibe los datos de pesaje desde la App y actualiza el
-- registro de salida. Usa BEGIN TRY / BEGIN TRAN para
-- garantizar que si algo falla, NINGUN cambio queda a medias.
-- ============================================================
IF OBJECT_ID('dbo.sp_AutorizarSalida', 'P') IS NOT NULL
    DROP PROCEDURE dbo.sp_AutorizarSalida;
GO

CREATE PROCEDURE dbo.sp_AutorizarSalida
    @Folio_Despacho           NVARCHAR(20),
    @Peso_Bascula_Salida      DECIMAL(10, 2),
    @Peso_Neto_Real           DECIMAL(10, 2),
    @Justificacion_Diferencia NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        BEGIN TRAN

            -- Valida que el folio exista y no haya sido procesado ya
            IF NOT EXISTS (
                SELECT 1
                FROM   dbo.Despachos_Salida
                WHERE  Folio_Despacho      = @Folio_Despacho
                  AND  Peso_Bascula_Salida IS NULL
            )
            BEGIN
                RAISERROR(
                    'El folio %s no existe o ya fue procesado anteriormente.',
                    16, 1, @Folio_Despacho
                );
            END

            UPDATE dbo.Despachos_Salida
            SET
                Peso_Bascula_Salida      = @Peso_Bascula_Salida,
                Peso_Neto_Real           = @Peso_Neto_Real,
                Justificacion_Diferencia = @Justificacion_Diferencia,
                Fecha_Hora_Salida        = GETDATE()
            WHERE  Folio_Despacho = @Folio_Despacho;

        COMMIT TRAN;

    END TRY
    BEGIN CATCH

        -- Si algo falla, deshace TODOS los cambios de esta transaccion
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        -- Re-lanza el error para que la App lo capture en su try-catch
        THROW;

    END CATCH
END;
GO
