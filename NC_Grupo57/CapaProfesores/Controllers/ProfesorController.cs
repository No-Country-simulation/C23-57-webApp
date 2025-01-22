using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaProfesores.Controllers
{
    public class ProfesorController : Controller
    {
        // GET: Profesor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PrincipalProfesor()
        {
            return View();
        }
    }
}