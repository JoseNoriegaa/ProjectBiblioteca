using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
namespace ProjectBiblioteca
{
    class Alumno
    {
        //cnn laptop-noriega
        //SqlConnection cnn = new SqlConnection("Data Source=DESKTOP-91F61D3;Initial Catalog=Biblioteca;Integrated security=true;");
        //cnn pc-noriega
        SqlConnection cnn = new SqlConnection("Data Source=DESKTOP-TIBD95D;Initial Catalog=Biblioteca;Integrated security=true;");

        public int Matricula { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Carrera { get; set; }

        public Alumno() { }
        public Alumno(int matricula, string nombre, string telefono, string correo, string carrera)
        {
            this.Matricula = matricula;
            this.Nombre = nombre;
            this.Correo = correo;
            this.Telefono = telefono;
            this.Carrera = carrera;
        }

        public void agregarAlumnoBD()
        {
            try
            {
                cnn.Open();
                SqlCommand cmd= new SqlCommand("Alta_Alumno",cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Matricula",this.Matricula);
                cmd.Parameters.AddWithValue("@Nombre",this.Nombre);
                cmd.Parameters.AddWithValue("@Correo",this.Correo);
                cmd.Parameters.AddWithValue("@Telefono",this.Telefono);
                cmd.Parameters.AddWithValue("@Carrera",this.Carrera);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha registrado ", this.Nombre);

            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error:\n",e.Message);
            }
            finally
            {
                cnn.Close();
            }

        }

    }
}
