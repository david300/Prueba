using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace proyecto_2
{
    public partial class Form1 : Form
    {
        private EstudiantesDatos estudiantesDatos;

        public Form1()
        {
            InitializeComponent();
            estudiantesDatos = new EstudiantesDatos();
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
            Estudiante estudiante = ObtenerDatosEstudiante();
            estudiantesDatos.Actualizar(estudiante);

        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            Estudiante estudiante = ObtenerDatosEstudiante();
            estudiantesDatos.Insertar(estudiante);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 0)
            {
               
                int idEstudiante = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id"].Value);

                DialogResult result = MessageBox.Show("¿Estás seguro de que deseas eliminar este estudiante?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    Estudiante estudiante = new Estudiante
                    {
                        Id = idEstudiante
                    };

                    estudiantesDatos.Eliminar(estudiante);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un estudiante para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public class Estudiante
        {   
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime FechaNacimiento { get; set; }
        }
        public class EstudiantesDatos
        {
            private ConexionBD conexionBD;

            public EstudiantesDatos()
            {
                conexionBD = new ConexionBD();
            }

            public void Insertar(Estudiante estudiante)
            {
                if (string.IsNullOrWhiteSpace(estudiante.Nombre) || string.IsNullOrWhiteSpace(estudiante.Apellido))
                {
                    MessageBox.Show("ingrese un nombre y apellido valido");
                    return;
                }

                if (estudiante.FechaNacimiento >= DateTime.Now.Date)
                {
                    MessageBox.Show("la fecha de nacimiento no puede ser la fecha actual");
                    return;
                }

                if (estudiante.FechaNacimiento.Year < 1998)
                {
                    MessageBox.Show("la fecha de nacimiento no pueder ser antes del 1998");
                    return;
                }

                string query = "INSERT INTO Estudiantes (Nombre, Apellido, FechaNacimiento) VALUES (@Nombre, @Apellido, @FechaNacimiento)";
                using (SqlConnection conexion = conexionBD.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", estudiante.FechaNacimiento);
                    cmd.ExecuteNonQuery();
                }
            }

            public void Actualizar(Estudiante estudiante)
            {
                if (string.IsNullOrWhiteSpace(estudiante.Nombre) || string.IsNullOrWhiteSpace(estudiante.Apellido))
                {
                    MessageBox.Show("ingrese un nombre y apellido valido");
                    return;
                }

                if (estudiante.FechaNacimiento >= DateTime.Now.Date)
                {
                    MessageBox.Show("la fecha de nacimiento no puede ser la fecha actual");
                    return;
                }

                if (estudiante.FechaNacimiento.Year < 1998)
                {
                    MessageBox.Show("la fecha de nacimiento no pueder ser antes del 1998");
                    return;
                }

                string query = "UPDATE Estudiantes SET Nombre = @Nombre, Apellido = @Apellido, FechaNacimiento = @FechaNacimiento WHERE Id = @Id";
                using (SqlConnection conexion = conexionBD.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Nombre", estudiante.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", estudiante.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", estudiante.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Id", estudiante.Id);
                    cmd.ExecuteNonQuery();
                }
            }

            public void Eliminar(Estudiante estudiante)
            {
                string query = "DELETE FROM Estudiantes WHERE Id = @Id";
                using (SqlConnection conexion = conexionBD.AbrirConexion())
                {
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Id", estudiante.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private Estudiante ObtenerDatosEstudiante()
        {
            string nombre = txtNombre.Text;
            string apellido = txtApellido.Text;
            DateTime fechaNacimiento = dtpFecha.Value;

            Estudiante estudiante = new Estudiante
            {
                Nombre = nombre,
                Apellido = apellido,
                FechaNacimiento = fechaNacimiento
            };

            return estudiante;
        }
       
    }
}
