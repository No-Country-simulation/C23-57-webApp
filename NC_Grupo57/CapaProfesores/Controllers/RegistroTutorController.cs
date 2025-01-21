using CapaDominio;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaProfesores.Controllers
{
    public class RegistroTutorController : Controller
    {
        // GET: RegistroTutor
        public ActionResult Index()
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
            return View("");
        }
    }
}