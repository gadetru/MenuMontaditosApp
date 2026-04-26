using MenuMontaditosApp.Data;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuMontaditosApp.Models;
using MySql.Data.MySqlClient;
using System.Transactions;

namespace MenuMontaditosApp.Services
{
    internal class PedidoService
    {
        public Database db = new Database();

        public void CrearPedido(string nombreCliente,List<LineaPedido> lineas)
        {
            using(var conn = db.GetConnection())
            {
                conn.Open();
                MySqlTransaction transaccion = conn.BeginTransaction();
                try
                {
                    // ── PASO 1: insertar la cabecera del pedido ──────────────

                    string queryPedido = @"
                        INSERT INTO Pedido (nombre_cliente, fecha_hora, total)
                        VALUES (@nombre, @fecha, @total);
                        SELECT LAST_INSERT_ID();";
                    // SELECT LAST_INSERT_ID() nos devuelve el id que MySQL
                    // asignó automáticamente al pedido que acabamos de insertar

                    MySqlCommand cmdPedido = new MySqlCommand(queryPedido, conn, transaccion);

                    // Rellenamos los parámetros con los datos reales
                    cmdPedido.Parameters.AddWithValue("@nombre", nombreCliente);
                    cmdPedido.Parameters.AddWithValue("@fecha", DateTime.Now);          
                    cmdPedido.Parameters.AddWithValue("@total", CalcularTotal(lineas));

                    // ExecuteScalar ejecuta la query y devuelve el primer valor
                    // que obtiene, que en este caso es el LAST_INSERT_ID
                    int idPedido = Convert.ToInt32(cmdPedido.ExecuteScalar());

                    // ── PASO 2: insertar cada línea del carrito ──────────────

                    foreach (LineaPedido linea in lineas)
                    {
                        string queryDetalle = @"
                            INSERT INTO DetallePedido (id_pedido, id_producto, cantidad)
                            VALUES (@idPedido, @idProducto, @cantidad)"
                        ;

                        MySqlCommand cmdDetalle = new MySqlCommand(queryDetalle, conn, transaccion);

                        cmdDetalle.Parameters.AddWithValue("@idPedido", idPedido);
                        cmdDetalle.Parameters.AddWithValue("@idProducto", linea.Producto.IdProducto);
                        cmdDetalle.Parameters.AddWithValue("@cantidad", linea.Cantidad);

                        cmdDetalle.ExecuteNonQuery();
                        // ExecuteNonQuery se usa cuando la query no devuelve datos,
                        // solo hace una acción (INSERT, UPDATE, DELETE)
                    }

                    // Todo fue bien — confirmamos la transacción
                    transaccion.Commit();
                }
                catch (Exception ex)
                {
                    // Algo falló — deshacemos todo para no dejar datos a medias
                    transaccion.Rollback();

                    // Relanzamos el error para que el ViewModel pueda avisarle al usuario
                    throw new Exception("Error al procesar el pedido: " + ex.Message);
                }

            
            }
        }
        // Método privado de ayuda para calcular el total del carrito
        // Es privado porque solo lo necesitamos dentro de este servicio
        private decimal CalcularTotal(List<LineaPedido> lineas)
        {
            decimal total = 0;

            foreach (LineaPedido linea in lineas)
            {
                total += linea.Producto.Precio * linea.Cantidad;
            }

            return total;
        }
    }
}
