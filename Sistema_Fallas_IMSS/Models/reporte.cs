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
    
    public partial class reporte
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public reporte()
        {
            this.reporte_fallas = new HashSet<reporte_fallas>();
        }
    
        public int Id_reporte { get; set; }
        public string ip_usuario { get; set; }
        public string descripcion { get; set; }
        public int estatus { get; set; }
        public System.DateTime fecha_registro { get; set; }
        public Nullable<System.DateTime> fecha_concluido { get; set; }
        public string contacto { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reporte_fallas> reporte_fallas { get; set; }
    }
}
