IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [CurrencyFavorites] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [CurrencyCode] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_CurrencyFavorites] PRIMARY KEY ([Id])
);

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

CREATE TABLE [FxRates] (
    [Id] int NOT NULL IDENTITY,
    [BaseCurrency] nvarchar(max) NOT NULL,
    [TargetCurrency] nvarchar(max) NOT NULL,
    [Rate] decimal(18,2) NOT NULL,
    [RetrievedAt] datetime2 NOT NULL,
    CONSTRAINT [PK_FxRates] PRIMARY KEY ([Id])
);

CREATE TABLE [UserSettings] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(max) NOT NULL,
    [PrimaryCurrency] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_UserSettings] PRIMARY KEY ([Id])
);

CREATE UNIQUE INDEX [IX_CurrencyFavorites_UserId_CurrencyCode] ON [CurrencyFavorites] ([UserId], [CurrencyCode]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250928054752_InitialCreate', N'9.0.9');

DECLARE @var sysname;
SELECT @var = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[FxRates]') AND [c].[name] = N'Rate');
IF @var IS NOT NULL EXEC(N'ALTER TABLE [FxRates] DROP CONSTRAINT [' + @var + '];');
ALTER TABLE [FxRates] ALTER COLUMN [Rate] decimal(18,6) NOT NULL;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250930022944_AddPrecisionToFxRate', N'9.0.9');

COMMIT;
GO

