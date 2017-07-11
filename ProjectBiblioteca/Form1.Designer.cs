namespace ProjectBiblioteca
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInicio = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.cbFiltroBusqueda_Home = new System.Windows.Forms.ComboBox();
            this.btnBuscar_Home = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBusqueda_Home = new System.Windows.Forms.TextBox();
            this.dgvPrestamos_Home = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabAlumno = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.btnBuscar_Alumno = new System.Windows.Forms.Button();
            this.txtBusqueda_Alumno = new System.Windows.Forms.TextBox();
            this.dgvAlumnos_Alumno = new System.Windows.Forms.DataGridView();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAdd_Alumno = new System.Windows.Forms.Button();
            this.txtTelefono_AlumnoAdd = new System.Windows.Forms.TextBox();
            this.txtEmail_AlumnoAdd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtNoControl_AlumnoAdd = new System.Windows.Forms.TextBox();
            this.cbCarrera_AlumnoAdd = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtNombre_AlumnoAdd = new System.Windows.Forms.TextBox();
            this.cbCuatrimestre_AlumnoAdd = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtApellido_AlumnoAdd = new System.Windows.Forms.TextBox();
            this.tabPersonal = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtNoEmpleado_Personal = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtNombre_Personal = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.cbOcupacion_Personal = new System.Windows.Forms.ComboBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtEMail_Personal = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtTelefono_Personal = new System.Windows.Forms.TextBox();
            this.btnAgregar_Personal = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.dgvLista_Personal = new System.Windows.Forms.DataGridView();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtBuscar_Personal = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.btnBuscar_Personal = new System.Windows.Forms.Button();
            this.tabPrestamo = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabInicio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos_Home)).BeginInit();
            this.tabAlumno.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos_Alumno)).BeginInit();
            this.tabPersonal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista_Personal)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "BIBLIOTECA";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabInicio);
            this.tabControl1.Controls.Add(this.tabAlumno);
            this.tabControl1.Controls.Add(this.tabPersonal);
            this.tabControl1.Controls.Add(this.tabPrestamo);
            this.tabControl1.Location = new System.Drawing.Point(1, 35);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(784, 527);
            this.tabControl1.TabIndex = 1;
            // 
            // tabInicio
            // 
            this.tabInicio.Controls.Add(this.label3);
            this.tabInicio.Controls.Add(this.cbFiltroBusqueda_Home);
            this.tabInicio.Controls.Add(this.btnBuscar_Home);
            this.tabInicio.Controls.Add(this.label2);
            this.tabInicio.Controls.Add(this.txtBusqueda_Home);
            this.tabInicio.Controls.Add(this.dgvPrestamos_Home);
            this.tabInicio.Location = new System.Drawing.Point(4, 22);
            this.tabInicio.Name = "tabInicio";
            this.tabInicio.Padding = new System.Windows.Forms.Padding(3);
            this.tabInicio.Size = new System.Drawing.Size(776, 501);
            this.tabInicio.TabIndex = 0;
            this.tabInicio.Text = "INICIO";
            this.tabInicio.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Filtrar  prestamos por:";
            // 
            // cbFiltroBusqueda_Home
            // 
            this.cbFiltroBusqueda_Home.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltroBusqueda_Home.FormattingEnabled = true;
            this.cbFiltroBusqueda_Home.Items.AddRange(new object[] {
            "TODOS",
            "ALUMNOS",
            "PERSONAL"});
            this.cbFiltroBusqueda_Home.Location = new System.Drawing.Point(21, 55);
            this.cbFiltroBusqueda_Home.Name = "cbFiltroBusqueda_Home";
            this.cbFiltroBusqueda_Home.Size = new System.Drawing.Size(121, 21);
            this.cbFiltroBusqueda_Home.TabIndex = 4;
            // 
            // btnBuscar_Home
            // 
            this.btnBuscar_Home.Location = new System.Drawing.Point(315, 110);
            this.btnBuscar_Home.Name = "btnBuscar_Home";
            this.btnBuscar_Home.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar_Home.TabIndex = 3;
            this.btnBuscar_Home.Text = "Buscar";
            this.btnBuscar_Home.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "BÚSQUEDA DE PRESTAMO";
            // 
            // txtBusqueda_Home
            // 
            this.txtBusqueda_Home.Location = new System.Drawing.Point(21, 110);
            this.txtBusqueda_Home.Name = "txtBusqueda_Home";
            this.txtBusqueda_Home.Size = new System.Drawing.Size(265, 20);
            this.txtBusqueda_Home.TabIndex = 1;
            // 
            // dgvPrestamos_Home
            // 
            this.dgvPrestamos_Home.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPrestamos_Home.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPrestamos_Home.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgvPrestamos_Home.Location = new System.Drawing.Point(21, 151);
            this.dgvPrestamos_Home.Name = "dgvPrestamos_Home";
            this.dgvPrestamos_Home.Size = new System.Drawing.Size(733, 315);
            this.dgvPrestamos_Home.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Nombre";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Nombre del libro";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "ISBN";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "ID Ejemplar";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "ID Prestamo";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Fecha De Entrega";
            this.Column6.Name = "Column6";
            // 
            // tabAlumno
            // 
            this.tabAlumno.Controls.Add(this.label13);
            this.tabAlumno.Controls.Add(this.btnBuscar_Alumno);
            this.tabAlumno.Controls.Add(this.txtBusqueda_Alumno);
            this.tabAlumno.Controls.Add(this.dgvAlumnos_Alumno);
            this.tabAlumno.Controls.Add(this.label12);
            this.tabAlumno.Controls.Add(this.btnAdd_Alumno);
            this.tabAlumno.Controls.Add(this.txtTelefono_AlumnoAdd);
            this.tabAlumno.Controls.Add(this.txtEmail_AlumnoAdd);
            this.tabAlumno.Controls.Add(this.label4);
            this.tabAlumno.Controls.Add(this.label11);
            this.tabAlumno.Controls.Add(this.label5);
            this.tabAlumno.Controls.Add(this.label10);
            this.tabAlumno.Controls.Add(this.txtNoControl_AlumnoAdd);
            this.tabAlumno.Controls.Add(this.cbCarrera_AlumnoAdd);
            this.tabAlumno.Controls.Add(this.label6);
            this.tabAlumno.Controls.Add(this.label9);
            this.tabAlumno.Controls.Add(this.txtNombre_AlumnoAdd);
            this.tabAlumno.Controls.Add(this.cbCuatrimestre_AlumnoAdd);
            this.tabAlumno.Controls.Add(this.label7);
            this.tabAlumno.Controls.Add(this.label8);
            this.tabAlumno.Controls.Add(this.txtApellido_AlumnoAdd);
            this.tabAlumno.Location = new System.Drawing.Point(4, 22);
            this.tabAlumno.Name = "tabAlumno";
            this.tabAlumno.Padding = new System.Windows.Forms.Padding(3);
            this.tabAlumno.Size = new System.Drawing.Size(776, 501);
            this.tabAlumno.TabIndex = 1;
            this.tabAlumno.Text = "ALUMNO";
            this.tabAlumno.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(422, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(67, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "BÚSQUEDA";
            // 
            // btnBuscar_Alumno
            // 
            this.btnBuscar_Alumno.Location = new System.Drawing.Point(624, 82);
            this.btnBuscar_Alumno.Name = "btnBuscar_Alumno";
            this.btnBuscar_Alumno.Size = new System.Drawing.Size(58, 23);
            this.btnBuscar_Alumno.TabIndex = 19;
            this.btnBuscar_Alumno.Text = "Buscar";
            this.btnBuscar_Alumno.UseVisualStyleBackColor = true;
            // 
            // txtBusqueda_Alumno
            // 
            this.txtBusqueda_Alumno.Location = new System.Drawing.Point(424, 84);
            this.txtBusqueda_Alumno.Name = "txtBusqueda_Alumno";
            this.txtBusqueda_Alumno.Size = new System.Drawing.Size(182, 20);
            this.txtBusqueda_Alumno.TabIndex = 18;
            // 
            // dgvAlumnos_Alumno
            // 
            this.dgvAlumnos_Alumno.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAlumnos_Alumno.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAlumnos_Alumno.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column7,
            this.Column8,
            this.Column9});
            this.dgvAlumnos_Alumno.Location = new System.Drawing.Point(425, 107);
            this.dgvAlumnos_Alumno.Name = "dgvAlumnos_Alumno";
            this.dgvAlumnos_Alumno.Size = new System.Drawing.Size(342, 371);
            this.dgvAlumnos_Alumno.TabIndex = 17;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Matricula";
            this.Column7.Name = "Column7";
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Nombre";
            this.Column8.Name = "Column8";
            // 
            // Column9
            // 
            this.Column9.HeaderText = "Carrera";
            this.Column9.Name = "Column9";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(421, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(185, 24);
            this.label12.TabIndex = 16;
            this.label12.Text = "LISTA ALUMNOS";
            // 
            // btnAdd_Alumno
            // 
            this.btnAdd_Alumno.Location = new System.Drawing.Point(105, 373);
            this.btnAdd_Alumno.Name = "btnAdd_Alumno";
            this.btnAdd_Alumno.Size = new System.Drawing.Size(145, 28);
            this.btnAdd_Alumno.TabIndex = 15;
            this.btnAdd_Alumno.Text = "Agregar";
            this.btnAdd_Alumno.UseVisualStyleBackColor = true;
            // 
            // txtTelefono_AlumnoAdd
            // 
            this.txtTelefono_AlumnoAdd.Location = new System.Drawing.Point(67, 332);
            this.txtTelefono_AlumnoAdd.Name = "txtTelefono_AlumnoAdd";
            this.txtTelefono_AlumnoAdd.Size = new System.Drawing.Size(183, 20);
            this.txtTelefono_AlumnoAdd.TabIndex = 14;
            // 
            // txtEmail_AlumnoAdd
            // 
            this.txtEmail_AlumnoAdd.Location = new System.Drawing.Point(56, 292);
            this.txtEmail_AlumnoAdd.Name = "txtEmail_AlumnoAdd";
            this.txtEmail_AlumnoAdd.Size = new System.Drawing.Size(194, 20);
            this.txtEmail_AlumnoAdd.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(186, 24);
            this.label4.TabIndex = 0;
            this.label4.Text = "NUEVO ALUMNO";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 332);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Telefono";
            this.label11.Click += new System.EventHandler(this.label11_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.Location = new System.Drawing.Point(14, 107);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "No. Control";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 296);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(36, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "E-Mail";
            // 
            // txtNoControl_AlumnoAdd
            // 
            this.txtNoControl_AlumnoAdd.Location = new System.Drawing.Point(81, 107);
            this.txtNoControl_AlumnoAdd.Name = "txtNoControl_AlumnoAdd";
            this.txtNoControl_AlumnoAdd.Size = new System.Drawing.Size(169, 20);
            this.txtNoControl_AlumnoAdd.TabIndex = 2;
            // 
            // cbCarrera_AlumnoAdd
            // 
            this.cbCarrera_AlumnoAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCarrera_AlumnoAdd.FormattingEnabled = true;
            this.cbCarrera_AlumnoAdd.Location = new System.Drawing.Point(67, 254);
            this.cbCarrera_AlumnoAdd.Name = "cbCarrera_AlumnoAdd";
            this.cbCarrera_AlumnoAdd.Size = new System.Drawing.Size(183, 21);
            this.cbCarrera_AlumnoAdd.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(44, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Nombre";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 258);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 9;
            this.label9.Text = "Carrera";
            // 
            // txtNombre_AlumnoAdd
            // 
            this.txtNombre_AlumnoAdd.Location = new System.Drawing.Point(67, 144);
            this.txtNombre_AlumnoAdd.Name = "txtNombre_AlumnoAdd";
            this.txtNombre_AlumnoAdd.Size = new System.Drawing.Size(183, 20);
            this.txtNombre_AlumnoAdd.TabIndex = 4;
            this.txtNombre_AlumnoAdd.TextChanged += new System.EventHandler(this.txtNombre_AlumnoAdd_TextChanged);
            // 
            // cbCuatrimestre_AlumnoAdd
            // 
            this.cbCuatrimestre_AlumnoAdd.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCuatrimestre_AlumnoAdd.FormattingEnabled = true;
            this.cbCuatrimestre_AlumnoAdd.Location = new System.Drawing.Point(88, 217);
            this.cbCuatrimestre_AlumnoAdd.Name = "cbCuatrimestre_AlumnoAdd";
            this.cbCuatrimestre_AlumnoAdd.Size = new System.Drawing.Size(162, 21);
            this.cbCuatrimestre_AlumnoAdd.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 183);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Apellido";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 220);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Cuatrimestre";
            // 
            // txtApellido_AlumnoAdd
            // 
            this.txtApellido_AlumnoAdd.Location = new System.Drawing.Point(67, 181);
            this.txtApellido_AlumnoAdd.Name = "txtApellido_AlumnoAdd";
            this.txtApellido_AlumnoAdd.Size = new System.Drawing.Size(183, 20);
            this.txtApellido_AlumnoAdd.TabIndex = 6;
            // 
            // tabPersonal
            // 
            this.tabPersonal.Controls.Add(this.btnBuscar_Personal);
            this.tabPersonal.Controls.Add(this.label21);
            this.tabPersonal.Controls.Add(this.txtBuscar_Personal);
            this.tabPersonal.Controls.Add(this.dgvLista_Personal);
            this.tabPersonal.Controls.Add(this.label20);
            this.tabPersonal.Controls.Add(this.btnAgregar_Personal);
            this.tabPersonal.Controls.Add(this.txtTelefono_Personal);
            this.tabPersonal.Controls.Add(this.label19);
            this.tabPersonal.Controls.Add(this.txtEMail_Personal);
            this.tabPersonal.Controls.Add(this.label18);
            this.tabPersonal.Controls.Add(this.cbOcupacion_Personal);
            this.tabPersonal.Controls.Add(this.label17);
            this.tabPersonal.Controls.Add(this.txtNombre_Personal);
            this.tabPersonal.Controls.Add(this.label16);
            this.tabPersonal.Controls.Add(this.txtNoEmpleado_Personal);
            this.tabPersonal.Controls.Add(this.label15);
            this.tabPersonal.Controls.Add(this.label14);
            this.tabPersonal.Location = new System.Drawing.Point(4, 22);
            this.tabPersonal.Name = "tabPersonal";
            this.tabPersonal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPersonal.Size = new System.Drawing.Size(776, 501);
            this.tabPersonal.TabIndex = 2;
            this.tabPersonal.Text = "PERSONAL";
            this.tabPersonal.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(7, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(204, 24);
            this.label14.TabIndex = 1;
            this.label14.Text = "NUEVO REGISTRO";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 73);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(135, 13);
            this.label15.TabIndex = 2;
            this.label15.Text = "NUMERO DE EMPLEADO";
            // 
            // txtNoEmpleado_Personal
            // 
            this.txtNoEmpleado_Personal.Location = new System.Drawing.Point(11, 90);
            this.txtNoEmpleado_Personal.Name = "txtNoEmpleado_Personal";
            this.txtNoEmpleado_Personal.Size = new System.Drawing.Size(243, 20);
            this.txtNoEmpleado_Personal.TabIndex = 3;
            this.txtNoEmpleado_Personal.Text = " ";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 127);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(116, 13);
            this.label16.TabIndex = 4;
            this.label16.Text = "NOMBRE COMPLETO";
            // 
            // txtNombre_Personal
            // 
            this.txtNombre_Personal.Location = new System.Drawing.Point(11, 144);
            this.txtNombre_Personal.Name = "txtNombre_Personal";
            this.txtNombre_Personal.Size = new System.Drawing.Size(243, 20);
            this.txtNombre_Personal.TabIndex = 5;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(11, 186);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 13);
            this.label17.TabIndex = 6;
            this.label17.Text = "OCUPACIÓN";
            // 
            // cbOcupacion_Personal
            // 
            this.cbOcupacion_Personal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOcupacion_Personal.FormattingEnabled = true;
            this.cbOcupacion_Personal.Location = new System.Drawing.Point(14, 203);
            this.cbOcupacion_Personal.Name = "cbOcupacion_Personal";
            this.cbOcupacion_Personal.Size = new System.Drawing.Size(121, 21);
            this.cbOcupacion_Personal.TabIndex = 7;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(11, 248);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(42, 13);
            this.label18.TabIndex = 8;
            this.label18.Text = "E-MAIL";
            // 
            // txtEMail_Personal
            // 
            this.txtEMail_Personal.Location = new System.Drawing.Point(6, 275);
            this.txtEMail_Personal.Name = "txtEMail_Personal";
            this.txtEMail_Personal.Size = new System.Drawing.Size(243, 20);
            this.txtEMail_Personal.TabIndex = 9;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(14, 322);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(64, 13);
            this.label19.TabIndex = 10;
            this.label19.Text = "TELEFONO";
            // 
            // txtTelefono_Personal
            // 
            this.txtTelefono_Personal.Location = new System.Drawing.Point(14, 339);
            this.txtTelefono_Personal.Name = "txtTelefono_Personal";
            this.txtTelefono_Personal.Size = new System.Drawing.Size(240, 20);
            this.txtTelefono_Personal.TabIndex = 11;
            // 
            // btnAgregar_Personal
            // 
            this.btnAgregar_Personal.Location = new System.Drawing.Point(146, 391);
            this.btnAgregar_Personal.Name = "btnAgregar_Personal";
            this.btnAgregar_Personal.Size = new System.Drawing.Size(108, 30);
            this.btnAgregar_Personal.TabIndex = 12;
            this.btnAgregar_Personal.Text = "Registrar";
            this.btnAgregar_Personal.UseVisualStyleBackColor = true;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(435, 12);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(72, 24);
            this.label20.TabIndex = 13;
            this.label20.Text = "LISTA";
            // 
            // dgvLista_Personal
            // 
            this.dgvLista_Personal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLista_Personal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLista_Personal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column10,
            this.Column11,
            this.Column12});
            this.dgvLista_Personal.Location = new System.Drawing.Point(439, 90);
            this.dgvLista_Personal.Name = "dgvLista_Personal";
            this.dgvLista_Personal.Size = new System.Drawing.Size(328, 402);
            this.dgvLista_Personal.TabIndex = 14;
            // 
            // Column10
            // 
            this.Column10.HeaderText = "No.Empleado";
            this.Column10.Name = "Column10";
            // 
            // Column11
            // 
            this.Column11.HeaderText = "Nombre";
            this.Column11.Name = "Column11";
            // 
            // Column12
            // 
            this.Column12.HeaderText = "Ocupación";
            this.Column12.Name = "Column12";
            // 
            // txtBuscar_Personal
            // 
            this.txtBuscar_Personal.Location = new System.Drawing.Point(439, 58);
            this.txtBuscar_Personal.Name = "txtBuscar_Personal";
            this.txtBuscar_Personal.Size = new System.Drawing.Size(234, 20);
            this.txtBuscar_Personal.TabIndex = 15;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(439, 40);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(55, 13);
            this.label21.TabIndex = 16;
            this.label21.Text = "Búsqueda";
            // 
            // btnBuscar_Personal
            // 
            this.btnBuscar_Personal.Location = new System.Drawing.Point(680, 54);
            this.btnBuscar_Personal.Name = "btnBuscar_Personal";
            this.btnBuscar_Personal.Size = new System.Drawing.Size(75, 23);
            this.btnBuscar_Personal.TabIndex = 17;
            this.btnBuscar_Personal.Text = "Buscar";
            this.btnBuscar_Personal.UseVisualStyleBackColor = true;
            // 
            // tabPrestamo
            // 
            this.tabPrestamo.Location = new System.Drawing.Point(4, 22);
            this.tabPrestamo.Name = "tabPrestamo";
            this.tabPrestamo.Size = new System.Drawing.Size(776, 501);
            this.tabPrestamo.TabIndex = 3;
            this.tabPrestamo.Text = "PRÉSTAMO";
            this.tabPrestamo.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "BIBLIOTECA";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabInicio.ResumeLayout(false);
            this.tabInicio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPrestamos_Home)).EndInit();
            this.tabAlumno.ResumeLayout(false);
            this.tabAlumno.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAlumnos_Alumno)).EndInit();
            this.tabPersonal.ResumeLayout(false);
            this.tabPersonal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLista_Personal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInicio;
        private System.Windows.Forms.Button btnBuscar_Home;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBusqueda_Home;
        private System.Windows.Forms.DataGridView dgvPrestamos_Home;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.TabPage tabAlumno;
        private System.Windows.Forms.ComboBox cbFiltroBusqueda_Home;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtEmail_AlumnoAdd;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbCarrera_AlumnoAdd;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbCuatrimestre_AlumnoAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtApellido_AlumnoAdd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNombre_AlumnoAdd;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtNoControl_AlumnoAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTelefono_AlumnoAdd;
        private System.Windows.Forms.Button btnAdd_Alumno;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnBuscar_Alumno;
        private System.Windows.Forms.TextBox txtBusqueda_Alumno;
        private System.Windows.Forms.DataGridView dgvAlumnos_Alumno;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPersonal;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtNoEmpleado_Personal;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DataGridView dgvLista_Personal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Button btnAgregar_Personal;
        private System.Windows.Forms.TextBox txtTelefono_Personal;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtEMail_Personal;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cbOcupacion_Personal;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtNombre_Personal;
        private System.Windows.Forms.Button btnBuscar_Personal;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox txtBuscar_Personal;
        private System.Windows.Forms.TabPage tabPrestamo;
    }
}

