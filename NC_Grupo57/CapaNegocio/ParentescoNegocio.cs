using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class ParentescoNegocio
    {
        private string QueryAgregar = "Insert Into Parentescos (Descripcion) Values (@Descripcion)";
        private string QueryBajaLogica = "Update Parentescos Set Activo = 0 Where Id_Parentesco = @IdParentesco";
        private string QueryBajaFisica = "Delete From Parentescos Where Id_Parentesco = @IdParentesco";
        private string QueryModificar = "Update Parentescos Set Descripcion = @Descripcion Where Id_Parentesco = @IdParentesco";
        private string QueryObtenerTodos = "Select Id_Parentesco, Descripcion From Parentescos";

        public void agregarParentesco(Parentesco parentesco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryAgregar);
                datos.setearParametro("@Descripcion", parentesco.Descripcion);
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

        public void bajaLogicaParentesco(int idParentesco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@IdParentesco", idParentesco);
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

        public void bajaFisicaParentesco(int idParentesco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@IdParentesco", idParentesco);
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

        public void modificarParentesco(Parentesco parentesco)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryModificar);
                datos.setearParametro("@IdParentesco", parentesco.Id_Parentesco);
                datos.setearParametro("@Descripcion", parentesco.Descripcion);
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

        public List<Parentesco> listarParentescos()
        {
            List<Parentesco> parentescos = new List<Parentesco>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodos);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Parentesco parentesco = new Parentesco
                    {
                        Id_Parentesco = (int)datos.Lector["Id_Parentesco"],
                        Descripcion = datos.Lector["Descripcion"].ToString()
                    };
                    parentescos.Add(parentesco);
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

            return parentescos;
        }
    }
}
