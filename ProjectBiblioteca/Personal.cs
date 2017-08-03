using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
namespace ProjectBiblioteca
{
    class Personal
    {
       
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());


        public int numeroDeEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Ocupacion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public Personal() { }
        public Personal(int numeroDeEmpleado, string nombre, string ocupacion, string correo, string telefono)
        {
            this.numeroDeEmpleado = numeroDeEmpleado;
            this.Nombre = nombre;
            this.Ocupacion = ocupacion;
            this.Correo = correo;
            this.Telefono = telefono;
        }

        public void agregarPersonalBD()
        {
            try
            {
                if (verificarPersonalRegistrado()==false)
                {
                    
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Agregar_Personal", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Numero_De_Empleado", this.numeroDeEmpleado);
                cmd.Parameters.AddWithValue("@Nombre", this.Nombre);
                cmd.Parameters.AddWithValue("@Ocupacion", this.Ocupacion);
                cmd.Parameters.AddWithValue("@Correo", this.Correo);
                cmd.Parameters.AddWithValue("@Telefono", this.Telefono);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha registrado " + this.Nombre);

                }
                else
                {
                    MessageBox.Show("Error.\nEmpleado existente");

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + e.Message);
            }
            finally
            {
                cnn.Close();
            }


        }
        public void actualizarPersonal(int numeroDeEmpleadoViejo)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Editar_Personal", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Numero_De_Empleado",this.numeroDeEmpleado);
                cmd.Parameters.AddWithValue("@Numero_De_Empleado_Viejo",numeroDeEmpleadoViejo);
                cmd.Parameters.AddWithValue("@Nombre",this.Nombre);
                cmd.Parameters.AddWithValue("@Ocupacion",this.Ocupacion);
                cmd.Parameters.AddWithValue("@Correo",this.Correo);
                cmd.Parameters.AddWithValue("@Telefono",this.Telefono);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Se ha actualizado " + this.Nombre);

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
        private bool verificarPersonalRegistrado()
        {
            bool salida = false;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("verificar_Personal", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroDeEmpleado", this.numeroDeEmpleado);
                
                if (int.Parse(cmd.ExecuteScalar().ToString())==1)
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
        public void borrarPersonalDB(int NumeroEmpl)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Eliminar_Personal", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Numero_De_Empleado", NumeroEmpl);
                if (MessageBox.Show("Esta seguro de borrar este registro permanentemete", "BORRAR", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    cmd.ExecuteNonQuery();
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

    }
}
