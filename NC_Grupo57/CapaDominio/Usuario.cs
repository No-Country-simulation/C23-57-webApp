using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class Usuario
    {
        public long Id_Usuario { get; set; }
        public int Id_Rol { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public DateTime Fecha_Nacimiento { get; set; }
        public DateTime Fecha_Alta { get; set; }
        public string Contrasenia { get; set; }
        public bool Activo { get; set; }
    }
}
