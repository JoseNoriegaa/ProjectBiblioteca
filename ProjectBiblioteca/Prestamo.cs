using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace ProjectBiblioteca
{
    class Prestamo
    {
        
        private SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        private SqlCommand cmd;
        public string libroID { get; set; }
        public int matriculaAlumno { get; set; }
        public DateTime fechaDePrestamo { get; set; }
        public DateTime fechaDeEntrega { get; set; }
        public int diasDePrestamo { get; set; }
        public string estadoDelLibro { get; set; }
        public int numeroEmpleado { get; set; }
        public string tipoPrestamo { get; set; }

        public Prestamo()
        {
        }

        public Prestamo(string libro, int matriculaAlumno, DateTime fechaDePrestamo, DateTime fechaDeEntrega, int dias, string estadoDelLibro, string tipoDePrestamno)
        {
            this.libroID = libro;
            this.matriculaAlumno = matriculaAlumno;
            this.fechaDeEntrega = fechaDeEntrega;
            this.fechaDePrestamo = fechaDePrestamo;
            this.diasDePrestamo = dias;
            if (string.IsNullOrEmpty(estadoDelLibro))
            {
                this.estadoDelLibro = "default";
            }
            else
            {
                this.estadoDelLibro = estadoDelLibro;
            }
            this.tipoPrestamo = tipoDePrestamno;

        }

        public Prestamo(int numeroEmpleado, string libro, DateTime fechaDePrestamo, DateTime fechaDeEntrega, int dias, string estadoDelLibro, string tipoDePrestamno)
        {
            this.libroID = libro;
            this.numeroEmpleado = numeroEmpleado;
            this.fechaDeEntrega = fechaDeEntrega;
            this.fechaDePrestamo = fechaDePrestamo;
            this.diasDePrestamo = dias;
            if (string.IsNullOrEmpty(estadoDelLibro))
            {
                this.estadoDelLibro = "default";
            }
            else
            {
                this.estadoDelLibro = estadoDelLibro;
            }
            this.tipoPrestamo = tipoDePrestamno;
        }

        public void registrarPrestamo()
        {

            try
            {
                cnn.Open();
                cmd = new SqlCommand("Registrar_Prestamo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Libro", this.libroID);
                if (tipoPrestamo == "Alumno")
                {
                    cmd.Parameters.AddWithValue("@idPersona", this.matriculaAlumno);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@idPersona", this.numeroEmpleado);
                }
                cmd.Parameters.AddWithValue("@Fecha_Prestamo", this.fechaDePrestamo);
                cmd.Parameters.AddWithValue("@Fecha_Entrega", this.fechaDeEntrega);
                cmd.Parameters.AddWithValue("@Dias_De_Prestamo", this.diasDePrestamo);
                cmd.Parameters.AddWithValue("@Estado_Del_Libro", this.estadoDelLibro);
                cmd.Parameters.AddWithValue("@tipoPrestamo", this.tipoPrestamo);
                cmd.Parameters.AddWithValue("@Estado", 1);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Se ha registrado el prestamo");



            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error" + e.Message, e.Source);
            }
            finally
            {
                cnn.Close();
            }


        }

 
        public void registrarDevolucion(int Id_Prestamo,string idLibro,int matricula)
        {
            try
            {
                cnn.Open(); 
                cmd = new SqlCommand("RegistrarDevolucion", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPrestamo", Id_Prestamo);
                cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                cmd.Parameters.AddWithValue("@Matricula", matricula);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error" + e.Message, e.Source);
            }
            finally
            {
                cnn.Close();
            }

        }
    }
}
