using CapaDominio;
using CapaNegocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaProfesores.Controllers
{
    public class RegisterController : Controller
    {
        private UsuarioNegocio negoUsu;
        private RolNegocio negoRol;

        public RegisterController()
        {
            negoUsu = new UsuarioNegocio();
            negoRol = new RolNegocio();
        }

        // GET: Register
        public ActionResult RegistroTutor()
        {
            List<Rol> roles = negoRol.obtenerTodosLosRoles();
            ViewBag.Roles = roles ?? new List<Rol>(); // Asignamos los roles al ViewBag
            return View();
        }

        [HttpPost]
        public ActionResult RegistroTutor(string txtNombre, string txtApellido, string txtTel, string txtDni, DateTime txtNacimiento, string typeofuser, string txtMail, string txtPass1, string txtPass2)
        {
            if (txtPass1 != txtPass2)
            {
                TempData["ErrorMessage"] = "Las contraseñas no coinciden.";
                return RedirectToAction("RegistroTutor", "Home");
            }

            Usuario usuario = new Usuario
            {
                Nombre = txtNombre,
                Apellido = txtApellido,
                Telefono = txtTel,
                DNI = txtDni,
                Id_Rol = int.Parse(typeofuser),
                Fecha_Nacimiento = txtNacimiento,
                Email = txtMail,
                Contrasenia = txtPass1, 
                Fecha_Alta = DateTime.Now,
                Activo = true
            };

            try
            {
                negoUsu.agregarUsuario(usuario);
                TempData["SuccessMessage"] = "Usuario registrado con éxito.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al registrar usuario: " + ex.Message;
            }

            return RedirectToAction("Index", "Home");
        }
    }
}