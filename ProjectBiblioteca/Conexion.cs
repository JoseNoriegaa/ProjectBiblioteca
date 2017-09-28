using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Resources;
using System.Reflection;
using System.Data.SqlClient;
using System.Windows;

namespace ProjectBiblioteca
{
    class Conexion
    {



        public string cxnString { get; set; }
        public Conexion() { }

        public string connectionString()
        {
            StreamReader sr ;
            string text = "";
            if (createTextFile())
            {
                sr = new StreamReader(@"C:\\Conexion\Cnn.txt");
                text = sr.ReadLine();
            }
            return text;
        }
        public bool probarConexion(string connectionString)
        {
            bool conectado = false;
            try
            {
                SqlConnection cnn = new SqlConnection(connectionString);
                cnn.Open();
                SqlCommand cmd = new SqlCommand("select * from Alumno", cnn);
                cmd.ExecuteNonQuery();
                cnn.Close();
                conectado = true;
            }
            catch (Exception e)
            {
                MessageBox.Show(@"Configure la conexión a la base de datos en: C:\Conexion\Cnn "+e.Message);
                
            }

            return conectado;
        }
        private bool createTextFile()
        {
            var already = false;
            if (File.Exists(@"C:\\Conexion\Cnn.txt")==false)
            {
                string folderName = @"c:\Conexion";

                System.IO.Directory.CreateDirectory(folderName);
                string fileName = "Cnn.txt";
                var pathString = System.IO.Path.Combine(folderName, fileName);

                if (!System.IO.File.Exists(pathString))
                {
                    using (System.IO.FileStream fs = System.IO.File.Create(pathString))
                    {
                        string t = "Data Source=  ;Initial catalog=DB_Biblioteca;Integrated security=true;";
                        byte[] text = ASCIIEncoding.ASCII.GetBytes(t);
                        foreach (var item in text)
                        {
                            fs.WriteByte(item);
                        }
                    }
                }
            }
            already = true;
            return already;
        }

    }
}
