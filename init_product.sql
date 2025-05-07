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
CREATE TABLE [ProductCategories] (
    [Id] int NOT NULL IDENTITY,
    [ParentCategoryId] int NULL,
    [CategoryName] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_ProductCategories] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductCategories_ProductCategories_ParentCategoryId] FOREIGN KEY ([ParentCategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [CategoryVariations] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [VariationName] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_CategoryVariations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_CategoryVariations_ProductCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Products] (
    [Id] int NOT NULL IDENTITY,
    [CategoryId] int NOT NULL,
    [Name] nvarchar(255) NOT NULL,
    [Description] text NOT NULL,
    [Price] decimal(10,2) NOT NULL DEFAULT 0.0,
    [Qty] int NOT NULL DEFAULT 0,
    [ProductImage] nvarchar(255) NOT NULL,
    [Sku] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Products_ProductCategories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [ProductCategories] ([Id]) ON DELETE NO ACTION
);

CREATE TABLE [VariationValues] (
    [Id] int NOT NULL IDENTITY,
    [VariationId] int NOT NULL,
    [Value] nvarchar(255) NOT NULL,
    CONSTRAINT [PK_VariationValues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VariationValues_CategoryVariations_VariationId] FOREIGN KEY ([VariationId]) REFERENCES [CategoryVariations] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ProductVariationValues] (
    [Id] int NOT NULL IDENTITY,
    [ProductId] int NOT NULL,
    [VariationValueId] int NOT NULL,
    CONSTRAINT [PK_ProductVariationValues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ProductVariationValues_Products_ProductId] FOREIGN KEY ([ProductId]) REFERENCES [Products] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_ProductVariationValues_VariationValues_VariationValueId] FOREIGN KEY ([VariationValueId]) REFERENCES [VariationValues] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_CategoryVariations_CategoryId] ON [CategoryVariations] ([CategoryId]);

CREATE INDEX [IX_ProductCategories_ParentCategoryId] ON [ProductCategories] ([ParentCategoryId]);

CREATE INDEX [IX_Products_CategoryId] ON [Products] ([CategoryId]);

CREATE INDEX [IX_ProductVariationValues_ProductId] ON [ProductVariationValues] ([ProductId]);

CREATE INDEX [IX_ProductVariationValues_VariationValueId] ON [ProductVariationValues] ([VariationValueId]);

CREATE INDEX [IX_VariationValues_VariationId] ON [VariationValues] ([VariationId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250415173745_Initial', N'9.0.4');

ALTER TABLE [Products] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250415181945_Second', N'9.0.4');

COMMIT;
GO

