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
    public class BarraService
    {
        private Database db = new Database();  

        // Traemos los pedidos activos no archivados.
        public List<Pedido> ObtenerPedidosActivos()
        {
            List<Pedido> lista = new List<Pedido>();

            using ( MySqlConnection conn = db.GetConnection())  // si hay alguna duda. al utilizar using, nos garantizamos que se cierre la conexion sin tener que poner conn.close().
            {
                conn.Open();

                string queryPedidos = @"SELECT id_pedido,nombre_cliente,fecha_hora,
                      estado FROM Pedido WHERE archivado = 0 ORDER BY fecha_hora ASC";

                MySqlCommand cmd = new MySqlCommand(queryPedidos,conn);
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
                }

                //cerramos lectura,
                reader.Close();

                // cargo los productos de cada pedido realizado.
                foreach(Pedido pedido in lista)
                {
                    pedido.Detalles = ObtenerDetalles(pedido.IdPedido, conn);
                }

            }
            return lista;

        }

        // sacar los prodcutos de un pedido:
        private List<DetallePedido> ObtenerDetalles(int idPedido, MySqlConnection conn)
        {
            List<DetallePedido> detalles = new List<DetallePedido>();

            string query = @"SELECT dp.id_detalle,dp.cantidad,p.id_producto,p.nombre,p.precio,p.categoria 
                FROM DetallePedido dp INNER JOIN Producto p ON dp.id_producto = p.id_producto 
                WHERE dp.id_pedido = @idPedido";

            MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@idPedido", idPedido);

            MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                DetallePedido detalle = new DetallePedido
                {
                    IdDetalle = reader.GetInt32("id_detalle"),
                    IdPedido = idPedido, // parámetro del método
                    Cantidad = reader.GetInt32("cantidad"),
                    Producto = new Producto // sacamos el objeto
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

        // guardar registro de pedidos terminados, cobrados y entregados :

        public void ArchivarPedido(int idPedido)
        {
            using (MySqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"UPDATE Pedido SET archivado = 1 WHERE id_pedido = @idPedido";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@idPedido", idPedido);

                cmd.ExecuteNonQuery();

            }
        }

    }
}
