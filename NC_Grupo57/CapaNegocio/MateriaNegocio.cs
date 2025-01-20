using CapaDominio;
using System;
using System.Collections.Generic;
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
    }
}
