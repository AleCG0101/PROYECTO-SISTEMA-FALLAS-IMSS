
SELECT * FROM areas_imss

USE SISTEMA_FALLAS_IMSS
GO

 --INSERT INTO hospitales_imss(nombre,director,direccion,municipio,estado) VALUES('IMSS H.G.Z No.46','DR.','Blvd. Agustin Castro','Gomez Palacio','Durango')
INSERT INTO hospitales_imss(NOMBRE_IMSS,DIRECTOR,DIRECCION,MUNICIPIO,ESTADO) VALUES('IMSS H.G.Z No.10','DR.','Blvd. Agustin Castro','Gomez Palacio','Durango')
INSERT INTO hospitales_imss(NOMBRE_IMSS,DIRECTOR,DIRECCION,MUNICIPIO,ESTADO) VALUES('IMSS H.G.Z No.53','DR.','Blvd. Agustin Castro','Gomez Palacio','Durango')
--INSERT INTO areas_imss VALUES ('ADMINISTRACION',1)
INSERT INTO areas_imss(NOMBRE_AREA,ID_HOSPITAL) VALUES('URGENCIAS COORDINACION',1)
INSERT INTO areas_imss(NOMBRE_AREA,ID_HOSPITAL) VALUES('URGENCIAS COORDINACION',2)
--INSERT INTO roles VALUES ('Master')
--INSERT INTO roles VALUES ('Administrador')
--INSERT INTO roles VALUES ('Auxiliar')
--INSERT INTO roles VALUES ('Usuario')
INSERT INTO OPERACION(NOMBRE) VALUES ('CREAR')
INSERT INTO OPERACION(NOMBRE) VALUES ('EDITAR')
INSERT INTO OPERACION(NOMBRE) VALUES ('VER')
INSERT INTO OPERACION(NOMBRE) VALUES ('ELIMINAR')
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(1,1)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(1,2)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(1,3)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(1,4)
INSERT INTO USUARIO(NOMBRE,MATRICULA,CUENTA,PASS,ID_ROL,ID_IMSS) VALUES('Salome Baltazar Isais Zu�iga','112125','Baltazar.Isais','1234',1,1)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(2,1)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(2,2)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(2,3)
--INSERT INTO usuarios VALUES ('Master','','Admin','123456',1)
--INSERT INTO usuarios VALUE ('Administrador','','Administrador','123456',2)
--INSERT INTO usuarios VALUES ('Auxiliar','','Auxiliar','123456',3,1)
--INSERT INTO usuarios VALUES ('Usuario de Prueba','','Usuario','123456',4)
select * from usuarios
INSERT INTO usuarios(NOMBRE,MATRICULA,CUENTA,PASS,ID_ROL,ID_IMSS) VALUES('Victor Lopez','1000000','Victor.Lopez','1234',2,1)
INSERT INTO ROL_OPERACION(ID_ROL,ID_OPERACION)VALUES(3,1)

SELECT * FROM (
                                    SELECT
                                        reporte.Id_reporte,
                                        existencias.nombre_persona usuario,
                                        reporte.descripcion,
                                        reporte.estatus,
                                        reporte.fecha_registro,
                                        reporte.fecha_concluido,
                                        reporte.contacto,
                                        fallas.descripcion falla,
                                        tipo.descripcion tipo
                                    FROM
                                        reporte
                                    INNER JOIN existencias ON reporte.ip_usuario = existencias.direccion_ip
                                    INNER JOIN fallas ON reporte.id_falla = fallas.Id_falla
                                    INNER JOIN tipos_falla tipo ON fallas.Id_tipo_falla = tipo.Id_tipo_falla
                                    UNION
                                    SELECT
                                        reporte.Id_reporte,
                                        existencias.nombre_persona usuario,
                                        reporte.descripcion,
                                        reporte.estatus,
                                        reporte.fecha_registro,
                                        reporte.fecha_concluido,
                                        reporte.contacto,
                                        fallas.falla falla,
                                        'Otro' AS tipo
                                    FROM
                                        reporte
                                    INNER JOIN existencias ON reporte.ip_usuario = existencias.direccion_ip
                                    INNER JOIN reporte_fallas fallas ON reporte.Id_reporte = fallas.id_reporte
                                  ) AS consulta
                                  ORDER BY consulta.Id_reporte DESC