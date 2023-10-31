using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema_Fallas_IMSS.ViewModels
{
    public class VM_Materiales
    {
        public List<VM_MaterialesCatalogo> Materiales { get; set; } = new List<VM_MaterialesCatalogo>();

    }

    public class VM_MaterialesCatalogo
    {
        public int Id_material { get; set; }
        [Required(ErrorMessage = "Introduce un nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Introduce una marca")]
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public List<SelectListItem> Tipos { get; set; }
        [Required(ErrorMessage = "Selecciona un tipo")]
        public string Tipo_hardware { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Centro_costos { get; set; }
        public string Proyecto { get; set; }
        public List<SelectListItem> Estados = new List<SelectListItem>();
        [Required(ErrorMessage = "Selecciona un estado")]
        public string Estado { get; set; }
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Comentarios { get; set; }
        public string Mensaje { get; set; }
    }

    public class VM_Existencias
    {
        public int Id_existencia { get; set; }
        [Required(ErrorMessage = "Seleccione una area")]
        public string Area { get; set; }
        public List<SelectListItem> Materiales { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> Areas { get; set; } = new List<SelectListItem>();
        [Required(ErrorMessage = "Seleccione un material")]
        public string Material { get; set; }
        [CustomValidation(typeof(VM_Existencias), "ValidarTipo")]
        public string Pc { get; set; }
        [CustomValidation(typeof(VM_Existencias), "ValidarTipo")]
        public string Cuenta { get; set; }
        [CustomValidation(typeof(VM_Existencias), "ValidarTipo")]
        public string Nombre_persona { get; set; }
        [CustomValidation(typeof(VM_Existencias), "ValidarTipo")]
        public string Direccion_ip { get; set; }
        public string Serial { get; set; }
        public string Nsm { get; set; }
        public string Nnn { get; set; }
        public int Tipo_existencia { get; set; }
        public bool Tipo { get; set; }
        public string Mensaje { get; set; }

        public static ValidationResult ValidarTipo(string value, ValidationContext validationContext)
        {
            VM_Existencias model = (VM_Existencias)validationContext.ObjectInstance;

            if (model.Tipo_existencia == 1)
            {
                return ValidationResult.Success;
            }
            else
            {
                if (string.IsNullOrEmpty(model.Pc) || string.IsNullOrEmpty(model.Cuenta) || string.IsNullOrEmpty(model.Nombre_persona) || string.IsNullOrEmpty(model.Direccion_ip))
                {
                    return new ValidationResult("Este campo es obligatorio");
                }
                return ValidationResult.Success;
            }

        }
    }

    public class VM_TipoHardware
    {
        public int Id_tipo_hardware { get; set; }
        [Required(ErrorMessage = "Introduza una descripcion")]
        public string Tipo_producto { get; set; }
        public string Mensaje { get; set; }
    }


}