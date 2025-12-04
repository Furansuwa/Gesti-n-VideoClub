USE VideoClubDB;
GO

-- Insertar datos semilla (Tipos, Géneros, Idiomas)
INSERT INTO TiposArticulos (Descripcion, Estado) VALUES 
('Película', 'Activo'), ('Serie', 'Activo'), ('Documental', 'Activo');

INSERT INTO Generos (Descripcion, Estado) VALUES 
('Acción', 'Activo'), ('Comedia', 'Activo'), ('Drama', 'Activo'), ('Ciencia Ficción', 'Activo'), ('Terror', 'Activo');

INSERT INTO Idiomas (Descripcion, Estado) VALUES 
('Español', 'Activo'), ('Inglés', 'Activo'), ('Francés', 'Activo'), ('Japonés', 'Activo');

-- Modificación para Identity en Empleados
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UserId' AND Object_ID = Object_ID(N'Empleados'))
BEGIN
    ALTER TABLE Empleados ADD UserId NVARCHAR(450) NULL;
END
GO

-- CORRECCIÓN DEL ERROR BIT vs STRING (Lo que faltaba)
-- Convertir Clientes.Estado de BIT a VARCHAR
BEGIN TRY
    ALTER TABLE Clientes ALTER COLUMN Estado VARCHAR(20) NOT NULL;
END TRY
BEGIN CATCH
    -- Si falla porque hay datos booleanos (1/0), los convertimos primero
    -- Nota: Esto asume que 1=Activo, 0=Inactivo
    ALTER TABLE Clientes ALTER COLUMN Estado VARCHAR(20) NULL; 
    UPDATE Clientes SET Estado = 'Activo' WHERE Estado = '1';
    UPDATE Clientes SET Estado = 'Inactivo' WHERE Estado = '0';
    ALTER TABLE Clientes ALTER COLUMN Estado VARCHAR(20) NOT NULL;
END CATCH

-- Convertir Empleados.Estado de BIT a VARCHAR
BEGIN TRY
    ALTER TABLE Empleados ALTER COLUMN Estado VARCHAR(20) NOT NULL;
END TRY
BEGIN CATCH
    ALTER TABLE Empleados ALTER COLUMN Estado VARCHAR(20) NULL; 
    UPDATE Empleados SET Estado = 'Activo' WHERE Estado = '1';
    UPDATE Empleados SET Estado = 'Inactivo' WHERE Estado = '0';
    ALTER TABLE Empleados ALTER COLUMN Estado VARCHAR(20) NOT NULL;
END CATCH
GO