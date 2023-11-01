USE SISTEMA_FALLAS_IMSS
GO

CREATE TABLE hospitales_imss(
Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
nombre VARCHAR(100) NOT NULL,
director VARCHAR(100) NOT NULL,
direccion VARCHAR(300) NULL,
municipio VARCHAR(100) NOT NULL,
estado VARCHAR(100) NOT NULL,
)

CREATE TABLE areas_imss(
Id_area INT IDENTITY(1,1) PRIMARY KEY,
nombre_area VARCHAR(250) NULL,
id_hospital INT NOT NULL
)

CREATE TABLE usuarios(
Id INT IDENTITY(1,1) PRIMARY KEY,
nombre VARCHAR(100) NULL,
matricula VARCHAR(100) NULL,
cuenta VARCHAR(100) NOT NULL,
pass VARCHAR (100) NOT NULL,
id_rol INT NOT NULL,
)

CREATE TABLE existencias(
Id_existencia INT IDENTITY(1,1) PRIMARY KEY,
id_material INT NOT NULL,
id_area INT NOT NULL,
tipo_existencia INT NOT NULL,
pc VARCHAR(100) NULL,
cuenta VARCHAR(100) NULL,
direccion_ip VARCHAR(100) NULL,
nombre_persona VARCHAR(100) NULL,
nsm VARCHAR(100) NULL,
nnn VARCHAR(100) NULL,
serial VARCHAR(100) NULL
)

CREATE TABLE roles(
Id_rol INT IDENTITY(1,1) PRIMARY KEY,
nombre VARCHAR(50) NOT NULL
)

CREATE TABLE rol_operacion(
Id INT IDENTITY(1,1) PRIMARY KEY,
Id_rol INT NOT NULL,
id_operacion INT NOT NULL
)

CREATE TABLE operacion(
Id_operacion INT IDENTITY(1,1) PRIMARY KEY,
nombre VARCHAR(50) NOT NULL
)

CREATE TABLE materiales(
Id_material INT IDENTITY(1,1) PRIMARY KEY,
nombre VARCHAR(100) NOT NULL,
marca VARCHAR(100) NOT NULL,
modelo VARCHAR(100) NULL,
id_tipo_hardware INT NOT NULL,
centro_costos VARCHAR(100) NOT NULL,
nombre_proyecto VARCHAR(100) NULL,
id_estado INT NOT NULL,
comentarios VARCHAR(200) NOT NULL,
)

CREATE TABLE material_estados(
Id_estado INT IDENTITY(1,1) PRIMARY KEY,
nombre VARCHAR(100) NOT NULL
) 

CREATE TABLE tipo_hardware(
Id_tipo_hardware INT IDENTITY(1,1) PRIMARY KEY,
tipo_producto VARCHAR (100) NULL
)

CREATE TABLE foro_soluciones(
Id_solucion INT IDENTITY(1,1) PRIMARY KEY,
titulo VARCHAR(250) NULL,
problema VARCHAR(MAX) NULL,
solucion VARCHAR(MAX) NULL,
autor VARCHAR(100) NULL,
correo VARCHAR(100) NULL,
)

-----------------------------------------------------------------------------------
CREATE TABLE reporte(
Id_reporte INT IDENTITY(1,1) PRIMARY KEY,
ip_usuario VARCHAR(100) NOT NULL,
descripcion VARCHAR(200) NOT NULL,
estatus INT NOT NULL,
fecha_registro DATETIME NOT NULL,
fecha_concluido DATETIME NULL,
contacto VARCHAR(50) NULL
)

CREATE TABLE tipos_falla(
Id_tipo_falla INT IDENTITY(1,1) PRIMARY KEY,
descripcion VARCHAR(100 )NULL
)

CREATE TABLE fallas(
Id_falla INT IDENTITY(1,1) PRIMARY KEY,
descripcion VARCHAR(100) NULL,
Id_tipo_falla INT NULL
)

CREATE TABLE reporte_fallas(
Id_reporte_falla INT IDENTITY(1,1) PRIMARY KEY,
id_reporte INT NOT NULL, --1, --1
id_falla int NULL, --1, --2
falla VARCHAR(300) NULL
)

------------------------------------------------------------------------------------

ALTER TABLE areas_imss
ADD FOREIGN KEY(id_hospital) REFERENCES hospitales_imss(Id)

ALTER TABLE usuarios
ADD FOREIGN KEY(id_rol) REFERENCES roles(Id_rol)

