using MenuMontaditosApp.Models;
using MenuMontaditosApp.ViewModels;

namespace MenuMontaditosApp.Views
{
    public partial class UsuariosPage : ContentPage
    {
        private UsuariosViewModel _viewModel;

        public UsuariosPage()
        {
            InitializeComponent();
            _viewModel = new UsuariosViewModel();
            BindingContext = _viewModel;
        }

        // Botón "Crear usuario"
        private async void Crear_Click(object sender, EventArgs e)
        {
            string nombre = EntryNuevoUsuario.Text;
            string password = EntryNuevoPassword.Text;
            string? rolSeleccionado = PickerRol.SelectedItem as string;

            // Validaciones básicas
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(password))
            {
                LabelError.Text = "Rellena nombre y contraseña";
                LabelError.IsVisible = true;
                return;
            }

            if (password.Length < 4)
            {
                LabelError.Text = "La contraseña debe tener al menos 4 caracteres";
                LabelError.IsVisible = true;
                return;
            }

            if (rolSeleccionado == null)
            {
                LabelError.Text = "Selecciona un rol";
                LabelError.IsVisible = true;
                return;
            }

            // Convertimos el string del Picker al enum
            Enums.RolUsuario rol = Enum.Parse<Enums.RolUsuario>(rolSeleccionado);

            // Creamos el usuario llamando al ViewModel
            _viewModel.CrearUsuario(nombre, password, rol);

            // Limpiamos los campos pa que se queden vacíos pa el siguiente
            EntryNuevoUsuario.Text = string.Empty;
            EntryNuevoPassword.Text = string.Empty;
            PickerRol.SelectedItem = null;
            LabelError.IsVisible = false;

            await DisplayAlertAsync(
                "Listo",
                $"Usuario {nombre} creado correctamente",
                "Vale"
            );
        }

        // Botón "Eliminar"
        private async void Eliminar_Click(object sender, EventArgs e)
        {
            Button? boton = sender as Button;
            if (boton == null) return;

            Usuario? usuario = boton.CommandParameter as Usuario;
            if (usuario == null) return;

            // Pedimos confirmación antes de eliminar pa evitar accidentes
            bool confirmado = await DisplayAlertAsync(
                "¿Eliminar usuario?",
                $"¿Seguro que quieres eliminar a {usuario.Nombre}?",
                "Sí, eliminar",
                "Cancelar"
            );

            if (confirmado)
            {
                _viewModel.EliminarUsuario(usuario);
            }
        }
    }
}