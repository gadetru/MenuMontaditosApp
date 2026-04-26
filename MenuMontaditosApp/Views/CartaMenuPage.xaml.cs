using MenuMontaditosApp.Models;
using MenuMontaditosApp.ViewModels;

namespace MenuMontaditosApp.Views
{
    public partial class CartaMenuPage : ContentPage
    {
        public CartaMenuPage()
        {
            InitializeComponent();
            BindingContext = new MenuClienteViewModel();
        }

        private void Comida_Click(object sender, EventArgs e)
        {
            (BindingContext as MenuClienteViewModel).CargarPorCategoria(Enums.CategoriaProducto.Comida);
        }

        private void Bebida_Click(object sender, EventArgs e)
        {
            (BindingContext as MenuClienteViewModel).CargarPorCategoria(Enums.CategoriaProducto.Bebida);
        }

        private void Postre_Click(object sender, EventArgs e)
        {
            (BindingContext as MenuClienteViewModel).CargarPorCategoria(Enums.CategoriaProducto.Postre);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var producto = boton.CommandParameter as Producto;
            (BindingContext as MenuClienteViewModel).AgregarProducto(producto);
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var linea = boton.CommandParameter as LineaPedido;
            (BindingContext as MenuClienteViewModel).QuitarProducto(linea);
        }

        private void Add_Click_Carrito(object sender, EventArgs e)
        {
            var boton = sender as Button;
            var linea = boton.CommandParameter as LineaPedido;
            (BindingContext as MenuClienteViewModel).AgregarLineaPedido(linea);
        }
    }
}