using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.Models
{
    public class DetallePedido
    {
        public int IdDetalle { get; set; }
        public int IdPedido {  get; set; } 
        public Producto Producto { get; set; } = new Producto();
        public int Cantidad {  get; set; }  
    }
}
