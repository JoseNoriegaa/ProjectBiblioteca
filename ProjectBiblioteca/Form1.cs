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
using Novacode;
using Microsoft.VisualBasic;
using BIBLIOTECA;

namespace ProjectBiblioteca
{
    public partial class Form1 : Form
    {
        #region variables
        Libro libro;
        Alumno alumno;
        Carrera carrera;
        Personal personal;
        Prestamo prestamo;
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
        LiveCharts.Wpf.Axis ax;
        SqlCommand cmd;
        SqlDataReader rd;
        List<Carrera> listaCarreras = new List<Carrera>();
        List<Carrera> listaCarreras2 = new List<Carrera>();

        #endregion

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
            fillDGVs("todo");
            fillCB();
            rbPrestamo.Checked = true;
            ShowIcon = true;
            llenarGrafica();
            List<Control> ls = new List<Control>() {tabInicio, tabPrestamo,tabAlumno,tabPersonal, tabAnalisis,tabLibro,tabPage2, tabAjustes_2,
                tabAjustes,tabControl1, gbAgregarCarrera_Alumno, gbGrafica_Analisis, gbOcupacion_Personal,gbEliminarCarrera_Alumno };
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

            foreach (DataGridViewRow row in dgvPrestamos_Home.Rows)
            {
                try
                {
                    string s = row.Cells["Fecha_De_Entrega"].Value.ToString();
                    fechas.Add(s);
                }
                catch (Exception)
                {

                }
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
                if (MessageBox.Show("¿DESEA ENVIAR NOTIFICACIONES POR CORREO A LOS PRESTAMOS PENDIENTES PARA HOY?",
                    "NOTIFICACIONES", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    enviarNotificaciones();
                }
            }
        }

        private void cbTipo_Prestamo_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNoControl_Empleado_Prestamo.Text = null;
            txtNombreLibro_Prestamo.Text = null;
            txtISBN_Prestamo.Text = null;
            txtIdEjemplar_Prestamo.Text = null;


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
            fillDGVs("alumnos-personal prestamo");
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
            fillDGVs("prestamos home");
        }

