//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sistema_Fallas_IMSS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class existencias
    {
        public int Id_existencia { get; set; }
        public int id_material { get; set; }
        public int id_area { get; set; }
        public int tipo_existencia { get; set; }
        public string pc { get; set; }
        public string cuenta { get; set; }
        public string direccion_ip { get; set; }
        public string nombre_persona { get; set; }
        public string nsm { get; set; }
        public string nnn { get; set; }
        public string serial { get; set; }
    
        public virtual areas_imss areas_imss { get; set; }
        public virtual materiales materiales { get; set; }
    }
}
