using MenuMontaditosApp.Data;
using MenuMontaditosApp.Models;
using MySql.Data.MySqlClient;

namespace MenuMontaditosApp.Services
{
    public class UsuarioService
    {
        private Database db = new Database();

        // Comprueba si el nombre y contraseña son correctos.
        // Si lo son, devuelve el Usuario completo. Si no, devuelve null.
        public Usuario? IniciarSesion(string nombre, string password)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Usuarios WHERE nombre = @nombre";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Sacamos la contraseña hasheada de la BD
                    string hashGuardado = reader.GetString("contraseña");

                    // BCrypt compara la contraseña que escribió el usuario
                    // con el hash guardado. Si coinciden, devuelve true.
                    bool coincide = BCrypt.Net.BCrypt.Verify(password, hashGuardado);

                    if (coincide)
                    {
                        Usuario usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32("id_usuario"),
                            Nombre = reader.GetString("nombre"),
                            Rol = Enum.Parse<Enums.RolUsuario>(reader.GetString("rol"), ignoreCase: true)
                        };
                        return usuario;
                    }
                }
            }

            // Si llegamos aqui, o no existe el usuario o la contraseña es incorrecta
            return null;
        }

        // Crea un usuario nuevo con la contraseña hasheada.
        // Esto lo usaremos pa crear los usuarios la primera vez.
        public void CrearUsuario(string nombre, string password, Enums.RolUsuario rol)
        {
            // BCrypt convierte la contraseña en un hash seguro
            string hash = BCrypt.Net.BCrypt.HashPassword(password);

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO Usuarios (nombre, contraseña, rol)
                                 VALUES (@nombre, @contrasena, @rol)";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@contrasena", hash);
                cmd.Parameters.AddWithValue("@rol", rol.ToString());

                cmd.ExecuteNonQuery();
            }
        }

        // comprueba si hay usuarios creados- true si sí hay

        public bool HayUsuarios()
        {
            using(MySqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Usuarios";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                // ExecuteScalar nos devuelve el primer valor del resultado,
                // que es el número de filas que hay en la tabla
                int total = Convert.ToInt32(cmd.ExecuteScalar());

                return total > 0;
            }
        }

        // Devuelve la lista de todos los usuarios que hay en la BD.
        // No devolvemos la contraseña por seguridad, total, pa que la quieres saber? eh?
        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();

            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT id_usuario, nombre, rol FROM Usuarios";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = reader.GetInt32("id_usuario"),
                        Nombre = reader.GetString("nombre"),
                        Rol = Enum.Parse<Enums.RolUsuario>(reader.GetString("rol"), ignoreCase: true)
                    };

                    lista.Add(usuario);
                }
            }

            return lista;
        }

        // Elimina un usuario de la BD por su id
        public void EliminarUsuario(int idUsuario)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "DELETE FROM Usuarios WHERE id_usuario = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idUsuario);

                cmd.ExecuteNonQuery();
            }
        }
    }
}