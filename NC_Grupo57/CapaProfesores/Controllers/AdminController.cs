using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaDominio;
using CapaNegocio;
using CapaProfesores.Permisos;

namespace CapaProfesores.Controllers
{
    public class AdminController : Controller
    {
        private UsuarioNegocio user = new UsuarioNegocio();
        private MateriaNegocio materia = new MateriaNegocio();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [PermisosRol(1)]
        public ActionResult PrincipalAdmin(string vista = "Materias", string filtro = "Todos")
        {
            if (vista == "Profesores")
            {
                var profesores = user.ObtenerProfesores(filtro); 
                ViewBag.Profesores = profesores;
                ViewBag.VistaActual = "Profesores";
            }
            else if (vista == "Materias")
            {
                var materias = materia.obtenerTodasLasMaterias(); 
                
                var profesoresActivos = user.ObtenerProfesores("Activos"); // Obtiene los profesores activos

                ViewBag.Materias = materias;
                
                ViewBag.Profesores = profesoresActivos;
                ViewBag.VistaActual = "Materias";
            }
            else if (vista == "Alumnos")
            {
                var alumnos = user.ObtenerAlumnos(filtro);
                ViewBag.Alumnos = alumnos;
                ViewBag.VistaActual = "Alumnos";
            }

                return View();
        }

        [HttpPost]
        public ActionResult AgregarMateria(string nombre)
        {
            if (!user.AgregarMateria(nombre))
            {
                return RedirectToAction("PrincipalAdmin");
            }
            return RedirectToAction("PrincipalAdmin");
        }

        [HttpPost]
        public ActionResult AgregarProfesor(string nombre, string apellido, string email, string contrasenia)
        {
            user.AgregarProfesor(nombre, apellido, email, contrasenia);
            return RedirectToAction("PrincipalAdmin", new { vista = "Profesores" });
        }

        [HttpPost]
        public ActionResult EliminarMateria(int id)
        {
            if (!user.EliminarMateria(id))
            {
                return RedirectToAction("PrincipalAdmin");
            }
            return RedirectToAction("PrincipalAdmin");
        }

        [HttpPost]
        public ActionResult EliminarProfesor(int id)
        {
            if (user.EliminarProfesor(id))
            {
                return RedirectToAction("PrincipalAdmin", new { vista = "Profesores" });
            }
            return RedirectToAction("PrincipalAdmin", new { vista = "Profesores" });
        }
        [HttpPost]
        public ActionResult EliminarAlumno(int id)
        {
            if (user.EliminarAlumno(id))
            {
                return RedirectToAction("PrincipalAdmin", new { vista = "Alumnos" });
            }
            return RedirectToAction("PrincipalAdmin", new { vista = "Alumnos" });
        }

        [HttpPost]
        public ActionResult ModificarMateria(int id, string profesorId, bool activo, string nombre)
        {
            if (!int.TryParse(profesorId, out int profesorIdInt))
            {
                // Manejar el caso en que la conversión falle
                return RedirectToAction("PrincipalAdmin", new { vista = "Materias", error = "InvalidProfesorId" });
            }

            if (!user.agregarMateriaxProfesor(id, profesorIdInt, activo, nombre))
            {
                return RedirectToAction("PrincipalAdmin");
            }

            return RedirectToAction("PrincipalAdmin");
        }

        [HttpPost]
        public ActionResult ModificarProfesor(int id, string nombre, string apellido, string email, bool activo)
        {
            user.modificarProfesor(id, nombre, apellido, email, activo.ToString());
            return RedirectToAction("PrincipalAdmin", new { vista = "Profesores" });
        }

        [HttpPost]
        public ActionResult ModificarAlumno(int id, string nombre, string apellido, string email, bool activo)
        {
            user.modificarAlumno(id, nombre, apellido, email, activo.ToString());
            return RedirectToAction("PrincipalAdmin", new { vista = "Alumnos" });
        }

        [HttpPost]
        public ActionResult FiltrarProfesores(string filtro)
        {
            ViewBag.VistaActual = "Profesores";
            List<Usuario> profesores = user.ObtenerProfesores(filtro);
            ViewBag.Profesores = profesores;
            ViewBag.Filtro = filtro;  
            return View("PrincipalAdmin");
        }
        [HttpPost]
        public ActionResult FiltrarAlumnos(string filtro)
        {
            ViewBag.VistaActual = "Alumnos";
            List<Usuario> alumnos = user.ObtenerAlumnos(filtro);
            ViewBag.Alumnos = alumnos;
            ViewBag.Filtro = filtro;
            return View("PrincipalAdmin");
        }


        [HttpPost]
        public ActionResult VerProfesoresAsignados(int idMateria)
        {
           
                var materiasxProfesor = user.obtenerProfesoresxMateria(idMateria);
                var profesoresMaterias = user.obtenerTodosLosProfesoresxMateria();

                TempData["ProfesoresAsignados"] = materiasxProfesor;
                TempData["MateriaId"] = idMateria;  // Aquí se almacena el ID de la materia
                TempData["ProfesoresMaterias"] = profesoresMaterias;

                return RedirectToAction("PrincipalAdmin", new { vista = "Materias" });
        }
        [HttpPost]
        public ActionResult QuitarProfesorDeMateria(int idProfesor, int idMateria)
        {
            if (!user.eliminarProfesorxMateria(idProfesor, idMateria))
            {
                return RedirectToAction("PrincipalAdmin", new { vista = "Materias" });
            }

            // Recargar la lista de profesores asignados a la materia
            var materiasxProfesor = user.obtenerProfesoresxMateria(idMateria);
            var profesoresMaterias = user.obtenerTodosLosProfesoresxMateria();

            TempData["ProfesoresAsignados"] = materiasxProfesor;
            TempData["MateriaId"] = idMateria;  // Mantener la modal abierta
            TempData["ProfesoresMaterias"] = profesoresMaterias;

            return RedirectToAction("PrincipalAdmin", new { vista = "Materias" });
        }



    }






}

