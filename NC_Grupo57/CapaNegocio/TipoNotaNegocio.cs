using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class TipoNotaNegocio
    {
        private string QueryAgregar = "Insert Into Tipos_Nota (Descripcion, Activo) Values (@Descripcion, @Activo)";
        private string QueryBajaLogica = "Update Tipos_Nota Set Activo = 0 Where Id_Tipo_Nota = @IdTipoNota";
        private string QueryBajaFisica = "Delete From Tipos_Nota Where Id_Tipo_Nota = @IdTipoNota";
        private string QueryModificar = "Update Tipos_Nota Set Descripcion = @Descripcion, Activo = @Activo Where Id_Tipo_Nota = @IdTipoNota";
        private string QueryObtenerTodas = "Select Id_Tipo_Nota, Descripcion, Activo From Tipos_Nota";
        private string QueryObtenerTodasLasActivas = "Select Id_Tipo_Nota, Descripcion, Activo From Tipos_Nota where activo = 1";
        public void agregarTipoNota(TipoNota tipoNota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryAgregar);
                datos.setearParametro("@Descripcion", tipoNota.Descripcion);
                datos.setearParametro("@Activo", tipoNota.Activo);
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

        public void bajaLogicaTipoNota(int idTipoNota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@IdTipoNota", idTipoNota);
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

        public void bajaFisicaTipoNota(int idTipoNota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@IdTipoNota", idTipoNota);
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

        public void modificarTipoNota(TipoNota tipoNota)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryModificar);
                datos.setearParametro("@Descripcion", tipoNota.Descripcion);
                datos.setearParametro("@Activo", tipoNota.Activo);
                datos.setearParametro("@IdTipoNota", tipoNota.Id_Tipo_Nota);
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

        public List<TipoNota> obtenerTodosLosTiposNota()
        {
            List<TipoNota> tiposNota = new List<TipoNota>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodasLasActivas);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    TipoNota tipoNota = new TipoNota
                    {
                        Id_Tipo_Nota = (int)datos.Lector["Id_Tipo_Nota"],
                        Descripcion = (string)datos.Lector["Descripcion"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                    tiposNota.Add(tipoNota);
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
            return tiposNota;
        }
        public List<TipoNota> obtenerTodosLosTiposNotaActivos()
        {
            List<TipoNota> tiposNota = new List<TipoNota>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodas);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    TipoNota tipoNota = new TipoNota
                    {
                        Id_Tipo_Nota = (int)datos.Lector["Id_Tipo_Nota"],
                        Descripcion = (string)datos.Lector["Descripcion"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                    tiposNota.Add(tipoNota);
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
            return tiposNota;
        }
    }
}
