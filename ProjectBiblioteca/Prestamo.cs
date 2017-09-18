using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using Novacode;
using System.IO;
using BIBLIOTECA.Properties;

namespace ProjectBiblioteca
{
    class Prestamo
    {

        private SqlConnection cnn = new SqlConnection(new Conexion().connectionString());
        private SqlCommand cmd;
        public string libroID { get; set; }
        public int matriculaAlumno { get; set; }
        public DateTime fechaDePrestamo { get; set; }
        public DateTime fechaDeEntrega { get; set; }
        public int diasDePrestamo { get; set; }
        public string estadoDelLibro { get; set; }
        public int numeroEmpleado { get; set; }
        public string tipoPrestamo { get; set; }

        public Prestamo()
        {
        }

        public Prestamo(string libro, int matriculaAlumno, DateTime fechaDePrestamo, DateTime fechaDeEntrega, int dias, string estadoDelLibro, string tipoDePrestamno)
        {
            this.libroID = libro;
            this.matriculaAlumno = matriculaAlumno;
            this.fechaDeEntrega = fechaDeEntrega;
            this.fechaDePrestamo = fechaDePrestamo;
            this.diasDePrestamo = dias;
            if (string.IsNullOrEmpty(estadoDelLibro))
            {
                this.estadoDelLibro = "default";
            }
            else
            {
                this.estadoDelLibro = estadoDelLibro;
            }
            this.tipoPrestamo = tipoDePrestamno;

        }

        public Prestamo(int numeroEmpleado, string libro, DateTime fechaDePrestamo, int dias, string estadoDelLibro, string tipoDePrestamno)
        {
            this.libroID = libro;
            this.numeroEmpleado = numeroEmpleado;
            this.fechaDePrestamo = fechaDePrestamo;
            this.diasDePrestamo = dias;
            if (string.IsNullOrEmpty(estadoDelLibro))
            {
                this.estadoDelLibro = "default";
            }
            else
            {
                this.estadoDelLibro = estadoDelLibro;
            }
            this.tipoPrestamo = tipoDePrestamno;
        }

        public void generarReciboPersonal(string nombre, string telefono, string titulo, string correo, string codigo,
            int numeroDeEmpleado, DateTime fechaPrestamo, string ocupacion)
        {
            try
            {
                if (!(Directory.Exists(@"C:\PRESTAMOS/")))
                {
                    Directory.CreateDirectory(@"C:\PRESTAMOS/");
                }
                string nuevo = $@"C:\PRESTAMOS/" + numeroDeEmpleado + ".docx";
                int c = 1;
                while (File.Exists(nuevo))
                {
                    nuevo = $@"C:\PRESTAMOS/{numeroDeEmpleado}({c}).docx";
                    c++;
                }

                byte[] filebytes = Resources.PRÉSTAMOS_DE_LIBROS_BIBLIOTECA_PERSONAL;

                File.WriteAllBytes(nuevo, filebytes);

                var doc = DocX.Load(nuevo);
                doc.ReplaceText("<NO. DE EMPLEADO>", numeroDeEmpleado.ToString());
                doc.ReplaceText("<NOMBRE>", nombre);
                doc.ReplaceText("<TELEFONO>", telefono);
                doc.ReplaceText("<CORREO>", correo);
                doc.ReplaceText("<TITULO>", titulo);
                doc.ReplaceText("<FECHA PRESTAMO>", fechaPrestamo.ToLongDateString().ToString());
                doc.ReplaceText("<CODIGO>", codigo);
                doc.ReplaceText("<OCUPACION>", ocupacion);

                doc.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR AL GENERR EL RECIBO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void generarRecibolumno(string nombre, string telefono, string titulo, string correo, string codigo, 
            string grupo, int cuatrimestre, int dias, int matricula, DateTime fechaPrestamo, DateTime fechaEntrega)
        {
             try
            {
                if (!(Directory.Exists(@"C:\PRESTAMOS/")))
                {
                    Directory.CreateDirectory(@"C:\PRESTAMOS/");
                }
                string nuevo = $@"C:\PRESTAMOS/" + matricula + ".docx";
                int c = 1;
                while (File.Exists(nuevo))
                {
                    nuevo = $@"C:\PRESTAMOS/{matricula}({c}).docx";
                    c++;
                }

                byte[] filebytes = Resources.PRESTAMOS_DE_LIBROS_BIBLIOTECA_ALUMNOS;
                File.WriteAllBytes(nuevo, filebytes);
                var doc = DocX.Load(nuevo);
                doc.ReplaceText("<NO. DE CONTROL>", matricula.ToString());
                doc.ReplaceText("<NOMBRE>", nombre);
                doc.ReplaceText("<TELEFONO>", telefono);
                doc.ReplaceText("<CORREO>", correo);
                doc.ReplaceText("<TITULO>", titulo);
                doc.ReplaceText("<FECHA PRESTAMO>", fechaPrestamo.ToLongDateString().ToString());
                doc.ReplaceText("<FECHA ENTREGA>", fechaEntrega.ToLongDateString().ToString());
                doc.ReplaceText("<CODIGO>", codigo);
                doc.ReplaceText("<GRUPO>", grupo);
                doc.ReplaceText("<DIAS>", dias.ToString());
                doc.ReplaceText("<CUATRIMESTRE>", cuatrimestre.ToString());

                doc.Save();
            }
            catch (Exception)
            {
                MessageBox.Show("HA OCURRIDO UN ERROR AL GENERAR EL RECIBO","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

     
        public void registrarPrestamo()
        {

            try
            {
                cnn.Open();
                cmd = new SqlCommand("Registrar_Prestamo", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Libro", this.libroID);
                if (tipoPrestamo == "Alumno")
                {
                    cmd.Parameters.AddWithValue("@idPersona", this.matriculaAlumno);
                    cmd.Parameters.AddWithValue("@Fecha_Entrega", this.fechaDeEntrega);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@idPersona", this.numeroEmpleado);
                    cmd.Parameters.AddWithValue("@Fecha_Entrega", "");
                }
                cmd.Parameters.AddWithValue("@Fecha_Prestamo", this.fechaDePrestamo);
                cmd.Parameters.AddWithValue("@Dias_De_Prestamo", this.diasDePrestamo);
                cmd.Parameters.AddWithValue("@Estado_Del_Libro", this.estadoDelLibro);
                cmd.Parameters.AddWithValue("@tipoPrestamo", this.tipoPrestamo);
                cmd.Parameters.AddWithValue("@Estado", 1);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Se ha registrado el prestamo","INFO",MessageBoxButtons.OK,MessageBoxIcon.Information);



            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error" + e.Message, e.Source,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }


        }

        public void BorrarHistorial()
        {
            try
            {
                cnn.Open();
                cmd = new SqlCommand("borrarHistorial", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                MessageBox.Show("Se ha eliminado el historial", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error" + e.Message, e.Source, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }
        }

        public void registrarDevolucion(int Id_Prestamo,string idLibro,int matricula)
        {
            try
            {
                cnn.Open(); 
                cmd = new SqlCommand("RegistrarDevolucion", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdPrestamo", Id_Prestamo);
                cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                cmd.Parameters.AddWithValue("@Matricula", matricula);
                cmd.ExecuteNonQuery();

            }
            catch (Exception e)
            {
                MessageBox.Show("Ha ocurrido un error" + e.Message, e.Source,MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            finally
            {
                cnn.Close();
            }

        }
    }
}
