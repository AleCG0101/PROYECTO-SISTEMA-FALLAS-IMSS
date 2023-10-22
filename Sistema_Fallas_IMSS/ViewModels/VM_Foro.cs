using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistema_Fallas_IMSS.ViewModels
{
    public class VM_Foro
    {
        public int Id_foro { get; set; }
        public string Titutlo { get; set; }
        public string Problema { get; set; }
        public string Solucion { get; set; }
        public string Autor { get; set; }
        public string Correo { get; set; }
    }
}