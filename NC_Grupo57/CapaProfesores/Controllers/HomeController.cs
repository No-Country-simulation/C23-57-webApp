using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;
//using CapaProfesores.Permisos;

namespace CapaProfesores.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //[PermisosRol(1)]  -> Autorizo acceso sólo al Id_Rol = 1 (Administrador)
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        public ActionResult register()
        {

            return View();
        }

        public ActionResult RegistroTutor()
        {
            RolNegocio negoRol = new RolNegocio();
            List<Rol> roles = negoRol.obtenerTodosLosRoles();

            if (roles == null || !roles.Any())
            {
                roles = new List<Rol>(); // Si está nula, inicializamos una lista vacía
            }

            ViewBag.Roles = roles; // Asignamos la lista al ViewBag
            return View();
        }

        public ActionResult RegistroEstudiante()
        {
            return View();
        }

        public ActionResult RegistroProfesor()
        {
            return View();
        }

        public ActionResult CrearCuenta()
        {
            return View();
        }
    }
}