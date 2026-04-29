using MenuMontaditosApp.Models;
using MenuMontaditosApp.ViewModels;
using System.Globalization;

namespace MenuMontaditosApp.Views
{
    public partial class ProductosCrudPage : ContentPage
    {
        private ProductosCrudViewModel _viewModel;

        // Aqui guardamos el producto que se está editando.
        // Si es null, significa que estamos creando uno nuevo.
        private Producto? _productoEditando;

        public ProductosCrudPage()
        {
            InitializeComponent();
            _viewModel = new ProductosCrudViewModel();
            BindingContext = _viewModel;
        }

        // Botón "Nuevo producto" — abre el formulario vacío
        private void MostrarFormulario_Click(object sender, EventArgs e)
        {
            _productoEditando = null;
            LabelTitulo.Text = "Nuevo producto";

            // Limpiamos los campos
            EntryNombre.Text = string.Empty;
            EntryPrecio.Text = string.Empty;
            PickerCategoria.SelectedItem = null;
            LabelError.IsVisible = false;

            // Mostramos el panel y ocultamos el botón "Nuevo producto"
            PanelFormulario.IsVisible = true;
            BotonNuevo.IsVisible = false;
        }

        // Botón "Editar" — abre el formulario con los datos del producto
        private void Editar_Click(object sender, EventArgs e)
        {
            Button? boton = sender as Button;
            if (boton == null) return;

            Producto? producto = boton.CommandParameter as Producto;
            if (producto == null) return;

            _productoEditando = producto;
            LabelTitulo.Text = "Editar producto";

            // Rellenamos los campos con los datos del producto
            EntryNombre.Text = producto.Nombre;
            EntryPrecio.Text = producto.Precio.ToString(CultureInfo.InvariantCulture);
            PickerCategoria.SelectedItem = producto.Categoria.ToString();
            LabelError.IsVisible = false;

            PanelFormulario.IsVisible = true;
            BotonNuevo.IsVisible = false;
        }

        // Botón "Cancelar" — cierra el formulario sin guardar
        private void Cancelar_Click(object sender, EventArgs e)
        {
            PanelFormulario.IsVisible = false;
            BotonNuevo.IsVisible = true;
        }

        // Botón "Guardar" — crea o modifica según el caso
        private void Guardar_Click(object sender, EventArgs e)
        {
            string nombre = EntryNombre.Text;
            string precioTexto = EntryPrecio.Text;
            string? categoriaSeleccionada = PickerCategoria.SelectedItem as string;

            // Validaciones básicas
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(precioTexto))
            {
                LabelError.Text = "Rellena nombre y precio";
                LabelError.IsVisible = true;
                return;
            }

            if (categoriaSeleccionada == null)
            {
                LabelError.Text = "Selecciona una categoría";
                LabelError.IsVisible = true;
                return;
            }

            // Convertimos el precio a decimal. Aceptamos punto y coma.
            string precioNormalizado = precioTexto.Replace(",", ".");
            decimal precio;
            bool precioValido = decimal.TryParse(precioNormalizado, NumberStyles.Any, CultureInfo.InvariantCulture, out precio);

            if (precioValido == false)
            {
                LabelError.Text = "El precio no es válido";
                LabelError.IsVisible = true;
                return;
            }

            // Convertimos el string del Picker al enum
            Enums.CategoriaProducto categoria = Enum.Parse<Enums.CategoriaProducto>(categoriaSeleccionada);

            // Si _productoEditando es null, estamos creando.
            // Si tiene un valor, estamos modificando.
            if (_productoEditando == null)
            {
                _viewModel.CrearProducto(nombre, precio, categoria);
            }
            else
            {
                _viewModel.ModificarProducto(_productoEditando.IdProducto, nombre, precio, categoria);
            }

            // Cerramos el formulario
            PanelFormulario.IsVisible = false;
            BotonNuevo.IsVisible = true;
        }

        // Botón "Eliminar"
        private async void Eliminar_Click(object sender, EventArgs e)
        {
            Button? boton = sender as Button;
            if (boton == null) return;

            Producto? producto = boton.CommandParameter as Producto;
            if (producto == null) return;

            bool confirmado = await DisplayAlertAsync(
                "¿Eliminar producto?",
                $"¿Seguro que quieres eliminar '{producto.Nombre}'?",
                "Sí, eliminar",
                "Cancelar"
            );

            if (confirmado)
            {
                _viewModel.EliminarProducto(producto);
            }
        }
    }
}