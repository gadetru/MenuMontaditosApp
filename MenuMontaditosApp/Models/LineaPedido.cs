using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MenuMontaditosApp.Models
{
    // en este modelo vamos a crear el pedido con la cantidad de productos que el cliente va a seleccionar en su vista.
    public class LineaPedido : INotifyPropertyChanged
    {
        public Producto Producto {  get; set; }

        private int cantidad;
        public int Cantidad 
        {
            get => cantidad;
            set
            {
                cantidad = value;
                OnPropertyChanged();  // estoy haciendo que el evento se dispare cada vez que cambie cantidad.
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;   // aquí declaro el evento
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)  // con callermembername, si no le paso dato, coge el valor de la propiedad donde lo llamo, en este caso, "cantidad" en el set.
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
