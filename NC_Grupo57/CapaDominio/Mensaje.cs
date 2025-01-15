using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class Mensaje
    {
        public long Id_Mensaje { get; set; }
        public long Id_Usuario_Remitente { get; set; }
        public long Id_Usuario_Destinatario { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha_Hora { get; set; }
        public bool Leido { get; set; }
        public bool Activo { get; set; }
    }
}
