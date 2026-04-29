using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.Models
{
    public class Producto // no olvidar que si la dejas inernal, no tienes apenas acceso luego
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public Enums.CategoriaProducto Categoria { get; set; } = Enums.CategoriaProducto.Comida;
        public bool Activo { get; set; } = true; // nueva propiedad, vamos a hacer un borrado lógico de productos

    }
}
