using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CapaDominio;
using CapaNegocio;
using CapaProfesores.Permisos;
namespace CapaProfesores.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult login()
        {
            return View("login","Home");
        }

        [HttpPost]

        public ActionResult login(string correo, string clave)
        {

            if (UsuarioNegocio.validarLogin(correo, clave))
            {
                FormsAuthentication.SetAuthCookie(correo, false);

                Usuario user = new UsuarioNegocio().buscarPorEmail(correo);
                Session["Usuario"] = user;

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("login", "Home");

        }
        
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;

            return RedirectToAction("Index","Home");
        }
    }    
}