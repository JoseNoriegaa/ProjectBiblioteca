using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System.Windows.Documents;

namespace ProjectBiblioteca
{
    class Correo
    {
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        SqlDataReader rd;
        SqlCommand cmd;
        public string correo { get; set; }
        public string Password { get; set; }
        public string Asunto { get; set; }
        public string Cuerpo { get; set; }

        public Correo() { }

        public Correo(string correo, string Password, string asunto, string cuerpo)
        {
            this.correo = correo;
            this.Password = Password;
            this.Asunto = asunto;
            this.Cuerpo = cuerpo;
        }

        public List<string> VerCorreo()
        {
            List<string> ls = new List<string>();
            try
            {
                cnn.Open();
                cmd = new SqlCommand("verCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    ls.Add(rd[0].ToString());
                    ls.Add(rd[1].ToString());
                    ls.Add(rd[2].ToString());
                    ls.Add(rd[3].ToString());
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
        public void agregarCorreo()
        {
            try
            {
                cnn.Open();
                cmd = new SqlCommand("ingresarCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Correo", correo);
                cmd.Parameters.AddWithValue("@Contraseña", Password);
                cmd.Parameters.AddWithValue("@Asunto", Asunto);
                cmd.Parameters.AddWithValue("@Cuerpo", Cuerpo);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha registrado correctamente su correo");
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
