using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CapaDominio;
using CapaNegocio;
using CapaProfesores.Permisos;
using Microsoft.Ajax.Utilities;

namespace CapaProfesores.Controllers
{
    public class ProfesorController : Controller
    {
        private MateriaNegocio materiaNegocio = new MateriaNegocio();
        private UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
        private NotaNegocio notaNegocio = new NotaNegocio();
        private TipoNotaNegocio tipoNotaNegocio = new TipoNotaNegocio();

        [PermisosRol(3)]
        [HttpGet]
        public ActionResult PrincipalProfesor()
        {
            Usuario usuario = Session["Usuario"] as Usuario;
            if (usuario == null) return RedirectToAction("Login", "Home");

            int idProfesor = (int)usuario.Id_Usuario;
            List<Materia> materias = materiaNegocio.ObtenerMateriasPorProfesor(idProfesor);

            // Diccionario para almacenar los códigos de comisión por materia
            Dictionary<int, List<MateriaProfesor>> codigosPorMateria = new Dictionary<int, List<MateriaProfesor>>();

            // Diccionario para almacenar los alumnos por materia.
            Dictionary<int,List<Usuario>> alumnosPorMateria = new Dictionary<int, List<Usuario>>();

            // Iterar sobre las materias para obtener códigos de comisión y alumnos por materia
            foreach (var materia in materias)
            {
                // Obtener los códigos de comisión de la materia
                List<MateriaProfesor> codigos = materiaNegocio.ObtenerCodigosComisionPorMateria(materia.Nombre);
                codigosPorMateria[materia.Id_Materia] = codigos;  // Guardamos los códigos en el diccionario
                // Obtener los alumnos de la materia
                //List<Usuario> alumnos = materiaNegocio.ObtenerAlumnosPorMateria(materia.Id_Materia);
                //alumnosPorMateria[materia.Id_Materia] = alumnos;  
            }
            foreach (var comision in codigosPorMateria)
            {
                int idMateria = comision.Key; // El ID de la materia
                List<Usuario> alumnosMateria = new List<Usuario>();

                foreach (var codigo in comision.Value) // Lista de MateriaProfesor (códigos de comisión)
                {
                    List<Usuario> alumnos = materiaNegocio.ObtenerAlumnosPorMateria(idMateria, codigo.Codigo_Comision);
                    alumnosMateria.AddRange(alumnos);
                }

                // Evitar duplicados en la lista de alumnos
                alumnosMateria = alumnosMateria.GroupBy(a => a.Id_Usuario).Select(g => g.First()).ToList();

                alumnosPorMateria[idMateria] = alumnosMateria;
            }

            // Obtener todos los alumnos activos
            var todosLosAlumnos = usuarioNegocio.ObtenerAlumnosActivos();

            // Filtrar los que NO están en la materia
            var alumnosDisponibles = todosLosAlumnos.Where(a =>
                !alumnosPorMateria.Values.SelectMany(al => al).Any(alumno => alumno.Id_Usuario == a.Id_Usuario))
                .ToList();

            ViewBag.AlumnosDisponibles = alumnosDisponibles; // Guardamos la lista filtrada


            ViewBag.MateriasProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor);
            ViewBag.AlumnosPorMateria = alumnosPorMateria;  // Diccionario con alumnos filtrados por materia
            ViewBag.TiposNota = tipoNotaNegocio.obtenerTodosLosTiposNotaActivos();
            ViewBag.CodigoComision = codigosPorMateria;  // Diccionario con códigos de comisión por materia

            
            return View(materias);
        }


        [HttpPost]
        public ActionResult CargarNota(long Id_Usuario_Alumno, long Codigo_Comision, int Id_Tipo_Nota, decimal Calificacion, string Observaciones)
        {
            try
            {
                Nota nuevaNota = new Nota
                {
                    Id_Usuario_Alumno = Id_Usuario_Alumno,
                    Codigo_Comision = Codigo_Comision,
                    Id_Tipo_Nota = Id_Tipo_Nota,
                    Calificacion = Calificacion,
                    Observaciones = Observaciones
                };

                notaNegocio.agregarNota(nuevaNota);
                TempData["Mensaje"] = "Nota cargada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al guardar la nota: " + ex.Message;
            }

            return RedirectToAction("PrincipalProfesor");
        }

