﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDominio
{
    public class MateriaProfesor
    {
        public long Id_Usuario_Profesor { get; set; }
        public int Id_Materia { get; set; }
        public long Codigo_Comision { get; set; }
        public bool Activo { get; set; }
    }
}
