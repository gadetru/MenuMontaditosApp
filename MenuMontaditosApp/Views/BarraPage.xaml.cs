using MenuMontaditosApp.ViewModels;

namespace MenuMontaditosApp.Views
{
    public partial class BarraPage : ContentPage
    {
        private BarraViewModel _viewModel;

        public BarraPage()
        {
            InitializeComponent();
            _viewModel = new BarraViewModel();
            BindingContext = _viewModel;
        }

        private async void Archivar_Click(object sender, EventArgs e)
        {
            Button? boton = sender as Button;
            if (boton == null) return;

            BarraPedidoItemViewModel? pedido = boton.CommandParameter as BarraPedidoItemViewModel;
            if (pedido == null) return;

            // Si se puede archivar (pedido completado), lo hacemos.
            // Si no, avisamos al usuario.
            if (_viewModel.ArchivarPedidoCommand.CanExecute(pedido))
            {
                _viewModel.ArchivarPedidoCommand.Execute(pedido);
            }
            else
            {
                await DisplayAlertAsync(
                    "Pedido no listo",
                    "Solo se pueden archivar pedidos completados.",
                    "Vale"
                );
            }
        }

        private async void GestionProductos_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("ProductosCrudPage");
        }
    }
}