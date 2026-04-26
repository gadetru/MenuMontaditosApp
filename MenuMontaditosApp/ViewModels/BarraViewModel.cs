using MenuMontaditosApp.Helpers;
using MenuMontaditosApp.Models;
using MenuMontaditosApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MenuMontaditosApp.ViewModels
{
    public class BarraViewModel : INotifyPropertyChanged
    {
        // Lista de pedidos que mostraremos en la vista de la barra.

        private ObservableCollection<BarraPedidoItemViewModel> _pedidos;
        public ObservableCollection<BarraPedidoItemViewModel> Pedidos
        {
            get { return _pedidos; }
            set
            {
                _pedidos = value;
                OnPropertyChanged();
            }
        }

        // Servicio para conectar con la base de datos
        private BarraService _barraService;

        // Comando para archivar un pedido
        public ICommand ArchivarPedidoCommand { get; }

        public BarraViewModel()
        {
            _barraService = new BarraService();
            _pedidos = new ObservableCollection<BarraPedidoItemViewModel>();

            ArchivarPedidoCommand = new RelayCommand(
                execute: delegate (object parametro)
                {
                    BarraPedidoItemViewModel pedidoItem = parametro as BarraPedidoItemViewModel;

                    if (pedidoItem == null) return;

                    _barraService.ArchivarPedido(pedidoItem.Pedido.IdPedido);

                    // Quitamos el pedido de la lista sin recargar todo
                    _pedidos.Remove(pedidoItem);
                },
                canExecute: delegate (object parametro)
                {
                    BarraPedidoItemViewModel pedidoItem = parametro as BarraPedidoItemViewModel;

                    if (pedidoItem == null) return false;

                    // Solo se puede archivar si el pedido está completado
                    return pedidoItem.Pedido.Estado == Enums.EstadoPedido.Completado;
                }
            );

            CargarPedidos();
        }

        private void CargarPedidos()
        {
            _pedidos.Clear();

            System.Collections.Generic.List<Pedido> lista = _barraService.ObtenerPedidosActivos();

            foreach (Pedido pedido in lista)
            {
                _pedidos.Add(new BarraPedidoItemViewModel(pedido));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }


}

