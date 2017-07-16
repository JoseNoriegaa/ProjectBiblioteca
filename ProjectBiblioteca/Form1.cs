using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectBiblioteca
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbFiltroBusqueda_Home.SelectedIndex = 0;
            cbTipo_Prestamo.SelectedIndex = 0;
        }

        private void tabPrestamo_Click(object sender, EventArgs e)
        {
            
            //cbTipo_Prestamo.SelectedIndex = 0;
            //ejemplo mandar correo
            //para = txpara.Text.Trim();
            //asunto = txasunto.Text.Trim();
            //texto = txtexto.Text.Trim();
            //mail = new MailMessage();
            //mail.To.Add(new MailAddress(this.para));
            //mail.From = new MailAddress("thejose123654@hotmail.com");
            //mail.Subject = asunto;
            //mail.Body = texto;
            //mail.IsBodyHtml = false;

            //SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
            //using (client)
            //{
            //    client.Credentials = new System.Net.NetworkCredential("thejose123654@hotmail.com", "-----");
            //    client.EnableSsl = true;
            //    client.Send(mail);
            //}
            //MessageBox.Show("El mensaje fue enviado correctamente");


        }

        private void cbTipo_Prestamo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTipo_Prestamo.SelectedItem.ToString())
            {
                case "ALUMNO":
                    cbDias_Prestamo.Items.Clear(); 
                    lAlumnos_Prestamo.Text = "LISTA ALUMNOS";
                    lNoControl_Empleado_Prestamo.Text = "No. de Control";
                    dgvListaAlumno_Prestamo.Columns[0].HeaderText = "No. de Control";
                    dgvListaAlumno_Prestamo.Columns[1].HeaderText = "Nombre Del Alumno";
                    cbDias_Prestamo.Items.Add(3);
                    cbDias_Prestamo.SelectedIndex = 0;
                    break;
                case "PERSONAL":
                    cbDias_Prestamo.Items.Clear();
                    lAlumnos_Prestamo.Text = "LISTA PERSONAL";
                    lNoControl_Empleado_Prestamo.Text = "No. de Empleado";
                    dgvListaAlumno_Prestamo.Columns[0].HeaderText = "No. de Empleado";
                    dgvListaAlumno_Prestamo.Columns[1].HeaderText = "Nombre Del Empleado";
                    cbDias_Prestamo.Items.Add("--");
                    cbDias_Prestamo.SelectedIndex = 0;
                    break;
            }
        }

        private void cbFiltroBusqueda_Home_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFiltroBusqueda_Home.SelectedItem.ToString())
            {
                case "TODOS":
                    dgvPrestamos_Home.Columns[0].HeaderText = "No. De Control/No. De Empleado";
                    break;
                case "ALUMNOS":
                    dgvPrestamos_Home.Columns[0].HeaderText = "No. De Control";
                    break;
                case "PERSONAL":
                    dgvPrestamos_Home.Columns[0].HeaderText = "No. De Empleado";
                    break;
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void checkPassCorreo_CheckedChanged(object sender, EventArgs e)
        {
            txtPassCorreo.UseSystemPasswordChar = !checkPassCorreo.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = "" + txtPassCorreo.Text[0]+ txtPassCorreo.Text[1];
            for (int i = 2; i < txtPassCorreo.Text.Length; i++)
            {
                password += "*";
            }
            if (MessageBox.Show("ESTA SEGURO QUE ESTE ES SU CORREO\nCorreo: "+txtCorreo.Text.ToUpper()+"\nContraceña:"+password,"CORREO",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                //registrar correo en la base de datos
            }
        }
    }
}
