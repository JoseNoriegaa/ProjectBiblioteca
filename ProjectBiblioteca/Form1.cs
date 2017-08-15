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
using System.Data.SqlClient;
using System.Configuration;
using LiveCharts.Wpf;
using LiveCharts;

namespace ProjectBiblioteca
{
    public partial class Form1 : Form
    {
        Libro libro;
        Alumno alumno;
        Carrera carrera;
        Personal personal;
        Prestamo prestamo;
        NotificacionesCorreo nCorreo = new NotificacionesCorreo();
        Correo correo = new Correo();
        bool actualizarAlumno = false;
        bool agregarAlumno = false;
        bool actualizarPersonal = false;
        bool agregarPersonal = false;
        bool actualizarLibro = false;
        bool agregarLibro = false;
        bool borrarAlumno = false;
        bool borrarPersonal = false;
        bool borrarLibro = false;
        bool selecAlumno_Personal = false;
        bool seleclibro = false;
        string para;
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        ColumnSeries col;
        Axis ax;
        SqlCommand cmd;
        SqlDataReader rd;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbFiltroBusqueda_Home.SelectedIndex = 0;
            cbTipo_Prestamo.SelectedIndex = 0;
            cbFiltro_Historial.SelectedIndex = 0;
            cbFiltro_Analisis.SelectedIndex = 0;
            fillDGVs();
            fillCB();
            verificarFechaPrestamo();
            ShowIcon = true;

            llenarGrafica();

            List<Control> ls = new List<Control>() {tabInicio, tabPrestamo,tabAlumno,tabPersonal, tabAnalisis,tabLibro,tabPage2, tabAjustes_2,
                tabAjustes,tabControl1, gbAgregarCarrera_Alumno, gbCorreo_herramientas, gbGrafica_Analisis, gbOcupacion_Personal,gbEliminarCarrera_Alumno };
            foreach (Control item1 in ls)
            {
                foreach (Control item in item1.Controls)
                {
                    item.MinimumSize = item.Size;
                }
            }
        }

        private void verificarFechaPrestamo()
        {
            List<string> fechas = new List<string>();
            for (int i = 0; i < dgvPrestamos_Home.Rows.Count; i++)
            {
                fechas.Add(dgvPrestamos_Home.Rows[i].Cells["Fecha_De_Entrega"].Value.ToString().Trim());
            }
            int contador = 0;
            for (int i = 0; i < fechas.Count; i++)
            {
                if (fechas[i] == DateTime.Today.Date.ToString().Remove(10))
                {
                    contador++;
                }
            }
            if (contador > 0)
            {
                notifyIcon1.Visible = true;
                string pluralosing = "";
                if (contador == 1)
                {
                    pluralosing = "LIBRO PENDIENTE";
                }
                else
                {
                    pluralosing = "LIBROS PENDIENTES";
                }
                notifyIcon1.ShowBalloonTip(2000, "BIBLIOTECA", "HAY " + contador + " " + pluralosing + " A DEVOLVER HOY " + DateTime.Today.Date.ToString().Remove(10), ToolTipIcon.Info);
            }
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
                    dtpPrestamo_Prestamo.Value = DateTime.Today;
                    switch (DateTime.Today.DayOfWeek)
                    {
                        case DayOfWeek.Monday:
                            dtpEntrega_Prestamo.Value = DateTime.Today.AddDays(3);
                            break;

                        case DayOfWeek.Tuesday:
                            dtpEntrega_Prestamo.Value = DateTime.Today.AddDays(3);
                            break;

                        case DayOfWeek.Wednesday:
                            dtpEntrega_Prestamo.Value = DateTime.Today.AddDays(5);
                            break;

                        case DayOfWeek.Thursday:
                            dtpEntrega_Prestamo.Value = DateTime.Today.AddDays(5);
                            break;

                        case DayOfWeek.Friday:
                            dtpEntrega_Prestamo.Value = DateTime.Today.AddDays(5);
                            break;
                    }
                    dtpEntrega_Prestamo.Text = dtpEntrega_Prestamo.Value.ToString();
                    break;
                case "PERSONAL":
                    cbDias_Prestamo.Items.Clear();
                    lAlumnos_Prestamo.Text = "LISTA PERSONAL";
                    lNoControl_Empleado_Prestamo.Text = "No. de Empleado";
                    dgvListaAlumno_Prestamo.Columns[0].HeaderText = "No. de Empleado";
                    dgvListaAlumno_Prestamo.Columns[1].HeaderText = "Nombre Del Empleado";
                    cbDias_Prestamo.Items.Add("--");
                    cbDias_Prestamo.SelectedIndex = 0;
                    dtpPrestamo_Prestamo.Value = DateTime.Today;
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
            fillDGVs();
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
            if (string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrEmpty(txtPassCorreo.Text))
            {
                MessageBox.Show("Los campos no pueden estar vacios");
            }
            else
            {
                if (MessageBox.Show("ESTA SEGURO QUE ESTE ES SU CORREO\nCorreo: " + txtCorreo.Text.ToUpper() + "\nContraceña:" + password, "CORREO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Correo correo = new Correo(txtCorreo.Text, txtPassCorreo.Text);
                    correo.agregarCorreo();
                }
            }
        }
        //variable para guardar el id antes de editar libro
        string idLibroViejo;
        private void btnAgregar_Libro_Click(object sender, EventArgs e)
        {
            bool bl = false;
            try
            {
                if (string.IsNullOrEmpty(txtId_Libro.Text) || string.IsNullOrEmpty(txtIsbn_Libro.Text) || string.IsNullOrEmpty(txtTitulo_Libro.Text) 
                    || string.IsNullOrEmpty(txtAutor_Libro.Text) || string.IsNullOrEmpty(txtClasificiacion_Libro.Text))
                {
                    actualizarLibro = false;
                    agregarLibro = false;
                    MessageBox.Show("Hay campos vacios que son requeridos");
                    bl =true;
                }
                else
                {
                    if (actualizarLibro==false)
                    {
                        agregarLibro=true;
                    }
                    try
                    {
                        libro = new Libro(txtId_Libro.Text.ToUpper(), txtIsbn_Libro.Text.ToUpper(), txtTitulo_Libro.Text.ToUpper(), int.Parse(txtAño_Libro.Text), txtAutor_Libro.Text.ToUpper(), txtClasificiacion_Libro.Text.ToUpper(), txtDescripcion_Libro.Text.ToUpper(), txtEditorial_Libro.Text.ToUpper(), txtLugar_Libro.Text.ToUpper(), txtEdicion_Libro.Text.ToUpper());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ha ingresado datos de manera incorrecta");
                        bl = true;
                    }

                }
                if (actualizarLibro && bl == false)
                {
                    libro.actualizarLibro(idLibroViejo);
                }
                else if(agregarLibro && bl == false)
                {
                    libro.agregarLibroBD();
                }
                if (bl==false)
                {
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
            }
            catch (Exception j)
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
                    dgvLista_libro.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd[3].ToString(), rd[4].ToString(), rd[5].ToString(), rd[7].ToString(), rd[8].ToString(), rd[9].ToString(), rd[6].ToString());
                }
                rd.Close();
                #endregion

                #region llenar DGV Alumnos-Personal Prestamo
                if (cbTipo_Prestamo.SelectedIndex == 0)
                {
                    cmd = new SqlCommand("Mostrar_Alumnos_SinLibro", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    dgvListaAlumno_Prestamo.Rows.Clear();
                    while (rd.Read())
                    {
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Correo"].ToString());
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
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString(), rd["Correo"].ToString());
                    }
                }
                rd.Close();
                #endregion

                #region llenar dgv Libro prestamo
                cmd = new SqlCommand("BuscarLibros_NoPrestados", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                rd = cmd.ExecuteReader();
                while (rd.Read())
                {
                    dgvListaLibros_Prestamo.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
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
                    dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Carrera"].ToString(), rd["cuatrimestre"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString());
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

                #region llenar dgv prestamos home
                switch (cbFiltroBusqueda_Home.SelectedItem.ToString())
                {
                    case "TODOS":
                        cmd = new SqlCommand("MostrarPrestamos_Alumno", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvPrestamos_Home.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvPrestamos_Home.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(), rd[3].ToString(), rd[4].ToString(), rd[5].ToString(), rd[6].ToString().Remove(10));
                        }
                        rd.Close();
                        cmd = new SqlCommand("MostrarPrestamos_Personal", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            dgvPrestamos_Home.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(), rd[3].ToString(), rd[4].ToString(), rd[5].ToString(), rd[6].ToString().Remove(10));

                        }
                        rd.Close();

                        break;
                    case "ALUMNOS":
                        cmd = new SqlCommand("MostrarPrestamos_Alumno", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvPrestamos_Home.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvPrestamos_Home.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(), rd[3].ToString(), rd[4].ToString(), rd[5].ToString(), rd[6].ToString().Remove(10));
                        }
                        rd.Close();
                        break;

                    case "PERSONAL":
                        cmd = new SqlCommand("MostrarPrestamos_Personal", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvPrestamos_Home.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvPrestamos_Home.Rows.Add(rd[0].ToString(), rd[1].ToString(), rd[2].ToString(), rd[3].ToString(), rd[4].ToString(), rd[5].ToString(), rd[6].ToString().Remove(10));
                        }
                        rd.Close();
                        break;
                }

