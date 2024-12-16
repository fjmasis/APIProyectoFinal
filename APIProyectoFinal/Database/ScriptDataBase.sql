-- 1. Tabla Empleados
CREATE TABLE Empleados (
    ID_Empleado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL,
    Apellido NVARCHAR(50) NOT NULL,
    Direccion NVARCHAR(255),
    Telefono NVARCHAR(15),
    Email NVARCHAR(100),
    Fecha_Ingreso DATE NOT NULL,
    Cargo NVARCHAR(50),
    Departamento NVARCHAR(50),
    Salario DECIMAL(10,2),
    Estado NVARCHAR(10) CHECK (Estado IN ('Activo', 'Inactivo')),
    Fecha_Nacimiento DATE
);

-- 2. Tabla Asistencias (Gestión de Control de Horas)
CREATE TABLE Asistencias (
    ID_Asistencia INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Fecha DATE NOT NULL,
    Hora_Entrada TIME,
    Hora_Salida TIME,
    Estado NVARCHAR(20) CHECK (Estado IN ('Presente', 'Ausente', 'Permiso')),
    Observaciones NVARCHAR(255),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

-- 3. Tabla Estados Laborales
CREATE TABLE EstadosLaborales (
    ID_EstadoLaboral INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Estado NVARCHAR(50) NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE,
    AdicionadoPor NVARCHAR(50),
    ModificadoPor NVARCHAR(50),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

-- 4. Tabla Evaluaciones de Desempeño
CREATE TABLE Evaluaciones (
    ID_Evaluacion INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Fecha_Evaluacion DATE NOT NULL,
    Comentarios NVARCHAR(MAX),
    Puntuacion INT CHECK (Puntuacion BETWEEN 1 AND 10),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

-- 5. Tabla Ausencias
CREATE TABLE Ausencias (
    ID_Ausencia INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE NOT NULL,
    Tipo_Ausencia NVARCHAR(50),
    Motivo NVARCHAR(255),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

-- 6. Tabla Vacaciones, Permisos y Licencias
CREATE TABLE Gestiones (
    ID_Gestion INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Fecha_Inicio DATE NOT NULL,
    Fecha_Fin DATE NOT NULL,
    Tipo NVARCHAR(50) CHECK (Tipo IN ('Vacación', 'Permiso', 'Licencia')),
    Motivo NVARCHAR(MAX),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

-- 7. Tabla Reportes de Puntualidad
CREATE TABLE ReportesPuntualidad (
    ID_Reporte INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Mes INT CHECK (Mes BETWEEN 1 AND 12),
    Anio INT,
    Dias_Tarde INT,
    Dias_Cumplidos INT,
    Horas_Extras DECIMAL(5,2),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

-- 8. Tabla Usuarios (Login)
CREATE TABLE Usuarios (
    ID_Usuario INT IDENTITY(1,1) PRIMARY KEY,
    Usuario NVARCHAR(50) NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(50) CHECK (Rol IN ('Admin', 'Empleado'))
);


INSERT INTO Usuarios (Usuario, Contraseña, Rol)
VALUES ('admin', 'admin123', 'Admin');



-- CRUD para cada tabla --

-- Procedimientos para Empleados
CREATE PROCEDURE spAgregarEmpleado
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Direccion NVARCHAR(255),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(100),
    @Fecha_Ingreso DATE,
    @Cargo NVARCHAR(50),
    @Departamento NVARCHAR(50),
    @Salario DECIMAL(10,2),
    @Estado NVARCHAR(10),
    @Fecha_Nacimiento DATE
AS
BEGIN
    INSERT INTO Empleados (Nombre, Apellido, Direccion, Telefono, Email, Fecha_Ingreso, Cargo, Departamento, Salario, Estado, Fecha_Nacimiento)
    VALUES (@Nombre, @Apellido, @Direccion, @Telefono, @Email, @Fecha_Ingreso, @Cargo, @Departamento, @Salario, @Estado, @Fecha_Nacimiento);
END;

CREATE PROCEDURE spObtenerEmpleados
AS
BEGIN
    SELECT * FROM Empleados;
END;

CREATE PROCEDURE spActualizarEmpleado
    @ID_Empleado INT,
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Direccion NVARCHAR(255),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(100),
    @Fecha_Ingreso DATE,
    @Cargo NVARCHAR(50),
    @Departamento NVARCHAR(50),
    @Salario DECIMAL(10,2),
    @Estado NVARCHAR(10),
    @Fecha_Nacimiento DATE
AS
BEGIN
    UPDATE Empleados
    SET Nombre = @Nombre, Apellido = @Apellido, Direccion = @Direccion, Telefono = @Telefono, Email = @Email,
        Fecha_Ingreso = @Fecha_Ingreso, Cargo = @Cargo, Departamento = @Departamento, Salario = @Salario,
        Estado = @Estado, Fecha_Nacimiento = @Fecha_Nacimiento
    WHERE ID_Empleado = @ID_Empleado;
END;

CREATE PROCEDURE spEliminarEmpleado
    @ID_Empleado INT
AS
BEGIN
    DELETE FROM Empleados WHERE ID_Empleado = @ID_Empleado;
END;

-- Procedimientos para Asistencias
CREATE PROCEDURE spAgregarAsistencia
    @ID_Empleado INT,
    @Fecha DATE,
    @Hora_Entrada TIME,
    @Hora_Salida TIME,
    @Estado NVARCHAR(20),
    @Observaciones NVARCHAR(255)
AS
BEGIN
    INSERT INTO Asistencias (ID_Empleado, Fecha, Hora_Entrada, Hora_Salida, Estado, Observaciones)
    VALUES (@ID_Empleado, @Fecha, @Hora_Entrada, @Hora_Salida, @Estado, @Observaciones);
END;

CREATE PROCEDURE spObtenerAsistencias
AS
BEGIN
    SELECT * FROM Asistencias;
END;

CREATE PROCEDURE spActualizarAsistencia
    @ID_Asistencia INT,
    @Hora_Salida TIME,
    @Estado NVARCHAR(20),
    @Observaciones NVARCHAR(255)
AS
BEGIN
    UPDATE Asistencias
    SET Hora_Salida = @Hora_Salida, Estado = @Estado, Observaciones = @Observaciones
    WHERE ID_Asistencia = @ID_Asistencia;
END;

CREATE PROCEDURE spEliminarAsistencia
    @ID_Asistencia INT
AS
BEGIN
    DELETE FROM Asistencias WHERE ID_Asistencia = @ID_Asistencia;
END;

-- Procedimientos para Estados Laborales
CREATE PROCEDURE spAgregarEstadoLaboral
    @ID_Empleado INT,
    @Estado NVARCHAR(50),
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @AdicionadoPor NVARCHAR(50),
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    INSERT INTO EstadosLaborales (ID_Empleado, Estado, Fecha_Inicio, Fecha_Fin, AdicionadoPor, ModificadoPor)
    VALUES (@ID_Empleado, @Estado, @Fecha_Inicio, @Fecha_Fin, @AdicionadoPor, @ModificadoPor);
END;

CREATE PROCEDURE spObtenerEstadosLaborales
AS
BEGIN
    SELECT * FROM EstadosLaborales;
END;

CREATE PROCEDURE spActualizarEstadoLaboral
    @ID_EstadoLaboral INT,
    @Estado NVARCHAR(50),
    @Fecha_Fin DATE,
    @ModificadoPor NVARCHAR(50)
AS
BEGIN
    UPDATE EstadosLaborales
    SET Estado = @Estado, Fecha_Fin = @Fecha_Fin, ModificadoPor = @ModificadoPor
    WHERE ID_EstadoLaboral = @ID_EstadoLaboral;
END;

CREATE PROCEDURE spEliminarEstadoLaboral
    @ID_EstadoLaboral INT
AS
BEGIN
    DELETE FROM EstadosLaborales WHERE ID_EstadoLaboral = @ID_EstadoLaboral;
END;

-- Procedimientos para Evaluaciones
CREATE PROCEDURE spAgregarEvaluacion
    @ID_Empleado INT,
    @Fecha_Evaluacion DATE,
    @Comentarios NVARCHAR(MAX),
    @Puntuacion INT
AS
BEGIN
    INSERT INTO Evaluaciones (ID_Empleado, Fecha_Evaluacion, Comentarios, Puntuacion)
    VALUES (@ID_Empleado, @Fecha_Evaluacion, @Comentarios, @Puntuacion);
END;

CREATE PROCEDURE spObtenerEvaluaciones
AS
BEGIN
    SELECT * FROM Evaluaciones;
END;

CREATE PROCEDURE spActualizarEvaluacion
    @ID_Evaluacion INT,
    @Comentarios NVARCHAR(MAX),
    @Puntuacion INT
AS
BEGIN
    UPDATE Evaluaciones
    SET Comentarios = @Comentarios, Puntuacion = @Puntuacion
    WHERE ID_Evaluacion = @ID_Evaluacion;
END;

CREATE PROCEDURE spEliminarEvaluacion
    @ID_Evaluacion INT
AS
BEGIN
    DELETE FROM Evaluaciones WHERE ID_Evaluacion = @ID_Evaluacion;
END;

-- Procedimientos para Ausencias
CREATE PROCEDURE spAgregarAusencia
    @ID_Empleado INT,
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @Tipo_Ausencia NVARCHAR(50),
    @Motivo NVARCHAR(255)
AS
BEGIN
    INSERT INTO Ausencias (ID_Empleado, Fecha_Inicio, Fecha_Fin, Tipo_Ausencia, Motivo)
    VALUES (@ID_Empleado, @Fecha_Inicio, @Fecha_Fin, @Tipo_Ausencia, @Motivo);
END;

CREATE PROCEDURE spObtenerAusencias
AS
BEGIN
    SELECT * FROM Ausencias;
END;

CREATE PROCEDURE spActualizarAusencia
    @ID_Ausencia INT,
    @Fecha_Fin DATE,
    @Motivo NVARCHAR(255)
AS
BEGIN
    UPDATE Ausencias
    SET Fecha_Fin = @Fecha_Fin, Motivo = @Motivo
    WHERE ID_Ausencia = @ID_Ausencia;
END;

CREATE PROCEDURE spEliminarAusencia
    @ID_Ausencia INT
AS
BEGIN
    DELETE FROM Ausencias WHERE ID_Ausencia = @ID_Ausencia;
END;

-- Procedimientos para Vacaciones, Permisos y Licencias (Gestiones)
CREATE PROCEDURE spAgregarGestion
    @ID_Empleado INT,
    @Fecha_Inicio DATE,
    @Fecha_Fin DATE,
    @Tipo NVARCHAR(50),
    @Motivo NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Gestiones (ID_Empleado, Fecha_Inicio, Fecha_Fin, Tipo, Motivo)
    VALUES (@ID_Empleado, @Fecha_Inicio, @Fecha_Fin, @Tipo, @Motivo);
END;

CREATE PROCEDURE spObtenerGestiones
AS
BEGIN
    SELECT * FROM Gestiones;
END;

CREATE PROCEDURE spActualizarGestion
    @ID_Gestion INT,
    @Fecha_Fin DATE,
    @Motivo NVARCHAR(MAX)
AS
BEGIN
    UPDATE Gestiones
    SET Fecha_Fin = @Fecha_Fin, Motivo = @Motivo
    WHERE ID_Gestion = @ID_Gestion;
END;

CREATE PROCEDURE spEliminarGestion
    @ID_Gestion INT
AS
BEGIN
    DELETE FROM Gestiones WHERE ID_Gestion = @ID_Gestion;
END;

-- Procedimientos para Reportes de Puntualidad
CREATE PROCEDURE spAgregarReportePuntualidad
    @ID_Empleado INT,
    @Mes INT,
    @Anio INT,
    @Dias_Tarde INT,
    @Dias_Cumplidos INT,
    @Horas_Extras DECIMAL(5,2)
AS
BEGIN
    INSERT INTO ReportesPuntualidad (ID_Empleado, Mes, Anio, Dias_Tarde, Dias_Cumplidos, Horas_Extras)
    VALUES (@ID_Empleado, @Mes, @Anio, @Dias_Tarde, @Dias_Cumplidos, @Horas_Extras);
END;

CREATE PROCEDURE spObtenerReportesPuntualidad
AS
BEGIN
    SELECT * FROM ReportesPuntualidad;
END;

CREATE PROCEDURE spActualizarReportePuntualidad
    @ID_Reporte INT,
    @Dias_Tarde INT,
    @Dias_Cumplidos INT,
    @Horas_Extras DECIMAL(5,2)
AS
BEGIN
    UPDATE ReportesPuntualidad
    SET Dias_Tarde = @Dias_Tarde, Dias_Cumplidos = @Dias_Cumplidos, Horas_Extras = @Horas_Extras
    WHERE ID_Reporte = @ID_Reporte;
END;

CREATE PROCEDURE spEliminarReportePuntualidad
    @ID_Reporte INT
AS
BEGIN
    DELETE FROM ReportesPuntualidad WHERE ID_Reporte = @ID_Reporte;
END;

-- Procedimientos para Usuarios
CREATE PROCEDURE spAgregarUsuario
    @Usuario NVARCHAR(50),
    @Contraseña NVARCHAR(255),
    @Rol NVARCHAR(50)
AS
BEGIN
    INSERT INTO Usuarios (Usuario, Contraseña, Rol)
    VALUES (@Usuario, @Contraseña, @Rol);
END;

CREATE PROCEDURE spObtenerUsuarios
AS
BEGIN
    SELECT * FROM Usuarios;
END;

CREATE PROCEDURE spActualizarUsuario
    @ID_Usuario INT,
    @Contraseña NVARCHAR(255),
    @Rol NVARCHAR(50)
AS
BEGIN
    UPDATE Usuarios
    SET Contraseña = @Contraseña, Rol = @Rol
    WHERE ID_Usuario = @ID_Usuario;
END;

CREATE PROCEDURE spEliminarUsuario
    @ID_Usuario INT
AS
BEGIN
    DELETE FROM Usuarios WHERE ID_Usuario = @ID_Usuario;
END;

IF OBJECT_ID('dbo.spAgregarAsistencia', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spAgregarAsistencia;
GO


CREATE PROCEDURE spAgregarAsistencia
    @ID_Empleado INT,
    @Fecha DATE,
    @Hora_Entrada TIME,
    @Hora_Salida TIME,
    @Estado NVARCHAR(20),
    @Observaciones NVARCHAR(255)
AS
BEGIN
    INSERT INTO Asistencias (ID_Empleado, Fecha, Hora_Entrada, Hora_Salida, Estado, Observaciones)
    VALUES (@ID_Empleado, @Fecha, @Hora_Entrada, @Hora_Salida, @Estado, @Observaciones);
END;


CREATE PROCEDURE dbo.spAgregarEmpleado
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Direccion NVARCHAR(255),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(100),
    @Fecha_Ingreso DATE,
    @Cargo NVARCHAR(50),
    @Departamento NVARCHAR(50),
    @Salario DECIMAL(10,2),
    @Estado NVARCHAR(10),
    @Fecha_Nacimiento DATE
AS
BEGIN
    INSERT INTO Empleados (Nombre, Apellido, Direccion, Telefono, Email, Fecha_Ingreso, Cargo, Departamento, Salario, Estado, Fecha_Nacimiento)
    VALUES (@Nombre, @Apellido, @Direccion, @Telefono, @Email, @Fecha_Ingreso, @Cargo, @Departamento, @Salario, @Estado, @Fecha_Nacimiento);
END;
GO



CREATE PROCEDURE spAgregarEmpleado
    @Nombre NVARCHAR(50),
    @Apellido NVARCHAR(50),
    @Direccion NVARCHAR(255),
    @Telefono NVARCHAR(15),
    @Email NVARCHAR(100),
    @Cargo NVARCHAR(50),
    @Departamento NVARCHAR(50),
    @Salario DECIMAL(10,2),
    @Estado NVARCHAR(10),
    @Fecha_Nacimiento DATE,
    @Fecha_Ingreso DATE
AS
BEGIN
    INSERT INTO Empleados (Nombre, Apellido, Direccion, Telefono, Email, Cargo, Departamento, Salario, Estado, Fecha_Nacimiento, Fecha_Ingreso)
    VALUES (@Nombre, @Apellido, @Direccion, @Telefono, @Email, @Cargo, @Departamento, @Salario, @Estado, @Fecha_Nacimiento, @Fecha_Ingreso);
END;


-- Procedimiento para obtener Cargos
CREATE PROCEDURE spObtenerCargos
AS
BEGIN
    SELECT Cargo FROM TablaCargos;
END;

-- Procedimiento para obtener Departamentos
CREATE PROCEDURE spObtenerDepartamentos
AS
BEGIN
    SELECT Departamento FROM TablaDepartamentos;
END;

DROP TABLE Asistencias;

CREATE TABLE Asistencias (
    ID_Asistencia INT IDENTITY(1,1) PRIMARY KEY,
    ID_Empleado INT NOT NULL,
    Fecha DATE NOT NULL,
    Hora_Entrada TIME,
    Hora_Salida TIME,
    Estado NVARCHAR(20) CHECK (Estado IN ('Presente', 'Ausente', 'Permiso')),
    Observaciones NVARCHAR(255),
    FOREIGN KEY (ID_Empleado) REFERENCES Empleados(ID_Empleado)
);

