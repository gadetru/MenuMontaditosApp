using MenuMontaditosApp.Models;
using MenuMontaditosApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MenuMontaditosApp.ViewModels
{
    public class ProductosCrudViewModel : INotifyPropertyChanged
    {
        // Lista de productos que se muestra en la vista
        private ObservableCollection<Producto> _productos;
        public ObservableCollection<Producto> Productos
        {
            get { return _productos; }
            set
            {
                _productos = value;
                OnPropertyChanged();
            }
        }

        // Servicio para hablar con la BD
        private ProductoService _productoService;

        public ProductosCrudViewModel()
        {
            _productoService = new ProductoService();
            _productos = new ObservableCollection<Producto>();

            CargarProductos();
        }

        // Carga la lista entera de productos desde la BD
        public void CargarProductos()
        {
            _productos.Clear();

            List<Producto> lista = _productoService.ObtenerProductos();

            foreach (Producto producto in lista)
            {
                _productos.Add(producto);
            }
        }

        // Crea un producto nuevo y recarga la lista
        public void CrearProducto(string nombre, decimal precio, Enums.CategoriaProducto categoria)
        {
            _productoService.CrearProducto(nombre, precio, categoria);
            CargarProductos();
        }

        // Modifica un producto existente y recarga la lista
        public void ModificarProducto(int id, string nombre, decimal precio, Enums.CategoriaProducto categoria)
        {
            _productoService.ModificarProducto(id, nombre, precio, categoria);
            CargarProductos();
        }

        // Elimina un producto de la BD y de la lista
        public void EliminarProducto(Producto producto)
        {
            _productoService.EliminarProducto(producto.IdProducto);
            _productos.Remove(producto);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}