                #endregion

                #region llenar dgv historial Herramientas
                switch (cbFiltro_Historial.SelectedIndex)
                {
                    case 0:
                        cmd = new SqlCommand("historialPrestamos_Alumno", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvPrestamos_Historial.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvPrestamos_Historial.Rows.Add(rd["Id_Prestamo"].ToString(), rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd["Fecha_Prestamo"].ToString().Remove(10), rd["Fecha_Entrega"].ToString().Remove(10));
                        }
                        rd.Close();
                        cmd = new SqlCommand("historialPrestamos_Personal", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            dgvPrestamos_Historial.Rows.Add(rd["Id_Prestamo"].ToString(), rd["Personal"].ToString(), rd["Nombre"].ToString(), rd["Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd["Fecha_Prestamo"].ToString().Remove(10), rd["Fecha_Entrega"].ToString().Remove(10));
                        }
                        rd.Close();

                        break;
                    case 1:
                        cmd = new SqlCommand("historialPrestamos_Alumno", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvPrestamos_Historial.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvPrestamos_Historial.Rows.Add(rd["Id_Prestamo"].ToString(), rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd["Fecha_Prestamo"].ToString().Remove(10), rd["Fecha_Entrega"].ToString().Remove(10));
                        }
                        rd.Close();
                        break;
                    case 2:
                        cmd = new SqlCommand("historialPrestamos_Personal", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvPrestamos_Historial.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvPrestamos_Historial.Rows.Add(rd["Id_Prestamo"].ToString(), rd["Personal"].ToString(), rd["Nombre"].ToString(), rd["Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd["Fecha_Prestamo"].ToString().Remove(10), rd["Fecha_Entrega"].ToString().Remove(10));
                        }
                        rd.Close();
                        break;
                }
                #endregion
            }

            finally
            {
                cnn.Close();
            }
        }

        private void dgvListaLibros_Prestamo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex<dgvListaLibros_Prestamo.Rows.Count && e.RowIndex>=0)
            {
                txtNombreLibro_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[1].Value.ToString();
                txtISBN_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[2].Value.ToString();
                txtIdEjemplar_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[0].Value.ToString();
                seleclibro = true;
            }
        }

