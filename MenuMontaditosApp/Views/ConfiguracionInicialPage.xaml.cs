using MenuMontaditosApp.Models;
using MenuMontaditosApp.Services;

namespace MenuMontaditosApp.Views
{
    public partial class ConfiguracionInicialPage : ContentPage
    {
        private UsuarioService _usuarioService;

        public ConfiguracionInicialPage()
        {
            InitializeComponent();
            _usuarioService = new UsuarioService();
        }

        private async void Crear_Click(object sender, EventArgs e)
        {
            string usuario = EntryUsuario.Text;
            string password = EntryPassword.Text;
            string passwordRepetir = EntryPasswordRepetir.Text;

            // Validamos que estén todos los campos rellenos
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordRepetir))
            {
                LabelError.Text = "Rellena todos los campos";
                LabelError.IsVisible = true;
                return;
            }

            // Validamos que la contraseña tenga un mínimo de caracteres
            if (password.Length < 4)
            {
                LabelError.Text = "La contraseña debe tener al menos 4 caracteres";
                LabelError.IsVisible = true;
                return;
            }

            // Validamos que las dos contraseñas sean iguales
            if (password != passwordRepetir)
            {
                LabelError.Text = "Las contraseñas no coinciden";
                LabelError.IsVisible = true;
                return;
            }

            // Si todo va bien, creamos el admin
            _usuarioService.CrearUsuario(usuario, password, Enums.RolUsuario.Administrador);

            await DisplayAlertAsync(
                "Listo",
                "Administrador creado correctamente. Ya puedes iniciar sesión.",
                "Vale"
            );

            // Limpiamos los campos pa que no quede el formulario relleno
            EntryUsuario.Text = string.Empty;
            EntryPassword.Text = string.Empty;
            EntryPasswordRepetir.Text = string.Empty;
            LabelError.IsVisible = false;

            // Mostramos el aviso. Como es await, esperamos a que el usuario pulse "Vale"
            await DisplayAlertAsync(
                "Listo",
                "Administrador creado correctamente. Ya puedes iniciar sesión.",
                "Vale"
            );

            // Una vez aceptado, redirigimos al login
            await Shell.Current.GoToAsync("//SeleccionRolPage"); // el doble // me borra el historial de navegacion,para evitar que me vuelvan a leer el formulario los canallas
        }
    }
}