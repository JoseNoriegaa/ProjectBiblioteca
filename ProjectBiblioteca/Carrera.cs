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
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void editarCarreraBD()
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("EditarCarrera", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdCarrera", this.IdCarrera);
                cmd.Parameters.AddWithValue("@NombreCarrera", this.nombreCarrera);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (MessageBox.Show("Al borrar este puesto se eliminaran todos los registros relacionados.\n¿Desea continuar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand("EliminarCarrera", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdCarrera", this.IdCarrera);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
