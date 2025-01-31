using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaProfesores.Permisos;

namespace CapaProfesores.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        [PermisosRol(1)]
        public ActionResult PrincipalAdmin()
        {
            return View();
        }
    }
}