using CapaDominio;
using System;
using System.Collections.Generic;
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
