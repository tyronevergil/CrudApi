CREATE TABLE Contacts (
    [ContactId] int IDENTITY(1,1),
    [Email] varchar(255)
)
GO

CREATE TABLE Names (
    [NameId] int IDENTITY(1,1),
    [ContactId] int NOT NULL,
    [First] varchar(255),
    [Middle] varchar(255),
    [Last] varchar(255)
)
GO

CREATE TABLE Addresses (
    [AddressId] int IDENTITY(1,1),
    [ContactId] int NOT NULL,
    [Street] varchar(255),
    [City] varchar(255),
    [State] varchar(255),
    [Zip] varchar(255)
)
GO

CREATE TABLE Phones (
    [PhoneId] int IDENTITY(1,1),
    [ContactId] int NOT NULL,
    [Number] varchar(255),
    [Type] varchar(255)
)
GO
