using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sistema_Fallas_IMSS.ViewModels
{
    public class VM_Hospital
    {
        public string Direccion_ip {  get; set; }
        public List<VM_Hospitales> Hospitales { get; set;}
    }
    public class VM_Hospitales
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Introduce un Nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Director es requerido")]
        public string Director { get; set; }
        [Required(ErrorMessage = "Introduce una Direccion")]
        public string Direccion { get; set; }
        [Required(ErrorMessage = "Introduce un Municipio")]
        public string Municipio { get; set; }
        [Required(ErrorMessage = "Introduce un Estado")]
        public string Estado { get; set; }
        public string Mensaje { get; set; }
    }
}