        private void checkPassCorreo_CheckedChanged(object sender, EventArgs e)
        {
            txtPassCorreo.UseSystemPasswordChar = !checkPassCorreo.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string password = "";
            for (int i = 0; i < txtPassCorreo.Text.Length; i++)
            {
                password += "*";
            }
            if (rbPrestamo.Checked)
            {
                if (string.IsNullOrEmpty(txtCorreo.Text) || string.IsNullOrEmpty(txtPassCorreo.Text) || string.IsNullOrEmpty(txtAsuntoCorreo_Herramientas.Text) || string.IsNullOrEmpty(txtCuerpoCorreo_Herramientas.Text))
                {
                    MessageBox.Show("Los campos no pueden estar vacios");
                }
                else
                {
                    if (MessageBox.Show("ESTA SEGURO QUE ESTE ES SU CORREO\nCorreo: " + txtCorreo.Text + "\nContraseña:" + password, "CORREO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Correo correo = new Correo(txtCorreo.Text, txtPassCorreo.Text, txtAsuntoCorreo_Herramientas.Text, txtCuerpoCorreo_Herramientas.Text);
                        correo.agregarCorreo();
                    }
                }
            }
            else
            {
                Correo correo = new Correo(txtAsuntoCorreo_Herramientas.Text, txtCuerpoCorreo_Herramientas.Text);
                correo.adddNotificacionesCorreo();
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
                    MessageBox.Show("Hay campos vacios que son requeridos", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    bl = true;
                }
                else
                {
                    if (!(actualizarLibro))
                    {
                        agregarLibro = true;
                    }
                    try
                    {
                        decimal PRECIO = string.IsNullOrEmpty(txtPrecioLibro.Text) ? 0m : decimal.Parse(txtPrecioLibro.Text);
                        libro = new Libro(txtId_Libro.Text, txtIsbn_Libro.Text, txtTitulo_Libro.Text, int.Parse(txtAño_Libro.Text),
                            txtAutor_Libro.Text, txtClasificiacion_Libro.Text, txtDescripcion_Libro.Text, txtEditorial_Libro.Text,
                            txtLugar_Libro.Text, txtEdicion_Libro.Text, PRECIO);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bl = true;
                    }

                }
                if (actualizarLibro && !(bl))
                {
                    libro.actualizarLibro(idLibroViejo);
                }
                else if (agregarLibro && !(bl))
                {
                    libro.agregarLibroBD();
                }
                if (!(bl))
                {
                    btnAgregar_Libro.Text = "Agregar";
                    txtId_Libro.Text = null;
                    txtPrecioLibro.Text = null;
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
                    fillDGVs("libro");
                    fillDGVs("libro prestamo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void fillDGVs(string dgv)
        {
            try
            {
                SqlCommand cmd;
                SqlDataReader rd;
                cnn.Open();
                #region llenar DGV libro
                if (dgv=="libro" || dgv=="todo")
                {
                    dgvLista_libro.Rows.Clear();

                    cmd = new SqlCommand("BuscarLibros_Todos", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        dgvLista_libro.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd[3].ToString(),
                            rd[4].ToString(), rd[5].ToString(), rd[7].ToString(), rd[8].ToString(), rd[9].ToString(), rd[6].ToString(), rd["Precio"].ToString());
                    }
                    rd.Close();
                }
                #endregion

                #region llenar DGV Alumnos-Personal Prestamo
                if (dgv== "alumnos-personal prestamo" || dgv == "todo")
                {
                    if (cbTipo_Prestamo.SelectedIndex == 0)
                    {
                        cmd = new SqlCommand("Mostrar_Alumnos_SinLibro", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();
                        dgvListaAlumno_Prestamo.Rows.Clear();
                        while (rd.Read())
                        {
                            dgvListaAlumno_Prestamo.Rows.Add(rd["Matricula"].ToString(), $"{ rd["Nombre"].ToString()} { rd["Apellido"].ToString()}", rd["Correo"].ToString());
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
                }
                #endregion

                #region llenar dgv Libro prestamo
                if (dgv== "libro prestamo" || dgv == "todo")
                {
                    cmd = new SqlCommand("BuscarLibros_NoPrestados", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    dgvListaLibros_Prestamo.Rows.Clear();
                    while (rd.Read())
                    {
                        dgvListaLibros_Prestamo.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString());
                    }
                    rd.Close();
                }

                #endregion

                #region llenar lista alumnos
                if (dgv== "alumno" || dgv == "todo")
                {
                    cmd = new SqlCommand("Mostrar_Alumnos", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    dgvAlumnos_Alumno.Rows.Clear();
                    while (rd.Read())
                    {
                        dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), $"{rd["Nombre"].ToString()} { rd["Apellido"].ToString()}", rd["Carrera"].ToString(),
                            rd["cuatrimestre"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString(), rd["Nombre"].ToString(), rd["Apellido"].ToString());
                    }
                    rd.Close();
                }
                #endregion

                #region llenar lista Personal
                if (dgv== "personal" || dgv == "todo")
                {
                    cmd = new SqlCommand("Mostrar_Personal", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    rd = cmd.ExecuteReader();
                    dgvLista_Personal.Rows.Clear();
                    while (rd.Read())
                    {
                        dgvLista_Personal.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString(), rd["Ocupacion"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString());
                    }
                    rd.Close();
                }

                #endregion

                #region llenar dgv prestamos home
                if (dgv== "prestamos home" || dgv == "todo")
                {
                    switch (cbFiltroBusqueda_Home.SelectedItem.ToString())
                    {
                        case "TODOS":
                            cmd = new SqlCommand("MostrarPrestamos_Alumno", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            rd = cmd.ExecuteReader();
                            dgvPrestamos_Home.Rows.Clear();
                            while (rd.Read())
                            {
                                dgvPrestamos_Home.Rows.Add(rd[1].ToString(), $"{rd[2].ToString()} {rd[3].ToString()}",
                                    rd[5].ToString(), rd[6].ToString(), rd[4].ToString(), rd[0].ToString(), rd[7].ToString().Remove(10));
                            }
                            rd.Close();
                            cmd = new SqlCommand("MostrarPrestamos_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            rd = cmd.ExecuteReader();
                            while (rd.Read())
                            {
                                dgvPrestamos_Home.Rows.Add(rd[1].ToString(), rd[2].ToString(), rd[4].ToString(), rd[5].ToString(), rd[3].ToString(), rd[0].ToString());
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
                                dgvPrestamos_Home.Rows.Add(rd[1].ToString(), $"{rd[2].ToString()} {rd[3].ToString()}", rd[4].ToString(), rd[5].ToString(), rd[6].ToString(), rd[0].ToString(), rd[7].ToString().Remove(10));
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
                                dgvPrestamos_Home.Rows.Add(rd[1].ToString(), rd[2].ToString(), rd[4].ToString(), rd[5].ToString(), rd[3].ToString(), rd[0].ToString(), rd[5].ToString());
                            }
                            rd.Close();
                            break;
                    }
                }
                if (dgvPrestamos_Home.Rows.Count == 0)
                {
                    bEliminarHistorial_herramientas.Enabled = true;
                }
                else
                {
                    bEliminarHistorial_herramientas.Enabled = false;
                }
                #endregion

                #region llenar dgv historial Herramientas
                if (dgv== "historial" || dgv == "todo")
                {
                    switch (cbFiltro_Historial.SelectedIndex)
                    {
                        case 0:
                            cmd = new SqlCommand("historialPrestamos_Alumno", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            rd = cmd.ExecuteReader();
                            dgvPrestamos_Historial.Rows.Clear();
                            while (rd.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(rd[0].ToString(), rd[1].ToString(), $"{rd[2].ToString()} {rd[3].ToString()}", rd[4].ToString(), rd[5].ToString(), rd[6].ToString(), rd[8].ToString().Remove(10), rd[7].ToString().Remove(10));
                            }
                            rd.Close();
                            cmd = new SqlCommand("historialPrestamos_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            rd = cmd.ExecuteReader();
                            while (rd.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(rd["Id_Prestamo"].ToString(), rd["Personal"].ToString(), rd["Nombre"].ToString(), rd["Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd["Fecha_Prestamo"].ToString().Remove(10));
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
                                dgvPrestamos_Historial.Rows.Add(rd[0].ToString(), rd[1].ToString(), $"{rd[2].ToString()} {rd[3].ToString()}", rd[4].ToString(), rd[5].ToString(), rd[6].ToString(), rd[8].ToString().Remove(10), rd[7].ToString().Remove(10));
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
                                dgvPrestamos_Historial.Rows.Add(rd["Id_Prestamo"].ToString(), rd["Personal"].ToString(), rd["Nombre"].ToString(), rd["Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(), rd["Fecha_Prestamo"].ToString().Remove(10));
                            }
                            rd.Close();
                            break;
                    }
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
            if (e.RowIndex < dgvListaLibros_Prestamo.Rows.Count && e.RowIndex >= 0)
            {
                txtNombreLibro_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[1].Value.ToString();
                txtISBN_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[2].Value.ToString();
                txtIdEjemplar_Prestamo.Text = dgvListaLibros_Prestamo.CurrentRow.Cells[0].Value.ToString();
                seleclibro = true;
            }
        }

        private void dgvListaAlumno_Prestamo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < dgvListaAlumno_Prestamo.Rows.Count && e.RowIndex >= 0)
            {
                txtNoControl_Empleado_Prestamo.Text = dgvListaAlumno_Prestamo.CurrentRow.Cells[0].Value.ToString();
                para = dgvListaAlumno_Prestamo.CurrentRow.Cells[2].Value.ToString();
                selecAlumno_Personal = true;
            }
        }
        
        private void btnAdd_Alumno_Click(object sender, EventArgs e)
        {
            bool bl = false;
            if (string.IsNullOrEmpty(txtNoControl_AlumnoAdd.Text.Trim()) || string.IsNullOrEmpty(txtNombre_AlumnoAdd.Text.Trim()) ||
                string.IsNullOrEmpty(txtApellido_Alumno.Text.Trim()))
            {
                MessageBox.Show("Campo no llenado");
                actualizarAlumno = false;
                agregarAlumno = false;
                bl = true;
            }
            else
            {
                if (!(actualizarAlumno))
                {
                    agregarAlumno = true;
                    try
                    {
                        alumno = new Alumno(int.Parse(txtNoControl_AlumnoAdd.Text), txtNombre_AlumnoAdd.Text, txtApellido_Alumno.Text, txtTelefono_AlumnoAdd.Text,
                        txtEmail_AlumnoAdd.Text, strCarrera, int.Parse(cbCuatrimestre_AlumnoAdd.SelectedItem.ToString()));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        alumno = new Alumno(int.Parse(txtNoControl_AlumnoAdd.Text), txtNombre_AlumnoAdd.Text, txtApellido_Alumno.Text, txtTelefono_AlumnoAdd.Text,
                            txtEmail_AlumnoAdd.Text, strCarrera, int.Parse(cbCuatrimestre_AlumnoAdd.SelectedItem.ToString()));
                        if (agregarAlumno && alumno.verificarAlumnoRegistrado(matriculavieja_Alumno))
                        {
                            bl = true;
                        }
                        else
                        {
                            bl = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bl = true;
                    }
                }

            }
            if (actualizarAlumno && !(bl))
            {
                alumno.actualizarAlumno(matriculavieja_Alumno);
            }
            else if (agregarAlumno && !(bl))
            {
                alumno.agregarAlumnoBD();
            }
            if (!(bl))
            {
                txtNoControl_AlumnoAdd.Text = null;
                txtNombre_AlumnoAdd.Text = null;
                txtTelefono_AlumnoAdd.Text = null;
                txtEmail_AlumnoAdd.Text = null;
                txtApellido_Alumno.Text = null;
                cbCuatrimestre_AlumnoAdd.SelectedIndex = -1;
                cbCarrera_AlumnoAdd.SelectedIndex = -1;
                fillDGVs("alumno");
                fillDGVs("alumnos - personal prestamo");
                actualizarAlumno = false;
                lblActualizar_Alumno.Visible = actualizarAlumno;
                btnAdd_Alumno.Text = "Agregar";
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
            listaCarreras.Clear();
            while (rd.Read())
            {
                listaCarreras.Add(new Carrera(rd[0].ToString(), rd[1].ToString()));
                cbCarrera_AlumnoAdd.Items.Add(rd[1].ToString());
                cbEliminarCarrera_Alumno.Items.Add(rd[1].ToString());
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
            if (string.IsNullOrEmpty(txtIdCarrera_Alumno.Text.Trim()) || string.IsNullOrEmpty(txtCarrera_Alumno.Text.Trim()))
            {
                MessageBox.Show("LOS CAMPOS NO PUEDEN ESTAR VACIOS");
            }
            else
            {
                if (btnAddCarrera_alumno.Text == "Agregar")
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
            gbAgregarCarrera_Alumno.Visible = false;
            gbEliminarCarrera_Alumno.Visible = false;
        }

        private void txtAlumno_Prestamo_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(txtAlumno_Prestamo.Text)))
            {
                SqlCommand cmd;
                SqlDataReader rd;
                cnn.Open();
                if (cbTipo_Prestamo.SelectedIndex == 0)
                {
                    cmd = new SqlCommand("Buscar_Alumno_sinLibro", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@coincidencia", txtAlumno_Prestamo.Text);
                    rd = cmd.ExecuteReader();
                    dgvListaAlumno_Prestamo.Rows.Clear();
                    while (rd.Read())
                    {
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Matricula"].ToString(), $"{ rd["Nombre"].ToString()} { rd["Apellido"].ToString()}", rd["Correo"].ToString());
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
                        dgvListaAlumno_Prestamo.Rows.Add(rd["Numero_De_Empleado"].ToString(), rd["Nombre"].ToString(), rd["Correo"].ToString());
                    }
                }
                cnn.Close();
            }
            else
            {
                fillDGVs("alumnos-personal prestamo");
            }
        }

        private void txtLibro_Prestamo_TextChanged(object sender, EventArgs e)
        {
            if (!(String.IsNullOrEmpty(txtLibro_Prestamo.Text)))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("BusquedaLibros_NoPrestados", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@coincidencia", txtLibro_Prestamo.Text);

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
                fillDGVs("libro prestamo"); 
            }
        }

        private void txtBusqueda_Libro_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtBusqueda_Libro.Text))
            {
                fillDGVs("libro");

            }
            else
            {
                if (txtBusqueda_Libro.Text == "SETLIBRO90")
                {
                    commandLine = txtBusqueda_Libro.Text;
                    CommandLine();
                }
            }
        }

        private void BusquedaLibro()
        {
            if (!(String.IsNullOrEmpty(txtBusqueda_Libro.Text)))
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
                    dgvLista_libro.Rows.Add(rd["Id_Libro"].ToString(), rd["Titulo"].ToString(), rd["ISBN"].ToString(),
                        rd["Año"].ToString(), rd["Clasificacion"].ToString(), rd["Autor"].ToString(), rd["Editorial"].ToString(),
                        rd["Lugar"].ToString(), rd["Edicion"].ToString(), rd["Descripcion"].ToString(), rd["Precio"].ToString());
                }
                cnn.Close();
            }
            else
            {
                fillDGVs("libro");
            }
        }

        private void CommandLine()
        {
            string message = null;
            string command = null;
            List<string> listaPersonas = new List<string>();
            bool hayPresamos=false;
            bool puedesBorrar = true;
            switch (commandLine)
            {
                case "SETALUMNO90":
                    message = "¿SEGURO QUE DESEA ELIMINAR TODOS LOS ALUMNOS?\nESTO TAMBIEN BORRARA TODO EL HISTORIAL DE PRÉSTAMOS DE ALUMNOS";
                    cbFiltroBusqueda_Home.SelectedIndex = 1;
                    for (int i = 0; i < dgvPrestamos_Home.RowCount; i++)
                    {
                        listaPersonas.Add(dgvPrestamos_Home.Rows[i].Cells[0].Value.ToString());
                    }
                    command = "ALUMNO";
                    break;
                case "SETPERSONAL90":
                    message = "¿SEGURO QUE DESEA ELIMINAR TODO EL PERSONAL?\nESTO TAMBIEN BORRARA TODO EL HISTORIAL DE PRÉSTAMOS DE PERSONAL";
                    cbFiltroBusqueda_Home.SelectedIndex = 2;
                    for (int i = 0; i < dgvPrestamos_Home.RowCount; i++)
                    {
                        listaPersonas.Add(dgvPrestamos_Home.Rows[i].Cells[0].Value.ToString());
                    }
                    command = "PERSONAL";
                    break;
                case "SETLIBRO90":
                    message = "¿SEGURO QUE DESEA ELIMINAR TODOS LOS LIBROS?\nESTO TAMBIEN BORRARA TODO EL HISTORIAL DE PRÉSTAMOS";
                    cbFiltroBusqueda_Home.SelectedIndex = 0;
                    if (dgvPrestamos_Home.RowCount!=0)
                    {
                        hayPresamos=true;
                    }
                    command = "LIBRO";
                    break;
            }

            if (MessageBox.Show(message, "ELIMINACIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                for (int i = 0; i < listaPersonas.Count; i++)
                {
                    if (command=="PERSONAL")
                    {
                        if (new Personal().verificarLibroPersonal(int.Parse(listaPersonas[i])))
                        {
                            MessageBox.Show("NO PUEDE ELIMINAR TODO EL PERSONAL DEBIDO A QUE HAY PERSONAL QUE CUENTA CON LIBROS EN ESTE MOMENTO", "OPERACIÓN CANCELADA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            i = listaPersonas.Count;
                            puedesBorrar = false;
                            continue;
                        }
                    }
                    else
                    {
                        if (new Alumno().verificarLibroEstadoAlumno(int.Parse(listaPersonas[i])))
                        {
                            MessageBox.Show("NO PUEDE ELIMINAR TODOS LOS ALUMNOS DEBIDO A QUE HAY ALUMNOS QUE CUENTAN CON LIBROS EN ESTE MOMENTO", "OPERACIÓN CANCELADA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            puedesBorrar = false;
                            i = listaPersonas.Count;
                        }
                    }
                }
                if (hayPresamos)
                {
                    MessageBox.Show("NO PUEDE ELIMINAR TODOS LOS LIBROS DEBIDO A QUE HAY LIBROS PRESTADOS EN ESTE MOMENTO", "OPERACIÓN CANCELADA", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                if (puedesBorrar && !(hayPresamos))
                {
                    cnn.Open();
                    cmd = new SqlCommand("COMMAND", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Command", command);
                    cmd.ExecuteNonQuery();
                    cnn.Close();
                }      
            }

            commandLine = null;
            txtBusqueda_Alumno.Text = null;
            txtBusqueda_Libro.Text = null;
            txtBuscar_Personal.Text = null;
            cbFiltroBusqueda_Home.SelectedIndex = 0;
            fillDGVs("todo");
        }

        string commandLine;
        private void txtBusqueda_Alumno_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBusqueda_Alumno.Text))
            {
                fillDGVs("alumno");
            }
            else
            {
                if (txtBusqueda_Alumno.Text == "SETALUMNO90")
                {
                    commandLine = txtBusqueda_Alumno.Text;
                    CommandLine();
                }
            }
        }

        private void BusquedaAlumno()
        {
            if (!(string.IsNullOrEmpty(txtBusqueda_Alumno.Text)))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("Buscar_Alumno", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@coincidencia", txtBusqueda_Alumno.Text);
                SqlDataReader rd = cmd.ExecuteReader();
                dgvAlumnos_Alumno.Rows.Clear();
                while (rd.Read())
                {
                    dgvAlumnos_Alumno.Rows.Add(rd["Matricula"].ToString(), $"{rd["Nombre"].ToString()} {rd["Apellido"].ToString()}", rd["Carrera"].ToString(), rd["Cuatrimestre"].ToString(), rd["Correo"].ToString(), rd["Telefono"].ToString()
                        , rd["Nombre"].ToString(), rd["Apellido"].ToString());
                }
                cnn.Close();
            }
            else
            {
                fillDGVs("alumno");
            }
           
        }

        private void txtBuscar_Personal_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBuscar_Personal.Text))
            {
                fillDGVs("personal");
            }
            else
            {
                if (txtBuscar_Personal.Text == "SETPERSONAL90")
                {
                    commandLine = txtBuscar_Personal.Text;
                    CommandLine();
                }
            }
        }

        private void BusquedaPersonal()
        {
            if (!(string.IsNullOrEmpty(txtBuscar_Personal.Text)))
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
                fillDGVs("personal");
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
                        if (new Alumno().verificarLibroEstadoAlumno(int.Parse(dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString())))
                        {
                            MessageBox.Show("NO PUEDE ELIMINAR ALUMNOS QUE CUENTAN CON LIBROS EN ESTE MOMENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            new Alumno().borrarAlumnoDB(int.Parse(dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString()));
                            fillDGVs("alumno");
                            fillDGVs("alumnos-personal prestamo");
                        }
                    }
                }
                else
                {
                    actualizarAlumno = true;
                    lblActualizar_Alumno.Visible = actualizarAlumno;
                    btnAdd_Alumno.Text = "Actualizar";
                    matriculavieja_Alumno = int.Parse(dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString());
                    txtNoControl_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[0].Value.ToString();
                    for (int i = 0; i < cbCuatrimestre_AlumnoAdd.Items.Count; i++)
                    {
                        if (dgvAlumnos_Alumno.CurrentRow.Cells[3].Value.ToString() == cbCuatrimestre_AlumnoAdd.Items[i].ToString())
                        {
                            cbCuatrimestre_AlumnoAdd.SelectedIndex = i;
                        }
                    }
                    for (int i = 0; i < listaCarreras.Count; i++)
                    {
                        if (dgvAlumnos_Alumno.CurrentRow.Cells[2].Value.ToString() == listaCarreras[i].IdCarrera)
                        {
                            cbCarrera_AlumnoAdd.SelectedItem = listaCarreras[i].nombreCarrera;
                        }
                    }
                    txtNombre_AlumnoAdd.Text = dgvAlumnos_Alumno.CurrentRow.Cells[6].Value.ToString();
                    txtApellido_Alumno.Text = dgvAlumnos_Alumno.CurrentRow.Cells[7].Value.ToString();
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
                        if (personal.verificarLibroPersonal(numeroDeEmpleado))
                        {
                            MessageBox.Show("NO PUEDE ELIMINAR PERSONAL QUE CUENTAN CON LIBROS EN ESTE MOMENTO", "", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        }
                        else
                        {
                            new Personal().borrarPersonalDB(numeroDeEmpleado);
                            fillDGVs("personal");
                            fillDGVs("alumnos - personal prestamo");

                        }

                    }
                }
                else
                {
                    numeroDeEmpleadoViejo = int.Parse(dgvLista_Personal.CurrentRow.Cells[0].Value.ToString());
                    actualizarPersonal = true;
                    lblActualizar_Personal.Visible = actualizarPersonal;
                    btnAgregar_Personal.Text = "Actualizar";
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
                if (string.IsNullOrEmpty(txtNoEmpleado_Personal.Text) || string.IsNullOrEmpty(txtNombre_Personal.Text))
                {
                    MessageBox.Show("No ha ingresado todos los datos necesarios");
                    agregarPersonal = false;
                    actualizarPersonal = false;
                    bl = true;
                }
                else
                {
                    if (!(actualizarPersonal))
                    {
                        agregarPersonal = true;
                    }
                    try
                    {
                        personal = new Personal(int.Parse(txtNoEmpleado_Personal.Text), txtNombre_Personal.Text.Trim(), cbOcupacion_Personal.SelectedItem.ToString().Trim(), txtEMail_Personal.Text.Trim(), txtTelefono_Personal.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bl = true;
                    }
                }
                if (actualizarPersonal && !(bl))
                {
                    personal.actualizarPersonal(numeroDeEmpleadoViejo);
                }
                else if (agregarPersonal && !(bl))
                {
                    personal.agregarPersonalBD();
                }
                if (!(bl))
                {
                    fillDGVs("personal");
                    fillDGVs("alumnos - personal prestamo");
                    txtNoEmpleado_Personal.Text = null;
                    txtNombre_Personal.Text = null;
                    cbOcupacion_Personal.SelectedIndex = -1;
                    txtEMail_Personal.Text = null;
                    txtTelefono_Personal.Text = null;
                    actualizarPersonal = false;
                    agregarPersonal = false;
                    lblActualizar_Personal.Visible = actualizarPersonal;
                    btnAgregar_Personal.Text = "Agregar";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOcupacion_Personal_Click(object sender, EventArgs e)
        {
            if (btnOcupacion_Personal.Text == "Agregar")
            {
                if (!(string.IsNullOrEmpty(txtOcupacion_Personal.Text.Trim())))
                {
                    new Puesto(txtOcupacion_Personal.Text).agregarOcupacion();
                    fillCB();
                }
                else
                {
                    MessageBox.Show("EL CAMPO NO PUEDE ESTAR VACIO", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                if (!(string.IsNullOrEmpty(txtOcupacion_Personal.Text.Trim())) && cbOcupacion_Personal.SelectedIndex >= 0)
                {
                    new Puesto(txtOcupacion_Personal.Text).editarPuestoBD(cbOcupacion_Personal.SelectedItem.ToString());
                }
                else
                {
                    MessageBox.Show("EL CAMPO NO PUEDE ESTAR VACIO Y DEBE DE SELECCIONAR UN ELEMENTO A EDITAR", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            txtOcupacion_Personal.Text = null;
            btnOcupacion_Personal.Text = "Agregar";
            gbOcupacion_Personal.Visible = false;
        }

        private void dgvLista_libro_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvLista_libro.Rows.Count > 0)
            {
                if (borrarLibro)
                {
                    if (MessageBox.Show("Al borrar el libro todo su historial de prestamos sera eliminado tambien \n¿Desea continuar?", "Eliminar Libro", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        new Libro().borrarLibroDB(dgvLista_libro.CurrentRow.Cells[0].Value.ToString());
                        fillDGVs("libro");
                        fillDGVs("libro prestamo");
                    }
                }
                else
                {
                    actualizarLibro = true;
                    btnAgregar_Libro.Text = "Actualizar";
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
                    txtPrecioLibro.Text = dgvLista_libro.CurrentRow.Cells[10].Value.ToString();
                }
            }
        }

        private void enviarCorreo(string subject, string body)
        {
            try
            {
                if (!(string.IsNullOrEmpty(para)) && new Correo().completo(new Correo().VerCorreo()))
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(new MailAddress(para));
                    mail.From = new MailAddress(new Correo().VerCorreo().correo);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;

                    SmtpClient client = new SmtpClient(" outlook.office365.co­m", 587);
                    using (client)
                    {
                        client.Credentials = new System.Net.NetworkCredential(new Correo().VerCorreo().correo, new Correo().VerCorreo().Password);
                        client.EnableSsl = true;
                        if (MessageBox.Show("¿DESEA ENVIAR CORREO?", "CORREO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            client.Send(mail);
                            MessageBox.Show("El mensaje fue enviado correctamente", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR A MANDAR EL CORREO\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        prestamo = new Prestamo(txtIdEjemplar_Prestamo.Text, int.Parse(txtNoControl_Empleado_Prestamo.Text),dtpPrestamo_Prestamo.Value.Date, dtpEntrega_Prestamo.Value.Date, int.Parse(cbDias_Prestamo.SelectedItem.ToString()), txtEstado_Prestamo.Text, "Alumno");

                    }
                    else
                    {
                        prestamo = new Prestamo(int.Parse(txtNoControl_Empleado_Prestamo.Text), txtIdEjemplar_Prestamo.Text, dtpPrestamo_Prestamo.Value.Date, 0, txtEstado_Prestamo.Text, "Personal");
                    }
                    
                    if (MessageBox.Show("¿Desea crear el recibo?", "RECIBO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (cbTipo_Prestamo.SelectedItem.ToString() == "ALUMNO")
                        {
                            try
                            {
                                cnn.Open();
                                cmd = new SqlCommand("Buscar_Alumno", cnn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Coincidencia", txtNoControl_Empleado_Prestamo.Text);
                                rd = cmd.ExecuteReader();
                                rd.Read();
                                string nombre = rd["Nombre"].ToString();
                                string telefono = rd["Telefono"].ToString();
                                string grupo = rd["Carrera"].ToString();
                                int cuatrimestre = int.Parse(rd["cuatrimestre"].ToString());
                                string correo = rd["Correo"].ToString();
                                para = correo;
                                rd.Close();
                                cmd = new SqlCommand("buscarLibro_ID", cnn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id", txtIdEjemplar_Prestamo.Text);
                                rd = cmd.ExecuteReader();
                                rd.Read();
                                string titulo = rd["Titulo"].ToString();
                                rd.Close();
                                prestamo.generarRecibolumno(nombre, telefono, titulo, correo, txtIdEjemplar_Prestamo.Text, grupo, cuatrimestre, 3, int.Parse(txtNoControl_Empleado_Prestamo.Text), dtpPrestamo_Prestamo.Value.Date, dtpEntrega_Prestamo.Value.Date);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("HA OCURRIDO UN ERROR\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally { cnn.Close(); }
                        }
                        else
                        {
                            try
                            {
                                cnn.Open();
                                cmd = new SqlCommand("Buscar_Personal", cnn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@coincidencia", txtNoControl_Empleado_Prestamo.Text);
                                rd = cmd.ExecuteReader();
                                rd.Read();
                                string nombre = rd["Nombre"].ToString();
                                string telefono = rd["Telefono"].ToString();
                                string ocupacion = rd["Ocupacion"].ToString();
                                string correo = rd["Correo"].ToString();
                                para = correo;
                                rd.Close();
                                cmd = new SqlCommand("buscarLibro_ID", cnn);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@id", txtIdEjemplar_Prestamo.Text);
                                rd = cmd.ExecuteReader();
                                rd.Read();
                                string titulo = rd["Titulo"].ToString();
                                rd.Close();
                                prestamo.generarReciboPersonal(nombre, telefono, titulo, correo, txtIdEjemplar_Prestamo.Text, int.Parse(txtNoControl_Empleado_Prestamo.Text), dtpPrestamo_Prestamo.Value.Date, ocupacion);

                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            finally
                            {
                                cnn.Close();
                            }
                        }
                    }
                    
                    prestamo.registrarPrestamo();
                    fillDGVs("libro prestamo");
                    fillDGVs("prestamos home");
                    fillDGVs("historial");
                    fillDGVs("alumnos-personal prestamo");
                    string subject = new Correo().VerCorreo().Asunto;
                    string body = new Correo().VerCorreo().Cuerpo;
                    enviarCorreo(subject, body);
                    para = null;
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
                    MessageBox.Show("Aun no ha seleccionado un libro y un alumno o personal", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            llenarGrafica();
        }

        private void txtBusqueda_Home_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!(String.IsNullOrEmpty(txtBusqueda_Home.Text)))
                {
                    cnn.Open();
                    SqlCommand cmd;
                    SqlDataReader reader;
                    switch (cbFiltroBusqueda_Home.SelectedItem.ToString())
                    {
                        case "TODOS":
                            cmd = new SqlCommand("BuscarPrestamo_Alumno_NoDevuelto", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);
                            reader = cmd.ExecuteReader();
                            dgvPrestamos_Home.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add(reader[0].ToString(), $"{reader[1].ToString()} {reader[2].ToString()}",reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString().Remove(10));
                            }
                            reader.Close();

                            cmd = new SqlCommand("BuscarPrestamo_Personal_NoDevuelto", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);

                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString().Remove(10));
                            }
                            break;
                        case "PERSONAL":

                            cmd = new SqlCommand("BuscarPrestamo_Personal_NoDevuelto", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);
                            reader = cmd.ExecuteReader();
                            dgvPrestamos_Home.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add(reader[0].ToString(), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(), reader[4].ToString(), reader[5].ToString().Remove(10));
                            }
                            reader.Close();

                            break;
                        case "ALUMNOS":
                            cmd = new SqlCommand("BuscarPrestamo_Alumno_NoDevuelto", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Home.Text);
                            reader = cmd.ExecuteReader();
                            dgvPrestamos_Home.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Home.Rows.Add(reader[0].ToString(), $"{reader[1].ToString()} {reader[2].ToString()}", reader[3].ToString(), reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString().Remove(10));
                            }
                            reader.Close();
                            break;
                    }
                }
                else
                {
                    fillDGVs("prestamos home");
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }
        }
        //variables para capturar el id de prestamo y libro del datagridview en home
        string idLibro;
        int matricula, idPrestamo;

        private void dgvPrestamos_Home_CellClick(object sender, DataGridViewCellEventArgs e)
        {//System.NullReferenceException
            idPrestamo = int.Parse(dgvPrestamos_Home.CurrentRow.Cells[5].Value.ToString());
            idLibro = dgvPrestamos_Home.CurrentRow.Cells[4].Value.ToString();
            matricula = int.Parse(dgvPrestamos_Home.CurrentRow.Cells[0].Value.ToString());
            btnDevolucion_Home.Enabled = true;
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
                tabAjustes,tabControl1, gbAgregarCarrera_Alumno, gbGrafica_Analisis, gbOcupacion_Personal,gbEliminarCarrera_Alumno };
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
            fillDGVs("historial");
        }

        private void txtBusqueda_Historial_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (!(String.IsNullOrEmpty(txtBusqueda_Historial.Text)))
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
                            dgvPrestamos_Historial.Rows.Clear();
                            while (reader.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(reader[0].ToString(), reader[1].ToString(), $"{reader[2].ToString()} {reader[3].ToString()}", reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(), reader[8].ToString());
                            }
                            reader.Close();

                            cmd = new SqlCommand("BuscarPrestamo_Personal", cnn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Coincidencia", txtBusqueda_Historial.Text);
                            reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                dgvPrestamos_Historial.Rows.Add(reader["Id_Prestamo"].ToString(), reader["Personal"].ToString(), reader["Nombre"].ToString(), reader["Libro"].ToString(), reader["Titulo"].ToString(), reader["ISBN"].ToString(), reader["Fecha_Prestamo"].ToString());
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
                                dgvPrestamos_Historial.Rows.Add(reader["Id_Prestamo"].ToString(), reader["Personal"].ToString(), reader["Nombre"].ToString(), reader["Libro"].ToString(), reader["Titulo"].ToString(), reader["ISBN"].ToString(), reader["Fecha_Prestamo"].ToString());
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
                                dgvPrestamos_Historial.Rows.Add(reader[0].ToString(), reader[1].ToString(), $"{reader[2].ToString()} {reader[3].ToString()}", reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), reader[7].ToString(),reader[8].ToString());
                            }
                            reader.Close();
                            break;
                    }
                }
                else
                {
                    fillDGVs("historial");
                }
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }
        }

        private void RegistrarDevolucion()
        {
            try
            {
                if (MessageBox.Show("¿Esta seguro de registrar la devolución?", "DEVOLUCIÓN", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    prestamo = new Prestamo();
                    prestamo.registrarDevolucion(idPrestamo, idLibro, matricula);
                }
                fillDGVs("prestamos home");
                fillDGVs("alumnos-personal prestamo");
                fillDGVs("libro prestamo");
                btnDevolucion_Home.Enabled = false;
                idPrestamo = 0;
                idLibro = null;
                matricula = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDevolucion_Home_Click(object sender, EventArgs e)
        {
            RegistrarDevolucion();
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

        private void txtIsbn_Libro_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string isbn = txtIsbn_Libro.Text;
                int hueco = 0;

                List<string> ls = new Libro().buscarLibro(isbn);
                if (ls.Count > 0)
                {
                    string menor = null;
                    for (int i = 1; i < ls.Count; i++)
                    {
                        menor = ls[i - 1].Remove(0, isbn.Length + 1);
                        if (int.Parse(menor) != int.Parse(ls[i].Remove(0, isbn.Length + 1)) - 1)
                        {
                            hueco = int.Parse(menor) + 1;
                            i = ls.Count;
                        }
                    }
                }
                if (hueco != 0)
                {
                    txtId_Libro.Text = isbn + "_" + hueco;
                }
                else
                {
                    int n = new Libro().countISBN(isbn);
                    if (n != 0 && !(actualizarLibro))
                    {
                        txtId_Libro.Text = isbn + "_" + (n + 1);
                    }
                    else
                    {
                        txtId_Libro.Text = isbn + "_1";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dtpEntrega_Prestamo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpEntrega_Prestamo.Value < dtpPrestamo_Prestamo.Value)
            {
                dtpEntrega_Prestamo.Value = dtpPrestamo_Prestamo.Value.AddDays(3);
            }
        }

        private void txtIsbn_Libro_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (char.IsNumber(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            enviarNotificaciones();
        }

        private void enviarNotificaciones()
        {
            List<string> fechas = new List<string>();
            List<string> nombres = new List<string>();
            List<string> correos = new List<string>();
            List<string> titulos = new List<string>();
            List<string> ids = new List<string>();

            for (int i = 0; i < dgvPrestamos_Home.Rows.Count; i++)
            {
                if (dgvPrestamos_Home.Rows[i].Cells["NoControl_Matricula"].Value.ToString().Length == 8 && dgvPrestamos_Home.Rows[i].Cells["Fecha_De_Entrega"].Value.ToString() == DateTime.Today.Date.ToString().Remove(10))
                {
                    ids.Add(dgvPrestamos_Home.Rows[i].Cells["NoControl_Matricula"].Value.ToString().Trim());
                    fechas.Add(dgvPrestamos_Home.Rows[i].Cells["Fecha_De_Entrega"].Value.ToString().Trim());
                    titulos.Add(dgvPrestamos_Home.Rows[i].Cells["Nombre_Libro"].Value.ToString().Trim());
                    nombres.Add(dgvPrestamos_Home.Rows[i].Cells["Nombre_"].Value.ToString().Trim());
                }
            }
            if (fechas.Count > 0)
            {
                for (int j = 0; j < ids.Count; j++)
                {
                    for (int i = 0; i < dgvAlumnos_Alumno.RowCount; i++)
                    {
                        if (dgvAlumnos_Alumno.Rows[i].Cells[0].Value.ToString() == ids[j] &&
                            !(string.IsNullOrEmpty(dgvAlumnos_Alumno.Rows[i].Cells["MailAlumno"].Value.ToString())))
                        {
                            correos.Add(dgvAlumnos_Alumno.Rows[i].Cells["MailAlumno"].Value.ToString());
                        }
                    }
                }
                para = null;
                for (int i = 0; i < correos.Count; i++)
                {
                    //Para que puedan cambiarlo
                    string subject = correo.VerCorreo().AsuntoNotificacion;
                    string body = correo.VerCorreo().CuerpoNotificacion;
                    para = correos[i];
                    //----Fijo
                  /*subject = "NOTIFICACIÓN BIBLIOTECA UTPP";
                    body = $"ALUMNO(A) {nombres[i]} SE LE DA AVISO DE QUE EL DÍA DE HOY {DateTime.Today.ToShortDateString()} " +
                        $"ES LA FECHA LÍMITE PARA DEVOLVER EL LIBRO '{titulos[i]}' PRESTADO POR LA BIBLIOTECA ESCOLAR DE LA UNIVERSIDAD TECNOLÓGICA DE PUERTO PEÑASCO.\n" +
                        $"SE LE PIDE PASAR A ENTREGAR EL LIBRO O BIEN RENOVAR SU PRÉSTAMO.\n" +
                        $"POR SU ATENCIÓN, GRACIAS.";*/
                    enviarCorreo(subject, body);
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            verificarFechaPrestamo();
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
                txtApellido_Alumno.Text = null;
                txtEmail_AlumnoAdd.Text = null;
                txtTelefono_AlumnoAdd.Text = null;
                btnAdd_Alumno.Text = "Agregar";
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
                txtPrecioLibro.Text = null;
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
            if (correo.completo(new Correo().VerCorreo()))
            {
                button1.Text = "Actualizar";
                txtCorreo.Text = string.IsNullOrEmpty(correo.VerCorreo().correo) ? null : correo.VerCorreo().correo;
                txtPassCorreo.Text = string.IsNullOrEmpty(correo.VerCorreo().Password) ? null : correo.VerCorreo().Password;
            }
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
            txtIdCarrera_Alumno.Text = null;
            txtCarrera_Alumno.Text = null;
        }
        string strCarrera=null;
        private void cbCarrera_AlumnoAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbAgregarCarrera_Alumno.Visible = false;
            gbEliminarCarrera_Alumno.Visible = false;
            string selected = cbCarrera_AlumnoAdd.SelectedItem.ToString();
            for (int i = 0; i < listaCarreras.Count; i++)
            {
                if (listaCarreras[i].nombreCarrera==selected)
                {
                    strCarrera = listaCarreras[i].IdCarrera;
                }
            }
        }

        private void btnEliminarOcupacion_Click(object sender, EventArgs e)
        {
            if (cbEliminarOcupacion.SelectedIndex >= 0)
            {
                new Puesto(cbEliminarOcupacion.SelectedItem.ToString()).eliminarPuestoBD();
                fillCB();
                gbEliminarOcupacion.Visible = false;
            }
            else
            {
                MessageBox.Show("SELECCIONE UN ELEMENTO", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cbActionOcupacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbActionOcupacion.SelectedIndex)
            {
                case 0:
                    gbOcupacion_Personal.Visible = true;
                    gbEliminarOcupacion.Visible = false;
                    btnOcupacion_Personal.Text = "Agregar";

                    break;
                case 1:
                    if (cbOcupacion_Personal.SelectedIndex != 0)
                    {
                        gbOcupacion_Personal.Visible = true;
                        gbEliminarOcupacion.Visible = false;
                        txtOcupacion_Personal.Text = cbOcupacion_Personal.SelectedItem.ToString();
                        btnOcupacion_Personal.Text = "Actualizar";
                    }
                    else
                    {
                        MessageBox.Show("SELECCIONE UN ELEMENTO EN OCUPACION", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    break;
                case 2:
                    gbEliminarOcupacion.Visible = true;
                    gbOcupacion_Personal.Visible = false;

                    break;
            }
        }

        private void cbOcupacion_Personal_SelectedIndexChanged(object sender, EventArgs e)
        {
            gbOcupacion_Personal.Visible = false;
            gbEliminarOcupacion.Visible = false;

        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            if (char.IsNumber(e.KeyChar) || e.KeyChar == (char)Keys.Back)
            {
                e.Handled = false;
            }
        }

        private void dgvPrestamos_Historial_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("¿Desea crear el recibo?", "RECIBO", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool b = false;
                try
                {
                    string.IsNullOrEmpty(dgvPrestamos_Historial.CurrentRow.Cells[7].Value.ToString());
                    b = true;
                }
                catch (Exception)
                {
                    b = false;
                }
                if (b)
                    try
                    {
                        cnn.Open();
                        cmd = new SqlCommand("Buscar_Alumno", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Coincidencia", dgvPrestamos_Historial.CurrentRow.Cells[1].Value.ToString());
                        rd = cmd.ExecuteReader();
                        rd.Read();
                        string nombre = rd["Nombre"].ToString();
                        string telefono = rd["Telefono"].ToString();
                        string grupo = rd["Carrera"].ToString();
                        int cuatrimestre = int.Parse(rd["cuatrimestre"].ToString());
                        string correo = rd["Correo"].ToString();
                        rd.Close();
                        cmd = new SqlCommand("buscarLibro_ID", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", dgvPrestamos_Historial.CurrentRow.Cells[3].Value.ToString());
                        rd = cmd.ExecuteReader();
                        rd.Read();
                        string titulo = rd["Titulo"].ToString();
                        rd.Close();
                        new Prestamo().generarRecibolumno(nombre, telefono, titulo, correo, dgvPrestamos_Historial.CurrentRow.Cells[3].Value.ToString(),
                            grupo, cuatrimestre, 3, int.Parse(dgvPrestamos_Historial.CurrentRow.Cells[1].Value.ToString()),
                            DateTime.Parse(dgvPrestamos_Historial.CurrentRow.Cells[6].Value.ToString()), DateTime.Parse(dgvPrestamos_Historial.CurrentRow.Cells[7].Value.ToString()));

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HA OCURRIDO UN ERROR\n" + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        cnn.Close();
                    }

                else
                {
                    try
                    {
                        cnn.Open();
                        string numeroEmpleado = dgvPrestamos_Historial.CurrentRow.Cells[1].Value.ToString();
                        cmd = new SqlCommand("Buscar_Personal", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@coincidencia", numeroEmpleado);
                        rd = cmd.ExecuteReader();
                        rd.Read();
                        string nombre = rd["Nombre"].ToString();
                        string telefono = rd["Telefono"].ToString();
                        string ocupacion = rd["Ocupacion"].ToString();
                        string correo = rd["Correo"].ToString();
                        rd.Close();
                        cmd = new SqlCommand("buscarLibro_ID", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", dgvPrestamos_Historial.CurrentRow.Cells[3].Value.ToString());
                        rd = cmd.ExecuteReader();
                        rd.Read();
                        string titulo = rd["Titulo"].ToString();
                        rd.Close();
                        string codigo = dgvPrestamos_Historial.CurrentRow.Cells[3].Value.ToString();
                        DateTime fechaPrestamo = DateTime.Parse(dgvPrestamos_Historial.CurrentRow.Cells[6].Value.ToString());
                        new Prestamo().generarReciboPersonal(nombre, telefono, titulo, correo, codigo,
                            int.Parse(numeroEmpleado), fechaPrestamo, ocupacion);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally { cnn.Close(); }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿ESTA SEGURO QUE DESEA ELIMINAR EL HISTORIAL POR COMPLETO?","ELIMINAR HISTORIAL",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation)==DialogResult.Yes)
            {
                new Prestamo().BorrarHistorial();
                fillDGVs("historial");
                fillDGVs("prestamos home");
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            verificarFechaPrestamo();
        }

        private void dgvPrestamos_Home_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            RegistrarDevolucion();
        }

        private void cbCuatrimestre_AlumnoAdd_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbCarrera_AlumnoAdd.Items.Clear();
            if (cbCuatrimestre_AlumnoAdd.SelectedIndex > 5)
            {
                for (int i = 0; i < listaCarreras.Count; i++)
                {
                    if (listaCarreras[i].nombreCarrera.Contains("INGENIERÍA") || listaCarreras[i].nombreCarrera.Contains("LICENCIATURA"))
                    {
                        cbCarrera_AlumnoAdd.Items.Add(listaCarreras[i].nombreCarrera);
                    }
                }
            }
            else
            {
                for (int i = 0; i < listaCarreras.Count; i++)
                {
                    if (!(listaCarreras[i].nombreCarrera.Contains("INGENIERÍA")) && !(listaCarreras[i].nombreCarrera.Contains("LICENCIATURA")))
                    {
                        cbCarrera_AlumnoAdd.Items.Add(listaCarreras[i].nombreCarrera);
                    }
                }
            }
        }

        private void btnbuscarAlumno_Click(object sender, EventArgs e)
        {
            BusquedaAlumno();
        }

        private void btnBusquedaPersonal_Click(object sender, EventArgs e)
        {
            BusquedaPersonal();
        }

        private void btnBusquedaLibro_Click(object sender, EventArgs e)
        {
            BusquedaLibro();
        }

        private void txtBusqueda_Libro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar==(char)Keys.Enter)
            {
                BusquedaLibro();
            }
        }

        private void txtBuscar_Personal_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BusquedaPersonal();
            }
        }

        private void txtBusqueda_Alumno_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                BusquedaAlumno();
            }
        }

        private void rbNotificacion_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtAsuntoCorreo_Herramientas.Text = new Correo().VerCorreo().AsuntoNotificacion;
                txtCuerpoCorreo_Herramientas.Text = new Correo().VerCorreo().CuerpoNotificacion;
            }
            catch (Exception)
            {
                txtAsuntoCorreo_Herramientas.Text = null;
                txtCuerpoCorreo_Herramientas.Text = null;
            }
        }

        private void rbPrestamo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                txtAsuntoCorreo_Herramientas.Text = new Correo().VerCorreo().Asunto;
                txtCuerpoCorreo_Herramientas.Text = new Correo().VerCorreo().Cuerpo;
            }
            catch (Exception)
            {
                txtAsuntoCorreo_Herramientas.Text = null;
                txtCuerpoCorreo_Herramientas.Text = null;
            }
        }

        private void llenarGrafica()
        {
            try
            {

                cnn.Open();
                lblMensaje_grafica.Visible = false;
                SqlCommand cmd;
                SqlDataReader rd;
                switch (cbFiltro_Analisis.SelectedItem)
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
                            titulos2.Add(rd[0].ToString());
                            valores2.Add(int.Parse(rd["Prestamos"].ToString()));
                        }
                        rd.Close();

                        for (int i = 0; i < titulos2.Count; i++)
                        {
                            for (int e = 0; e < ((titulos1.Count) - 1); e++)
                            {
                                if (titulos2[i] == titulos1[e])
                                {
                                    valores1[e] = valores1[e] + valores2[i];
                                    titulos2.RemoveAt(i);
                                    valores2.RemoveAt(i);
                                    if (titulos2.Count == 0)
                                    {
                                        i = titulos2.Count;
                                        break;
                                    }
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
                            Title = "Cantidad:",
                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                            
                        };
                        ax = new LiveCharts.Wpf.Axis()
                        {
                            Separator = new Separator() { Step = 1, IsEnabled = false }
                        };
                        ax.Labels = new List<string>();
                        col.Values.Clear();
                        ax.Labels.Clear();
                        grafica_Analisis.Series.Clear();
                        grafica_Analisis.AxisX.Clear();
                        grafica_Analisis.AxisY.Clear();

                        List<ChartData> ch = new List<ChartData>();


                        for (int i = 0; i < titulos1.Count; i++)
                        {
                            for (int e = i+1; e < titulos1.Count; e++)
                            {
                                if (titulos1[i]==titulos1[e])
                                {
                                    valores1[i] += valores1[e];
                                    valores1.RemoveAt(e);
                                    titulos1.RemoveAt(e);
                                }
                            }
                        }


                        for (int i = 0; i < titulos1.Count; i++)
                        {
                            ch.Add(new ChartData(titulos1[i], valores1[i]));
                        }

                        var lista = new ChartData().ordenar(ch);
                        if (lista.Count < 7)
                        {
                            foreach (var item in lista)
                            {
                                ax.Labels.Add(item.AxName);
                                col.Values.Add(item.ColumValue);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                ax.Labels.Add(lista[i].AxName);
                                col.Values.Add(lista[i].ColumValue);
                            }
                        }
                        break;
                    case "CARRERAS":

                        cmd = new SqlCommand("carreraPrestamos", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();

                        col = new ColumnSeries()
                        {
                            Title = "Cantidad:",

                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                            
                        };
                        ax = new LiveCharts.Wpf.Axis()
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
                        var CarrLst = new List<ChartData>();
                        for (int i = 0; i < carreras1.Count; i++)
                        {
                            CarrLst.Add(new ChartData(carreras1[i], prestamos1[i]));
                        }

                        var listaC = new ChartData().ordenar(CarrLst);
                        if (listaC.Count < 7)
                        {
                            foreach (var item in listaC)
                            {
                                ax.Labels.Add(item.AxName);
                                col.Values.Add(item.ColumValue);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                ax.Labels.Add(listaC[i].AxName);
                                col.Values.Add(listaC[i].ColumValue);
                            }
                        }
                        break;
                    case "PUESTOS":

                        cmd = new SqlCommand("puestosPrestamos", cnn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        rd = cmd.ExecuteReader();

                        col = new ColumnSeries()
                        {
                            Title = "Cantidad:",

                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                        };
                        ax = new LiveCharts.Wpf.Axis()
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
                        var PstLst = new List<ChartData>();
                        for (int i = 0; i < puestos.Count; i++)
                        {
                            PstLst.Add(new ChartData(puestos[i], prestamosP[i]));
                        }

                        var listaP = new ChartData().ordenar(PstLst);
                        if (listaP.Count < 7)
                        {
                            foreach (var item in listaP)
                            {
                                ax.Labels.Add(item.AxName);
                                col.Values.Add(item.ColumValue);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                ax.Labels.Add(listaP[i].AxName);
                                col.Values.Add(listaP[i].ColumValue);
                            }
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
                            Title = "Cantidad:",


                            DataLabels = true,
                            Values = new ChartValues<int>(),
                            LabelPoint = point => point.Y.ToString(),
                        };
                        ax = new LiveCharts.Wpf.Axis()
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
                            var FecLst = new List<ChartData>();
                            for (int i = 0; i < fechas.Count; i++)
                            {
                                FecLst.Add(new ChartData(fechas[i], fechasP[i]));
                            }

                            var listaF = new ChartData().ordenar(FecLst);
                            if (listaF.Count < 7)
                            {
                                foreach (var item in listaF)
                                {
                                    ax.Labels.Add(item.AxName);
                                    col.Values.Add(item.ColumValue);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < 7; i++)
                                {
                                    ax.Labels.Add(listaF[i].AxName);
                                    col.Values.Add(listaF[i].ColumValue);
                                }
                            }
                        }
                        break;
                }

                grafica_Analisis.Series.Add(col);
                grafica_Analisis.AxisX.Add(ax);

                grafica_Analisis.AxisY.Add(new LiveCharts.Wpf.Axis
                {

                    Separator = new Separator()
                }
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR.\n" + ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }
        }
    }
}
