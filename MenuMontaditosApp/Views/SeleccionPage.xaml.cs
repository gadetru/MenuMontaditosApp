namespace MenuMontaditosApp.Views
{
    public partial class SeleccionPage : ContentPage
    {
        public SeleccionPage()
        {
            InitializeComponent();
        }

        private async void BtnCliente_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("CartaMenuPage");
        }

        private async void BtnCocina_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("CocinaPage");
        }

        private async void BtnBarra_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("BarraPage");
        }
        private async void BtnRoles_click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("UsuariosPage");
        }
    }
}