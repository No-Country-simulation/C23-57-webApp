using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class NotaAlumno
    {
        public long Codigo_Comision { get; set; }
        public long Id_Usuario { get; set; }
        public string NombreAlumno { get; set; }
        public string ApellidoAlumno { get; set; }
        public string Materia { get; set; }
        public string DNI { get; set; }
        public int Id_TipoNota { get; set; }
        public string TipoNota { get; set; }
        public decimal Calificacion { get; set; }
        public string Observaciones { get; set; }
    }
}
