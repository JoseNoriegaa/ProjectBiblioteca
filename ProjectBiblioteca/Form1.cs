using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
        }

        private void tabPrestamo_Click(object sender, EventArgs e)
        {
            cbTipo_Prestamo.SelectedIndex = 0;
        }

        private void cbTipo_Prestamo_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbTipo_Prestamo.SelectedItem.ToString())
            {
                case "ALUMNO":
                    lAlumnos_Prestamo.Text = "LISTA ALUMNOS";
                    lNoControl_Empleado_Prestamo.Text = "No. de Control";
                    dgvListaAlumno_Prestamo.Columns[0].HeaderText = "No. de Control";
                    dgvListaAlumno_Prestamo.Columns[1].HeaderText = "Nombre Del Alumno";
                    break;
                case "PERSONAL":
                    lAlumnos_Prestamo.Text = "LISTA PERSONAL";
                    lNoControl_Empleado_Prestamo.Text = "No. de Empleado";
                    dgvListaAlumno_Prestamo.Columns[0].HeaderText = "No. de Empleado";
                    dgvListaAlumno_Prestamo.Columns[1].HeaderText = "Nombre Del Empleado";

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
    }
}
