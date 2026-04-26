namespace MenuMontaditosApp.Views
{
    public partial class SeleccionRolPage : ContentPage
    {
        public SeleccionRolPage()
        {
            InitializeComponent();
        }

        // El cliente entra directo a la vista de la carta, sin login
        private async void Cliente_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("CartaMenuPage");
        }

        // El personal pasa primero por la pantalla de login
        private async void Personal_Click(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("LoginPage");
        }
    }
}