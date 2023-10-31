using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema_Fallas_IMSS.ViewModels
{
    public class VM_Foro
    {
        public int Id_foro { get; set; }
        [Required] 
        public string Titutlo { get; set; }
        [Required]
        public string Problema { get; set; }
        [Required]
        public string Solucion { get; set; }
        [Required]
        public string Autor { get; set; }
        [Required]
        public string Correo { get; set; }
        public string Mensaje { get; set; }
    }
}