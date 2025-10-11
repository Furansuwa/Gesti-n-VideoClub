USE VideoClubDB;
GO

-- Insertar Tipos de Artículos
INSERT INTO TiposArticulos (Descripcion, Estado) VALUES 
('Película', 'Activo'),
('Serie', 'Activo'),
('Documental', 'Activo');

-- Insertar Géneros
INSERT INTO Generos (Descripcion, Estado) VALUES 
('Acción', 'Activo'),
('Comedia', 'Activo'),
('Drama', 'Activo'),
('Ciencia Ficción', 'Activo'),
('Terror', 'Activo');

-- Insertar Idiomas
INSERT INTO Idiomas (Descripcion, Estado) VALUES 
('Español', 'Activo'),
('Inglés', 'Activo'),
('Francés', 'Activo'),
('Japonés', 'Activo');