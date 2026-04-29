using MenuMontaditosApp.Data;
using MenuMontaditosApp.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.Services
{
    public class ProductoService
    {
        private Database db = new Database();


        //*********lista de productos****************

        public List<Producto> ObtenerProductos()
        {
            List<Producto> lista = new List<Producto>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                // Solo cogemos los productos activos
                string query = "SELECT * FROM Producto WHERE activo = 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        IdProducto = reader.GetInt32("id_producto"),
                        Nombre = reader.GetString("nombre"),
                        Precio = reader.GetDecimal("precio"),
                        Categoria = Enum.Parse<Enums.CategoriaProducto>(reader.GetString("categoria"), ignoreCase: true),
                        Activo = reader.GetBoolean("activo")
                    });
                }
            }

            return lista;
        }
        // LISTAR por categoría ( comida/bebida/postre ) 
        public List<Producto> ObtenerPorCategoria(Enums.CategoriaProducto categoria)
        {
            List<Producto> lista = new List<Producto>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Producto WHERE categoria = @categoria AND activo = 1";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@categoria", categoria.ToString().ToLower());

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        IdProducto = reader.GetInt32("id_producto"),
                        Nombre = reader.GetString("nombre"),
                        Precio = reader.GetDecimal("precio"),
                        Categoria = categoria,
                        Activo = reader.GetBoolean("activo")
                    });
                }
            }

            return lista;
        }

        // CREAR un producto nuevo
        public void CrearProducto(string nombre, decimal precio, Enums.CategoriaProducto categoria)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO Producto (nombre, precio, categoria)
                         VALUES (@nombre, @precio, @categoria)";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@categoria", categoria.ToString().ToLower());

                cmd.ExecuteNonQuery();
            }
        }

        // MODIFICAR un producto existente
        public void ModificarProducto(int idProducto, string nombre, decimal precio, Enums.CategoriaProducto categoria)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = @"UPDATE Producto
                         SET nombre = @nombre, precio = @precio, categoria = @categoria
                         WHERE id_producto = @id";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.Parameters.AddWithValue("@precio", precio);
                cmd.Parameters.AddWithValue("@categoria", categoria.ToString().ToLower());
                cmd.Parameters.AddWithValue("@id", idProducto);

                cmd.ExecuteNonQuery();
            }
        }

        // ELIMINAR un producto
        public void EliminarProducto(int idProducto)
        {
            using (var conn = db.GetConnection())
            {
                conn.Open();

                // Borrado lógico — marcamos el producto como inactivo
                // pero no lo borramos para que los pedidos antiguos sigan funcionando,sino nos casca el histórico, que no piensas, chaval...
                string query = "UPDATE Producto SET activo = 0 WHERE id_producto = @id";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", idProducto);

                cmd.ExecuteNonQuery();
            }
        }
    }
    
}
    

