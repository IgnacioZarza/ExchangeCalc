/* =================================================================================
   Script de creación de base de datos inicial y migraciones de ExchangeCalc
   Generado a partir de las migraciones de EF Core
   Fecha de generación: 2025-09-30
   Autor: [Juan Ignacio Zarza Palacios]
   Versión de EF Core: 9.0.9
   Propósito: Crear tablas, índices y registrar migraciones sin ejecutar Update-Database
================================================================================= */

/* =================================================================================
   1. Crear tabla de historial de migraciones (__EFMigrationsHistory)
   Esta tabla permite a EF Core llevar control de las migraciones aplicadas.
================================================================================= */
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

/* =================================================================================
   2. Iniciar transacción para asegurar atomicidad de los cambios
================================================================================= */
BEGIN TRANSACTION;

-- ===================================
-- 2.1 Crear tabla CurrencyFavorites
-- Almacena las monedas favoritas de cada usuario
-- Índice único para evitar duplicados por usuario
-- ===================================
CREATE TABLE [CurrencyFavorites] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [CurrencyCode] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_CurrencyFavorites] PRIMARY KEY ([Id])
);
CREATE UNIQUE INDEX [IX_CurrencyFavorites_UserId_CurrencyCode]
    ON [CurrencyFavorites] ([UserId], [CurrencyCode]);

-- ===================================
-- 2.2 Crear tabla ExchangeLogs
-- Registra peticiones y operaciones realizadas en la app
-- ===================================
CREATE TABLE [ExchangeLogs] (
    [Id] int NOT NULL IDENTITY,
    [Timestamp] datetime2 NOT NULL,
    [Route] nvarchar(max) NOT NULL,
    [HttpMethod] nvarchar(max) NOT NULL,
    [QueryString] nvarchar(max) NOT NULL,
    [Body] nvarchar(max) NOT NULL,
    [UserIp] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ExchangeLogs] PRIMARY KEY ([Id])
);

-- ===================================
-- 2.3 Crear tabla FxRates
-- Almacena las tasas de cambio
-- Se ajusta la precisión de la columna Rate
-- ===================================
CREATE TABLE [FxRates] (
    [Id] int NOT NULL IDENTITY,
    [BaseCurrency] nvarchar(max) NOT NULL,
    [TargetCurrency] nvarchar(max) NOT NULL,
    [Rate] decimal(18,2) NOT NULL,
    [RetrievedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_FxRates] PRIMARY KEY ([Id])
);

-- Ajuste de precisión decimal a 6 decimales
DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] 
    ON [d].[parent_column_id] = [c].[column_id] 
    AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FxRates]') AND [c].[name] = N'Rate');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [FxRates] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [FxRates] ALTER COLUMN [Rate] decimal(18,6) NOT NULL;

-- ===================================
-- 2.4 Crear tabla UserSettings
-- Almacena configuraciones de usuario como moneda principal
-- ===================================
CREATE TABLE [UserSettings] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [PrimaryCurrency] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserSettings] PRIMARY KEY ([Id])
);

-- ===================================
-- 3. Registrar migraciones aplicadas
-- Esto evita que EF Core intente volver a aplicarlas
-- ===================================
INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES 
(N'20250928054752_InitialCreate', N'9.0.9'),
(N'20250930022944_AddPrecisionToFxRate', N'9.0.9');

-- ===================================
-- 4. Commit de la transacción
-- Si todo sale bien, se confirman los cambios
-- ===================================
ROLLBACK TRANSACTION
--COMMIT TRANSACTION;
GO

/* =================================================================================
   5. Validación básica post-creación
   Verifica que las tablas críticas existan
================================================================================= */
IF OBJECT_ID(N'[CurrencyFavorites]') IS NULL
    PRINT 'ERROR: La tabla CurrencyFavorites no se creó correctamente.';
IF OBJECT_ID(N'[FxRates]') IS NULL
    PRINT 'ERROR: La tabla FxRates no se creó correctamente.';
IF OBJECT_ID(N'[UserSettings]') IS NULL
    PRINT 'ERROR: La tabla UserSettings no se creó correctamente.';
IF OBJECT_ID(N'[ExchangeLogs]') IS NULL
    PRINT 'ERROR: La tabla ExchangeLogs no se creó correctamente.';

PRINT 'Script de liberación ejecutado correctamente. Las tablas y migraciones están listas.';
GO

/* =================================================================================
   FIN DEL SCRIPT
   - Ejecutar este script en la base de datos de producción crea todas las tablas necesarias
   - Registra las migraciones aplicadas para que EF Core no intente aplicarlas de nuevo
   - Asegura que la aplicación pueda arrancar sin problemas de migraciones
================================================================================= */
