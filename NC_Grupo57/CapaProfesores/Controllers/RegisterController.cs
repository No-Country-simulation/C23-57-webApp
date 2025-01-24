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
        private Usuario usuario;
        private UsuarioNegocio negoUsu;
        private Rol rol;
        private RolNegocio negoRol;
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ResgistroTutor(string txtNombre, string txtApellido, string txtTel, string txtDni, DateTime txtNacmiento, string typeofuser)
        {
            negoRol = new RolNegocio();
            List<Rol> roles = negoRol.obtenerTodosLosRoles();

            if (roles == null || !roles.Any())
            {
                roles = new List<Rol>(); // Si está nula, inicializamos una lista vacía
            }

            ViewBag.Roles = roles; // Asignamos la lista al ViewBag
            
            rol = new Rol();
            
            
            usuario = new Usuario();
            usuario.Nombre = txtNombre;
            usuario.Apellido = txtApellido;
            usuario.Telefono = txtTel;
            usuario.DNI = txtDni;
            usuario.Id_Rol = 1;  
            usuario.Fecha_Nacimiento = txtNacmiento;
            usuario.Fecha_Alta = DateTime.Now;
            usuario.Activo = false;


            return View();
        }
    }
}