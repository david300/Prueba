using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace proyecto_2
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public class ConexionBD
        {
            public SqlConnection conexion;
            public string connectionString = "server=desktop-ukems2i\\sqlexpress; database=MiBaseDeDatos; integrated security=true";


            public ConexionBD()
            {
                conexion = new SqlConnection(connectionString);
            }

            public SqlConnection AbrirConexion()
            {
                if (conexion.State == System.Data.ConnectionState.Closed)
                {
                    conexion.Open();
                }
                return conexion;
            }

            public SqlConnection CerrarConexion()
            {
                if (conexion.State == System.Data.ConnectionState.Open)
                {
                    conexion.Close();
                }
                return conexion;
            }
        }

        private void btnMostrar_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable();
            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.AbrirConexion())
            {
                SqlDataAdapter adapter = new SqlDataAdapter("select * from Estudiantes", conexion);
                adapter.SelectCommand.CommandType = CommandType.Text;
                adapter.Fill(dt);
                dataGridView1.DataSource = dt;
            }

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);
            string nuevoNombre = txtNombre.Text;
            string nuevoApellido = txtApellido.Text;
            DateTime nuevaFechaNacimiento = dtpFecha.Value;

            string query = "UPDATE Estudiantes SET Nombre = @Nombre, Apellido = @Apellido, FechaNacimiento = @FechaNacimiento WHERE Id = @Id";
            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.AbrirConexion())
            {

                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Nombre", nuevoNombre);
                cmd.Parameters.AddWithValue("@Apellido", nuevoApellido);
                cmd.Parameters.AddWithValue("@FechaNacimiento", nuevaFechaNacimiento);
                cmd.Parameters.AddWithValue("@Id", id);
            }

        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {

            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            DateTime fechaNacimiento = dtpFecha.Value;

            string query = "INSERT INTO Estudiantes (Nombre, Apellido, FechaNacimiento) VALUES (@Nombre, @Apellido, @FechaNacimiento)";

            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@Apellido", apellido);
                cmd.Parameters.AddWithValue("@FechaNacimiento", fechaNacimiento);

            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);

            string query = "DELETE FROM Estudiantes WHERE Id = @Id";
            ConexionBD conexionBD = new ConexionBD();
            using (SqlConnection conexion = conexionBD.AbrirConexion())
            {
                SqlCommand cmd = new SqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@Id", id);
            }
        }
    }
}
