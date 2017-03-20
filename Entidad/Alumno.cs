using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Alumno
    {
        private string apellido;
        private int codCurso;
        private int legajo;

        public int Legajo
        {
            get { return legajo; }
            set { legajo = value; }
        }
        

        public int CodCurso
        {
            get { return codCurso; }
            set { codCurso = value; }
        }
        
        public string Apellido
        {
            get { return apellido; }
            set { apellido = value; }
        }
        
    }
}
