using MenuMontaditosApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuMontaditosApp.Models;
using System.Data;
using System.Data.Common;
using MySql.Data.MySqlClient;

namespace MenuMontaditosApp.Services
{
    public class CocinaService
    {
        private Database db = new Database();

        // me guardo los pedidos activos
        public List<Pedido> ObtenerPedidosActivos()

        {
            List<Pedido> lista = new List<Pedido>();
            using (MySqlConnection conn = db.GetConnection()) // voy a intentar no usar mas var para no tener tantos lios luego, conn es del tipo MysqlConnection y punto en boca.
            {
                conn.Open();
                string queryPedidos = @"
                    SELECT id_pedido, nombre_cliente, fecha_hora, estado 
                    FROM Pedido 
                    WHERE estado IN ('pendiente', 'proceso')
                    ORDER BY fecha_hora ASC";

                MySqlCommand cmd = new MySqlCommand(queryPedidos, conn);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Pedido pedido = new Pedido
                    {
                        IdPedido = reader.GetInt32("id_pedido"),
                        NombreCliente = reader.GetString("nombre_cliente"),
                        FechaHora = reader.GetDateTime("fecha_hora"),
                        Estado = Enum.Parse<Enums.EstadoPedido>(reader.GetString("estado"), ignoreCase: true)
                    };
                    lista.Add(pedido);
                };
                reader.Close();

                foreach (Pedido pedido in lista)
                {
                    pedido.Detalles = ObtenerDetalles(pedido.IdPedido, conn);
                }
            }

            return lista;
        }

            //***** sacar los productos de un pedido.*********

        private List<DetallePedido> ObtenerDetalles(int idPedido, MySqlConnection conn)
        {
            List<DetallePedido> detalles = new List<DetallePedido>();

            // Hacemos un JOIN para traer los datos del producto directamente
            // así no tenemos que hacer otra consulta extra por cada producto
            string query = @"
            SELECT dp.id_detalle, dp.cantidad,
                    p.id_producto, p.nombre, p.precio, p.categoria
            FROM DetallePedido dp
            INNER JOIN Producto p ON dp.id_producto = p.id_producto
            WHERE dp.id_pedido = @idPedido";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idPedido", idPedido);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DetallePedido detalle = new DetallePedido
                {
                    IdDetalle = reader.GetInt32("id_detalle"),
                    IdPedido = idPedido,
                    Cantidad = reader.GetInt32("cantidad"),
                    Producto = new Producto
                    {
                        IdProducto = reader.GetInt32("id_producto"),
                        Nombre = reader.GetString("nombre"),
                        Precio = reader.GetDecimal("precio"),
                        Categoria = Enum.Parse<Enums.CategoriaProducto>(reader.GetString("categoria"), ignoreCase: true)
                    }
                };

                detalles.Add(detalle);
            }

            reader.Close();
            return detalles;
        }

            // ── Actualizar estado de un pedido ─────────────────────────
        public void ActualizarEstado(int idPedido, Enums.EstadoPedido nuevoEstado)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                UPDATE Pedido 
                SET estado = @estado 
                WHERE id_pedido = @idPedido";

                MySqlCommand cmd = new MySqlCommand(query, conn);

                // Convertimos el enum a texto en minúsculas para que coincida con MySQL
                cmd.Parameters.AddWithValue("@estado", nuevoEstado.ToString().ToLower());
                cmd.Parameters.AddWithValue("@idPedido", idPedido);

                cmd.ExecuteNonQuery();
            }
        }

    }
        
}

