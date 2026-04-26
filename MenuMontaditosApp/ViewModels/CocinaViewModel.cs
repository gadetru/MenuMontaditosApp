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
    public class CocinaViewModel : INotifyPropertyChanged
    {
        // Lista de pedidos que se mostrarán en la vista
        private ObservableCollection<PedidoItemViewModel> _pedidos;
        public ObservableCollection<PedidoItemViewModel> Pedidos
        {
            get { return _pedidos; }
            set
            {
                _pedidos = value;
                OnPropertyChanged();
            }
        }

        // Servicio para conectar con la base de datos
        private CocinaService _cocinaService;

        // Comandos para los botones
        public ICommand PedidoEnProcesoCommand { get; }
        public ICommand PedidoCompletadoCommand { get; }

        public CocinaViewModel()
        {
            _cocinaService = new CocinaService();
            _pedidos = new ObservableCollection<PedidoItemViewModel>();

            PedidoEnProcesoCommand = new RelayCommand(
                execute: PedidoEnProceso,
                canExecute: PuedeProcesar
            );

            PedidoCompletadoCommand = new RelayCommand(
                execute: PedidoCompletado,
                canExecute: PuedeCompletar
            );

            // Cargamos los pedidos al arrancar
            CargarPedidos();
        }

        // Carga los pedidos activos de la base de datos
        private void CargarPedidos()
        {
            _pedidos.Clear();

            System.Collections.Generic.List<Pedido> lista = _cocinaService.ObtenerPedidosActivos();

            foreach (Pedido pedido in lista)
            {
                _pedidos.Add(new PedidoItemViewModel(pedido, delegate () // está chulo delegate,me sirve pa pasar una funcion entera en lugar de un parámetro.
                {

                    (PedidoCompletadoCommand as RelayCommand).RaiseCanExecuteChanged();
                } ));
            }
        }

        // Se ejecuta cuando pulsan "En proceso"
        private void PedidoEnProceso(object parametro)
        {
            PedidoItemViewModel pedidoItem = parametro as PedidoItemViewModel;

            if (pedidoItem == null) return;

            _cocinaService.ActualizarEstado(
                pedidoItem.Pedido.IdPedido,
                Enums.EstadoPedido.Proceso
            );

            pedidoItem.Estado = Enums.EstadoPedido.Proceso; // aquí actualizo el estado del pedido sin recargar toda la vista, no se pierden los checks :)
            // Recargamos la lista para reflejar el cambio
            //CargarPedidos(); // quedas fuera porque sino, me recargas la vista del pedido entera y pierdo los datos de los checks.
        }

        // Se ejecuta cuando pulsan "Completado"
        private void PedidoCompletado(object parametro)
        {
            PedidoItemViewModel pedidoItem = parametro as PedidoItemViewModel;

            if (pedidoItem == null) return;

            _cocinaService.ActualizarEstado(
                pedidoItem.Pedido.IdPedido,
                Enums.EstadoPedido.Completado
            );

            // Recargamos la lista para reflejar el cambio
            CargarPedidos();
        }

        // El botón "En proceso" siempre está activo
        private bool PuedeProcesar(object parametro)
        {
            return true;
        }

        // El botón "Completado" solo se activa si todos los checks están marcados
        private bool PuedeCompletar(object parametro)
        {
            PedidoItemViewModel pedidoItem = parametro as PedidoItemViewModel;

            if (pedidoItem == null) return false;

            return pedidoItem.TodosListos;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}
