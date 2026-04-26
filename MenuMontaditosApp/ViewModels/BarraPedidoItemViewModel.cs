using MenuMontaditosApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Packaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.ViewModels
{
    public class BarraPedidoItemViewModel : INotifyPropertyChanged
    {
        // El pedido completo con todos los datos.
        public Pedido Pedido { get; set;  }

        // estado del pedido para utilizarlo en la vista 
        private Enums.EstadoPedido _estado;
        public Enums.EstadoPedido Estado
        {
            get { return _estado; }
            set
            {
                _estado = value;
                OnPropertyChanged();
            }
        }

        public BarraPedidoItemViewModel(Pedido pedido)
        {
            Pedido = pedido;
            _estado = pedido.Estado;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

    }
}
