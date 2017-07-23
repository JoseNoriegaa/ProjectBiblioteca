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
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace ProjectBiblioteca
{
    public partial class Form1 : Form
    {
        Libro libro;
        Alumno alumno;

        //cnn laptop-noriega
        //SqlConnection cnn = new SqlConnection("Data Source=DESKTOP-91F61D3;Initial Catalog=Biblioteca;Integrated security=true;");
        //cnn pc-noriega
        SqlConnection cnn = new SqlConnection("Data Source=DESKTOP-TIBD95D;Initial Catalog=Biblioteca;Integrated security=true;");


        public Form1()
        {
            InitializeComponent();
            fillDGVs();
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
            string password = "" + txtPassCorreo.Text[0] + txtPassCorreo.Text[1];
            for (int i = 2; i < txtPassCorreo.Text.Length; i++)
            {
                password += "*";
            }
            if (MessageBox.Show("ESTA SEGURO QUE ESTE ES SU CORREO\nCorreo: " + txtCorreo.Text.ToUpper() + "\nContraceña:" + password, "CORREO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //registrar correo en la base de datos
            }
        }

        private void btnAgregar_Libro_Click(object sender, EventArgs e)
        {
            try
            {
                libro = new Libro(txtId_Libro.Text.ToUpper(), txtIsbn_Libro.Text.ToUpper(), txtTitulo_Libro.Text.ToUpper(), int.Parse(txtAño_Libro.Text), txtAutor_Libro.Text.ToUpper(), txtClasificiacion_Libro.Text.ToUpper(), txtDescripcion_Libro.Text.ToUpper(), txtEditorial_Libro.Text.ToUpper(), txtLugar_Libro.Text.ToUpper(), txtEdicion_Libro.Text.ToUpper());
                libro.agregarLibroBD();

                txtId_Libro.Text = null;
                txtIsbn_Libro.Text = null;
                txtTitulo_Libro.Text = null;
                txtAño_Libro.Text = null;
                txtClasificiacion_Libro.Text = null;
                txtAutor_Libro.Text = null;
                txtDescripcion_Libro.Text = null;
                txtEditorial_Libro.Text = null;
                txtLugar_Libro.Text = null;
                txtEdicion_Libro.Text = null;
                fillDGVs();
            }
            catch { }

        }

        private void fillDGVs()
        {
            try
            {
                SqlCommand cmd;
                SqlDataReader rd;
                cnn.Open();
                #region llenar DGV libro
                dgvLista_libro.Rows.Clear();
                dgvLista_Personal.Rows.Clear();
                cmd = new SqlCommand("BuscarLibros_Todos", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dgvListaLibros_Prestamo.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
                    dgvLista_libro.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
                }
                rd.Close();
                #endregion

                #region llenar DGV Alumnos-Personal Prestamo
                if (cbTipo_Prestamo.Text == "Alumnos")
                {
                    cmd = new SqlCommand("Mostrar_Alumnos", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString());
                    }
                }
                else
                {
                    cmd = new SqlCommand("Mostrar_Personal", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString());
                    }
                }
                rd.Close();

                #endregion

                //#region llenar lista alumnos
                //cmd = new SqlCommand("Mostrar_Alumnos", cnn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //rd = cmd.ExecuteReader();
                //while (rd.Read())
                //{
                //    dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString());
                //}
                //rd.Close();

                //#endregion

                //#region llenar lista Personal
                //cmd = new SqlCommand("Mostrar_Personal", cnn);
                //cmd.CommandType = CommandType.StoredProcedure;
                //rd = cmd.ExecuteReader();
                //while (rd.Read())
                //{
                //    dgvLista_Personal.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString());
                //}
                //rd.Close();

                //#endregion




            }

            finally
            {
                cnn.Close();
            }

        }



        private void dgvListaLibros_Prestamo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNombreLibro_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[1].Value.ToString();
            txtISBN_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[2].Value.ToString();
            txtIdEjemplar_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[0].Value.ToString();
        }

        private void dgvListaAlumno_Prestamo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtNoControl_Empleado_Prestamo.Text = dgvListaAlumno_Prestamo.CurrentRow.Cells[0].Value.ToString();
        }

        private void btnAdd_Alumno_Click(object sender, EventArgs e)
        {
            alumno = new Alumno(int.Parse(txtApellido_AlumnoAdd.Text), txtNombre_AlumnoAdd.Text.ToUpper(), txtTelefono_AlumnoAdd.Text.ToUpper(), txtCorreo.Text.ToUpper(), cbCarrera_AlumnoAdd.Text.ToUpper());
            fillDGVs();
        }

        private void txtTelefono_AlumnoAdd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
