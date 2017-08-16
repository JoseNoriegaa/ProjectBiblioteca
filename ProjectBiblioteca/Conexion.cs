using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBiblioteca
{
    class Conexion
    {
        string cxnString;
        public Conexion() { }

        public string connectionString()
        {
            cxnString = @"Data Source=DESKTOP-TIBD95D;Initial Catalog=Biblioteca;Integrated security=true;";
            return cxnString;
        }


    }
}
