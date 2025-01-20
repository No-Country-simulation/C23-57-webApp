using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class RolNegocio
    {
        string QueryAgregar = @"Insert Into Roles (Descripcion) Values (@Descripcion)";
        string QueryModificar = @"Update Roles Set Descripcion = @Descripcion Where Id_Rol = @IdRol";
        string QueryBajaLogica = @"Update Roles Set Activo = 0 Where Id_Rol = @IdRol";
        string QueryBajaFisica = @"Delete From Roles Where Id_Rol = @IdRol";
        string QueryObtenerTodos = @"Select Id_Rol, Descripcion From Roles";
        public void agregarRol(Rol rol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.settearConsulta(QueryAgregar);
                datos.setearParametro("@Descripcion", rol.Descripcion);
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

        public void modificarRol(Rol rol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryModificar);
                datos.setearParametro("@Descripcion", rol.Descripcion);
                datos.setearParametro("@IdRol", rol.Id_Rol);
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

        public void bajaLogicaRol(int idRol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                
                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@IdRol", idRol);
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

        public void bajaFisicaRol(int idRol)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {        
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@IdRol", idRol);
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

        public List<Rol> obtenerTodosLosRoles()
        {
            List<Rol> listaRoles = new List<Rol>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.settearConsulta(QueryObtenerTodos);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Rol rol = new Rol
                    {
                        Id_Rol = Convert.ToInt32(datos.Lector["Id_Rol"]),
                        Descripcion = datos.Lector["Descripcion"].ToString()
                    };
                    listaRoles.Add(rol);
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

            return listaRoles;
        }
    }
}
