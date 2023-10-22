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
            List<VM_Foro> data = ObtenerEntradasForo();
            return View(data);
        }

        public List<VM_Foro> ObtenerEntradasForo()
        {
            using (var context = new IMSSEntities())
            {
                List<VM_Foro> data = context.foro_soluciones.Select(foro =>
                new VM_Foro
                {
                    Id_foro = foro.Id_solucion,
                    Titutlo = foro.titulo,
                    Problema = foro.problema,
                    Solucion = foro.solucion,
                    Autor = foro.autor,
                    Correo = foro.correo,
                }).ToList();

                return data;
            }
        }
        [HttpPost]
        public ActionResult AbrirModalForo(int id_foro)
        {

        }
    }
}