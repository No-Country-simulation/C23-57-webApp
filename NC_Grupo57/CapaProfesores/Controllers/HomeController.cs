using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;

namespace CapaProfesores.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

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