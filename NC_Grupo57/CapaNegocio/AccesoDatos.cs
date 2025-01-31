using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=NCG57; integrated security=true;");
            comando = new SqlCommand();
            comando.Connection = conexion; // Asigna la conexión al comando
        }

        public void settearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
            comando.Parameters.Clear(); // Limpia parámetros antes de cada consulta
        }

        public void ejecutarLectura()
        {
            try
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar lectura: " + ex.Message);
            }
        }

        public void cerrarConexion()
        {
            if (lector != null && !lector.IsClosed)
            {
                lector.Close();
            }

            if (conexion.State == System.Data.ConnectionState.Open)
                conexion.Close();
        }

        public void ejecutarAccion()
        {
            try
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar acción: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }

        public void setearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        public object ejecutarEscalar()
        {
            try
            {
                if (conexion.State != System.Data.ConnectionState.Open)
                    conexion.Open();

                return comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en ejecutarEscalar: " + ex.Message);
            }
            finally
            {
                conexion.Close();
            }
        }
    }
}
