using MenuMontaditosApp.Models;
using MenuMontaditosApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MenuMontaditosApp.Helpers;

namespace MenuMontaditosApp.ViewModels
{
    internal class MenuClienteViewModel : INotifyPropertyChanged // heredamos esta interfaz para que la UI note los cambios
    {
        //************ Pruductos del barete********
        private ObservableCollection<Producto> productos;
        public ObservableCollection<Producto> Productos
        {
            get => productos;
            set
            {
                productos = value;
                OnPropertyChanged(); // aquí notificamos de cambios a la UI cuando se setee el campo productos.
            }
        }

        //********************** el carro de la compra**********************
        public ObservableCollection<LineaPedido> Carrito { get; set; } = new();  // aquí la lista de lo que voy a pedir pa come.
        public decimal Total => Carrito.Sum(x => x.Producto.Precio * x.Cantidad);  // propiedad para calcular el total de la cuenta.

        //********* el cliente **************

        private string _nombreCliente = string.Empty;
        public string NombreCliente
        {
            get => _nombreCliente;
            set
            {
                _nombreCliente = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PuedeConfirmar)); // recalcula si el botón está activo
                (ConfirmarPedidoCommand as RelayCommand)?.RaiseCanExecuteChanged(); // le digo al helper que ya podemos levantar el boton
            }
        }


        // El botón sólo se activa si hay nombre Y hay algo en el carrito
        public bool PuedeConfirmar =>
            !string.IsNullOrWhiteSpace(NombreCliente) && Carrito.Count > 0;

        // ── Comando confirmar pedido ───────────────────────────────
        public ICommand ConfirmarPedidoCommand { get; }

        // ************* los servicios para conectar BD*************
        private readonly ProductoService _productoService;
        private readonly PedidoService _pedidoService;

        public MenuClienteViewModel()
        {
            _productoService = new ProductoService();
            _pedidoService = new PedidoService();

            ConfirmarPedidoCommand = new RelayCommand(
                execute: delegate (object parametro) { ConfirmarPedido(); },
                canExecute: delegate (object parametro) { return PuedeConfirmar; }
            );
            Carrito.CollectionChanged += (_, _) =>
            {
                OnPropertyChanged(nameof(PuedeConfirmar));
                OnPropertyChanged(nameof(Total));
            };
            
        }

        //****************** Lógica del carro***********
        private void CargarProductos()
        {
            var lista = _productoService.ObtenerProductos();
            Productos = new ObservableCollection<Producto>(lista);
        }

        public void CargarPorCategoria(Enums.CategoriaProducto categoria)
        {
            var lista = _productoService.ObtenerPorCategoria(categoria);
            Productos = new ObservableCollection<Producto>(lista);
        }

        //un cambio, una funcioncita para que sean los productos del carro los que tengan la opcion pa quitar y agregar más productos de un tipo.*
        public void AgregarLineaPedido(LineaPedido linea)
        {
            AgregarProducto(linea.Producto);
            OnPropertyChanged(nameof(Total)); // pa que actualice el precio.
        }

        //************** logica para el carrito de productos para pedidos ******************
        public void AgregarProducto(Producto Producto)
        {
            var linea = Carrito.FirstOrDefault(x => x.Producto.IdProducto == Producto.IdProducto);
            
            if(linea == null)
            {
                Carrito.Add(new LineaPedido { Producto = Producto, Cantidad = 1 });
            }
            else
            {
                linea.Cantidad++;
            }
            OnPropertyChanged(nameof(Total));

        }
        public void QuitarProducto(LineaPedido linea)
        {
            
            if(linea != null)
            {
                linea.Cantidad--;
                if(linea.Cantidad <= 0)
                {
                    Carrito.Remove(linea);
                }
            }
            OnPropertyChanged(nameof(Total));
            OnPropertyChanged(nameof(PuedeConfirmar));
            (ConfirmarPedidoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        // ── Confirmar pedido ───────────────────────────────────────
        private void ConfirmarPedido()
        {
            // Guardar en BBDD
            _pedidoService.CrearPedido(NombreCliente, Carrito.ToList());

            // Popup de confirmación
            //string nombre = NombreCliente;
            //MessageBox.Show(
            //    $"¡Gracias, {nombre}!\n\nTu pedido está en camino.\nEn unos minutos te llamaremos.",
            //    "Pedido confirmado",
            //    MessageBoxButton.OK,
            //    MessageBoxImage.Information
            //);

            // Reset completo
            ResetearVista();
        }

        public void ResetearVista()
        {
            Carrito.Clear();
            NombreCliente = string.Empty;
            Productos = new ObservableCollection<Producto>();
            OnPropertyChanged(nameof(Total));
            (ConfirmarPedidoCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }

        
    }
}
