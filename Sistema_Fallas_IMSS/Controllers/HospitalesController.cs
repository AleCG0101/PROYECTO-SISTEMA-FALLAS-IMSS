﻿using Sistema_Fallas_IMSS.Models;
using Sistema_Fallas_IMSS.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Sistema_Fallas_IMSS.Controllers
{
    public class HospitalesController : Controller
    {
        // GET: Hospitales
        [HttpGet]
        public ActionResult Index()
        {         

            return View();
        }

        public ActionResult HospitalesGrid(string _search)
        {
            _search = string.IsNullOrEmpty(_search) ? "" : _search;
            using(var context = new IMSSEntities())
            {
                VM_Hospital data = new VM_Hospital
                {

                    Hospitales = (from hospital in context.hospitales_imss
                                  where hospital.nombre.Contains(_search)
                                    || hospital.direccion.Contains(_search)
                                    || hospital.municipio.Contains(_search)
                                  select new VM_Hospitales
                                  {
                                      Id = hospital.Id,
                                      Nombre = hospital.nombre,
                                      Director = hospital.director,
                                      Direccion = hospital.direccion,
                                      Municipio = hospital.municipio,
                                      Estado = hospital.estado,

                                  }).ToList(),
                }; 
                return PartialView("_HospitalesGrid", data);
            }
        }

        public ActionResult AreasGrid(string _search)
        {
            _search = string.IsNullOrEmpty(_search) ? "" : _search;
            using (var context = new IMSSEntities())
            {
                VM_Area data = new VM_Area
                {
                    Areas = (from area in context.areas_imss
                             join hospital in context.hospitales_imss
                                on area.id_hospital equals hospital.Id
                            where area.nombre_area.Contains(_search)
                             select new VM_Areas
                             {
                                 Id = area.Id_area,
                                 Nombre_area = area.nombre_area,
                                 Id_hospital = area.id_hospital,
                                 Hospital = hospital.nombre,
                             }).ToList(),
                };
                return PartialView("_AreasGrid", data);
            }
        }

        [HttpPost]
        public ActionResult AbrirModalHospital(int _id_hospital)
        {
            using(var context = new IMSSEntities())
            {
                VM_Hospitales data = new VM_Hospitales();
                if (_id_hospital > 0)
                {
                    var hospital = context.hospitales_imss.Find(_id_hospital);

                    data = new VM_Hospitales{
                        Id = hospital.Id,
                        Nombre = hospital.nombre,
                        Director = hospital.director,
                        Direccion = hospital.direccion,
                        Municipio = hospital.municipio,
                        Estado = hospital.estado,
                    };
                }

                return PartialView("_ModalHospital",data);
            }
           
        }

        [HttpPost]
        public ActionResult AbrirModalArea(int _id_area)
        {
            using(var context = new IMSSEntities())
            {
                VM_Areas data = new VM_Areas();
                if (_id_area > 0)
                {
                    var area = context.areas_imss.Find(_id_area);
                    data.Id = area.Id_area;
                    data.Nombre_area = area.nombre_area;
                    data.Id_hospital = area.id_hospital;
                    data.Hospital = area.id_hospital.ToString();
                    
                }
                data.Hospitales = (from hospitales in context.hospitales_imss
                                   select new SelectListItem
                                   {
                                       Value = hospitales.Id.ToString(),
                                       Text = hospitales.nombre,
                                   }).ToList();
                return PartialView("_ModalAreas", data);
            }
        }

        [HttpPost]
        public PartialViewResult RegistrarEditarHospital(VM_Hospitales _hospital)
        {
            if (!ModelState.IsValid)
            {
                _hospital.Mensaje = "validacion";
                return PartialView("_ModalHospital", _hospital);
            }
            using (var context = new IMSSEntities())
            {
                try
                {
                    if (_hospital.Id > 0)
                    {
                        var hospital = context.hospitales_imss.Find(_hospital.Id);
                        hospital.nombre = _hospital.Nombre;
                        hospital.director = _hospital.Director;
                        hospital.direccion = _hospital.Direccion;
                        hospital.municipio = _hospital.Municipio;
                        hospital.estado = _hospital.Estado;
                    }
                    else
                    {
                        hospitales_imss hospital = new hospitales_imss
                        {
                            nombre = _hospital.Nombre,
                            director = _hospital.Director,
                            direccion = _hospital.Direccion,
                            municipio = _hospital.Municipio,
                            estado = _hospital.Estado,
                        };
                        context.hospitales_imss.Add(hospital);

                    }
                    context.SaveChanges();
                    _hospital.Mensaje = "okay";
                    return PartialView("_ModalHospital", _hospital);
                }
                catch (Exception)
                {
                    _hospital.Mensaje = "error";
                    return PartialView("_ModalHospital", _hospital);
                    throw;
                }
                
            }
           
        }

        [HttpPost]
        public ActionResult RegistrarEditarArea(VM_Areas _area)
        {
            using(var context = new IMSSEntities())
            {
                _area.Hospitales = (from hospitales in context.hospitales_imss
                                   select new SelectListItem
                                   {
                                       Value = hospitales.Id.ToString(),
                                       Text = hospitales.nombre,
                                   }).ToList();
                if (!ModelState.IsValid)
                {
                    _area.Mensaje = "validacion";
                    return PartialView("_ModalAreas", _area);
                }
                try
                {
                    if (_area.Id > 0)
                    {
                        var area = context.areas_imss.Find(_area.Id);
                        area.nombre_area = _area.Nombre_area;
                        area.id_hospital = _area.Id_hospital;

                    }
                    else
                    {
                        areas_imss area = new areas_imss
                        {
                            nombre_area = _area.Nombre_area,
                            id_hospital = _area.Id_hospital,
                        };
                        context.areas_imss.Add(area);
                    }
                    context.SaveChanges();
                    _area.Mensaje = "okay";
                    return PartialView("_ModalAreas", _area);
                }
                catch (Exception)
                {
                    _area.Mensaje = "error";
                    return PartialView("_ModalAreas", _area);
                }
            }
        }
    }
}