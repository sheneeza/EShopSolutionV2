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
CREATE TABLE [Addresses] (
    [Id] int NOT NULL IDENTITY,
    [Street1] nvarchar(max) NOT NULL,
    [Street2] nvarchar(max) NOT NULL,
    [City] nvarchar(max) NOT NULL,
    [ZipCode] int NOT NULL,
    [State] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id])
);

CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Gender] nvarchar(max) NOT NULL,
    [Phone] int NOT NULL,
    [Profile_PIC] nvarchar(max) NOT NULL,
    [UserId] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

CREATE TABLE [Orders] (
    [Id] int NOT NULL IDENTITY,
    [Order_Date] datetime2 NOT NULL,
    [CustomerId] int NOT NULL,
    [CustomerName] nvarchar(max) NOT NULL,
    [PaymentMethodId] int NOT NULL,
    [PaymentName] nvarchar(max) NOT NULL,
    [ShippingAddress] nvarchar(max) NOT NULL,
    [ShippingMethod] nvarchar(max) NOT NULL,
    [BillingAmount] decimal(18,2) NOT NULL,
    [Order_Status] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Orders] PRIMARY KEY ([Id])
);

CREATE TABLE [PaymentTypes] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_PaymentTypes] PRIMARY KEY ([Id])
);

CREATE TABLE [ShoppingCarts] (
    [Id] int NOT NULL IDENTITY,
    [CustomerId] int NOT NULL,
    [CustomerName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ShoppingCarts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShoppingCarts_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [UserAddresses] (
    [Id] int NOT NULL IDENTITY,
    [Customer_Id] int NOT NULL,
    [CustomerId] int NOT NULL,
    [Address_Id] int NOT NULL,
    [AddressId] int NOT NULL,
    [IsDefaultAddress] bit NOT NULL,
    CONSTRAINT [PK_UserAddresses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserAddresses_Addresses_AddressId] FOREIGN KEY ([AddressId]) REFERENCES [Addresses] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserAddresses_Customers_CustomerId] FOREIGN KEY ([CustomerId]) REFERENCES [Customers] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [Order_Details] (
    [Id] int NOT NULL IDENTITY,
    [Order_Id] int NOT NULL,
    [OrderId] int NOT NULL,
    [Product_Id] int NOT NULL,
    [Product_name] nvarchar(max) NOT NULL,
    [Qty] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [Discount] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_Order_Details] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Order_Details_Orders_OrderId] FOREIGN KEY ([OrderId]) REFERENCES [Orders] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [PaymentMethods] (
    [Id] int NOT NULL IDENTITY,
    [Payment_Type_Id] int NOT NULL,
    [PaymentTypeId] int NOT NULL,
    [Provider] nvarchar(max) NOT NULL,
    [AccountNumber] int NOT NULL,
    [Expiry] datetime2 NOT NULL,
    [isDefault] bit NOT NULL,
    CONSTRAINT [PK_PaymentMethods] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PaymentMethods_PaymentTypes_PaymentTypeId] FOREIGN KEY ([PaymentTypeId]) REFERENCES [PaymentTypes] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [ShoppingCartItems] (
    [Id] int NOT NULL IDENTITY,
    [Cart_Id] int NOT NULL,
    [ShoppingCartId] int NOT NULL,
    [ProductId] int NOT NULL,
    [Qty] int NOT NULL,
    [Price] decimal(18,2) NOT NULL,
    [ProductName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_ShoppingCartItems] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ShoppingCartItems_ShoppingCarts_ShoppingCartId] FOREIGN KEY ([ShoppingCartId]) REFERENCES [ShoppingCarts] ([Id]) ON DELETE CASCADE
);

CREATE INDEX [IX_Order_Details_OrderId] ON [Order_Details] ([OrderId]);

CREATE INDEX [IX_PaymentMethods_PaymentTypeId] ON [PaymentMethods] ([PaymentTypeId]);

CREATE INDEX [IX_ShoppingCartItems_ShoppingCartId] ON [ShoppingCartItems] ([ShoppingCartId]);

CREATE INDEX [IX_ShoppingCarts_CustomerId] ON [ShoppingCarts] ([CustomerId]);

CREATE INDEX [IX_UserAddresses_AddressId] ON [UserAddresses] ([AddressId]);

CREATE INDEX [IX_UserAddresses_CustomerId] ON [UserAddresses] ([CustomerId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250415155225_Init', N'9.0.4');

COMMIT;
GO

