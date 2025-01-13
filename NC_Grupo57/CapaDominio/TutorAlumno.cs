using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class TutorAlumno
    {
        public long Id_Usuario_Alumno { get; set; }
        public long Id_Usuario_Tutor { get; set; }
        public int Id_Parentesco { get; set; }
        public bool Activo { get; set; }
    }
}
