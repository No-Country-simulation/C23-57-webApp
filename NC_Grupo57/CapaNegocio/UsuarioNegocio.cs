using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CapaDominio;

namespace CapaNegocio
{
    public class UsuarioNegocio
    {
        //Ema, el @ antes de cada string es para que tome literal todo lo que continúa sin tener que concatenar strings para ponerlo en varias líneas!
        private string QueryAgregar = @"Insert Into Usuarios 
                             (Id_Rol, Nombre, Apellido, DNI, Telefono, Email, Fecha_Nacimiento, Fecha_Alta, Contrasenia, Activo) 
                             Values 
                             (@IdRol, @Nombre, @Apellido, @DNI, @Telefono, @Email, @FechaNacimiento, @FechaAlta, @Contrasenia, 0)";
        private string QueryLogin = "Select Contrasenia From Usuarios Where Email = @Email";
        private string QueryModificar = @"Update Usuarios 
                                  Set 
                                      Id_Rol = @IdRol,
                                      Nombre = @Nombre,
                                      Apellido = @Apellido,
                                      DNI = @DNI,
                                      Telefono = @Telefono,
                                      Email = @Email,
                                      Fecha_Nacimiento = @FechaNacimiento,
                                      Fecha_Alta = @FechaAlta,
                                      Contrasenia = @Contrasenia,
                                      Activo = @Activo
                                  Where Id_Usuario = @IdUsuario";
        private string QueryBajaLogica = @"Update Usuarios Set Activo = 0 Where Id_Usuario = @IdUsuario";
        private string QueryBajaFisica = @"Delete From Usuarios Where Id_Usuario = @IdUsuario";
        private string QueryBuscarPorRol = @"Select Id_Usuario, Id_Rol, Nombre, Apellido, DNI, Telefono, Email, 
                                Fecha_Nacimiento, Fecha_Alta, Activo 
                                From Usuarios 
                                Where Id_Rol = @IdRol";
        private string QueryBuscarProfesoresxMateria = @"Select u.Id_Usuario, u.Id_Rol, u.Nombre, u.Apellido, u.DNI, u.Telefono, u.Email, 
                                u.Fecha_Nacimiento, u.Fecha_Alta, u.Activo
                                FROM Usuarios u
                                INNER JOIN Materias_x_Profesor mp ON u.Id_Usuario = mp.Id_Usuario_Profesor
                                WHERE mp.Id_Materia = @IdMateria";
        private string QueryBuscarPorID = @"Select Id_Usuario, Id_Rol, Nombre, Apellido, DNI, Telefono, Email, 
                                Fecha_Nacimiento, Fecha_Alta, Activo 
                         From Usuarios 
                         Where Id_Usuario = @IdUsuario";
        private string QueryBuscarPorEmail = @"Select Id_Usuario, Id_Rol, Nombre, Apellido, DNI, Telefono, Email, 
                                Fecha_Nacimiento, Fecha_Alta, Activo 
                         From Usuarios 
                         Where Email = @Email";

