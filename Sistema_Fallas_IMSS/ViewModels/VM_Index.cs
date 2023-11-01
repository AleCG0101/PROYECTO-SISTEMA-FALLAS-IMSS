using Sistema_Fallas_IMSS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema_Fallas_IMSS.ViewModels
{
    public class VM_Index
    {
        public int Id_Usuario { get; set; }
        public string Persona { get; set; }
        public string Usuario { get; set; }
        public string Direccion_ip { get; set; }
        public string Estatus { get; set; }
        public VM_Reportes Reporte { get; set; }
        public List<VM_Reportes> Reportes { get; set; } = new List<VM_Reportes>();
       public int Rol { get; set; }
    }

    public class VM_Reportes
    {
        public int Id_reporte { get; set; }
        public string Usuario { get; set; }
        public string Nombre_area { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public int Estatus { get; set; }
        public DateTime Fecha_registro { get; set; }
        public string Fecha { get; set; }
        public DateTime? Fecha_concluido { get; set; }
        public string Contacto { get; set; }
        public List<SelectListItem> Tipos { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Fallas { get; set; } = new List<SelectListItem>();
        public string Falla { get; set; }
        public string Otra_falla { get; set; }
        public string Tipo { get; set; }
        public string Estado { get; set; }
    }
    public class VM_TipoFallas
    {
        public int Id_tipo { get; set; }
        public string Tipo_falla { get; set; }
        public List<VM_Fallas> Fallas { get; set; } = new List<VM_Fallas>();
        public string Mensaje { get; set; }

    }
    public class VM_Fallas
    {
        public int Id { get; set; }
        public int? Id_Tipo { get; set; }
        public string Ddl_tipo { get; set; }
        public List<SelectListItem> Tipos { get; set; } = new List<SelectListItem>();
        public string Descripcion { get; set; }
        public string Mensaje { get; set; }
    }
}