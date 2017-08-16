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

        public Libro(){ }

        public Libro(string id, string isbn, string titulo, int año,string autor, string clasificacion, string descripcion, string editorial, string lugar, string edicion)
        {
            this.ID_Libro = id;
            this.ISBN = isbn;
            this.titulo = titulo;
            this.año = año;
            this.autor = autor;
            this.clasificacion = clasificacion;
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
        public void borrarLibroDB(string idLibro)
        {
            try
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
