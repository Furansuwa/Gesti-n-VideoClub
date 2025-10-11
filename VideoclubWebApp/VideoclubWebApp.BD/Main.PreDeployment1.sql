/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

-- Crear la base de datos
CREATE DATABASE VideoClubDB;
GO

USE VideoClubDB;
GO

-- Tabla: Tipos de Artículos
CREATE TABLE TiposArticulos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(100) NOT NULL,
    Estado VARCHAR(20) NOT NULL
);

-- Tabla: Géneros
CREATE TABLE Generos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(100) NOT NULL,
    Estado VARCHAR(20) NOT NULL
);

-- Tabla: Idiomas
CREATE TABLE Idiomas (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Descripcion VARCHAR(100) NOT NULL,
    Estado VARCHAR(20) NOT NULL
);

-- Tabla: Artículos
CREATE TABLE Articulos (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Titulo VARCHAR(200) NOT NULL,
    TipoArticuloId INT NOT NULL,
    IdiomaId INT NOT NULL,
    GeneroId INT NOT NULL,
    RentaPorDia DECIMAL(10,2) NOT NULL,
    DiasRenta INT NOT NULL,
    MontoEntregaTardia DECIMAL(10,2) NOT NULL,
    Estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (TipoArticuloId) REFERENCES TiposArticulos(Id),
    FOREIGN KEY (IdiomaId) REFERENCES Idiomas(Id),
    FOREIGN KEY (GeneroId) REFERENCES Generos(Id)
);

-- Tabla: Elenco
CREATE TABLE Elenco (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(150) NOT NULL,
    Estado VARCHAR(20) NOT NULL
);

-- Tabla: Relación Elenco - Artículo
CREATE TABLE ElencoArticulo (
    ArticuloId INT NOT NULL,
    ElencoId INT NOT NULL,
    Rol VARCHAR(100) NOT NULL,
    PRIMARY KEY (ArticuloId, ElencoId),
    FOREIGN KEY (ArticuloId) REFERENCES Articulos(Id),
    FOREIGN KEY (ElencoId) REFERENCES Elenco(Id)
);

-- Tabla: Clientes
CREATE TABLE Clientes (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(150) NOT NULL,
    Cedula VARCHAR(20) NOT NULL,
    NoTarjetaCR VARCHAR(50),
    LimiteCredito DECIMAL(10,2),
    TipoPersona VARCHAR(20) NOT NULL,
    Estado VARCHAR(20) NOT NULL
);

-- Tabla: Empleados
CREATE TABLE Empleados (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre VARCHAR(150) NOT NULL,
    Cedula VARCHAR(20) NOT NULL,
    TandaLabor VARCHAR(50),
    PorcientoComision DECIMAL(5,2),
    FechaIngreso DATE NOT NULL,
    Estado VARCHAR(20) NOT NULL
);

-- Tabla: Rentas
CREATE TABLE Rentas (
    NoRenta INT PRIMARY KEY IDENTITY(1,1),
    EmpleadoId INT NOT NULL,
    ClienteId INT NOT NULL,
    ArticuloId INT NOT NULL,
    FechaRenta DATE NOT NULL,
    FechaDevolucion DATE,
    MontoPorDia DECIMAL(10,2) NOT NULL,
    CantidadDias INT NOT NULL,
    Comentario VARCHAR(250),
    Estado VARCHAR(20) NOT NULL,
    FOREIGN KEY (EmpleadoId) REFERENCES Empleados(Id),
    FOREIGN KEY (ClienteId) REFERENCES Clientes(Id),
    FOREIGN KEY (ArticuloId) REFERENCES Articulos(Id)
);
