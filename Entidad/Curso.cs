using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidad
{
    public class Curso
    {
        private int codCurso;
        private int duracion;
        private string nombre;
        
        public string Nombre
        {
            
            get { return nombre; }
            set { nombre = value; }
        }
        
        public int Duracion
        {
            get { return duracion; }
            set { duracion = value; }
        }
           
        public int CodCurso
        {
            get { return codCurso; }
            set { codCurso = value; }
        }
        
    }
}
