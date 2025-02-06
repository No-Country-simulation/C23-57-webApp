using CapaDominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class MateriaNegocio
    {
        private string QueryAgregar = @"Insert Into Materias (Nombre, Activo) Values (@Nombre, @Activo)";
        private string QueryBajaFisica = @"Delete From Materias Where Id_Materia = @IdMateria";
        private string QueryBajaLogica = @"Update Materias Set Activo = 0 Where Id_Materia = @IdMateria";
        private string QueryModificar = @"Update Materias 
                                         Set Nombre = @Nombre, 
                                             Activo = @Activo 
                                         Where Id_Materia = @IdMateria";
        private string QueryObtenerTodas = "Select Id_Materia, Nombre, Activo From Materias";

        private string QueryObtenerPorID = "Select Id_Materia, Nombre, Activo From Materias Where Id_Materia = @IdMateria";
        public void agregarMateria(Materia materia)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryAgregar);
                datos.setearParametro("@Nombre", materia.Nombre);
                datos.setearParametro("@Activo", materia.Activo);
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

        public void bajaLogicaMateria(int idMateria)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {

                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@IdMateria", idMateria);
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

        public bool EliminarAlumnoDeMateria(int idAlumno, long codigoComision)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("Delete from Notas where Id_Usuario_Alumno = @IdAlumno AND Codigo_Comision = @CodigoComision");
                datos.setearParametro("@IdAlumno", idAlumno);
                datos.setearParametro("@CodigoComision", codigoComision);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }

            try
            {
                datos.settearConsulta("Delete from Materias_x_Alumno where Id_Usuario_Alumno = @IdAlumno AND Codigo_Comision = @CodigoComision");
                datos.setearParametro("@IdAlumno", idAlumno);
                datos.setearParametro("@CodigoComision", codigoComision);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return true;

        }
        public void bajaFisicaMateria(int idMateria)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@IdMateria", idMateria);
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

        public void modificarMateria(Materia materia)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryModificar);
                datos.setearParametro("@IdMateria", materia.Id_Materia);
                datos.setearParametro("@Nombre", materia.Nombre);
                datos.setearParametro("@Activo", materia.Activo);
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

        public List<Materia> obtenerTodasLasMaterias()
        {
            List<Materia> materias = new List<Materia>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodas);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Materia materia = new Materia
                    {
                        Id_Materia = (int)datos.Lector["Id_Materia"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                    materias.Add(materia);
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
            return materias;
        }

        public Materia obtenerPorID(int idMateria)
        {
            //Lo hago nulo porque si no la encuentra devuelve null
            Materia materia = null;
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerPorID);
                datos.setearParametro("@IdMateria", idMateria);
                datos.ejecutarLectura();
                if (datos.Lector.Read())
                {
                    materia = new Materia
                    {
                        Id_Materia = (int)datos.Lector["Id_Materia"],
                        Nombre = (string)datos.Lector["Nombre"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
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

            return materia;
        }
        //PRUEBA
        public bool AgregarAlumnoAMateria(int idAlumno, long codigoComision)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("INSERT INTO Materias_x_Alumno(Id_Usuario_Alumno, Codigo_Comision, Activo) VALUES(@IdAlumno, @CodigoComision, 1)");
                datos.setearParametro("@IdAlumno", idAlumno);
                datos.setearParametro("@CodigoComision", codigoComision);
                datos.ejecutarLectura();
            }
            catch
            {
                return false;
            }
            return true;
        }
        public List<MateriaProfesor> obtenerDatosDeMateriasxProfesor(int idProfesor)
        {
            List<MateriaProfesor> listaUsuarios = new List<MateriaProfesor>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta("SELECT Id_Usuario_Profesor, Id_Materia, Codigo_Comision, Activo FROM Materias_x_Profesor WHERE Id_Usuario_Profesor = @IdProfesor");
                datos.setearParametro("@IdProfesor", idProfesor);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {

                    MateriaProfesor usuario = new MateriaProfesor
                    {
                        Id_Usuario_Profesor = Convert.ToInt64(datos.Lector["Id_Usuario_Profesor"]),
                        Id_Materia = Convert.ToInt32(datos.Lector["Id_Materia"]),
                        Codigo_Comision = Convert.ToInt64(datos.Lector["Codigo_Comision"]),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"])
                    };

                    listaUsuarios.Add(usuario);
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
            /*if (listaUsuarios.Count == 0)
            {
                return null;
            }*/

            return listaUsuarios;
        }
        public List<Usuario> ObtenerAlumnosPorMateria(int idMateria, long codigoComision)
        {
            List<Usuario> alumnos = new List<Usuario>();

            AccesoDatos datos = new AccesoDatos();
            {
                datos.settearConsulta(@"
            SELECT u.Id_Usuario AS Id_Alumno, u.Nombre, u.Apellido, u.DNI, u.Activo
            FROM Materias_x_Alumno mxa
            JOIN Usuarios u ON mxa.Id_Usuario_Alumno = u.Id_Usuario
            JOIN Materias_x_Profesor mxp ON mxa.Codigo_Comision = mxp.Codigo_Comision
            WHERE mxp.Id_Materia = @IdMateria 
            AND mxp.Codigo_Comision = @CodigoComision AND u.Activo = 1");

                datos.setearParametro("@IdMateria", idMateria);
                datos.setearParametro("@CodigoComision", codigoComision);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    alumnos.Add(new Usuario
                    {
                        Id_Usuario = Convert.ToInt64(datos.Lector["Id_Alumno"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = datos.Lector["DNI"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    });
                }
            }
            return alumnos;
        }
        public List<Usuario> ObtenerAlumnosPorMateria(int idMateria)
        {
            List<Usuario> alumnos = new List<Usuario>();

            AccesoDatos datos = new AccesoDatos();
            {
                datos.settearConsulta(@"SELECT u.Id_Usuario, u.Nombre, u.Apellido, u.DNI, u.Activo
                         FROM Usuarios u 
                         INNER JOIN Materias_x_Alumno am ON u.Id_Usuario = am.Id_Usuario_Alumno
                         WHERE u.Activo = 1 AND am.Activo = 1");


                datos.setearParametro("@IdMateria", idMateria);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    alumnos.Add(new Usuario
                    {
                        Id_Usuario = Convert.ToInt64(datos.Lector["Id_Usuario"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = datos.Lector["DNI"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    });
                }
            }
            /*if (alumnos.Count() == 0)
            {
                return null;
            }*/

            return alumnos;
        }

        public List<MateriaProfesor> ObtenerCodigosComisionPorMateria(string nombreComision)
        {
            List<MateriaProfesor> alumnos = new List<MateriaProfesor>();

            AccesoDatos datos = new AccesoDatos();
            {
                datos.settearConsulta(@"
            SELECT mxp.Codigo_Comision
            FROM Materias_x_Profesor mxp
            INNER JOIN Materias m ON mxp.Id_Materia = m.Id_Materia
            WHERE m.Nombre = @nombreComision;");

                datos.setearParametro("@nombreComision", nombreComision);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    alumnos.Add(new MateriaProfesor
                    {
                        Codigo_Comision = (long)datos.Lector["Codigo_Comision"]
                    });
                }
            }
            return alumnos;
        }

        public List<Materia> ObtenerMateriasPorProfesor(long idProfesor)
        {
            List<Materia> materias = new List<Materia>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(@"SELECT m.Id_Materia, m.Nombre, m.Activo
                                    FROM Materias m
                                    INNER JOIN Materias_x_Profesor mp ON m.Id_Materia = mp.Id_Materia
                                    WHERE mp.Id_Usuario_Profesor = @IdProfesor AND m.Activo = 1");

                datos.setearParametro("@IdProfesor", idProfesor);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    materias.Add(new Materia
                    {
                        Id_Materia = Convert.ToInt32(datos.Lector["Id_Materia"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Activo = (bool)datos.Lector["Activo"]
                    });
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

            return materias;
        }

        
    }
}
