using CapaDominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class MensajeNegocio
    {
        private string QueryAgregar = "Insert Into Mensajes (Id_Usuario_Remitente, Id_Usuario_Destinatario, Detalle, Fecha_Hora, Leido, Activo) " +
                               "Values (@Id_Usuario_Remitente, @Id_Usuario_Destinatario, @Detalle, @Fecha_Hora, @Leido, @Activo)";
        private string QueryBajaLogica = "Update Mensajes Set Activo = 0 Where Id_Mensaje = @Id_Mensaje";
        private string QueryBajaFisica = "Delete From Mensajes Where Id_Mensaje = @Id_Mensaje";
        private string QueryModificar = "Update Mensajes Set Id_Usuario_Remitente = @Id_Usuario_Remitente, " +
                               "Id_Usuario_Destinatario = @Id_Usuario_Destinatario, Detalle = @Detalle, " +
                               "Fecha_Hora = @Fecha_Hora, Leido = @Leido, Activo = @Activo " +
                               "Where Id_Mensaje = @Id_Mensaje";
        private string QueryObtenerTodosPorRemitente = "Select Id_Mensaje, Id_Usuario_Remitente, Id_Usuario_Destinatario, Detalle, Fecha_Hora, Leido, Activo " +
                               "From Mensajes Where Id_Usuario_Remitente = @Id_Usuario_Remitente";
        private string QueryObtenerTodosPorDestinatario = "Select Id_Mensaje, Id_Usuario_Remitente, Id_Usuario_Destinatario, Detalle, Fecha_Hora, Leido, Activo " +
                               "From Mensajes Where Id_Usuario_Destinatario = @Id_Usuario_Destinatario";
        public void agregarMensaje(Mensaje mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryAgregar);
                datos.setearParametro("@Id_Usuario_Remitente", mensaje.Id_Usuario_Remitente);
                datos.setearParametro("@Id_Usuario_Destinatario", mensaje.Id_Usuario_Destinatario);
                datos.setearParametro("@Detalle", mensaje.Detalle);
                datos.setearParametro("@Fecha_Hora", mensaje.Fecha_Hora);
                datos.setearParametro("@Leido", mensaje.Leido);
                datos.setearParametro("@Activo", mensaje.Activo);
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

        public void bajaLogicaMensaje(long idMensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaLogica);
                datos.setearParametro("@Id_Mensaje", idMensaje);
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

        public void bajaFisicaMensaje(long idMensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryBajaFisica);
                datos.setearParametro("@Id_Mensaje", idMensaje);
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

        public void modificarMensaje(Mensaje mensaje)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryModificar);
                datos.setearParametro("@Id_Mensaje", mensaje.Id_Mensaje);
                datos.setearParametro("@Id_Usuario_Remitente", mensaje.Id_Usuario_Remitente);
                datos.setearParametro("@Id_Usuario_Destinatario", mensaje.Id_Usuario_Destinatario);
                datos.setearParametro("@Detalle", mensaje.Detalle);
                datos.setearParametro("@Fecha_Hora", mensaje.Fecha_Hora);
                datos.setearParametro("@Leido", mensaje.Leido);
                datos.setearParametro("@Activo", mensaje.Activo);
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

        public List<Mensaje> obtenerMensajesPorRemitente(long idUsuarioRemitente)
        {
            List<Mensaje> mensajes = new List<Mensaje>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodosPorRemitente);
                datos.setearParametro("@Id_Usuario_Remitente", idUsuarioRemitente);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Mensaje mensaje = new Mensaje
                    {
                        Id_Mensaje = (long)datos.Lector["Id_Mensaje"],
                        Id_Usuario_Remitente = (long)datos.Lector["Id_Usuario_Remitente"],
                        Id_Usuario_Destinatario = (long)datos.Lector["Id_Usuario_Destinatario"],
                        Detalle = datos.Lector["Detalle"].ToString(),
                        Fecha_Hora = (DateTime)datos.Lector["Fecha_Hora"],
                        Leido = (bool)datos.Lector["Leido"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                    mensajes.Add(mensaje);
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

            return mensajes;
        }

        public List<Mensaje> obtenerMensajesPorDestinatario(long idUsuarioDestinatario)
        {
            List<Mensaje> mensajes = new List<Mensaje>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.settearConsulta(QueryObtenerTodosPorDestinatario);
                datos.setearParametro("@Id_Usuario_Destinatario", idUsuarioDestinatario);
                datos.ejecutarLectura();
                while (datos.Lector.Read())
                {
                    Mensaje mensaje = new Mensaje
                    {
                        Id_Mensaje = (long)datos.Lector["Id_Mensaje"],
                        Id_Usuario_Remitente = (long)datos.Lector["Id_Usuario_Remitente"],
                        Id_Usuario_Destinatario = (long)datos.Lector["Id_Usuario_Destinatario"],
                        Detalle = datos.Lector["Detalle"].ToString(),
                        Fecha_Hora = (DateTime)datos.Lector["Fecha_Hora"],
                        Leido = (bool)datos.Lector["Leido"],
                        Activo = (bool)datos.Lector["Activo"]
                    };
                    mensajes.Add(mensaje);
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

            return mensajes;
        }
    }
}
