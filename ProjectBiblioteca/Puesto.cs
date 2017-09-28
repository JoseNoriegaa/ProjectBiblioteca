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
    class Puesto
    {

        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());

        public string nombre { get; set; }

        public Puesto() { }

        public Puesto(string nombre)
        {
            this.nombre = nombre.ToUpper();
        }

        public void agregarOcupacion()
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Agregar_Ocupacion", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Puesto", this.nombre.ToUpper());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha agregado correctamente ".ToUpper() + this.nombre.ToUpper());
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, "ERROR: " + ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                cnn.Close();
            }
        }
        
        public void editarPuestoBD(string puestoViejo)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("editarOcupacion", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@puesto", this.nombre);
                cmd.Parameters.AddWithValue("@puestoViejo", puestoViejo);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, "ERROR: " + ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void eliminarPuestoBD()
        {
            try
            {
                if (MessageBox.Show("Al borrar este puesto se eliminaran todos los registros relacionados.\n¿Desea continuar?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    cnn.Open();
                    SqlCommand cmd = new SqlCommand("eliminarPuesto", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@puesto", this.nombre);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, "ERROR: " + ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
