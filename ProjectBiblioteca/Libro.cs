using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;

namespace ProjectBiblioteca
{
    class Libro
    {
        SqlConnection cnn = new SqlConnection(new Conexion().connectionString());

        public string ID_Libro { get; set; }
        public string ISBN { get; set; }
        public string titulo { get; set; }
        public int año { get; set; }
        public string clasificacion {get; set; }
        public string autor { get; set; }
        public  string descripcion{ get; set; }
        public string editorial{ get; set; }
        public string lugar { get; set; }
        public string edicion { get; set; }
        public decimal precio { get; set; }


        public Libro(){ }

        public Libro(string ID_Libro, string ISBN, string titulo, int año,string autor, string clasificacion, string descripcion,
            string editorial, string lugar, string edicion, decimal precio)
        {
            this.ID_Libro = ID_Libro;
            this.ISBN = ISBN;
            this.titulo = titulo;
            this.año = año;
            this.autor = autor;
            this.clasificacion = clasificacion;
            this.precio = precio;
            if (String.IsNullOrEmpty(descripcion))
            {
                this.descripcion = "default";
            }
            else {
                this.descripcion = descripcion;
            }

            this.editorial = editorial;
            this.lugar = lugar;
            this.edicion = edicion;
        }

        public void agregarLibroBD()
        {
            try
            {
                if (verificarLibroRegistrado()==false)
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand("Agregar_Libro", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdLibro", this.ID_Libro);
                    cmd.Parameters.AddWithValue("@ISBN", this.ISBN);
                    cmd.Parameters.AddWithValue("@Titulo", this.titulo);
                    cmd.Parameters.AddWithValue("@Año", this.año);
                    cmd.Parameters.AddWithValue("@Clasificacion", this.clasificacion);
                    cmd.Parameters.AddWithValue("@Autor", this.autor);
                    cmd.Parameters.AddWithValue("@Descripcion", this.descripcion);
                    cmd.Parameters.AddWithValue("@Editorial", this.editorial);
                    cmd.Parameters.AddWithValue("@Lugar", this.lugar);
                    cmd.Parameters.AddWithValue("@Edicion", this.edicion);
                    cmd.Parameters.AddWithValue("@Status", 0);
                    cmd.Parameters.AddWithValue("@Precio", this.precio);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Se ha registrado el libro " + this.titulo);

                }
                else {
                MessageBox.Show("Error.\nId existente");
            }
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

        public void actualizarLibro(string idLibro)
        {
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("EditarLibro", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IdLibro", this.ID_Libro);
                cmd.Parameters.AddWithValue("@ISBN", this.ISBN);
                cmd.Parameters.AddWithValue("@Titulo", this.titulo);
                cmd.Parameters.AddWithValue("@Año", this.año);
                cmd.Parameters.AddWithValue("@Clasificacion", this.clasificacion);
                cmd.Parameters.AddWithValue("@Autor", this.autor);
                cmd.Parameters.AddWithValue("@Descripcion", this.descripcion);
                cmd.Parameters.AddWithValue("@Editorial", this.editorial);
                cmd.Parameters.AddWithValue("@Lugar", this.lugar);
                cmd.Parameters.AddWithValue("@Edicion", this.edicion);
                cmd.Parameters.AddWithValue("@IdLibroViejo", idLibro);
                cmd.Parameters.AddWithValue("@Status", 0);
                cmd.Parameters.AddWithValue("@Precio", this.precio);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Se ha actualizado el libro " + this.titulo);
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

        public int countISBN(string isbn)
        {
            int salida = 0;
            cnn.Open();
            SqlCommand cmd = new SqlCommand("countISBN", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ISBN", isbn);
            SqlDataReader rd =  cmd.ExecuteReader();
            if (rd.Read())
            {
                salida= int.Parse(rd[0].ToString());
            }
            else
            {
                salida= 0;
            }
            cnn.Close();
            return salida;
        }

        private bool verificarLibroRegistrado()
        {
            bool salida = false;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("verificarLibro", cnn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", this.ID_Libro);

                if (int.Parse(cmd.ExecuteScalar().ToString()) == 1)
                {
                    salida = true;
                }
                else
                {
                    salida = false;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Ha ocurrido un error.\n" + e.Message, e.Source);
            }
            finally
            {
                cnn.Close();
            }
            return salida;
        }

        private bool verificarLibroEstado(string ID_Libro)
        {
            bool salida = false;
            try
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("verificarLibroPrestado", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", ID_Libro);
                SqlDataReader rd = cmd.ExecuteReader();
                rd.Read();
                var v = rd[0];
                if (bool.Parse(v.ToString()) == true)
                {
                    salida = true;
                }
                else
                {
                    salida = false;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Ha ocurrido un error.\n" + e.Message, e.Source);
            }
            finally
            {
                cnn.Close();
            }
            return salida;
        }

        public List<string> buscarLibro(string isbn)
        {
            List<string> ls = new List<string>();
            cnn.Open();
            SqlCommand cmd = new SqlCommand("buscarLibro", cnn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ISBN", isbn);
            SqlDataReader rd = cmd.ExecuteReader();
            while (rd.Read())
            {
                ls.Add(rd[0].ToString());
            }
            return ls;

        }
        public void borrarLibroDB(string idLibro)
        {
            try
            {
                if (verificarLibroEstado(idLibro))
                {
                    MessageBox.Show("NO PUEDE ELIMINAR LIBROS QUE SE ENCUENTRAN PRESTADOS EN ESTE MOMENTO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    cnn.Open();
                    SqlCommand cmd = new SqlCommand("EliminarLibro", cnn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IdLibro", idLibro);
                    if (MessageBox.Show("Esta seguro de borrar este registro permanentemete", "BORRAR", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
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
