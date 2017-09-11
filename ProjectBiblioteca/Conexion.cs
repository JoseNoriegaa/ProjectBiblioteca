using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBiblioteca
{
    class Conexion
    {
        string cxnString= @"Data Source=(LocalDb)\MSSQLLocalDb; AttachDbFilename=|DataDirectory|\Biblioteca.mdf; Initial Catalog = Biblioteca; Integrated Security = TRUE";
        public Conexion() { }

        public string connectionString()
        {
            cxnString = @"Data Source=DESKTOP-68MLGQ1\SQLEXPRESS;Initial Catalog=DB_Biblioteca_Prueba;Integrated security=true;";
            return cxnString;
        }


    }
}
