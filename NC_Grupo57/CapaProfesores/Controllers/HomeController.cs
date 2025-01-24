using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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

        [HttpGet]
        public ActionResult RegistroTutor()
        {
            RolNegocio negoRol = new RolNegocio();
            List<Rol> roles = negoRol.obtenerTodosLosRoles();
            
            ViewBag.Roles = roles;
            return View();
        }

        [HttpPost]
        public ActionResult RegistroTutor(string txtMail, string txtPass1, string txtPass2, string txtNombre, string txtApellido, string txtTel, string txtDni, DateTime? txtNacimiento, int typeofuser)
        {
            int idRolSeleccionado = typeofuser;
            RolNegocio negoRol = new RolNegocio();
            List<Rol> roles = negoRol.obtenerTodosLosRoles();
            if (string.IsNullOrEmpty(txtMail) || string.IsNullOrEmpty(txtPass1) || string.IsNullOrEmpty(txtNombre))
            {
                ViewBag.Error = "Por favor complete todos los campos obligatorios.";
                return View();
            }

            if (txtNacimiento == null)
            {
                txtNacimiento = DateTime.Now;
            }

            if (txtPass1 != txtPass2)
            {
                ViewBag.Error = "Las contraseñas no coinciden.";
                return View();
            }

            Usuario usuario = new Usuario
            {
                Nombre = txtNombre,
                Apellido = txtApellido,
                Telefono = txtTel,
                DNI = txtDni,
                Fecha_Nacimiento = (DateTime)txtNacimiento,
                Fecha_Alta = DateTime.Now,
                Id_Rol = typeofuser,
                Activo = false,
                Email = txtMail,
                Contrasenia = txtPass1
            };

            try
            {
                UsuarioNegocio negoUsu = new UsuarioNegocio();
                negoUsu.agregarUsuario(usuario);
                TempData["SuccessMessage"] = "El usuario fue registrado correctamente.";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al registrar el usuario: {ex.Message}";
                return View();
            }
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