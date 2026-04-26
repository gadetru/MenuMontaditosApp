using MenuMontaditosApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MenuMontaditosApp.ViewModels
{
    // esta clase va a ser auxiliar para hacer un checkeo de los productos completados en la cocina sin tener que persistir en la base de datos.
    public class PedidoItemViewModel : INotifyPropertyChanged
    {
        // El pedido real con todos sus datos
        public Pedido Pedido { get; set; }
        private Enums.EstadoPedido _estado;
        public Enums.EstadoPedido Estado // Sacho el estado del objeto en sí, para poder controlar en tiempo real sus cambios en la vista y poder trabajar con ello
        {
            get { return _estado; }
            set
            { _estado = value;
              OnPropertyChanged(); // aquí estoy dandole la funcion para comprobar el cambio.

            }
        }
           

        // Lista de productos del pedido, cada uno con su checkbox
        public ObservableCollection<DetallePedidoItemViewModel> Detalles { get; set; }

        // Esto calcula si todos los checkboxes están marcados
        // Lo usaremos para habilitar o deshabilitar el botón de completado

        //public bool TodosListos
        //{
        //    get
        //    {
        //        foreach (DetallePedidoItemViewModel detalle in Detalles)
        //        {
        //            if (detalle.EstaListo == false)
        //            {
        //                return false;
        //            }
        //        }
        //        return true;
        //    }
        //}

        // queda más siple así, solo hay que entenderlo bien
        public bool TodosListos => Detalles.All(d => d.EstaListo);
        // Acción que llamaremos cuando cambie un checkbox,sin todos los checks, no se confirma na.
        private readonly Action _onCheckboxCambiado;

        public PedidoItemViewModel(Pedido pedido,Action onCheckboxCambiado)
        {
            Pedido = pedido;
            _estado = pedido.Estado; // he añadido el estado al constructor.

            _onCheckboxCambiado = onCheckboxCambiado; // para avisar desde la vista el checkbox actualizado.

            // Convertimos cada DetallePedido en un DetallePedidoItemViewModel
            Detalles = new ObservableCollection<DetallePedidoItemViewModel>();

            foreach (DetallePedido detalle in pedido.Detalles)
            {
                bool esEditable = pedido.Estado == Enums.EstadoPedido.Proceso;

                DetallePedidoItemViewModel item = new DetallePedidoItemViewModel(detalle,esEditable);

                // Cuando cambie un checkbox, recalculamos TodosListos
                item.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == nameof(DetallePedidoItemViewModel.EstaListo))
                    {
                        OnPropertyChanged(nameof(TodosListos));
                        //se avisa al cocinaviewmodel que mire los comandos.
                        _onCheckboxCambiado();
                    }
                };

                Detalles.Add(item);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }

    // Clase que representa un producto dentro del pedido con su checkbox
    public class DetallePedidoItemViewModel : INotifyPropertyChanged
    {
        // El detalle real con el producto y la cantidad
        public DetallePedido Detalle { get; set; }

        // El checkbox — cuando cambia avisa a la UI
        private bool _estaListo = false;
        public bool EstaListo
        {
            get => _estaListo;
            set
            {
                _estaListo = value;
                OnPropertyChanged();
            }
        }

        private bool _editable = false;
        public bool Editable
        {
            get { return _editable; }
            set
            {
                _editable = value;
                OnPropertyChanged();
            }
        }

        public DetallePedidoItemViewModel(DetallePedido detalle,bool editable)
        {
            Detalle = detalle;
            _editable = editable; // le metemos aquí la referencia de editable al constructor pa que sepa en qué estado andaba el prodcuto al inicar.
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? nombre = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
    }
}
