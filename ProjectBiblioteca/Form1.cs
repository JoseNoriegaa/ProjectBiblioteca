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
        Carrera carrera;
        Personal personal;
        bool actualizarAlumno = false;
        bool actualizarPersonal = false;
        bool actualizarLibro = false;

        //cnn laptop-noriega
        //SqlConnection cnn = new SqlConnection("Data Source=DESKTOP-91F61D3;Initial Catalog=Biblioteca;Integrated security=true;");
        //cnn pc-noriega
        SqlConnection cnn = new SqlConnection("Data Source=DESKTOP-TIBD95D;Initial Catalog=Biblioteca;Integrated security=true;");


        public Form1()
        {
            InitializeComponent();
            fillDGVs();
            fillCB();
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
            fillDGVs();
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
        //variable para guardar el id antes de editar libro
        string idLibroViejo;
        private void btnAgregar_Libro_Click(object sender, EventArgs e)
        {
            try
            {
                libro = new Libro(txtId_Libro.Text.ToUpper(), txtIsbn_Libro.Text.ToUpper(), txtTitulo_Libro.Text.ToUpper(), int.Parse(txtAño_Libro.Text), txtAutor_Libro.Text.ToUpper(), txtClasificiacion_Libro.Text.ToUpper(), txtDescripcion_Libro.Text.ToUpper(), txtEditorial_Libro.Text.ToUpper(), txtLugar_Libro.Text.ToUpper(), txtEdicion_Libro.Text.ToUpper());

                if (actualizarLibro==true)
                {
                    libro.actualizarLibro(idLibroViejo);
                }
                else
                { 
                libro.agregarLibroBD();
                }
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
                actualizarLibro = false;
                lblActualizar_Libro.Visible = actualizarLibro;
                fillDGVs();
            }
            catch(Exception j)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + j.Message, j.Source);

            }

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
                dgvListaLibros_Prestamo.Rows.Clear();

                cmd = new SqlCommand("BuscarLibros_Todos", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dgvListaLibros_Prestamo.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
                    dgvLista_libro.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd[3].ToString(), rd[4].ToString(), rd[5].ToString(), rd[7].ToString(), rd[8].ToString(), rd[9].ToString(), rd[6].ToString());
                }
                rd.Close();
                #endregion

                #region llenar DGV Alumnos-Personal Prestamo
                if (cbTipo_Prestamo.SelectedIndex == 0)
                {
                    cmd = new SqlCommand("Mostrar_Alumnos", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    dgvListaAlumno_Prestamo.Rows.Clear();
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
                    dgvListaAlumno_Prestamo.Rows.Clear();

                    while (rd.Read())
                    {
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString());
                    }
                }
                rd.Close();

                #endregion

                #region llenar lista alumnos
                cmd = new SqlCommand("Mostrar_Alumnos", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                dgvAlumnos_Alumno.Rows.Clear();
                while (rd.Read())
                {
                    dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Carrera"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString());
                }
                rd.Close();

                #endregion

                #region llenar lista Personal
                cmd = new SqlCommand("Mostrar_Personal", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dgvLista_Personal.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString(), rd["Ocupacion"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString());
                }
                rd.Close();

                #endregion




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


        //Combo box de cuatrimestre en agregar alumno no se utiliza

        private void btnAdd_Alumno_Click(object sender, EventArgs e)
        {   
            alumno = new Alumno(int.Parse(txtNoControl_AlumnoAdd.Text), txtNombre_AlumnoAdd.Text.ToUpper(), txtTelefono_AlumnoAdd.Text.ToUpper(), txtEmail_AlumnoAdd.Text.ToUpper(), cbCarrera_AlumnoAdd.Text.ToUpper());

            if (actualizarAlumno == true)
            {
                alumno.actualizarAlumno(matriculavieja_Alumno);
            }
            else
            {
                alumno.agregarAlumnoBD();
            }

            txtNoControl_AlumnoAdd.Text = null;
            txtNombre_AlumnoAdd.Text = null;
            
            txtTelefono_AlumnoAdd.Text = null;
            txtEmail_AlumnoAdd.Text = null;
            cbCuatrimestre_AlumnoAdd.SelectedIndex = -1;
            cbCarrera_AlumnoAdd.SelectedIndex = -1;
            fillDGVs();
            actualizarAlumno = false;
            lblActualizar_Alumno.Visible = actualizarAlumno;
        }

        private void txtTelefono_AlumnoAdd_TextChanged(object sender, EventArgs e)
        {

        }

        private void rbNuevaCarrera_Alumno_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNuevaCarrera_Alumno.Checked)
            {
                gbAgregarCarrera_Alumno.Visible = true;
            }
            else
            {
                gbAgregarCarrera_Alumno.Visible = false;
            }
        }
        private void fillCB()
        {
            #region Carrera Alumno
            cnn.Open();
            SqlCommand cmd = new SqlCommand("verCarreras",cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rd = cmd.ExecuteReader();
            
            cbCarrera_AlumnoAdd.Items.Clear();
            while (rd.Read())
            {
                cbCarrera_AlumnoAdd.Items.Add(rd[0].ToString());
            }
            rd.Close();
            #endregion

            #region Ocupacion Personal
            cmd = new SqlCommand("Mostrar_Ocupacion", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            rd = cmd.ExecuteReader();
            cbOcupacion_Personal.Items.Clear();
            while (rd.Read())
            {
                cbOcupacion_Personal.Items.Add(rd[0].ToString());
            }
            rd.Close();
            #endregion

            cnn.Close();
        }

        private void btnAddCarrera_alumno_Click(object sender, EventArgs e)
        {
            carrera = new Carrera(txtCarrrera_Alumno.Text);
            carrera.agregarCarreraBD();
            rbNuevaCarrera_Alumno.Checked = false;
            fillCB();
        }

        private void txtAlumno_Prestamo_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtAlumno_Prestamo.Text)==false)
            {


                SqlCommand cmd;
                SqlDataReader rd;
                cnn.Open();
            if (cbTipo_Prestamo.SelectedIndex == 0)
            {
                cmd = new SqlCommand("Buscar_Alumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", txtAlumno_Prestamo.Text);
                rd = cmd.ExecuteReader();
                dgvListaAlumno_Prestamo.Rows.Clear();
                while (rd.Read())
                {
                    dgvListaAlumno_Prestamo.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString());
                }
            }
            else
            {
                    cmd = new SqlCommand("Buscar_Personal", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Nombre", txtAlumno_Prestamo.Text);
                    rd = cmd.ExecuteReader();
                    dgvListaAlumno_Prestamo.Rows.Clear();
                    while (rd.Read())
                    {
                        dgvListaAlumno_Prestamo.Rows.Add(rd[0].ToString(), rd["Nombre"].ToString());
                    }
                }
                cnn.Close();
            }
            else
            {
                fillDGVs();

            }

        }

        private void txtLibro_Prestamo_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtLibro_Prestamo.Text) == false)
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Buscar_Libro", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdLibro", txtLibro_Prestamo.Text);
                cmd.Parameters.AddWithValue("@Autor", txtLibro_Prestamo.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtLibro_Prestamo.Text);
                cmd.Parameters.AddWithValue("@Titulo", txtLibro_Prestamo.Text);
                cmd.Parameters.AddWithValue("@ISBN", txtLibro_Prestamo.Text);


                SqlDataReader rd = cmd.ExecuteReader();
                dgvListaLibros_Prestamo.Rows.Clear();
                while (rd.Read())
                {
                    dgvListaLibros_Prestamo.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
                }
                cnn.Close();
            }
            else
            {
                fillDGVs();
            }
            
            
        }

        private void txtBusqueda_Libro_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtBusqueda_Libro.Text) == false)
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Buscar_Libro", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdLibro", txtBusqueda_Libro.Text);
                cmd.Parameters.AddWithValue("@Autor", txtBusqueda_Libro.Text);
                cmd.Parameters.AddWithValue("@Descripcion", txtBusqueda_Libro.Text);
                cmd.Parameters.AddWithValue("@Titulo", txtBusqueda_Libro.Text);
                cmd.Parameters.AddWithValue("@ISBN", txtBusqueda_Libro.Text);


                SqlDataReader rd = cmd.ExecuteReader();
                dgvLista_libro.Rows.Clear();
                while (rd.Read())
                {
                    dgvLista_libro.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
                }
                cnn.Close();
            }
            else
            {
                fillDGVs();
            }
        }

        private void txtBusqueda_Alumno_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBusqueda_Alumno.Text)==false)
            {
                cnn.Open();
               SqlCommand cmd = new SqlCommand("Buscar_Alumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", txtBusqueda_Alumno.Text);
               SqlDataReader rd = cmd.ExecuteReader();
               dgvAlumnos_Alumno.Rows.Clear();
                while (rd.Read())
                {
                    dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Carrera"].ToString());
                }
                cnn.Close();
            }
            else
            {
                fillDGVs();
            }
            
        }

        private void txtBuscar_Personal_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscar_Personal.Text)==false)
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Buscar_Personal", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", txtBuscar_Personal.Text);
                SqlDataReader rd = cmd.ExecuteReader();
                dgvLista_Personal.Rows.Clear();
                while (rd.Read())
                {
                    dgvLista_Personal.Rows.Add(rd[0].ToString(), rd["Nombre"].ToString(), rd["Ocupacion"].ToString());
                }
                cnn.Close();
            }
            else
            {
                fillDGVs();
            }
        }
        //variable para guardar matricula antes de actualizar
        int matriculavieja_Alumno;
        private void dgvAlumnos_Alumno_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            actualizarAlumno = true;
            lblActualizar_Alumno.Visible = actualizarAlumno;
            matriculavieja_Alumno = int.Parse(dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString());
            txtNoControl_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString();
            for (int i = 0; i < cbCarrera_AlumnoAdd.Items.Count; i++)
            {
                if (dgvAlumnos_Alumno.CurrentRow.Cells[2].Value.ToString()==cbCarrera_AlumnoAdd.Items[i].ToString())
                {
                    
                    cbCarrera_AlumnoAdd.SelectedIndex=i;
                }
            }
            txtNombre_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[1].Value.ToString();
            txtEmail_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[3].Value.ToString();
            txtTelefono_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[4].Value.ToString();


        }

        private void tabAlumno_Click(object sender, EventArgs e)
        {

        }

        
        //variable para guardar numero de empleado antes de editar
        int numeroDeEmpleadoViejo;
        private void dgvLista_Personal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            numeroDeEmpleadoViejo = int.Parse(dgvLista_Personal.CurrentRow.Cells[0].Value.ToString());
            actualizarPersonal = true;
            lblActualizar_Personal.Visible = actualizarPersonal;


            txtNoEmpleado_Personal.Text = dgvLista_Personal.CurrentRow.Cells[0].Value.ToString();
            txtNombre_Personal.Text= dgvLista_Personal.CurrentRow.Cells[1].Value.ToString();
            for (int i = 0; i < cbOcupacion_Personal.Items.Count; i++)
            {
                if (dgvLista_Personal.CurrentRow.Cells[2].Value.ToString()==cbOcupacion_Personal.Items[i].ToString())
                {
                    cbOcupacion_Personal.SelectedIndex = i;
                }
            }
            txtEMail_Personal.Text = dgvLista_Personal.CurrentRow.Cells[3].Value.ToString();
            txtTelefono_Personal.Text = dgvLista_Personal.CurrentRow.Cells[4].Value.ToString();

        }

        private void btnAgregar_Personal_Click(object sender, EventArgs e)
        {

            personal = new Personal(int.Parse(txtNoEmpleado_Personal.Text), txtNombre_Personal.Text.ToUpper().Trim(), cbOcupacion_Personal.SelectedItem.ToString().ToUpper().Trim(), txtEMail_Personal.Text.ToUpper().Trim(), txtTelefono_Personal.Text.ToUpper().Trim());
            if (actualizarPersonal == true)
            {
                personal.actualizarPersonal(numeroDeEmpleadoViejo);
            }
            else
            {
                personal.agregarPersonalBD();
            }
            fillDGVs();

            txtNoEmpleado_Personal.Text = null;
            txtNombre_Personal.Text = null;
            cbOcupacion_Personal.SelectedIndex = -1;
            txtEMail_Personal.Text = null;
            txtTelefono_Personal.Text = null;
            actualizarPersonal = false;
            lblActualizar_Personal.Visible = actualizarPersonal;
        }

        private void btnOcupacion_Personal_Click(object sender, EventArgs e)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Agregar_Ocupacion", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Puesto", txtOcupacion_Personal.Text.ToUpper());
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha agregado correctamente " + txtOcupacion_Personal.Text.ToUpper());
                cbAddOcupacion_Personal.Checked = false;
            }
            catch (Exception f)
            {

                MessageBox.Show("Ha ocurrido un error.\n" + f.Message);

            }
            finally
            {
                cnn.Close();
                fillCB();
            }
            

        }

        private void cbAddOcupacion_Personal_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAddOcupacion_Personal.Checked)
            {
                gbOcupacion_Personal.Visible = true;
            }
            else
            {
                gbOcupacion_Personal.Visible = false;
            }
        }
       
        private void dgvLista_libro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLista_libro.Rows.Count>0)
            {

            
            actualizarLibro = true;
            lblActualizar_Libro.Visible = actualizarLibro;
            idLibroViejo=dgvLista_libro.CurrentRow.Cells[0].Value.ToString();
            txtId_Libro.Text = dgvLista_libro.CurrentRow.Cells[0].Value.ToString();
            txtIsbn_Libro.Text = dgvLista_libro.CurrentRow.Cells[2].Value.ToString();
            txtTitulo_Libro.Text = dgvLista_libro.CurrentRow.Cells[1].Value.ToString();
            txtAño_Libro.Text = dgvLista_libro.CurrentRow.Cells[3].Value.ToString();
            txtClasificiacion_Libro.Text = dgvLista_libro.CurrentRow.Cells[4].Value.ToString();
            txtAutor_Libro.Text = dgvLista_libro.CurrentRow.Cells[5].Value.ToString();
            txtEditorial_Libro.Text = dgvLista_libro.CurrentRow.Cells[6].Value.ToString();
            txtLugar_Libro.Text = dgvLista_libro.CurrentRow.Cells[7].Value.ToString();
            txtEdicion_Libro.Text = dgvLista_libro.CurrentRow.Cells[8].Value.ToString();
            txtDescripcion_Libro.Text = dgvLista_libro.CurrentRow.Cells[9].Value.ToString();
            }

        }
    }
}
