using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.Models
{
    public class Pedido
    {
        public int IdPedido { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public Enums.EstadoPedido Estado { get; set; }
        public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();

    }
}
