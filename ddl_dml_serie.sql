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
  estado SMALLINT NOT NULL,
);

ALTER TABLE Serie ADD urlPortada VARCHAR(500) NULL;
ALTER TABLE Serie ADD idiomaOriginal VARCHAR(100) DEFAULT 'Español' NULL;

GO
ALTER PROC paSerieListar @parametro VARCHAR(100)
AS
  SELECT * FROM Serie
  WHERE estado<>-1 AND titulo+sinopsis+director+idiomaOriginal LIKE '%'+REPLACE(@parametro,' ','%')+'%'
  ORDER BY estado DESC, titulo ASC;

-- Ejemplo de ejecución:
EXEC paSerieListar 'game thrones';
EXEC paSerieListar 'David';
EXEC paSerieListar 'inglés';
EXEC paSerieListar 'coreano';

-- DML
INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado, urlPortada, idiomaOriginal)
VALUES ('Breaking Bad', 'Un profesor de química se convierte en fabricante de metanfetaminas tras ser diagnosticado con cáncer.', 'Vince Gilligan', 62, '2008-01-20', 1, 'https://example.com/breaking_bad.jpg', 'Inglés');

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado, urlPortada, idiomaOriginal)
VALUES ('Game of Thrones', 'Nobles luchan por el control del Trono de Hierro en un mundo de fantasía medieval.', 'David Benioff, D.B. Weiss', 73, '2011-04-17', 1, 'https://example.com/game_of_thrones.jpg', 'Inglés');

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado, urlPortada, idiomaOriginal)
VALUES ('Stranger Things', 'Un grupo de niños investiga la desaparición de su amigo y se enfrenta a fenómenos paranormales.', 'The Duffer Brothers', 34, '2016-07-15', 1, 'https://example.com/stranger_things.jpg', 'Inglés');

INSERT INTO Serie(titulo, sinopsis, director, episodios, fechaEstreno, estado, urlPortada, idiomaOriginal)
VALUES ('Squid Game', 'Un grupo de personas en problemas económicos participa en un mortal juego de supervivencia.', 'Hwang Dong-hyuk', 9, '2021-09-17', 1, 'https://example.com/squid_game.jpg', 'Coreano');

UPDATE Serie SET episodios=60 WHERE titulo='Breaking Bad';
UPDATE Serie SET estado=-1 WHERE titulo='Game of Thrones';
UPDATE Serie SET estado=1 WHERE titulo='Game of Thrones';


SELECT * FROM Serie;