using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    internal static class EncriptadorContrasenias
    {
        public static string EncriptarContrasenia(string contrasenia)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(contrasenia);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static bool ValidarContrasenia(string contraseniaIngresada, string hashAlmacenado)
        {
            string hashIngresado = EncriptarContrasenia(contraseniaIngresada);
            return hashIngresado == hashAlmacenado;
        }
    }
}
