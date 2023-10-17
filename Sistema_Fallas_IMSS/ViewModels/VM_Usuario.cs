using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema_Fallas_IMSS.ViewModels
{
    public class VM_Usuario
    {
       

    }

    public class VM_ListUsuarios
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public string Nombre { get; set; }
        public string Matricula { get; set; }
        [Required(ErrorMessage = "El campo Cuenta es requerido")]
        public string Cuenta { get; set; }
        [CustomValidation(typeof(VM_ListUsuarios), "ValidarPassowrd", ErrorMessage = "El campo contraseña es requerido")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Selecciona un rol")]
        public string Rol { get; set; }
        public string Mensaje { get; set; }
        public List<SelectListItem> Roles { get; set; } = new List<SelectListItem>();

        public static ValidationResult ValidarPassowrd(string value, ValidationContext validationContext)
        {
            VM_ListUsuarios model = (VM_ListUsuarios)validationContext.ObjectInstance;

            if (model.Id == 0)
            {
                if (string.IsNullOrEmpty(model.Password))
                {
                    return new ValidationResult("El campo contraseña es requerido");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else
            {
                return ValidationResult.Success;
            }

        }
    }
}