        private void dgvListaAlumno_Prestamo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex<dgvListaAlumno_Prestamo.Rows.Count && e.RowIndex>=0)
            {
                txtNoControl_Empleado_Prestamo.Text = dgvListaAlumno_Prestamo.CurrentRow.Cells[0].Value.ToString();
                para = dgvListaAlumno_Prestamo.CurrentRow.Cells[2].Value.ToString();
                selecAlumno_Personal = true;
            }
        }


        //Combo box de cuatrimestre en agregar alumno no se utiliza

        private void btnAdd_Alumno_Click(object sender, EventArgs e)
        {
            bool bl = false;
            if (string.IsNullOrEmpty(txtNoControl_AlumnoAdd.Text.Trim()) || string.IsNullOrEmpty(txtNombre_AlumnoAdd.Text.Trim()) 
                || cbCarrera_AlumnoAdd.SelectedIndex==0 || cbCuatrimestre_AlumnoAdd.SelectedIndex==0)
            {
                MessageBox.Show("Campo no llenado");
                actualizarAlumno = false;
                agregarAlumno = false;
                bl = true;
            }
            else
            {
                if (actualizarAlumno==false)
                {
                    agregarAlumno = true;
                }
                try
                {
                    alumno = new Alumno(int.Parse(txtNoControl_AlumnoAdd.Text), txtNombre_AlumnoAdd.Text.ToUpper(), txtTelefono_AlumnoAdd.Text, txtEmail_AlumnoAdd.Text.ToUpper(), cbCarrera_AlumnoAdd.Text.ToUpper(), int.Parse(cbCuatrimestre_AlumnoAdd.SelectedItem.ToString()));
                }
                catch (Exception)
                {
                    MessageBox.Show("Formato no valido");
                    bl = true;
                }
            }
            if (actualizarAlumno && bl == false)
            {
                alumno.actualizarAlumno(matriculavieja_Alumno);
            }
            else if(agregarAlumno && bl == false)
            {
                alumno.agregarAlumnoBD();
            }
            if (bl ==false)
            {
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
        }



        private void fillCB()
        {
            #region Carrera Alumno
            cnn.Open();
            SqlCommand cmd = new SqlCommand("verCarreras", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader rd = cmd.ExecuteReader();

            cbCarrera_AlumnoAdd.Items.Clear();
            cbEliminarCarrera_Alumno.Items.Clear();
            cbCarrera_AlumnoAdd.Items.Add("Seleccione una opcion");
            cbEliminarCarrera_Alumno.Items.Add("Seleccione una opcion");
            while (rd.Read())
            {
                cbCarrera_AlumnoAdd.Items.Add(rd[0].ToString());
                cbEliminarCarrera_Alumno.Items.Add(rd[0].ToString());
            }
            cbCarrera_AlumnoAdd.SelectedItem = "Seleccione una opcion";
            rd.Close();
            #endregion

            #region Ocupacion Personal
            cmd = new SqlCommand("Mostrar_Ocupacion", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            rd = cmd.ExecuteReader();
            cbOcupacion_Personal.Items.Clear();
            cbOcupacion_Personal.Items.Add("Seleccione una opcion");
            while (rd.Read())
            {
                cbOcupacion_Personal.Items.Add(rd[0].ToString());
            }
            rd.Close();
            cbOcupacion_Personal.SelectedItem = "Seleccione una opcion";
            #endregion

            cnn.Close();
        }

        private void btnAddCarrera_alumno_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIdCarrera_Alumno.Text.Trim()) || string.IsNullOrEmpty(txtCarrera_Alumno.Text.Trim()))
            {
                MessageBox.Show("LOS CAMPOS NO PUEDEN ESTAR VACIOS");
            }
            else
            {
                if (btnAddCarrera_alumno.Text=="Agregar")
                {
                    carrera = new Carrera(txtIdCarrera_Alumno.Text, txtCarrera_Alumno.Text);
                    carrera.agregarCarreraBD();
                    gbAgregarCarrera_Alumno.Visible = false;
                    fillCB();
                }
                else
                {
                    carrera = new Carrera(txtIdCarrera_Alumno.Text, txtCarrera_Alumno.Text);
                    carrera.editarCarreraBD(cbCarrera_AlumnoAdd.SelectedItem.ToString());
                    gbAgregarCarrera_Alumno.Visible = false;
                    fillCB();
                }
            }
        }

        private void txtAlumno_Prestamo_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtAlumno_Prestamo.Text) == false)
            {
                SqlCommand cmd;
                SqlDataReader rd;
                cnn.Open();
                if (cbTipo_Prestamo.SelectedIndex == 0)
                {
                    cmd = new SqlCommand("Buscar_Alumno", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@coincidencia", txtAlumno_Prestamo.Text);
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
                    cmd.Parameters.AddWithValue("@coincidencia", txtAlumno_Prestamo.Text);
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
            if (string.IsNullOrEmpty(txtBusqueda_Alumno.Text) == false)
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Buscar_Alumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@coincidencia", txtBusqueda_Alumno.Text);
                SqlDataReader rd = cmd.ExecuteReader();
                dgvAlumnos_Alumno.Rows.Clear();
                while (rd.Read())
                {
                    dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), rd["Nombre"].ToString(), rd["Carrera"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString());

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
            if (string.IsNullOrEmpty(txtBuscar_Personal.Text) == false)
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Buscar_Personal", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@coincidencia", txtBuscar_Personal.Text);
                SqlDataReader rd = cmd.ExecuteReader();
                dgvLista_Personal.Rows.Clear();
                while (rd.Read())
                {
                    dgvLista_Personal.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString(), rd["Ocupacion"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString());
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
            if (dgvAlumnos_Alumno.Rows.Count > 0)
            {
                if (borrarAlumno == true)
                {
                    if (MessageBox.Show("Al borrar el alumno todo su historial de prestamos sera eliminado tambien \n¿Desea continuar?", "Eliminar alumno", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        new Alumno().borrarAlumnoDB(int.Parse(dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString()));
                        fillDGVs();
                    }
                }
                else
                {
                    actualizarAlumno = true;
                    lblActualizar_Alumno.Visible = actualizarAlumno;
                    matriculavieja_Alumno = int.Parse(dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString());
                    txtNoControl_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString();
                    for (int i = 0; i < cbCarrera_AlumnoAdd.Items.Count; i++)
                    {
                        if (dgvAlumnos_Alumno.CurrentRow.Cells[2].Value.ToString() == cbCarrera_AlumnoAdd.Items[i].ToString())
                        {
                            cbCarrera_AlumnoAdd.SelectedIndex = i;
                        }
                    }
                    for (int i = 0; i < cbCuatrimestre_AlumnoAdd.Items.Count; i++)
                    {
                        if (dgvAlumnos_Alumno.CurrentRow.Cells[3].Value.ToString() == cbCuatrimestre_AlumnoAdd.Items[i].ToString())
                        {
                            cbCuatrimestre_AlumnoAdd.SelectedIndex = i;
                        }
                    }
                    txtNombre_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[1].Value.ToString();
                    txtEmail_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[4].Value.ToString();
                    txtTelefono_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[5].Value.ToString();
                }
            }
        }

        //variable para guardar numero de empleado antes de editar
        int numeroDeEmpleadoViejo;
        private void dgvLista_Personal_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLista_Personal.Rows.Count > 0)
            {
                if (borrarPersonal)
                {
                    if (MessageBox.Show("Al borrar el empleado todo su historial de prestamos sera eliminado tambien \n¿Desea continuar?", "Eliminar alumno", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        int numeroDeEmpleado = int.Parse(dgvLista_Personal.CurrentRow.Cells[0].Value.ToString());
                        new Personal().borrarPersonalDB(numeroDeEmpleado);
                        fillDGVs();
                    }
                }
                else
                {
                    numeroDeEmpleadoViejo = int.Parse(dgvLista_Personal.CurrentRow.Cells[0].Value.ToString());
                    actualizarPersonal = true;
                    lblActualizar_Personal.Visible = actualizarPersonal;

                    txtNoEmpleado_Personal.Text = dgvLista_Personal.CurrentRow.Cells[0].Value.ToString();
                    txtNombre_Personal.Text = dgvLista_Personal.CurrentRow.Cells[1].Value.ToString();
                    for (int i = 0; i < cbOcupacion_Personal.Items.Count; i++)
                    {
                        if (dgvLista_Personal.CurrentRow.Cells[2].Value.ToString() == cbOcupacion_Personal.Items[i].ToString())
                        {
                            cbOcupacion_Personal.SelectedIndex = i;
                        }
                    }
                    txtEMail_Personal.Text = dgvLista_Personal.CurrentRow.Cells[3].Value.ToString();
                    txtTelefono_Personal.Text = dgvLista_Personal.CurrentRow.Cells[4].Value.ToString();
                }
            }
        }

        private void btnAgregar_Personal_Click(object sender, EventArgs e)
        {
            bool bl = false;
            try
            {
                if (string.IsNullOrEmpty(txtNoEmpleado_Personal.Text) || string.IsNullOrEmpty(txtNombre_Personal.Text)
                    || cbOcupacion_Personal.SelectedIndex==0)
                {
                    MessageBox.Show("No ha ingresado todos los datos necesarios");
                    agregarPersonal = false;
                    actualizarPersonal = false;
                    bl = true;
                }
                else
                {
                    if (actualizarPersonal == false)
                    {
                        agregarPersonal = true;
                    }
                    try
                    {
                        personal = new Personal(int.Parse(txtNoEmpleado_Personal.Text), txtNombre_Personal.Text.ToUpper().Trim(), cbOcupacion_Personal.SelectedItem.ToString().ToUpper().Trim(), txtEMail_Personal.Text.ToUpper().Trim(), txtTelefono_Personal.Text.ToUpper().Trim());
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Ha ingresado datos de forma incorrecta");
                        bl = true;
                    }
                }
                if (actualizarPersonal && bl == false)
                {
                    personal.actualizarPersonal(numeroDeEmpleadoViejo);
                }
                else if (agregarPersonal && bl == false)
                {
                    personal.agregarPersonalBD();
                }
                if (bl==false)
                {
                    fillDGVs();
                    txtNoEmpleado_Personal.Text = null;
                    txtNombre_Personal.Text = null;
                    cbOcupacion_Personal.SelectedIndex = -1;
                    txtEMail_Personal.Text = null;
                    txtTelefono_Personal.Text = null;
                    actualizarPersonal = false;
                    agregarPersonal = false;
                    lblActualizar_Personal.Visible = actualizarPersonal;
                }
            }
            catch (Exception f)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + f.Message, f.Source);
            }
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
            if (dgvLista_libro.Rows.Count > 0)
            {
                if (borrarLibro)
                {
                    if (MessageBox.Show("Al borrar el libro todo su historial de prestamos sera eliminado tambien \n¿Desea continuar?", "Eliminar alumno", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        new Libro().borrarLibroDB(int.Parse(dgvLista_libro.CurrentRow.Cells[0].Value.ToString()));
                        fillDGVs();
                    }
                }
                else
                {
                    actualizarLibro = true;
                    lblActualizar_Libro.Visible = actualizarLibro;
                    idLibroViejo = dgvLista_libro.CurrentRow.Cells[0].Value.ToString();
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

        private void enviarCorreo()
        {
            try
            {
                if (string.IsNullOrEmpty(para) == false && new Correo().countCorreo() != 0 && new NotificacionesCorreo().countNotificacionesCorreo() != 0)
                {
                    string asunto = txtAsuntoCorreo_Herramientas.Text;
                    string texto = txtCuerpoCorreo_Herramientas.Text;
                    MailMessage mail = new MailMessage();
                    mail.To.Add(new MailAddress(para));
                    mail.From = new MailAddress(txtCorreo.Text);
                    mail.Subject = asunto;
                    mail.Body = texto;
                    mail.IsBodyHtml = false;

                    SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587);
                    using (client)
                    {
                        client.Credentials = new System.Net.NetworkCredential(txtCorreo.Text, txtPassCorreo.Text);
                        client.EnableSsl = true;
                        client.Send(mail);
                    }
                    MessageBox.Show("El mensaje fue enviado correctamente");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR A MANDAR EL CORREO");
            }
        }

        private void bRealizarPrestamo_Prestamo_Click(object sender, EventArgs e)
        {
            try
            {
                if (seleclibro && selecAlumno_Personal)
                {
                    if (cbTipo_Prestamo.SelectedItem.ToString() == "ALUMNO")
                    {
                        prestamo = new Prestamo(txtIdEjemplar_Prestamo.Text.ToUpper(), int.Parse(txtNoControl_Empleado_Prestamo.Text), dtpPrestamo_Prestamo.Value.Date, dtpEntrega_Prestamo.Value.Date, int.Parse(cbDias_Prestamo.SelectedItem.ToString()), txtEstado_Prestamo.Text.ToUpper(), "Alumno");
                    }
                    else
                    {
                        prestamo = new Prestamo(int.Parse(txtNoControl_Empleado_Prestamo.Text), txtIdEjemplar_Prestamo.Text.ToUpper(), dtpPrestamo_Prestamo.Value.Date, dtpEntrega_Prestamo.Value.Date, 0, txtEstado_Prestamo.Text.ToUpper(), "Personal");
                    }
                    prestamo.registrarPrestamo();
                    txtAlumno_Prestamo.Text = null;
                    txtISBN_Prestamo.Text = null;
                    txtLibro_Prestamo.Text = null;
                    txtIdEjemplar_Prestamo.Text = null;
                    txtNombreLibro_Prestamo.Text = null;
                    txtNoControl_Empleado_Prestamo.Text = null;
                    txtEstado_Prestamo.Text = null;
                    selecAlumno_Personal = false;
                    seleclibro = false;
                }
                else
                {
                    MessageBox.Show("Aun no ha seleccionado un libro y un alumno o personal");
                }
            }
            catch (Exception j)
            {
                MessageBox.Show("Ha ocurrido un error.\n" + j.Message, j.Source);
            }
            fillDGVs();
            llenarGrafica();
        }

        private void txtBusqueda_Home_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txtBusqueda_Home.Text) == false)
                {
                    cnn.Open();
                    SqlCommand cmd;
                    SqlDataReader reader;
                    switch (cbFiltroBusqueda_Home.SelectedItem.ToString())
                    {
                        case "TODOS":
                            cmd = new SqlCommand("BuscarPrestamo_Alumno", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);
                            reader = cmd.ExecuteReader();
                            int i = 0;
                            dgvPrestamos_Home.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add();
                                dgvPrestamos_Home.Rows[i].Cells[0].Value = reader[0];
                                dgvPrestamos_Home.Rows[i].Cells["Nombre_"].Value = reader[1];
                                dgvPrestamos_Home.Rows[i].Cells["Nombre_Libro"].Value = reader[2];
                                dgvPrestamos_Home.Rows[i].Cells["ISBN_"].Value = reader[3];
                                dgvPrestamos_Home.Rows[i].Cells["Id_Ejemplar"].Value = reader[4];
                                dgvPrestamos_Home.Rows[i].Cells["Id_Prestamo"].Value = reader[5];
                                dgvPrestamos_Home.Rows[i].Cells["Fecha_De_Entrega"].Value = reader[6].ToString().Remove(10);
                                i++;
                            }
                            reader.Close();

                            cmd = new SqlCommand("BuscarPrestamo_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);

                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add();
                                dgvPrestamos_Home.Rows[i].Cells["NoControl_Matricula"].Value = reader[0];
                                dgvPrestamos_Home.Rows[i].Cells["Nombre_"].Value = reader[1];
                                dgvPrestamos_Home.Rows[i].Cells["Nombre_Libro"].Value = reader[2];
                                dgvPrestamos_Home.Rows[i].Cells["ISBN_"].Value = reader[3];
                                dgvPrestamos_Home.Rows[i].Cells["Id_Ejemplar"].Value = reader[4];
                                dgvPrestamos_Home.Rows[i].Cells["Id_Prestamo"].Value = reader[5];
                                dgvPrestamos_Home.Rows[i].Cells["Fecha_De_Entrega"].Value = reader[6].ToString().Remove(10);
                                i++;
                            }
                            break;
                        case "PERSONAL":

                            cmd = new SqlCommand("BuscarPrestamo_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);
                            reader = cmd.ExecuteReader();
                            int d = 0;
                            dgvPrestamos_Home.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add();
                                dgvPrestamos_Home.Rows[d].Cells["NoControl_Matricula"].Value = reader[0];
                                dgvPrestamos_Home.Rows[d].Cells["Nombre_"].Value = reader[1];
                                dgvPrestamos_Home.Rows[d].Cells["Nombre_Libro"].Value = reader[2];
                                dgvPrestamos_Home.Rows[d].Cells["ISBN_"].Value = reader[3];
                                dgvPrestamos_Home.Rows[d].Cells["Id_Ejemplar"].Value = reader[4];
                                dgvPrestamos_Home.Rows[d].Cells["Id_Prestamo"].Value = reader[5];
                                dgvPrestamos_Home.Rows[d].Cells["Fecha_De_Entrega"].Value = reader[6].ToString().Remove(10);
                                d++;
                            }
                            reader.Close();

                            break;
                        case "ALUMNOS":
                            cmd = new SqlCommand("BuscarPrestamo_Alumno", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);
                            reader = cmd.ExecuteReader();
                            int s = 0;
                            dgvPrestamos_Home.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add();
                                dgvPrestamos_Home.Rows[s].Cells["NoControl_Matricula"].Value = reader[0];
                                dgvPrestamos_Home.Rows[s].Cells["Nombre_"].Value = reader[1];
                                dgvPrestamos_Home.Rows[s].Cells["Nombre_Libro"].Value = reader[2];
                                dgvPrestamos_Home.Rows[s].Cells["ISBN_"].Value = reader[3];
                                dgvPrestamos_Home.Rows[s].Cells["Id_Ejemplar"].Value = reader[4];
                                dgvPrestamos_Home.Rows[s].Cells["Id_Prestamo"].Value = reader[5];
                                dgvPrestamos_Home.Rows[s].Cells["Fecha_De_Entrega"].Value = reader[6].ToString().Remove(10);
                                s++;
                            }
                            reader.Close();
                            break;
                    }
                }
                else
                {
                    fillDGVs();
                }
                cnn.Close();
            }
            catch (Exception j)
            {
                MessageBox.Show("Ha ocurrido un error" + j.Message, j.Source);
            }
            finally
            {
                cnn.Close();
            }
        }
        //variables para capturar el id de prestamo y libro del datagridview en home
        string idPrestamo, idLibro;
        int matricula;

        private void dgvPrestamos_Home_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0 && e.RowIndex<dgvPrestamos_Home.Rows.Count)
            {
                idPrestamo = dgvPrestamos_Home.CurrentRow.Cells[5].Value.ToString();
                idLibro = dgvPrestamos_Home.CurrentRow.Cells[4].Value.ToString();
                matricula = int.Parse(dgvPrestamos_Home.CurrentRow.Cells[0].Value.ToString());
                btnDevolucion_Home.Enabled = true;
            }
        }

        private void mOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }

        private void sALIRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iNFORMACIÓNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000, "BIBLIOTECA", "LA APLICACIÓN SE ENCUENTRA EN SEGUNDO PLANO", ToolTipIcon.Info);
            }
            int tamañoappx = 1061;
            int tamañoappy = 600;
            int tamañoControlx = 0;
            int tamañoControly = 0;
            double tamPorcen = 0;
            double w = 0;
            double h = 0;
            List<Control> ls = new List<Control>() {tabInicio, tabPrestamo,tabAlumno,tabPersonal, tabAnalisis,tabLibro,tabPage2, tabAjustes_2,
                tabAjustes,tabControl1, gbAgregarCarrera_Alumno, gbCorreo_herramientas, gbGrafica_Analisis, gbOcupacion_Personal,gbEliminarCarrera_Alumno };
            foreach (Control item1 in ls)
            {
                foreach (Control item in item1.Controls)
                {
                    tamañoControlx = item.MinimumSize.Width;
                    tamañoControly = item.MinimumSize.Height;
                    tamPorcen = (double)tamañoControlx / (double)tamañoappx;
                    w = tamPorcen * this.Width;
                    tamPorcen = (double)tamañoControly / (double)tamañoappy;
                    h = tamPorcen * this.Height;
                    item.Size = new Size((int)w, (int)h);
                }
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Show();
                WindowState = FormWindowState.Normal;
            }
        }

        private void cbFiltro_Historial_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillDGVs();
        }

        private void txtBusqueda_Historial_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (String.IsNullOrEmpty(txtBusqueda_Historial.Text) == false)
                {
                    cnn.Open();
                    SqlCommand cmd;
                    SqlDataReader reader;
                    switch (cbFiltro_Historial.SelectedItem.ToString())
                    {
                        case "TODOS":
                            cmd = new SqlCommand("BuscarPrestamo_Alumno", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Historial.Text);
                            reader = cmd.ExecuteReader();
                            int i = 0;
                            dgvPrestamos_Historial.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(reader[5].ToString(), reader[0].ToString(), reader[1].ToString(), reader[4].ToString(), reader[2].ToString(), reader[3].ToString(), reader[7].ToString().Remove(10), reader[6].ToString().Remove(10));
                                i++;
                            }
                            reader.Close();

                            cmd = new SqlCommand("BuscarPrestamo_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Historial.Text);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(reader[5].ToString(), reader[0].ToString(), reader[1].ToString(), reader[4].ToString(), reader[2].ToString(), reader[3].ToString(), reader[7].ToString().Remove(10), reader[6].ToString().Remove(10));


                            }
                            break;
                        case "PERSONAL":

                            cmd = new SqlCommand("BuscarPrestamo_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Historial.Text);

                            reader = cmd.ExecuteReader();
                            dgvPrestamos_Historial.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(reader[5].ToString(), reader[0].ToString(), reader[1].ToString(), reader[4].ToString(), reader[2].ToString(), reader[3].ToString(), reader[7].ToString().Remove(10), reader[6].ToString().Remove(10));
                            }
                            reader.Close();

                            break;
                        case "ALUMNOS":
                            cmd = new SqlCommand("BuscarPrestamo_Alumno", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Historial.Text);

                            reader = cmd.ExecuteReader();
                            dgvPrestamos_Historial.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(reader[5].ToString(), reader[0].ToString(), reader[1].ToString(), reader[4].ToString(), reader[2].ToString(), reader[3].ToString(), reader[7].ToString().Remove(10), reader[6].ToString().Remove(10));
                            }
                            reader.Close();
                            break;
                    }
                }
                else
                {
                    fillDGVs();
                }
                cnn.Close();
            }
            catch (Exception j)
            {
                MessageBox.Show("Ha ocurrido un error" + j.Message, j.Source);
            }
            finally
            {
                cnn.Close();
            }
        }

        private void btnDevolucion_Home_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Esta seguro de registrar la devolución.", "DEVOLUCIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    prestamo = new Prestamo();
                    prestamo.registrarDevolucion(idPrestamo, idLibro,  matricula);
                }
            }
            catch (Exception j)
            {
                MessageBox.Show("Ha ocurrido un error" + j.Message, j.Source);
            }
            fillDGVs();

            btnDevolucion_Home.Enabled = false;
        }

        private void cbFiltro_Analisis_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarGrafica();
        }

        private void tabAjustes_2_Click(object sender, EventArgs e)
        {
            llenarGrafica();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            fillCB();
        }

        private void btnBorrar_Alumno_Click(object sender, EventArgs e)
        {
            borrarAlumno = true;
            lblBorrar_Alumno.Visible = borrarAlumno;
        }

        private void btnBorrar_Personal_Click(object sender, EventArgs e)
        {
            borrarPersonal = true;
            lblBorrar_Personal.Visible = borrarPersonal;
        }

        private void chkBorrar_Personal_CheckedChanged(object sender, EventArgs e)
        {
            borrarPersonal = chkBorrar_Personal.Checked;
            lblBorrar_Personal.Visible = borrarPersonal;
            if (borrarPersonal)
            {

                actualizarPersonal = false;
                lblActualizar_Personal.Visible = actualizarPersonal;
                txtNoEmpleado_Personal.Text = null;
                txtNombre_Personal.Text = null;
                cbOcupacion_Personal.SelectedIndex = -1;
                txtEMail_Personal.Text = null;
                txtTelefono_Personal.Text = null;
            }
        }

        private void btnBorrar_Alumno_CheckedChanged(object sender, EventArgs e)
        {
            borrarAlumno = chkBorrar_Personal.Checked;
            lblBorrar_Personal.Visible = borrarPersonal;
        }

        private void chkBorrar_Alumno_CheckedChanged(object sender, EventArgs e)
        {
            borrarAlumno = chkBorrar_Alumno.Checked;
            lblBorrar_Alumno.Visible = borrarAlumno;
            if (borrarAlumno)
            {
                actualizarAlumno = false;
                lblActualizar_Alumno.Visible = actualizarAlumno;
                txtNoControl_AlumnoAdd.Text = null;
                cbCarrera_AlumnoAdd.SelectedIndex = -1;
                cbCuatrimestre_AlumnoAdd.SelectedIndex = -1;
                txtNombre_AlumnoAdd.Text = null;
                txtEmail_AlumnoAdd.Text = null;
                txtTelefono_AlumnoAdd.Text = null;
            }
        }

        private void chkBorrar_Libro_CheckedChanged(object sender, EventArgs e)
        {
            borrarLibro = chkBorrar_Libro.Checked;
            lblBorrar_Libro.Visible = borrarLibro;
            if (borrarLibro)
            {
                actualizarLibro = false;
                lblActualizar_Libro.Visible = actualizarLibro;
                txtId_Libro.Text = null;
                txtIsbn_Libro.Text = null;
                txtTitulo_Libro.Text = null;
                txtAño_Libro.Text = null;
                txtClasificiacion_Libro.Text = null;
                txtAutor_Libro.Text = null;
                txtEditorial_Libro.Text = null;
                txtLugar_Libro.Text = null;
                txtEdicion_Libro.Text = null;
                txtDescripcion_Libro.Text = null;
            }
        }

        private void btnDeleteCarrera_alumno_Click(object sender, EventArgs e)
        {
            carrera = new Carrera(cbEliminarCarrera_Alumno.SelectedItem.ToString());
            carrera.eliminarCarreraBD();
            gbEliminarCarrera_Alumno.Visible = false;
            fillCB();
        }


        private void tabAjustes_2_SelectedIndexChanged(object sender, EventArgs e)
        {          
            if (correo.countCorreo() != 0)
            {
                btnGuardarCorreo_Herramientas.Text = "Actualizar";
                txtCorreo.Text = string.IsNullOrEmpty(correo.VerCorreo()[0]) ? null : correo.VerCorreo()[0];
                txtPassCorreo.Text = string.IsNullOrEmpty(correo.VerCorreo()[1]) ? null : correo.VerCorreo()[1];
            }

            if (nCorreo.countNotificacionesCorreo() != 0)
            {
                button2.Text = "ACTUALIZAR";
                txtAsuntoCorreo_Herramientas.Text = string.IsNullOrEmpty(nCorreo.verNotificacionesCorreo()[0]) == false ? nCorreo.verNotificacionesCorreo()[0] : null;
                txtCuerpoCorreo_Herramientas.Text = string.IsNullOrEmpty(nCorreo.verNotificacionesCorreo()[1]) == false ? nCorreo.verNotificacionesCorreo()[1] : null;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAsuntoCorreo_Herramientas.Text) || string.IsNullOrEmpty(txtCuerpoCorreo_Herramientas.Text))
            {
                MessageBox.Show("Los campos no pueden estar vacios");
            }
            else
            {
                NotificacionesCorreo nCorreo = new NotificacionesCorreo(txtAsuntoCorreo_Herramientas.Text, txtCuerpoCorreo_Herramientas.Text);
                nCorreo.agregarNotificacionesCorreo();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label59_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {


        }

        private void cbCuatrimestre_AlumnoAdd_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbActionsCarrera.SelectedIndex.ToString())
            {
                case "0":
                    gbAgregarCarrera_Alumno.Visible = true;
                    gbEliminarCarrera_Alumno.Visible = false;
                    btnAddCarrera_alumno.Text = "Agregar";
                    break;
                case "1":
                    gbAgregarCarrera_Alumno.Visible = true;
                    gbEliminarCarrera_Alumno.Visible = false;
                    btnAddCarrera_alumno.Text = "Editar";
                    break;
                case "2":
                    gbAgregarCarrera_Alumno.Visible = false;
                    gbEliminarCarrera_Alumno.Visible = true;
                    break;

            }
        }

        private void gbEliminarCarrera_Alumno_Enter(object sender, EventArgs e)
        {

        }

        private void gbAgregarCarrera_Alumno_Enter(object sender, EventArgs e)
        {

        }

        private void llenarGrafica()
        {
            try
            {

                cnn.Open();
                lblMensaje_grafica.Visible = false;
                SqlCommand cmd;
                SqlDataReader rd;
                switch (cbFiltro_Analisis.SelectedItem.ToString())
                {
                    case "LIBROS":

                        cmd = new SqlCommand("librosPrestados_Alumno", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        List<string> titulos1 = new List<string>();
                        List<int> valores1 = new List<int>();

                        while (rd.Read())
                        {
                            titulos1.Add(rd["Titulo"].ToString());
                            valores1.Add(int.Parse(rd["Prestamos"].ToString()));
                        }

                        rd.Close();
                        List<string> titulos2 = new List<string>();
                        List<int> valores2 = new List<int>();
                        cmd = new SqlCommand("librosPrestados_Personal", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            titulos2.Add(rd["Titulo"].ToString());
                            valores2.Add(int.Parse(rd["Prestamos"].ToString()));
                        }
                        rd.Close();

                        for (int i = 0; i < titulos2.Count; i++)
                        {
                            for (int e = 0; e < titulos1.Count; e++)
                            {
                                if (titulos2[i] == titulos1[e])
                                {
                                    valores1[e] = valores1[e] + valores2[i];
                                    titulos2.RemoveAt(i);
                                    valores2.RemoveAt(i);
                                }
                            }
                        }
                        for (int i = 0; i < titulos2.Count; i++)
                        {
                            titulos1.Add(titulos2[i]);
                            valores1.Add(valores2[i]);

                        }
                        col = new ColumnSeries()
                        {
                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                        };
                        ax = new Axis()
                        {
                            Separator = new Separator() { Step = 1, IsEnabled = false }
                        };
                        ax.Labels = new List<string>();
                        col.Values.Clear();
                        ax.Labels.Clear();
                        grafica_Analisis.Series.Clear();
                        grafica_Analisis.AxisX.Clear();
                        grafica_Analisis.AxisY.Clear();
                        for (int i = 0; i < titulos1.Count; i++)
                        {
                            col.Values.Add(valores1[i]);
                            ax.Labels.Add(titulos1[i]);
                        }
                        break;
                    case "CARRERAS":

                        cmd = new SqlCommand("carreraPrestamos", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();

                        col = new ColumnSeries()
                        {
                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                        };
                        ax = new Axis()
                        {
                            Separator = new Separator() { Step = 1, IsEnabled = false }
                        };
                        ax.Labels = new List<string>();
                        col.Values.Clear();
                        ax.Labels.Clear();
                        grafica_Analisis.Series.Clear();
                        grafica_Analisis.AxisX.Clear();
                        grafica_Analisis.AxisY.Clear();
                        List<string> carreras1 = new List<string>();
                        List<int> prestamos1 = new List<int>();


                        while (rd.Read())
                        {
                            prestamos1.Add(int.Parse(rd["Prestamos"].ToString()));

                            carreras1.Add(rd["Carrera"].ToString());
                        }
                        for (int i = 0; i < prestamos1.Count; i++)
                        {
                            for (int e = carreras1.Count - 1; e > i; e--)
                            {
                                if (carreras1[i] == carreras1[e])
                                {
                                    prestamos1[i] += prestamos1[e];
                                    prestamos1.RemoveAt(e);
                                    carreras1.RemoveAt(e);
                                }
                            }
                        }
                        for (int i = 0; i < carreras1.Count; i++)
                        {
                            ax.Labels.Add(carreras1[i]);
                            col.Values.Add(prestamos1[i]);
                        }
                        break;
                    case "PUESTOS":

                        cmd = new SqlCommand("puestosPrestamos", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();

                        col = new ColumnSeries()
                        {
                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                        };
                        ax = new Axis()
                        {
                            Separator = new Separator() { Step = 1, IsEnabled = false }
                        };
                        ax.Labels = new List<string>();
                        col.Values.Clear();
                        ax.Labels.Clear();
                        grafica_Analisis.Series.Clear();
                        grafica_Analisis.AxisX.Clear();
                        grafica_Analisis.AxisY.Clear();

                        List<string> puestos = new List<string>();
                        List<int> prestamosP = new List<int>();

                        while (rd.Read())
                        {
                            prestamosP.Add(int.Parse(rd["Prestamos"].ToString()));

                            puestos.Add(rd["Ocupacion"].ToString());
                        }
                        for (int i = 0; i < prestamosP.Count; i++)
                        {
                            for (int e = puestos.Count - 1; e > i; e--)
                            {
                                if (puestos[i] == puestos[e])
                                {
                                    prestamosP[i] += prestamosP[e];
                                    prestamosP.RemoveAt(e);
                                    puestos.RemoveAt(e);
                                }
                            }
                        }
                        for (int i = 0; i < puestos.Count; i++)
                        {
                            ax.Labels.Add(puestos[i]);
                            col.Values.Add(prestamosP[i]);
                        }

                        break;
                    case "FECHA":

                        cmd = new SqlCommand("fechaPrestamos", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        List<string> fechas = new List<string>();
                        List<int> fechasP = new List<int>();

                        col = new ColumnSeries()
                        {
                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                        };
                        ax = new Axis()
                        {
                            Separator = new Separator() { Step = 1, IsEnabled = false },
                        };
                        ax.Labels = new List<string>();
                        col.Values.Clear();
                        ax.Labels.Clear();
                        grafica_Analisis.Series.Clear();
                        grafica_Analisis.AxisX.Clear();
                        grafica_Analisis.AxisY.Clear();
                        while (rd.Read())
                        {

                            fechas.Add(rd["mes"].ToString() + "/" + rd["año"].ToString());
                            fechasP.Add(int.Parse(rd["Prestamos"].ToString()));

                            // ax.Labels.Add(rd["mes"].ToString() + "/" + rd["año"].ToString());
                            // col.Values.Add(int.Parse( rd["Prestamos"].ToString()));

                        }
                        for (int i = 0; i < fechas.Count; i++)
                        {
                            for (int e = fechas.Count - 1; e > i; e--)
                            {
                                if (fechas[i] == fechas[e])
                                {
                                    fechasP[i] += fechasP[e];
                                    fechas.RemoveAt(e);
                                    fechasP.RemoveAt(e);
                                }
                            }
                        }
                        if (fechas.Count <= 1)
                        {
                            lblMensaje_grafica.Visible = true;
                            lblMensaje_grafica.Text = "SE REQUIEREN REGISTROS DE AL MENOS 2 MESES";
                            ax.Labels.Clear();
                            col.Values.Clear();
                        }
                        else
                        {
                            for (int i = 0; i < fechas.Count; i++)
                            {
                                ax.Labels.Add(fechas[i]);
                                col.Values.Add(fechasP[i]);
                            }
                        }
                        break;
                }

                grafica_Analisis.Series.Add(col);
                grafica_Analisis.AxisX.Add(ax);

                grafica_Analisis.AxisY.Add(new Axis
                {

                    Separator = new Separator()
                }
                );
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
