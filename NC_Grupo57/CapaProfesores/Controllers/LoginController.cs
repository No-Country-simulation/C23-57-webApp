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
                int idRol = user.Id_Rol;
                switch (idRol)
                {
                    case 1:
                        return RedirectToAction("PrincipalAdmin", "Admin");
                    case 2:
                        return RedirectToAction("PrincipalProfesor", "Profesor"); //Deberia redireccionar a la vista del panel de Alumno
                    case 3:
                        return RedirectToAction("PrincipalProfesor", "Profesor");
                    case 4:
                        return RedirectToAction("PrincipalProfesor", "Profesor");  //Deberia redireccionar a la vista del panel de Tutor
                    default:
                        return RedirectToAction("login", "Home");
                }                
            }
            else
            {
                ViewBag.Error = "Error de credenciales";
                
                return RedirectToAction("login", "Home");
            }


        }
        
        public ActionResult CerrarSesion()
        {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;

            return RedirectToAction("Index","Home");
        }
    }    
}