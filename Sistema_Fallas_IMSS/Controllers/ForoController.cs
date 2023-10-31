using Sistema_Fallas_IMSS.Models;
using Sistema_Fallas_IMSS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema_Fallas_IMSS.Controllers
{
    public class ForoController : Controller
    {
        // GET: Foro
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PartialIndex(string _search)
        {
            _search = string.IsNullOrEmpty(_search) ? "" : _search;
            List<VM_Foro> data = ObtenerEntradasForo(_search);
            return PartialView("_PartialIndex",data);
        }

        public List<VM_Foro> ObtenerEntradasForo(string _search)
        {
            using (var context = new IMSSEntities())
            {
                List<VM_Foro> data = context.foro_soluciones
                    .Where(foro => foro.titulo.Contains(_search) || foro.problema.Contains(_search))
                    .Select(foro =>
                new VM_Foro
                {
                    Id_foro = foro.Id_solucion,
                    Titutlo = foro.titulo,
                    Problema = foro.problema,
                    Solucion = foro.solucion,
                    Autor = foro.autor,
                    Correo = foro.correo,
                }).OrderByDescending(x=> x.Id_foro).ToList();

                return data;
            }
        }
        [HttpPost]
        public ActionResult AbrirModalForo(int id_foro)
        {
            using (var context = new IMSSEntities())
            {
                VM_Foro data = new VM_Foro();

                if (id_foro > 0)
                {
                    var foro = context.foro_soluciones.Find(id_foro);
                    if (foro != null)
                    {
                        data = new VM_Foro
                        {
                            Id_foro = foro.Id_solucion,
                            Titutlo = foro.titulo,
                            Problema = foro.problema,
                            Solucion = foro.solucion,
                            Autor = foro.autor,
                            Correo = foro.correo,
                        };

                        return PartialView("_ModalRegistrarForo",data);
                    }                   
                }
                return PartialView("_ModalRegistrarForo", data);
            }
        }

        [HttpPost]
        public ActionResult GenerarRegistroForo(VM_Foro _data)
        {
            if (!ModelState.IsValid)
            {
                _data.Mensaje = "validacion";
                return PartialView("_ModalRegistrarForo", _data);
            }
                
            using (var context = new IMSSEntities())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var foro = context.foro_soluciones.Find(_data.Id_foro);
                        if (foro != null)
                        {
                            foro.titulo = _data.Titutlo;
                            foro.problema = _data.Problema;
                            foro.solucion = _data.Solucion;
                            foro.autor = _data.Autor;
                            foro.correo = _data.Correo;
                            context.SaveChanges();
                        }
                        else
                        {
                            foro_soluciones newForo = new foro_soluciones
                            {
                                titulo = _data.Titutlo,
                                problema = _data.Problema,
                                solucion = _data.Solucion,
                                autor = _data.Autor,
                                correo = _data.Correo,
                            };
                            context.foro_soluciones.Add(newForo);
                            context.SaveChanges();
                        }                       
                        transaction.Commit();
                        _data.Mensaje = "okay";
                        return PartialView("_ModalRegistrarForo", _data);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        _data.Mensaje = "error";
                        return PartialView("_ModalRegistrarForo", _data);
                        throw;
                    }

                }

            }
            
        }
    }
}