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
        
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());

        public int Matricula { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Carrera { get; set; }
        public int cuatrimestre{ get; set; }


        public Alumno() { }
        public Alumno(int matricula, string nombre,  string telefono, string correo, string carrera,int cuatrimestre)
        {
            this.Matricula = matricula;
            this.Nombre = nombre.Trim();
            this.Correo = correo;
            this.Telefono = telefono;
            this.Carrera = carrera;
            this.cuatrimestre = cuatrimestre;
            ;
        }

        public void agregarAlumnoBD()
        {
            try
            {
                if (verificarAlumnoRegistrado(this.Matricula)==false)
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand("Alta_Alumno", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Matricula", this.Matricula);
                    cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                    cmd.Parameters.AddWithValue("@Correo", this.Correo);
                    cmd.Parameters.AddWithValue("@Telefono", this.Telefono);
                    cmd.Parameters.AddWithValue("@Carrera", this.Carrera);
                    cmd.Parameters.AddWithValue("@cuatrimestre", this.cuatrimestre);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Se ha registrado "+ this.Nombre);
                }
                else
                {
                    MessageBox.Show("Error.\nMatricula existente");
                }
                
                

            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + e.Message, e.Source);

            }
            finally
            {
                cnn.Close();
            }

        }
        public void actualizarAlumno(int matriculaVieja)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Editar_Alumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Matricula", this.Matricula);
                cmd.Parameters.AddWithValue("@MatriculaVieja", matriculaVieja);
                cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                cmd.Parameters.AddWithValue("@Correo", this.Correo);
                cmd.Parameters.AddWithValue("@Telefono", this.Telefono);
                cmd.Parameters.AddWithValue("@Carrera", this.Carrera);
                cmd.Parameters.AddWithValue("@cuatrimestre", this.cuatrimestre);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha actualizado "+ this.Nombre);
            }
            catch (Exception e)
            {

                MessageBox.Show("Ha ocurrido un error.\n" + e.Message, e.Source);
            }
            finally
            {
                cnn.Close();
            }


        }
        public bool verificarAlumnoRegistrado(int numeroDeControl)
        {
            bool salida = false;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("verificarAlumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Matricula", numeroDeControl);
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                if (int.Parse(rd[0].ToString()) == 1)
                {
                    salida = true;
                }
                else
                {
                    salida = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + e.Message, e.Source);

            }
            finally
            {
                cnn.Close();
            }
            return salida;
        }
        public void borrarAlumnoDB(int matricula)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Eliminar_Alumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Matricula", matricula);
                if (MessageBox.Show("Esta seguro de borrar este registro permanentemete", "BORRAR", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                { 
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + e.Message, e.Source);
            } finally
            {
                cnn.Close();
            }


        }
    }
}
