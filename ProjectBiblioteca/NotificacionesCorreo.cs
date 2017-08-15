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
    class NotificacionesCorreo
    {
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        SqlDataReader rd;

        public string Asunto { get; set; }
        public string Cuerpo { get; set; }
        public NotificacionesCorreo() { }

        public NotificacionesCorreo(string Asunto, string Cuerpo)
        {
            this.Asunto = Asunto;
            this.Cuerpo = Cuerpo;
        }

        public int countNotificacionesCorreo()
        {
            int salida = 0;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("countNotificacionesCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                salida = rd.Read() ? int.Parse(rd[0].ToString()) : 0;
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                cnn.Close();
            }
            return salida;
        }

        public List<string> verNotificacionesCorreo()
        {
            List<string> ls = new List<string>();
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("verNotificacionesCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    ls.Add(rd[0].ToString());
                    ls.Add(rd[1].ToString());
                }
                else
                {
                    ls.Add("");
                }
            }
            catch (Exception f)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + f.Message, f.Source);
            }
            finally
            {
                cnn.Close();
            }
            return ls;

        }

        public void agregarNotificacionesCorreo()
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("ingresarNotificacionesCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Asunto", Asunto);
                cmd.Parameters.AddWithValue("@Cuerpo", Cuerpo);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha registrado correctamente la información");
            }
            catch (Exception f)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + f.Message, f.Source);
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
