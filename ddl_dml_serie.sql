CREATE DATABASE Parcial2Kecm;
GO
USE [master]
GO
CREATE LOGIN [usrparcial2] WITH PASSWORD = N'12345678',
	DEFAULT_DATABASE = [Parcial2Kecm],
	CHECK_EXPIRATION = OFF,
	CHECK_POLICY = ON

USE [Parcial2Kecm]
GO
DROP USER [usrparcial2]
GO

DROP LOGIN [usrparcial2]


GO
USE [Parcial2Kecm]
GO
CREATE USER [usrparcial2] FOR LOGIN [usrparcial2]
GO
ALTER ROLE [db_owner] ADD MEMBER [usrparcial2]
GO

DROP TABLE Serie;

CREATE TABLE Serie (
  id INT NOT NULL PRIMARY KEY IDENTITY(1,1),
  titulo VARCHAR(250) NOT NULL,
  sinopsis VARCHAR(5000) NOT NULL,
  director VARCHAR(100) NOT NULL,
  episodios INT NOT NULL,
  fechaEstreno DATE NOT NULL,
  estado SMALLINT NOT NULL
);

GO
CREATE PROC paSerieListar @parametro VARCHAR(100)
AS
  SELECT * FROM Serie
  WHERE estado<>-1 AND titulo+sinopsis+director LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY estado DESC, titulo ASC;

-- Ejemplo de ejecución:
EXEC paSerieListar 'game thrones';
EXEC paSerieListar 'David';

-- DML
INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado)
VALUES ('Breaking Bad', 'Un profesor de química se convierte en fabricante de metanfetaminas tras ser diagnosticado con cáncer.', 'Vince Gilligan', 62, '2008-01-20', 1);

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado)
VALUES ('Game of Thrones', 'Nobles luchan por el control del Trono de Hierro en un mundo de fantasía medieval.', 'David Benioff, D.B. Weiss', 73, '2011-04-17', 1);

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado)
VALUES ('Stranger Things', 'Un grupo de niños investiga la desaparición de su amigo y se enfrenta a fenómenos paranormales.', 'The Duffer Brothers', 34, '2016-07-15', 1);

UPDATE Serie SET episodios=60 WHERE titulo='Breaking Bad';
UPDATE Serie SET estado=-1 WHERE titulo='Game of Thrones';
UPDATE Serie SET estado=1 WHERE titulo='Game of Thrones';


SELECT * FROM Serie;
