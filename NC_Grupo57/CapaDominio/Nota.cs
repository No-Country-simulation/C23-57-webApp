using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class Nota
    {
        public long Id_Usuario_Alumno { get; set; }
        public long Codigo_Comision { get; set; }
        public int Id_Tipo_Nota { get; set; }
        public decimal Calificacion { get; set; }
        public string Observaciones { get; set; }
    }
}
