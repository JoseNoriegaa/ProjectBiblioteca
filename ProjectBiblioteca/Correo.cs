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
        public string AsuntoNotificacion { get; set; }
        public string CuerpoNotificacion { get; set; }

        public Correo() { }

        public Correo(string correo, string Password, string Asunto, string Cuerpo)
        {
            this.correo = correo;
            this.Password = Password;
            this.Asunto = Asunto;
            this.Cuerpo = Cuerpo;
        }

        public Correo(string AsuntoNotificacion, string CuerpoNotificacion)
        {
            this.AsuntoNotificacion = AsuntoNotificacion;
            this.CuerpoNotificacion = CuerpoNotificacion;
        }

        public bool completo(Correo c)
        {
            if (String.IsNullOrEmpty(c.correo) || String.IsNullOrEmpty(c.Password) || String.IsNullOrEmpty(c.Asunto) || String.IsNullOrEmpty(c.Cuerpo)
                || String.IsNullOrEmpty(c.AsuntoNotificacion) || String.IsNullOrEmpty(c.CuerpoNotificacion))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public Correo VerCorreo()
        {
            Correo Correo = new Correo();
            try
            {
                cnn.Open();
                cmd = new SqlCommand("verCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                rd = cmd.ExecuteReader();
                if (rd.Read())
                {
                    Correo.correo = rd[0].ToString();
                    Correo.Password = rd[1].ToString();
                    Correo.Asunto = rd[2].ToString();
                    Correo.Cuerpo = rd[3].ToString();
                    Correo.AsuntoNotificacion = rd[4].ToString();
                    Correo.CuerpoNotificacion = rd[5].ToString();
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
            return Correo;

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
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void adddNotificacionesCorreo()
        {
            try
            {
                cnn.Open();
                cmd = new SqlCommand("adddNotificacionesCorreo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Asunto", AsuntoNotificacion);
                cmd.Parameters.AddWithValue("@Cuerpo", CuerpoNotificacion);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha registrado correctamente asunto y correo para notificaciones");
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
