using CapaDominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NotaNegocio
    {
        private string QueryAgregar = "Insert Into Notas (Id_Usuario_Alumno, Codigo_Comision, Id_Tipo_Nota, Calificacion, Observaciones) " +
                               "Values (@IdUsuarioAlumno, @CodigoComision, @IdTipoNota, @Calificacion, @Observaciones)";
        private string QueryBajaLogica = "Update Notas Set Activo = 0 Where Id_Usuario_Alumno = @IdUsuarioAlumno " +
                               "And Codigo_Comision = @CodigoComision And Id_Tipo_Nota = @IdTipoNota";
        private string QueryBajaFisica = "Delete From Notas Where Id_Usuario_Alumno = @IdUsuarioAlumno " +
                               "And Codigo_Comision = @CodigoComision And Id_Tipo_Nota = @IdTipoNota";
        private string QueryModificar = "Update Notas Set Calificacion = @Calificacion, Observaciones = @Observaciones " +
                               "Where Id_Usuario_Alumno = @IdUsuarioAlumno And Codigo_Comision = @CodigoComision " +
                               "And Id_Tipo_Nota = @IdTipoNota";
        private string QueryObtenerTodas = "Select Id_Usuario_Alumno, Codigo_Comision, Id_Tipo_Nota, Calificacion, Observaciones From Notas";
        private string QueryObtenerPorTipoNota = "Select Id_Usuario_Alumno, Codigo_Comision, Id_Tipo_Nota, Calificacion, Observaciones " +
                               "From Notas Where Id_Tipo_Nota = @IdTipoNota";

        public void agregarNota(Nota nota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryAgregar);
                datos.setearParametro("@IdUsuarioAlumno", nota.Id_Usuario_Alumno);
                datos.setearParametro("@CodigoComision", nota.Codigo_Comision);
                datos.setearParametro("@IdTipoNota", nota.Id_Tipo_Nota);
                datos.setearParametro("@Calificacion", nota.Calificacion);
                datos.setearParametro("@Observaciones", nota.Observaciones);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void bajaLogicaNota(Nota nota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@IdUsuarioAlumno", nota.Id_Usuario_Alumno);
                datos.setearParametro("@CodigoComision", nota.Codigo_Comision);
                datos.setearParametro("@IdTipoNota", nota.Id_Tipo_Nota);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void bajaFisicaNota(Nota nota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@IdUsuarioAlumno", nota.Id_Usuario_Alumno);
                datos.setearParametro("@CodigoComision", nota.Codigo_Comision);
                datos.setearParametro("@IdTipoNota", nota.Id_Tipo_Nota);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificarNota(Nota nota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryModificar);
                datos.setearParametro("@IdUsuarioAlumno", nota.Id_Usuario_Alumno);
                datos.setearParametro("@CodigoComision", nota.Codigo_Comision);
                datos.setearParametro("@IdTipoNota", nota.Id_Tipo_Nota);
                datos.setearParametro("@Calificacion", nota.Calificacion);
                datos.setearParametro("@Observaciones", nota.Observaciones);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public List<Nota> listarNotas()
        {
            List<Nota> notas = new List<Nota>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodas);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Nota nota = new Nota
                    {
                        Id_Usuario_Alumno = (long)datos.Lector["Id_Usuario_Alumno"],
                        Codigo_Comision = (long)datos.Lector["Codigo_Comision"],
                        Id_Tipo_Nota = (int)datos.Lector["Id_Tipo_Nota"],
                        Calificacion = (decimal)datos.Lector["Calificacion"],
                        Observaciones = datos.Lector["Observaciones"] as string
                    };
                    notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return notas;
        }
        //PRUEBA
        public NotaAlumno ObtenerNotaAlumnoMateria(long idAlumno, int idMateria)
        {
            var notas = new List<NotaAlumno>();
            NotaAlumno notaAlumno = new NotaAlumno();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(@"
                SELECT 
                    MP.Codigo_Comision as Comision_Alumno,
                    U.Id_Usuario as Id_Alumno,
                    U.Nombre AS Nombre_Alumno,
                    U.Apellido AS Apellido_Alumno,
                    U.DNI as DNI_Alumno,
                    M.Nombre AS Materia,
                    TN.Descripcion AS Tipo_Nota,
                    N.Id_Tipo_Nota,
                    N.Calificacion,
                    N.Observaciones
                FROM Notas N
                INNER JOIN Usuarios U ON N.Id_Usuario_Alumno = U.Id_Usuario
                INNER JOIN Materias_x_Profesor MP ON N.Codigo_Comision = MP.Codigo_Comision
                INNER JOIN Materias M ON MP.Id_Materia = M.Id_Materia
                INNER JOIN Tipos_Nota TN ON N.Id_Tipo_Nota = TN.Id_Tipo_Nota
                WHERE N.Id_Usuario_Alumno = @IdAlumno AND M.Id_Materia = @IdMateria");
                datos.setearParametro("@IdAlumno", idAlumno);
                datos.setearParametro("@IdMateria", idMateria);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
             
                    
                        notaAlumno.Codigo_Comision = (long)datos.Lector["Comision_Alumno"];
                        notaAlumno.Id_Usuario = (long)datos.Lector["Id_Alumno"];
                        notaAlumno.NombreAlumno = datos.Lector["Nombre_Alumno"].ToString();
                        notaAlumno.ApellidoAlumno = datos.Lector["Apellido_Alumno"].ToString();
                        notaAlumno.DNI = datos.Lector["DNI_Alumno"].ToString();
                        notaAlumno.Materia = datos.Lector["Materia"].ToString();
                        notaAlumno.Id_TipoNota = (int)datos.Lector["Id_Tipo_Nota"];
                        notaAlumno.TipoNota = datos.Lector["Tipo_Nota"].ToString();
                        notaAlumno.Calificacion = Convert.ToDecimal(datos.Lector["Calificacion"]);
                        notaAlumno.Observaciones = datos.Lector["Observaciones"] as string;
                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las notas del alumno.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return notaAlumno;
        }
        public List<NotaAlumno> ObtenerNotasPorAlumnoMateria(long idAlumno, int idMateria)
        {
            var notas = new List<NotaAlumno>();

            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(@"
                SELECT 
                    MP.Codigo_Comision as Comision_Alumno,
                    U.Id_Usuario as Id_Alumno,
                    U.Nombre AS Nombre_Alumno,
                    U.Apellido AS Apellido_Alumno,
                    U.DNI as DNI_Alumno,
                    M.Nombre AS Materia,
                    TN.Descripcion AS Tipo_Nota,
                    N.Id_Tipo_Nota,
                    N.Calificacion,
                    N.Observaciones
                FROM Notas N
                INNER JOIN Usuarios U ON N.Id_Usuario_Alumno = U.Id_Usuario
                INNER JOIN Materias_x_Profesor MP ON N.Codigo_Comision = MP.Codigo_Comision
                INNER JOIN Materias M ON MP.Id_Materia = M.Id_Materia
                INNER JOIN Tipos_Nota TN ON N.Id_Tipo_Nota = TN.Id_Tipo_Nota
                WHERE N.Id_Usuario_Alumno = @IdAlumno AND M.Id_Materia = @IdMateria");
                datos.setearParametro("@IdAlumno", idAlumno);
                datos.setearParametro("@IdMateria", idMateria);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    notas.Add(new NotaAlumno
                    {
                        Codigo_Comision = (long)datos.Lector["Comision_Alumno"],
                        Id_Usuario = (long)datos.Lector["Id_Alumno"],
                        NombreAlumno = datos.Lector["Nombre_Alumno"].ToString(),
                        ApellidoAlumno = datos.Lector["Apellido_Alumno"].ToString(),
                        DNI = datos.Lector["DNI_Alumno"].ToString(),
                        Materia = datos.Lector["Materia"].ToString(),
                        Id_TipoNota = (int)datos.Lector["Id_Tipo_Nota"],
                        TipoNota = datos.Lector["Tipo_Nota"].ToString(),
                        Calificacion = Convert.ToDecimal(datos.Lector["Calificacion"]),
                        Observaciones = datos.Lector["Observaciones"] as string
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las notas del alumno.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return notas;
        }

            
        public List<Nota> obtenerNotasPorAlumnoYComision(long idAlumno, long codigoComision)
        {
            List<Nota> notas = new List<Nota>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("SELECT Id_Usuario_Alumno, Codigo_Comision, Id_Tipo_Nota, Calificacion, Observaciones FROM Notas WHERE Id_Usuario_Alumno = @idAlumno AND Codigo_Comision = @codigoComision");
                datos.setearParametro("@idAlumno", idAlumno);
                datos.setearParametro("@codigoComision", codigoComision);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Nota nota = new Nota
                    {
                        Id_Usuario_Alumno = (long)datos.Lector["Id_Usuario_Alumno"],
                        Codigo_Comision = (long)datos.Lector["Codigo_Comision"],
                        Id_Tipo_Nota = (int)datos.Lector["Id_Tipo_Nota"],
                        Calificacion = (decimal)datos.Lector["Calificacion"],
                        Observaciones = datos.Lector["Observaciones"] as string
                    };
                    notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las notas del alumno.", ex);
            }
            finally
            {
                datos.cerrarConexion();
            }

            return notas;
        }
        public List<TipoNota> listarDescripcionNotas(int idTipoNota)
        {
            List<TipoNota> notas = new List<TipoNota>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("SELECT Id_Tipo_Nota, Descripcion, Activo FROM Tipos_Nota WHERE Id_Tipo_Nota = @idTipoNota");
                datos.setearParametro("@idTipoNota", idTipoNota); 
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    TipoNota nota = new TipoNota
                    {
                        Id_Tipo_Nota = Convert.ToInt32(datos.Lector["Id_Tipo_Nota"]),
                        Descripcion = Convert.ToString(datos.Lector["Descripcion"]), 
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]) 
                    };

                    notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la descripción de la nota.", ex); // ✅ Mejora el manejo de errores
            }
            finally
            {
                datos.cerrarConexion();
            }

            return notas;
        }
        //PRUEBA


        public List<Nota> obtenerNotasPorIdTipoNota(int idTipoNota)
        {
            List<Nota> notas = new List<Nota>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerPorTipoNota);
                datos.setearParametro("@IdTipoNota", idTipoNota);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Nota nota = new Nota
                    {
                        Id_Usuario_Alumno = (long)datos.Lector["Id_Usuario_Alumno"],
                        Codigo_Comision = (long)datos.Lector["Codigo_Comision"],
                        Id_Tipo_Nota = (int)datos.Lector["Id_Tipo_Nota"],
                        Calificacion = (decimal)datos.Lector["Calificacion"],
                        Observaciones = datos.Lector["Observaciones"] as string
                    };
                    notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            return notas;
        }
    }
}
