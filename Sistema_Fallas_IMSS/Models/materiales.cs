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
    
    public partial class materiales
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public materiales()
        {
            this.existencias = new HashSet<existencias>();
        }
    
        public int Id_material { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int id_tipo_hardware { get; set; }
        public string centro_costos { get; set; }
        public string nombre_proyecto { get; set; }
        public int id_estado { get; set; }
        public string comentarios { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<existencias> existencias { get; set; }
        public virtual material_estados material_estados { get; set; }
        public virtual tipo_hardware tipo_hardware { get; set; }
    }
}
