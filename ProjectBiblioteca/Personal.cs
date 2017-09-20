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
        private SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        public int numeroDeEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Ocupacion { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }

        public Personal() { }

        public Personal(int numeroDeEmpleado, string Nombre, string Ocupacion, string Correo, string Telefono)
        {
            this.numeroDeEmpleado = numeroDeEmpleado;
            this.Nombre = Nombre;
            this.Ocupacion = Ocupacion;
            this.Correo = Correo;
            this.Telefono = Telefono;
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
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                MessageBox.Show("SE HA ACTUALIZADO " + this.Nombre,"INFO",MessageBoxButtons.OK,MessageBoxIcon.Information);

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

        public bool verificarLibroPersonal(int numeroDeEmpleado)
        {
            bool salida = false;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("verificar_Libro_Personal", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NumeroDeEmpleado", numeroDeEmpleado);

                if (int.Parse(cmd.ExecuteScalar().ToString()) !=0)
                {
                    salida = true;
                }
                else
                {
                    salida = false;
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
            return salida;
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
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (MessageBox.Show("Esta seguro de borrar este registro permanentemete".ToUpper(), "BORRAR", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
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
