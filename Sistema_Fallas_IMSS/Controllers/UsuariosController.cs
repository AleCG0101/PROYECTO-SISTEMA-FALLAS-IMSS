using Sistema_Fallas_IMSS.Models;
using Sistema_Fallas_IMSS.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Sistema_Fallas_IMSS.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UsuariosGrid()
        {
            using (var context = new IMSSEntities())
            {
                List<VM_ListUsuarios> data = (from usuario in context.usuarios
                                         join roles in context.roles on usuario.id_rol equals roles.Id_rol
                                         where usuario.id_rol > (User.Identity.Name == "Master" ? 0 : 1)
                                         select new VM_ListUsuarios
                                         {
                                             Id = usuario.Id,
                                             Nombre = usuario.nombre,
                                             Cuenta = usuario.cuenta,
                                             Rol = roles.nombre
                                         }).ToList();

                return PartialView("_IndexGrid", data);
            }
        }
        [HttpPost]
        public PartialViewResult AbrirModalUsuarios(int _id_usuario)
        {
            using(var context = new IMSSEntities())
            {
                VM_ListUsuarios data = new VM_ListUsuarios();
                data.Roles = (from roles in context.roles
                              select new SelectListItem
                              {
                                  Value = roles.Id_rol.ToString(),
                                  Text = roles.nombre,
                              }).ToList();
                if (_id_usuario > 0)
                {
                    var usuario = (from usuarios in context.usuarios
                                   join roles in context.roles on usuarios.id_rol equals roles.Id_rol
                                   where usuarios.Id == _id_usuario
                                   select new VM_ListUsuarios
                                   {
                                       Id = usuarios.Id,
                                       Nombre = usuarios.nombre,
                                       Cuenta = usuarios.cuenta,
                                       Rol = roles.Id_rol.ToString(),
                                   }).FirstOrDefault();
                    data.Id = usuario.Id;
                    data.Nombre = usuario.Nombre;
                    data.Cuenta = usuario.Cuenta;
                    data.Rol = usuario.Rol;

                    return PartialView("_ModalUsuario", data);
                }
                data.Rol = "";
                return PartialView("_ModalUsuario", data);
            }
        }

        [HttpPost]
        public ActionResult RegistrarEditarUsuario(VM_ListUsuarios _usuario)
        {
            try
            {
                using (var context = new IMSSEntities())
                {
                    _usuario.Roles = (from roles in context.roles
                                      select new SelectListItem
                                      {
                                          Value = roles.Id_rol.ToString(),
                                          Text = roles.nombre,
                                      }).ToList();
                    if (!ModelState.IsValid)
                    {
                        _usuario.Mensaje = "validar";
                        return PartialView("_ModalUsuario", _usuario);
                    }
                    if (_usuario.Id > 0)
                    {
                        var usuario = context.usuarios.Find(_usuario.Id);
                        usuario.nombre = _usuario.Nombre;
                        usuario.cuenta = _usuario.Cuenta;
                        usuario.pass = string.IsNullOrEmpty(_usuario.Password) ? usuario.pass : Encriptar(_usuario.Password);
                        usuario.id_rol = Convert.ToInt32(_usuario.Rol);
                    }
                    else
                    {
                        usuarios usuario = new usuarios
                        {
                            nombre = _usuario.Nombre,
                            cuenta = _usuario.Cuenta,
                            pass = Encriptar(_usuario.Password),
                            id_rol = Convert.ToInt32(_usuario.Rol),
                        };
                        context.usuarios.Add(usuario);
                    }
                    context.SaveChanges();
                    _usuario.Mensaje = "okay";
                    return PartialView("_ModalUsuario", _usuario);
                }
            }
            catch (Exception)
            {
                _usuario.Mensaje = "error";
                return PartialView("_ModalUsuario", _usuario);
                throw;
            }
        }
        [HttpPost]
        public int EliminarUsuario(int _id_usuario)
        {
            try
            {
                using (var context = new IMSSEntities())
                {
                    var usuario = context.usuarios.Find(_id_usuario);
                    context.usuarios.Remove(usuario);
                    context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public string Encriptar(string _password)
        {
            byte[] data = System.Text.Encoding.Unicode.GetBytes(_password);
            string result = Convert.ToBase64String(data);
            return result;
        }
    }
}