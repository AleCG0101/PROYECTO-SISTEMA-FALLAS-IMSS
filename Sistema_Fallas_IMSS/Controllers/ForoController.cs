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
    }
}