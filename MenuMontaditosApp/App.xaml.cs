using Microsoft.Extensions.DependencyInjection;
using MenuMontaditosApp.Services; // no olvidemos importar el service...

namespace MenuMontaditosApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // Creamos la ventana principal con appshell como siempre:
            Window ventana = new Window(new AppShell());

            // una vez creada, probamos si  hay usuarios creados o no.

            ventana.Created += async (sender, e) =>
            {
                UsuarioService usuarioService = new UsuarioService();

                if (usuarioService.HayUsuarios() == false) // esto significará que no ha encontrao usuarios registrados.
                {
                    await Shell.Current.GoToAsync("ConfiguracionInicialPage"); // nos vamos a la ventana de registro de usuario.
                }
            };
            return ventana; // y ya estaría :)
        }
    }
}