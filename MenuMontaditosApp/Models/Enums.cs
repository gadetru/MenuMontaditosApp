using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.Models
{
    public class Enums
    {
        public enum CategoriaProducto
        {
            Comida,
            Bebida,
            Postre
   
        }

        public enum EstadoPedido
        {
            Pendiente,
            Proceso,
            Completado
        }

        public enum RolUsuario
        {
            Cocina,
            Administrador
        }
    }

   
}