        [HttpPost]
        public ActionResult VerNotasAlumno(int Id_Usuario_Alumno, long Codigo_Comision, int idMateria)
        {
            try
            {
                var usuario = Session["Usuario"] as Usuario;
                if (usuario == null) return RedirectToAction("Login", "Home");

                var notasxAlumno = notaNegocio.ObtenerNotasPorAlumnoMateria(Id_Usuario_Alumno, idMateria);

                ViewBag.IdMateriaSeleccionada = idMateria;
                ViewBag.NotasxAlumno = notasxAlumno ?? new List<NotaAlumno>();
                ViewBag.IdAlumnoSeleccionado = Id_Usuario_Alumno;

                int idProfesor = (int)usuario.Id_Usuario;
                List<Materia> materias = materiaNegocio.ObtenerMateriasPorProfesor(idProfesor);
                Dictionary<int, List<Usuario>> alumnosPorMateria = new Dictionary<int, List<Usuario>>();
                Dictionary<int, List<MateriaProfesor>> codigosPorMateria = new Dictionary<int, List<MateriaProfesor>>();

                foreach (var materia in materias)
                {
                    List<MateriaProfesor> codigos = materiaNegocio.ObtenerCodigosComisionPorMateria(materia.Nombre);
                    codigosPorMateria[materia.Id_Materia] = codigos;  // Guardamos los códigos en el diccionario
                }
                foreach (var comision in codigosPorMateria)
                {
                    int id_Materia = comision.Key; // El ID de la materia
                    List<Usuario> alumnosMateria = new List<Usuario>();

                    foreach (var codigo in comision.Value) // Lista de MateriaProfesor (códigos de comisión)
                    {
                        List<Usuario> alumnos = materiaNegocio.ObtenerAlumnosPorMateria(id_Materia, codigo.Codigo_Comision);
                        alumnosMateria.AddRange(alumnos);
                    }

                    // Evitar duplicados en la lista de alumnos
                    alumnosMateria = alumnosMateria.GroupBy(a => a.Id_Usuario).Select(g => g.First()).ToList();

                    alumnosPorMateria[id_Materia] = alumnosMateria;
                }


                // Enviar los datos a la vista
                ViewBag.AlumnosPorMateria = alumnosPorMateria;
                ViewBag.MateriasProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor);
                ViewBag.TiposNota = tipoNotaNegocio.obtenerTodosLosTiposNotaActivos();

                return View("PrincipalProfesor", materias);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener notas: " + ex.Message;
                return RedirectToAction("PrincipalProfesor");
            }
        }





        [HttpGet]
        public ActionResult VerNotasAlumno(int Id_Usuario_Alumno, int idMateria)
        {
            try
            {
                var usuario = Session["Usuario"] as Usuario;
                if (usuario == null) return RedirectToAction("Login", "Home");

                // Obtener las notas del alumno en la materia seleccionada
                var notasxAlumno = notaNegocio.ObtenerNotasPorAlumnoMateria(Id_Usuario_Alumno, idMateria);

                // Asegurar que hay notas antes de enviarlas a la vista
                if (notasxAlumno == null || !notasxAlumno.Any())
                {
                    TempData["Error"] = "No se encontraron notas registradas para este alumno en la materia seleccionada.";
                    return RedirectToAction("PrincipalProfesor");
                }

                ViewBag.NotasxAlumno = notasxAlumno;
                ViewBag.IdAlumnoSeleccionado = Id_Usuario_Alumno;
                ViewBag.IdMateriaSeleccionada = idMateria;

                Dictionary<int, List<MateriaProfesor>> codigosPorMateria = new Dictionary<int, List<MateriaProfesor>>();
                // Recargar datos de la vista
                int idProfesor = (int)usuario.Id_Usuario;
                List<Materia> materias = materiaNegocio.ObtenerMateriasPorProfesor(idProfesor);
                Dictionary<int,List<Usuario>> alumnosPorMateria = new Dictionary<int, List<Usuario>>();

                foreach (var materia in materias)
                {
                    List<MateriaProfesor> codigos = materiaNegocio.ObtenerCodigosComisionPorMateria(materia.Nombre);
                    codigosPorMateria[materia.Id_Materia] = codigos;  // Guardamos los códigos en el diccionario
                }
                foreach (var comision in codigosPorMateria)
                {
                    int id_Materia = comision.Key; // El ID de la materia
                    List<Usuario> alumnosMateria = new List<Usuario>();

                    foreach (var codigo in comision.Value) // Lista de MateriaProfesor (códigos de comisión)
                    {
                        List<Usuario> alumnos = materiaNegocio.ObtenerAlumnosPorMateria(id_Materia, codigo.Codigo_Comision);
                        alumnosMateria.AddRange(alumnos);
                    }

                    // Evitar duplicados en la lista de alumnos
                    alumnosMateria = alumnosMateria.GroupBy(a => a.Id_Usuario).Select(g => g.First()).ToList();

                    alumnosPorMateria[idMateria] = alumnosMateria;
                }


                ViewBag.MateriasProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor);
                ViewBag.AlumnosPorMateria = alumnosPorMateria;
                ViewBag.TiposNota = tipoNotaNegocio.obtenerTodosLosTiposNotaActivos();

                return View("PrincipalProfesor", materias);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al obtener notas: " + ex.Message;
                return RedirectToAction("PrincipalProfesor");
            }
        }
        [HttpPost]
        public ActionResult EditarNota(long Id_Usuario_Alumno, long Codigo_Comision, int? Id_Tipo_Nota, decimal Calificacion, string Observaciones)
        {
            /*if (Calificacion == null)
            {
                TempData["Error"] = "Debe seleccionar un tipo de nota.";
                return RedirectToAction("PrincipalProfesor");

            }*/
            if (Id_Tipo_Nota == null)
            {
                TempData["Error"] = "Debe seleccionar un tipo de nota.";
                return RedirectToAction("PrincipalProfesor");
            }
            try
            {
                Nota notaActualizada = new Nota
                {
                    Id_Usuario_Alumno = Id_Usuario_Alumno,
                    Codigo_Comision = Codigo_Comision,
                    Id_Tipo_Nota = (int)Id_Tipo_Nota,
                    Calificacion = Calificacion,
                    Observaciones = Observaciones
                };

                notaNegocio.modificarNota(notaActualizada);
                TempData["Mensaje"] = "Nota actualizada exitosamente.";

                // Redirigir a VerNotasAlumno con GET
                return RedirectToAction("VerNotasAlumno", new { Id_Usuario_Alumno, idMateria = (int)Codigo_Comision });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al actualizar la nota: " + ex.Message;
                return RedirectToAction("PrincipalProfesor");
            }
        }
        [HttpPost]
        public ActionResult AgregarAlumno(int Id_Materia, int Id_Alumno)
        {
            try
            {
                var usuario = Session["Usuario"] as Usuario;
                if (usuario == null) return RedirectToAction("Login", "Home");

                int idProfesor = (int)usuario.Id_Usuario;
                var materiaProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor)
                                         .SingleOrDefault(mp => mp.Id_Materia == Id_Materia);

                if (materiaProfesor == null)
                {
                    TempData["Error"] = "No se encontró la materia o no está asignada al profesor.";
                    return RedirectToAction("PrincipalProfesor");
                }

                // Agregar alumno a la materia
                bool resultado = materiaNegocio.AgregarAlumnoAMateria(Id_Alumno, materiaProfesor.Codigo_Comision);

                if (resultado)
                {
                    TempData["Mensaje"] = "Alumno agregado exitosamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo agregar el alumno. Verifique los datos.";
                }

                // Obtener todos los alumnos activos
                List<Usuario> alumnosActivos = usuarioNegocio.ObtenerAlumnosActivos();

                // Obtener alumnos ya inscritos en la materia
                List<Usuario> alumnosEnMateria = materiaNegocio.ObtenerAlumnosPorMateria(Id_Materia, materiaProfesor.Codigo_Comision);

                // Filtrar los alumnos que NO están en la materia
                List<Usuario> alumnosDisponibles = alumnosActivos
                    .Where(a => !alumnosEnMateria.Any(am => am.Id_Usuario == a.Id_Usuario))
                    .ToList();

                // Guardar en el ViewBag
                ViewBag.AlumnosDisponibles = alumnosDisponibles;
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al agregar el alumno: " + ex.Message;
            }

            return RedirectToAction("PrincipalProfesor");
        }
        [HttpGet]
        public JsonResult ObtenerAlumnosDisponibles(int idMateria)
        {
            try
            {
                // Obtener todos los alumnos activos
                List<Usuario> todosLosAlumnos = usuarioNegocio.ObtenerAlumnosActivos();

                // Obtener la materia del profesor para asegurar que tiene acceso
                var usuario = Session["Usuario"] as Usuario;
                if (usuario == null) return Json(new { error = "Usuario no autenticado" }, JsonRequestBehavior.AllowGet);

                int idProfesor = (int)usuario.Id_Usuario;
                var materiaProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor)
                                                     .SingleOrDefault(mp => mp.Id_Materia == idMateria);

                if (materiaProfesor == null)
                {
                    return Json(new { error = "No se encontró la materia o no está asignada al profesor." }, JsonRequestBehavior.AllowGet);
                }

                // Obtener alumnos ya inscritos en la materia y comisión
                List<Usuario> alumnosEnMateria = materiaNegocio.ObtenerAlumnosPorMateria(idMateria, materiaProfesor.Codigo_Comision);

                // Filtrar los alumnos que NO están inscritos en la materia
                List<Usuario> alumnosDisponibles = todosLosAlumnos
                    .Where(a => !alumnosEnMateria.Any(am => am.Id_Usuario == a.Id_Usuario))
                    .ToList();

                // Retornar la lista en formato JSON
                var resultado = alumnosDisponibles.Select(a => new
                {
                    a.Id_Usuario,
                    a.Nombre,
                    a.Apellido,
                    a.DNI
                }).ToList();

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error al obtener alumnos: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult EliminarAlumno(int Id_Materia, int Id_Alumno)
        {
            try
            {
                var usuario = Session["Usuario"] as Usuario;
                if (usuario == null) return RedirectToAction("Login", "Home");

                int idProfesor = (int)usuario.Id_Usuario;
                var materiaProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor)
                                                     .SingleOrDefault(mp => mp.Id_Materia == Id_Materia);

                if (materiaProfesor == null)
                {
                    TempData["Error"] = "No se encontró la materia o no está asignada al profesor.";
                    return RedirectToAction("PrincipalProfesor");
                }

                // Eliminar el alumno de la materia
                bool resultado = materiaNegocio.EliminarAlumnoDeMateria(Id_Alumno, materiaProfesor.Codigo_Comision);

                if (resultado)
                {
                    TempData["Mensaje"] = "Alumno eliminado exitosamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar el alumno. Verifique los datos.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al eliminar el alumno: " + ex.Message;
            }

            return RedirectToAction("PrincipalProfesor");
        }
        [HttpGet]
        public JsonResult ObtenerAlumnosMateria(int idMateria)
        {
            try
            {
                Usuario usuario = Session["Usuario"] as Usuario;
                if (usuario == null) return Json(new { error = "Usuario no autenticado" }, JsonRequestBehavior.AllowGet);

                int idProfesor = (int)usuario.Id_Usuario;
                var materiaProfesor = materiaNegocio.obtenerDatosDeMateriasxProfesor(idProfesor).SingleOrDefault(mp => mp.Id_Materia == idMateria);

                if (materiaProfesor == null)
                {
                    return Json(new { error = "No se encontró la materia o no está asignada al profesor." }, JsonRequestBehavior.AllowGet);
                }

                List<Usuario> alumnosEnMateria = materiaNegocio.ObtenerAlumnosPorMateria(idMateria, materiaProfesor.Codigo_Comision);

                var resultado = alumnosEnMateria.Select(a => new { a.Id_Usuario, a.Nombre, a.Apellido, a.DNI }).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error al obtener alumnos: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }




    }
}