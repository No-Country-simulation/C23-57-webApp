using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaProfesores.Permisos;

namespace CapaProfesores.Controllers
{
    public class ProfesorController : Controller
    {
        // GET: Profesor
        public ActionResult Index()
        {
            return View();
        }
        [PermisosRol(3)] //Solo Accesible para rol PROFESOR
        public ActionResult PrincipalProfesor()
        {
            return View();
        }
    }
}