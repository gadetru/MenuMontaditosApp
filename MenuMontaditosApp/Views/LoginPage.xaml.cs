using MenuMontaditosApp.Models;
using MenuMontaditosApp.Services;

namespace MenuMontaditosApp.Views
{
    public partial class LoginPage : ContentPage
    {
        private UsuarioService _usuarioService;

        public LoginPage()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
        }

        private async void Entrar_Click(object sender, EventArgs e)
        {
            string usuario = EntryUsuario.Text;
            string password = EntryPassword.Text;

            // Comprobamos que haya escrito algo en los dos campos
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                LabelError.Text = "Rellena usuario y contraseña";
                LabelError.IsVisible = true;
                return;
            }

            // Intentamos iniciar sesión con el servicio
            Usuario? usuarioLogueado = _usuarioService.IniciarSesion(usuario, password);

            if (usuarioLogueado == null)
            {
                LabelError.Text = "Usuario o contraseña incorrectos";
                LabelError.IsVisible = true;
                return;
            }

            // Login correcto. Redirigimos según el rol del usuario
            if (usuarioLogueado.Rol == Enums.RolUsuario.Cocina)
            {
                await Shell.Current.GoToAsync("CocinaPage");
            }
            else if (usuarioLogueado.Rol == Enums.RolUsuario.Administrador)
            {
                // El admin va a la SeleccionPage donde puede elegir las 3 vistas
                await Shell.Current.GoToAsync("SeleccionPage");
            }
        }
    }
}