ALTER TABLE rol_operacion
ADD FOREIGN KEY(Id_rol) REFERENCES roles(Id_rol)

ALTER TABLE rol_operacion
ADD FOREIGN KEY(id_operacion) REFERENCES operacion(Id_operacion)

ALTER TABLE materiales
ADD FOREIGN KEY(id_tipo_hardware ) REFERENCES tipo_hardware(Id_tipo_hardware)

ALTER TABLE materiales
ADD FOREIGN KEY(id_estado ) REFERENCES material_estados(Id_estado)

ALTER TABLE existencias
ADD FOREIGN KEY(id_material) REFERENCES materiales(Id_material)

ALTER TABLE existencias
ADD FOREIGN KEY(id_area) REFERENCES areas_imss(Id_area)

----------------------------------------------------------------------------------

ALTER TABLE fallas
ADD FOREIGN KEY(Id_tipo_falla) REFERENCES tipos_falla(Id_tipo_falla)

ALTER TABLE reporte_fallas
ADD FOREIGN KEY(id_reporte) REFERENCES reporte(Id_reporte)
----------------------------------------------------------------------------------

ALTER TABLE reporte
ADD atendio VARCHAR(150) DEFAULT NULL

ALTER TABLE reporte
ADD solucion VARCHAR(300) DEFAULT NULL

--INSERTAR

 INSERT INTO hospitales_imss(nombre,director,direccion,municipio,estado) VALUES('IMSS H.G.Z No.46','DR.','Blvd. Agustin Castro','Gomez Palacio','Durango')
 INSERT INTO areas_imss VALUES ('ADMINISTRACION',1)

 INSERT INTO tipo_hardware VALUES('Computadora')
 INSERT materiales (nombre, marca, modelo,id_tipo_hardware, centro_costos, nombre_proyecto, id_estado, comentarios) VALUES (N'Computadora', N'Lenovo', N'M10', 1, N'Administracion', N'Administracion', 1, N'Ninguno')

INSERT INTO roles VALUES ('Master')
INSERT INTO roles VALUES ('Administrador')
INSERT INTO roles VALUES ('Auxiliar')
INSERT INTO roles VALUES ('Usuario')

INSERT INTO usuarios VALUES ('Master','','Admin','123456',1)
INSERT INTO usuarios VALUES ('Administrador','','Administrador','123456',1)
INSERT INTO usuarios VALUES ('Auxiliar','','Auxiliar','123456',3)
INSERT INTO usuarios VALUES ('Usuario de Prueba','','Usuario','123456',4)

INSERT INTO material_estados VALUES('Activo')
INSERT INTO material_estados VALUES('Inactivo')
---------------------------------------------------------------------------------
INSERT INTO tipos_falla(descripcion) VALUES
('Software'),
('Hardware'),
('Correo'),
('Internet'),
('Impresora')

INSERT INTO fallas(Id_tipo_falla, descripcion) VALUES
(1,'No puedes iniciar sesión en la cuenta'),
(1,'Instalación de alguna aplicación'),
(1,'Actualización de alguna aplicación'),
(1,'Esta lento el equipo'),
(1,'Tus documentos se hicieron acceso directo (Virus)'),
(1,'Se te olvido la contraseña para iniciar sesión'),
(1,'El equipo se reinicia o apaga solo'),
(2,'Tu equipo no prende'),
(2,'Monitor dañado'),
(2,'Ratón o teclado dañado'),
(2,'Pantalla se queda en negro'),
(2,'El equipo se calienta'),
(3,'Buzón lleno'),
(3,'No puedo iniciar sesión'),
(3,'Olvide mi contraseña'),
(4,'El icono del internet se muestra una X'),
(4,'El icono del internet se muestra con un signo de advertencia'),
(4,'No puedo acceder a las paginas'),
(4,'Cable dañado'),
(5,'No imprime'),
(5,'Enlazar impresora en mi equipo'),
(5,'Reemplazo de toner'),
(5,'Hojas se quedan atoradas'),
(5,'No puedo escanear')

INSERT INTO foro_soluciones(titulo, problema, solucion, autor, correo) VALUES
('Problema de prueba','Este es el problema de una entrada de prieba','El problema puede corresponder a su conexion a internet, no se estan introducioendo todos los datos necesarios o hay alguna falla interna, favor de acudir personalmente con el encargado del area de sistemas para identificar el caso especifico', 'Victor Manuel Lopez', 'victor.lopez@outlook.com')

select * from reporte