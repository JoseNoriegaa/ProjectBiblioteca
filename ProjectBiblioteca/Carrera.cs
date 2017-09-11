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
    class Carrera
    {
        
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());

        public string nombreCarrera { get; set; }
        public string IdCarrera { get; set; }

        public Carrera() { }

        public Carrera(string IdCarrera, string nombreCarrera)
        {
            this.nombreCarrera = nombreCarrera.ToUpper();
            this.IdCarrera = IdCarrera.ToUpper();
        }


        public Carrera(string IdCarrera)
        {
            this.IdCarrera = IdCarrera.ToUpper();
        }

        public void agregarCarreraBD()
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("AgregarCarrera", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCarrera",this.IdCarrera);
                cmd.Parameters.AddWithValue("@Carrera",this.nombreCarrera);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha agregado '" + this.nombreCarrera + "'");
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

        public void editarCarreraBD(string IdViejo)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("EditarCarrera", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCarreraViejo", IdViejo);
                cmd.Parameters.AddWithValue("@IdCarrera", this.IdCarrera);
                cmd.Parameters.AddWithValue("@NombreCarrera", this.nombreCarrera);
                cmd.ExecuteNonQuery();
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
        public void eliminarCarreraBD()
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("EliminarCarrera", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCarrera", this.IdCarrera);
                cmd.ExecuteNonQuery();
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
    }
}
