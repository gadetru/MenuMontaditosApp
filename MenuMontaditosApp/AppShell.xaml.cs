namespace MenuMontaditosApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // navegación entre las distintas vistas 
            // Registramos las rutas de las páginas pa poder navegar a ellas con GoToAsync
            Routing.RegisterRoute("CartaMenuPage", typeof(Views.CartaMenuPage));
            Routing.RegisterRoute("CocinaPage", typeof(Views.CocinaPage));
            Routing.RegisterRoute("BarraPage", typeof(Views.BarraPage));
            Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
            Routing.RegisterRoute("ConfiguracionInicialPage", typeof(Views.ConfiguracionInicialPage));
            Routing.RegisterRoute("SeleccionPage", typeof(Views.SeleccionPage));
            Routing.RegisterRoute("SeleccionRolPage", typeof(Views.SeleccionRolPage));
            Routing.RegisterRoute("UsuariosPage", typeof(Views.UsuariosPage));

        }
    }
}
