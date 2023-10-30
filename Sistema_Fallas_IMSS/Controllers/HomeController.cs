using Sistema_Fallas_IMSS.Models;
using Sistema_Fallas_IMSS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.IO;
using System.Web.Mvc;
using Rotativa;
using System.Runtime.Remoting;

namespace Sistema_Fallas_IMSS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult IndexUsuario()
        {
            return RedirectToAction("Index");
        }
        public ActionResult IndexAdmin()
        {
            return RedirectToAction("Index");
        }
        public ActionResult Index()
        {
            string usuario = User.Identity.Name;
            using(var context = new IMSSEntities())
            {
                if (usuario == null)
                {
                    return Redirect("~/Account/Login");
                }
                //var contexto = System.Web.HttpContext.Current;
                //string localIP = contexto.Request.UserHostAddress;
                string localIP = "";
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());// objeto para guardar la ip
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        localIP = ip.ToString().Trim();// esta es nuestra ip
                    }
                }

                var persona = context.existencias.Where(model => model.direccion_ip == localIP).Select(model => model.nombre_persona).FirstOrDefault();
                VM_Index data = new VM_Index
                {
                    Direccion_ip = localIP,
                    Usuario = usuario,
                    Persona = persona != null ? persona.ToString() : "Sin existencia actual",
                    Reporte = ObtnerDatos(),
                    Estatus = "1",
                };
                if (ObtenerRol(usuario) > 3)
                {
                    return View("IndexUsuario", data);
                }
                else
                    return View("IndexAdmin", data);
            }
            
        }
        public static int ObtenerRol(string _usuario)
        {
            using (var context = new IMSSEntities())
            {
                var usuario = context.usuarios.Where(us => us.cuenta == _usuario).FirstOrDefault();

                return (int)usuario.id_rol;
            }
        }

        public VM_Reportes ObtnerDatos()
        {
            using(var context = new IMSSEntities())
            {
                VM_Reportes reportes = new VM_Reportes
                {
                    Tipos = context.tipos_falla.Select(model => new SelectListItem
                    {
                        Value = model.Id_tipo_falla.ToString(),
                        Text = model.descripcion,
                    }).ToList(),
                };
                reportes.Tipos.Insert(reportes.Tipos.Count, new SelectListItem
                {
                    Value = "0",
                    Text = "Otro"
                });
                return reportes;
            }
        }
        [HttpPost]
        public ActionResult ModalReporte()
        {
            VM_Reportes data = ObtnerDatos();

            return PartialView("_ModalReporte",data);
        }
     
        [HttpGet]
        public JsonResult ObtenerFallas(int _id_tipo)
        {
            using (var context = new IMSSEntities())
            {
                List<SelectListItem> items = context.fallas.Where(x => x.Id_tipo_falla == _id_tipo).Select(x => new SelectListItem
                {
                    Value = x.Id_falla.ToString(),
                    Text = x.descripcion,
                }).ToList();

                return Json(items, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public int GenerarReporte(VM_Reportes _reporte)
        {
            using (var context = new IMSSEntities())
            {
                using(var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        reporte reporte = new reporte
                        {
                            ip_usuario = _reporte.Usuario,
                            descripcion = _reporte.Descripcion,
                            contacto = _reporte.Contacto,
                            estatus = 1,
                            fecha_registro = DateTime.Now,
                        };
                        context.reporte.Add(reporte);
                        context.SaveChanges();

                        reporte_fallas fallas = new reporte_fallas
                        {
                            id_reporte = reporte.Id_reporte,
                            falla = _reporte.Otra_falla,
                            id_falla = _reporte.Falla != "null" ? Convert.ToInt32(_reporte.Falla) : 0,
                        };
                        context.reporte_fallas.Add(fallas);
                        context.SaveChanges();
                        transaction.Commit();
                        return 1;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
        [HttpPost]
        public int FinalizarReporte(int _id_reporte)
        {
            try
            {
                using (var context = new IMSSEntities())
                {
                    var reporte = context.reporte.Find(_id_reporte);
                    reporte.estatus = 2;
                    reporte.fecha_concluido = DateTime.Now;
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
            
        }
        [HttpPost]
        public int CancelarReporte(int _id_reporte)
        {
            try
            {
                using (var context = new IMSSEntities())
                {
                    var reporte = context.reporte.Find(_id_reporte);
                    reporte.estatus = 3;
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        }
        public ActionResult Reporte(int id_reporte)
        {
            VM_Reportes reporte = ObtenerReporte(id_reporte);
            reporte.Fecha = reporte.Fecha_registro.ToLongDateString();
            reporte.Contacto = String.IsNullOrEmpty(reporte.Contacto) ? "Sin contacto" : reporte.Contacto;
            reporte.Estado = reporte.Estatus == 1 ? "En proceso" : reporte.Estatus == 2 ? "Atendido" : "Cancelado";
            return View("Documento",reporte);
        }

        public ActionResult Imprimir(int _id_reporte)
        {           
            return new ActionAsPdf("Reporte", new { id_reporte = _id_reporte });
        }

        public ActionResult AbrirModalFallas()
        {
            using (var context = new IMSSEntities())
            {
                var tipo_fallas = context.tipos_falla.Select(m => new { m.Id_tipo_falla, m.descripcion }).ToList();
                List<VM_TipoFallas> data = new List<VM_TipoFallas>();
                foreach (var item in tipo_fallas)
                {
                    VM_TipoFallas falla = new VM_TipoFallas { 
                        Id_tipo = item.Id_tipo_falla,
                        Tipo_falla = item.descripcion,
                    }; 
                    falla.Fallas = (from fallas in context.fallas
                                           join tipos in context.tipos_falla on fallas.Id_tipo_falla equals tipos.Id_tipo_falla
                                           where tipos.Id_tipo_falla == item.Id_tipo_falla
                                           select new VM_Fallas
                                           {
                                               Id = fallas.Id_falla,
                                               Descripcion = fallas.descripcion,
                                               Id_Tipo = tipos.Id_tipo_falla,

                                           }).ToList();
                    data.Add(falla);
                }
                return PartialView("_ModalTipoFallas",data);
            }
        }

        [HttpPost]
        public ActionResult ModalTipo(int _id_tipo)
        {
            VM_TipoFallas data = new VM_TipoFallas();
            if (_id_tipo == 0)
            {
                return PartialView("_ModalTipo", data);
            }
            else
            {
                using (var context = new IMSSEntities())
                {
                    var tipo = context.tipos_falla.Find(_id_tipo);
                    data.Id_tipo = tipo.Id_tipo_falla;
                    data.Tipo_falla = tipo.descripcion;
                    return PartialView("_ModalTipo", data);
                }
            }
            
        }
        [HttpPost]
        public ActionResult ModalFalla(int _id_falla, int _id_tipo)
        {
            VM_Fallas data = new VM_Fallas();

            using (var context = new IMSSEntities())
            {
                data.Tipos = context.tipos_falla.Select(x => new SelectListItem
                {
                    Value = x.Id_tipo_falla.ToString(),
                    Text = x.descripcion,
                }).ToList();
                data.Ddl_tipo = _id_tipo.ToString();

                if (_id_falla == 0)
                {
                    return PartialView("_ModalFallas", data);
                }
                var falla = context.fallas.Find(_id_falla);

                data.Id = falla.Id_falla;
                data.Ddl_tipo = falla.Id_tipo_falla.ToString();
                data.Descripcion = falla.descripcion;
                return PartialView("_ModalFallas", data);
            }
        }

        [HttpPost]
        public ActionResult RegistrarEditarTipo(VM_TipoFallas _tipo)
        {
            try
            {
                using (var context = new IMSSEntities())
                {
                    if (_tipo.Id_tipo > 0)
                    {
                        var tipo = context.tipos_falla.Find(_tipo.Id_tipo);
                        tipo.descripcion = _tipo.Tipo_falla;

                    }
                    else
                    {
                        tipos_falla nuevoTipo = new tipos_falla
                        {
                            descripcion = _tipo.Tipo_falla,
                        };
                        context.tipos_falla.Add(nuevoTipo);

                    }
                    context.SaveChanges();
                    _tipo.Mensaje = "okay";
                    return PartialView("_ModalTipo", _tipo);
                }
            }
            catch (Exception)
            {
                _tipo.Mensaje = "error";
                return PartialView("_ModalTipo", _tipo);
            }
        }

        [HttpPost]
        public ActionResult RegistrarEditarFalla(VM_Fallas _falla)
        {
            try
            {
                using (var context = new IMSSEntities())
                {
                    if (_falla.Id > 0)
                    {
                        var falla = context.tipos_falla.Find(_falla.Id);
                        falla.descripcion = _falla.Descripcion;
                        falla.Id_tipo_falla = Convert.ToInt32(_falla.Ddl_tipo);
                    }
                    else
                    {
                        fallas nuevaFalla = new fallas
                        {
                            descripcion = _falla.Descripcion,
                            Id_tipo_falla = Convert.ToInt32(_falla.Ddl_tipo),
                    };
                        context.fallas.Add(nuevaFalla);

                    }
                    context.SaveChanges();
                    _falla.Mensaje = "okay";
                    return PartialView("_ModalFallas", _falla);
                }
            }
            catch (Exception)
            {
                _falla.Mensaje = "error";
                return PartialView("_ModalFallas", _falla);
            }
        }

        private VM_Reportes ObtenerReporte(int _id_reporte)
        {
            using (var context = new IMSSEntities())
            {

                var sqlString = $@"SELECT * FROM (
                                    SELECT
                                        reporte.Id_reporte,
                                        existencias.nombre_persona usuario,
                                        reporte.descripcion,
                                        reporte.estatus,
                                        reporte.fecha_registro,
                                        reporte.fecha_concluido,
                                        reporte.contacto,
                                        fallas.descripcion falla,
                                        tipo.descripcion tipo,
                                        area.nombre_area
                                    FROM
                                        reporte
                                    INNER JOIN existencias ON reporte.ip_usuario = existencias.direccion_ip
                                    INNER JOIN areas_imss area ON existencias.id_area = area.Id_area
                                    LEFT JOIN reporte_fallas rfallas ON reporte.Id_reporte = rfallas.id_reporte
                                    INNER JOIN fallas ON rfallas.id_falla = fallas.Id_falla
                                    INNER JOIN tipos_falla tipo ON fallas.Id_tipo_falla = tipo.Id_tipo_falla
                                    UNION
                                    SELECT DISTINCT
                                        reporte.Id_reporte,
                                        COALESCE(existencias.nombre_persona, reporte.ip_usuario) AS usuario,
                                        reporte.descripcion,
                                        reporte.estatus,
                                        reporte.fecha_registro,
                                        reporte.fecha_concluido,
                                        reporte.contacto,
                                        COALESCE(fallas.descripcion, rfallas.falla) AS falla,
										COALESCE(tipo.descripcion, 'Otro') AS tipo,
                                        COALESCE(area.nombre_area, 'Sin registrar') AS usuario
                                    FROM
                                        reporte
                                    LEFT JOIN existencias ON reporte.ip_usuario = existencias.direccion_ip
									LEFT JOIN areas_imss area ON existencias.id_area = area.Id_area
									LEFT JOIN reporte_fallas rfallas ON reporte.Id_reporte = rfallas.id_reporte
									LEFT JOIN fallas ON rfallas.id_falla = fallas.Id_falla
									LEFT JOIN tipos_falla tipo ON fallas.Id_tipo_falla = tipo.Id_tipo_falla
                                  ) AS consulta
                                    WHERE consulta.Id_reporte = {_id_reporte};";

                return context.Database.SqlQuery<VM_Reportes>(sqlString).FirstOrDefault();
            }
        }

        public ActionResult IndexGrid(string _estatus, string _search)
        {
            _search = string.IsNullOrEmpty(_search) ? "%%" : "%" + _search + "%";
            _estatus = _estatus == "0" ? "%%" : _estatus;
            VM_Index data = new VM_Index
            {
                Reportes = ObtenerReportes(_estatus, _search),
            };
            return PartialView("_IndexGrid",data);
        }

        private List<VM_Reportes> ObtenerReportes(string _estatus, string _search)
        {
            using (var context = new IMSSEntities())
            {

                var sqlString = $@"SELECT * FROM (
                                    SELECT
                                        reporte.Id_reporte,
                                        existencias.nombre_persona usuario,
                                        reporte.descripcion,
                                        reporte.estatus,
                                        reporte.fecha_registro,
                                        reporte.fecha_concluido,
                                        reporte.contacto,
                                        fallas.descripcion falla,
                                        tipo.descripcion tipo,
                                        area.nombre_area
                                    FROM
                                        reporte
                                    INNER JOIN existencias ON reporte.ip_usuario = existencias.direccion_ip
                                    INNER JOIN areas_imss area ON existencias.id_area = area.Id_area
                                    LEFT JOIN reporte_fallas rfallas ON reporte.Id_reporte = rfallas.id_reporte
                                    INNER JOIN fallas ON rfallas.id_falla = fallas.Id_falla
                                    INNER JOIN tipos_falla tipo ON fallas.Id_tipo_falla = tipo.Id_tipo_falla
                                    UNION
                                    SELECT DISTINCT
                                        reporte.Id_reporte,
                                        COALESCE(existencias.nombre_persona, reporte.ip_usuario) AS usuario,
                                        reporte.descripcion,
                                        reporte.estatus,
                                        reporte.fecha_registro,
                                        reporte.fecha_concluido,
                                        reporte.contacto,
                                        COALESCE(fallas.descripcion, rfallas.falla) AS falla,
										COALESCE(tipo.descripcion, 'Otro') AS tipo,
                                        COALESCE(area.nombre_area, 'Sin registrar') AS usuario
                                    FROM
                                        reporte
                                    LEFT JOIN existencias ON reporte.ip_usuario = existencias.direccion_ip
									LEFT JOIN areas_imss area ON existencias.id_area = area.Id_area
									LEFT JOIN reporte_fallas rfallas ON reporte.Id_reporte = rfallas.id_reporte
									LEFT JOIN fallas ON rfallas.id_falla = fallas.Id_falla
									LEFT JOIN tipos_falla tipo ON fallas.Id_tipo_falla = tipo.Id_tipo_falla
                                  ) AS consulta
                                    WHERE 
                                    consulta.estatus LIKE '{_estatus}'
                                    AND(
                                    consulta.descripcion LIKE '{_search}'
                                    OR consulta.falla LIKE '{_search}'
                                    OR consulta.tipo LIKE '{_search}'
                                    OR consulta.nombre_area LIKE '{_search}'
                                    OR consulta.usuario LIKE '{_search}'
                                    )
                                  ORDER BY consulta.Id_reporte DESC";

                return context.Database.SqlQuery<VM_Reportes>(sqlString).ToList();
            }
        }
    }
}