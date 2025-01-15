using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
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
                             (@IdRol, @Nombre, @Apellido, @DNI, @Telefono, @Email, @FechaNacimiento, @FechaAlta, @Contrasenia, @Activo)";
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
        private string QueryBuscarPorID = @"Select Id_Usuario, Id_Rol, Nombre, Apellido, DNI, Telefono, Email, 
                                Fecha_Nacimiento, Fecha_Alta, Activo 
                         From Usuarios 
                         Where Id_Usuario = @IdUsuario";
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
                datos.setearParametro("@Activo", usuario.Activo);
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