        public void agregarUsuario(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                //Antes de que la contraseña sea mandada a la BD la encripto, de esa forma sigue quedando anónima. Lo uso en varios métodos!
                string contraseniaEncriptada = EncriptadorContrasenias.EncriptarContrasenia(usuario.Contrasenia);
                datos.settearConsulta(QueryAgregar);
  
                datos.setearParametro("@IdRol", usuario.Id_Rol);
                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Apellido", usuario.Apellido);
                datos.setearParametro("@DNI", usuario.DNI);
                datos.setearParametro("@Telefono", usuario.Telefono);
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@FechaNacimiento", usuario.Fecha_Nacimiento);
                datos.setearParametro("@FechaAlta", usuario.Fecha_Alta);
                datos.setearParametro("@Contrasenia", contraseniaEncriptada); //mando la encriptada, no la literal
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

        public void modificarUsuario(Usuario usuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
               datos.settearConsulta(QueryModificar);

                string contraseniaEncriptada = EncriptadorContrasenias.EncriptarContrasenia(usuario.Contrasenia);

                datos.setearParametro("@IdRol", usuario.Id_Rol);
                datos.setearParametro("@Nombre", usuario.Nombre);
                datos.setearParametro("@Apellido", usuario.Apellido);
                datos.setearParametro("@DNI", usuario.DNI);
                datos.setearParametro("@Telefono", usuario.Telefono);
                datos.setearParametro("@Email", usuario.Email);
                datos.setearParametro("@FechaNacimiento", usuario.Fecha_Nacimiento);
                datos.setearParametro("@FechaAlta", usuario.Fecha_Alta);
                datos.setearParametro("@Contrasenia", contraseniaEncriptada);
                datos.setearParametro("@Activo", usuario.Activo);
                datos.setearParametro("@IdUsuario", usuario.Id_Usuario);

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

        public void bajaLogica(long idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@IdUsuario", idUsuario);
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

        public List<Usuario> obtenerUsuariosPorRol(int idRol)
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(QueryBuscarPorRol);
                datos.setearParametro("@IdRol", idRol);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        Id_Usuario = Convert.ToInt64(datos.Lector["Id_Usuario"]),
                        Id_Rol = Convert.ToInt32(datos.Lector["Id_Rol"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = datos.Lector["DNI"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Fecha_Nacimiento = Convert.ToDateTime(datos.Lector["Fecha_Nacimiento"]),
                        Fecha_Alta = Convert.ToDateTime(datos.Lector["Fecha_Alta"]),
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

            return listaUsuarios;
        }
        
        public Usuario buscarPorID(long idUsuario)
        {
            Usuario usuario = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(QueryBuscarPorID);
                datos.setearParametro("@IdUsuario", idUsuario);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuario
                    {
                        Id_Usuario = Convert.ToInt64(datos.Lector["Id_Usuario"]),
                        Id_Rol = Convert.ToInt32(datos.Lector["Id_Rol"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = datos.Lector["DNI"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Fecha_Nacimiento = Convert.ToDateTime(datos.Lector["Fecha_Nacimiento"]),
                        Fecha_Alta = Convert.ToDateTime(datos.Lector["Fecha_Alta"]),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"])
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

            return usuario;
        }

        //Estaría bueno que se pueda buscar por email también

        public Usuario buscarPorEmail(string email)
        {
            Usuario usuario = null;
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(QueryBuscarPorEmail);
                datos.setearParametro("@Email", email);
                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    usuario = new Usuario
                    {
                        Id_Usuario = Convert.ToInt64(datos.Lector["Id_Usuario"]),
                        Id_Rol = Convert.ToInt32(datos.Lector["Id_Rol"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = datos.Lector["DNI"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Fecha_Nacimiento = Convert.ToDateTime(datos.Lector["Fecha_Nacimiento"]),
                        Fecha_Alta = Convert.ToDateTime(datos.Lector["Fecha_Alta"]),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"])
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

            return usuario;
        }
        //PRUEBA
            public List<MateriaProfesor> obtenerTodosLosProfesoresxMateria()
        {
            List<MateriaProfesor> listaUsuarios = new List<MateriaProfesor>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta("SELECT Id_Usuario_Profesor, Id_Materia, Codigo_Comision, Activo  FROM Materias_x_Profesor");
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
        public List<Usuario> obtenerProfesoresxMateria(int idMateria)
        {
            List<Usuario> listaUsuarios = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(QueryBuscarProfesoresxMateria);
                datos.setearParametro("@IdMateria", idMateria);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    
                    Usuario usuario = new Usuario
                    {
                        Id_Usuario = Convert.ToInt64(datos.Lector["Id_Usuario"]),
                        Id_Rol = Convert.ToInt32(datos.Lector["Id_Rol"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        DNI = datos.Lector["DNI"].ToString(),
                        Telefono = datos.Lector["Telefono"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Fecha_Nacimiento = Convert.ToDateTime(datos.Lector["Fecha_Nacimiento"]),
                        Fecha_Alta = Convert.ToDateTime(datos.Lector["Fecha_Alta"]),
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



        public bool agregarMateriaxProfesor(int id, int profesorId, bool activoMateria, string nuevoNombreMateria)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Primero verificamos si la materia ya tiene un profesor asignado
                datos.settearConsulta("SELECT COUNT(*) FROM Materias_x_Profesor WHERE Id_Materia = @IdMateria AND Id_Usuario_Profesor = @idProfesor");
                datos.setearParametro("@IdMateria", id);
                datos.setearParametro("@idProfesor", profesorId);

                int count = Convert.ToInt32(datos.ejecutarEscalar()); // Devuelve la cantidad de registros encontrados

                datos.cerrarConexion(); // Cerramos la conexión antes de continuar

                if (count > 0)
                {
                    // Si ya existe, hacemos un UPDATE
                    datos = new AccesoDatos();
                    datos.settearConsulta("UPDATE Materias_x_Profesor SET Activo = @activo WHERE Id_Materia = @IdMateria AND Id_Usuario_Profesor = @idProfesor");
                }
                else
                {
                    // Si no existe, hacemos un INSERT
                    datos = new AccesoDatos();
                    datos.settearConsulta("INSERT INTO Materias_x_Profesor (Id_Materia, Id_Usuario_Profesor, Activo) VALUES (@IdMateria, @idProfesor, @activo)");
                }

                datos.setearParametro("@IdMateria", id);
                datos.setearParametro("@idProfesor", profesorId);
                datos.setearParametro("@activo", activoMateria);
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

            // Ahora actualizamos el nombre de la materia
            try
            {
                datos = new AccesoDatos();
                datos.settearConsulta("UPDATE Materias SET Nombre = @nuevoNombre WHERE Id_Materia = @IdMateria");
                datos.setearParametro("@nuevoNombre", nuevoNombreMateria);
                datos.setearParametro("@IdMateria", id);
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



        public void modificarProfesor(int id, string nombre, string apellido, string email, string activo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Activo = @Activo WHERE Id_Usuario = @Id");
                datos.setearParametro("@Id", id);
                datos.setearParametro("@Nombre", nombre);
                datos.setearParametro("@Apellido", apellido);
                datos.setearParametro("@Email", email);
                datos.setearParametro("@Activo", activo);
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

        public void modificarAlumno(int id, string nombre, string apellido, string email, string activo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("UPDATE Usuarios SET Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Activo = @Activo WHERE Id_Usuario = @Id");
                datos.setearParametro("@Id", id);
                datos.setearParametro("@Nombre", nombre);
                datos.setearParametro("@Apellido", apellido);
                datos.setearParametro("@Email", email);
                datos.setearParametro("@Activo", activo);
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


        public bool EliminarMateria(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Eliminar las relaciones de la materia en la tabla Materias_x_Profesor
                datos.settearConsulta("DELETE FROM Materias_x_Profesor WHERE Id_Materia = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            datos = new AccesoDatos();
            try
            {
                // Eliminar la materia de la tabla Materias
                datos.settearConsulta("DELETE FROM Materias WHERE Id_Materia = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return true;
        }


        public List<Usuario> ObtenerProfesores(string filtro)
        {
            List<Usuario> profesores = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id_Usuario, Nombre, Apellido, Email, Activo, DNI FROM Usuarios WHERE Id_Rol = 3";

                if (filtro == "Activos")
                {
                    consulta += " AND Activo = 1";
                }
                else if (filtro == "Inactivos")
                {
                    consulta += " AND Activo = 0";
                }                
                // No se agrega ningún filtro adicional para 'Todos'

                datos.settearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario profesor = new Usuario
                    {
                        Id_Usuario = Convert.ToInt32(datos.Lector["Id_Usuario"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                        DNI = datos.Lector["DNI"].ToString()
                    };
                    profesores.Add(profesor);
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
            if(profesores.Count == 0)
            {
                return null;
            }
            return profesores;
        }
        public List<Usuario> ObtenerAlumnosActivos()
        {
            List<Usuario> profesores = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id_Usuario, Nombre, Apellido, Email, Activo, DNI FROM Usuarios WHERE Id_Rol = 2 AND Activo = 1";


                datos.settearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario profesor = new Usuario
                    {
                        Id_Usuario = Convert.ToInt32(datos.Lector["Id_Usuario"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                        DNI = datos.Lector["DNI"].ToString()
                    };
                    profesores.Add(profesor);
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
            if (profesores.Count == 0)
            {
                return null;
            }
            return profesores;
        }
        public List<Usuario> ObtenerAlumnos(string filtro)
        {
            List<Usuario> alumnos = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = "SELECT Id_Usuario, Nombre, Apellido, Email, Activo, DNI FROM Usuarios WHERE Id_Rol = 2";

                if (filtro == "Activos")
                {
                    consulta += " AND Activo = 1";
                }
                else if (filtro == "Inactivos")
                {
                    consulta += " AND Activo = 0";
                }
                // No se agrega ningún filtro adicional para 'Todos'

                datos.settearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario alumno = new Usuario
                    {
                        Id_Usuario = Convert.ToInt32(datos.Lector["Id_Usuario"]),
                        Nombre = datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"].ToString(),
                        Email = datos.Lector["Email"].ToString(),
                        Activo = Convert.ToBoolean(datos.Lector["Activo"]),
                        DNI = datos.Lector["DNI"].ToString()
                    };
                    alumnos.Add(alumno);
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
            if (alumnos.Count == 0)
            {
                return null;
            }
            return alumnos;
        }

        public bool eliminarProfesorxMateria(int idProfesor, int idMateria)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("DELETE FROM Materias_x_Profesor WHERE Id_Usuario_Profesor = @IdProfesor AND Id_Materia = @IdMateria ");
                datos.setearParametro("@IdProfesor", idProfesor);
                datos.setearParametro("@IdMateria", idMateria);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return true;
        }
        
        public bool EliminarProfesor(int id)
        {

            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Eliminar las relaciones de la materia en la tabla Materias_x_Profesor
                datos.settearConsulta("DELETE FROM Materias_x_Profesor WHERE Id_Usuario_Profesor = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                return false;
                //throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

            datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("DELETE FROM Usuarios WHERE Id_Usuario = @Id");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                return false;
                //throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return true;
        }
        public bool EliminarAlumno(int id)
        {

            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Eliminar las relaciones de la materia en la tabla Materias_x_Alumno
                datos.settearConsulta("DELETE FROM Materias_x_Alumno WHERE Id_Usuario_Alumno = @Id");
                datos.setearParametro("@Id", id);
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

            datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("DELETE FROM Notas WHERE Id_Usuario_Alumno = @Id");
                datos.setearParametro("@Id", id);
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
                datos.settearConsulta("DELETE FROM Usuarios WHERE Id_Usuario = @Id");
                datos.setearParametro("@Id", id);
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
        public bool AgregarMateria(string nombre)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("INSERT INTO Materias (Nombre, Activo) VALUES (@Nombre, 1)");
                datos.setearParametro("@Nombre", nombre);
                datos.ejecutarAccion();
            }
            catch (Exception)
            {
                //throw ex;
                return false;
            }
            finally
            {
                datos.cerrarConexion();
            }
            return true;
        }
        public void AgregarProfesor(string nombre, string apellido, string email, string contrasenia)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta("INSERT INTO Usuarios (Nombre, Apellido, Email, Contrasenia, Activo, Id_Rol) VALUES (@Nombre, @Apellido, @Email, @Contrasenia, 1, 3)");
                datos.setearParametro("@Nombre", nombre);
                datos.setearParametro("@Apellido", apellido);
                datos.setearParametro("@Email", email);
                datos.setearParametro("@Contrasenia", contrasenia);
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

        public void bajaFisica(long idUsuario)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@IdUsuario", idUsuario);
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

        public static bool validarLogin(string email, string contraseniaIngresada)
        {
            //instancio un objeto de la misma clase porque el método es estático y lo necesito para usar la query que es una prop privada!
            UsuarioNegocio u = new UsuarioNegocio();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(u.QueryLogin);
                datos.setearParametro("@Email", email);

                datos.ejecutarLectura();

                if (datos.Lector.Read())
                {
                    string hashAlmacenado = datos.Lector["Contrasenia"].ToString();
                    return EncriptadorContrasenias.ValidarContrasenia(contraseniaIngresada, hashAlmacenado);
                }
                return false;
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
    }
}
