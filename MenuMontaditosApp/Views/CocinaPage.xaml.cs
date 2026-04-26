using MenuMontaditosApp.ViewModels;

namespace MenuMontaditosApp.Views
{
    public partial class CocinaPage : ContentPage
    {
        // Aqui guardamos el ViewModel pa poder llamarlo desde los métodos de los botones.
        // Antes en WPF usábamos el RelativeSource desde el XAML, en MAUI lo hacemos asi
        // que es más facil de entender.
        private CocinaViewModel _viewModel;

        public CocinaPage()
        {
            InitializeComponent();
            _viewModel = new CocinaViewModel();
            BindingContext = _viewModel;
        }

        // Boton "En proceso" — recogemos el pedido del CommandParameter
        // y se lo pasamos al ViewModel pa que lo procese
        private void EnProceso_Click(object sender, EventArgs e)
        {
            Button? boton = sender as Button;
            if (boton == null) return;

            PedidoItemViewModel? pedido = boton.CommandParameter as PedidoItemViewModel;

            // Reusamos el comando que ya tiene el ViewModel,
            // asi no duplicamos lógica
            _viewModel.PedidoEnProcesoCommand.Execute(pedido);

            // Una vez en proceso, activamos los checkboxes del pedido pa que se puedan marcar
            foreach (DetallePedidoItemViewModel detalle in pedido.Detalles)
            {
                detalle.Editable = true;
            }
        }

        private async void Completado_Click(object sender, EventArgs e)
        {
            Button? boton = sender as Button;
            if (boton == null) return;
            
            PedidoItemViewModel? pedido = boton.CommandParameter as PedidoItemViewModel;
            
            // Comprobamos que se puede ejecutar antes de hacerlo.
            // Si no se puede (no estan todos los checks marcados), avisamos al usuario.
            if (_viewModel.PedidoCompletadoCommand.CanExecute(pedido))
            {
                _viewModel.PedidoCompletadoCommand.Execute(pedido);
            }
            else
            {
                await DisplayAlertAsync(
                    "Pedido no completado",
                    "Hay productos sin marcar como listos.",
                    "Vale"
                );
            }
        }
    }
}