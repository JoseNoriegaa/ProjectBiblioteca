using ProjectBiblioteca;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BIBLIOTECA
{
    public partial class CommandLine : Form
    {
        private SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        private SqlCommand cmd;
        private string command;
        private string message;
        public CommandLine(string command)
        {
            InitializeComponent();
            switch (command)
            {
                case "SETALUMNO90":
                    message = "¿SEGURO QUE DESEA ELIMINAR TODOS LOS ALUMNOS?";
                    this.command = "ALUMNO";
                    break;
                case "SETPERSONAL90":
                    message = "¿SEGURO QUE DESEA ELIMINAR TODO EL PERSONAL?";
                    this.command = "PERSONAL";
                    break;
                case "SETLIBRO90":
                    message = "¿SEGURO QUE DESEA ELIMINAR TODOS LOS LIBROS?";
                    this.command = "LIBRO";
                    break;
            }
        }

        private void btnCommand_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(message,"ELIMINACIÓN",MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                cnn.Open();
                cmd = new SqlCommand("COMMAND", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Command", command);
                cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }
    }
}
