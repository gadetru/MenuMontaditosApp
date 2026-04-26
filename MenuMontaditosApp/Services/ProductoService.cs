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

                string query = "SELECT * FROM Producto";
                MySqlCommand cmd = new MySqlCommand(query, conn);

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(new Producto
                    {
                        IdProducto = reader.GetInt32("id_producto"),
                        Nombre = reader.GetString("nombre"),
                        Precio = reader.GetDecimal("precio"),
                        Categoria = Enum.Parse<Enums.CategoriaProducto>(reader.GetString("categoria"), ignoreCase: true) // así extraeremos el enum de mysql y lo pasamos a un enum numerico de c#
                    });
                }
            }

            return lista;
        }

        public List<Producto> ObtenerPorCategoria(Enums.CategoriaProducto categoria)
        {
            List<Producto> lista = new List<Producto>();

            using (var conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Producto WHERE categoria = @categoria";
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
                        Categoria = categoria
                    });
                }
            }

            return lista;
        }
    }
}
    

