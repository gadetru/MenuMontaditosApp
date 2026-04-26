using MenuMontaditosApp.Models;
using MenuMontaditosApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MenuMontaditosApp.ViewModels
{
    public class UsuariosViewModel : INotifyPropertyChanged
    {
        // Lista de usuarios que se mostrará en la vista
        private ObservableCollection<Usuario> _usuarios;
        public ObservableCollection<Usuario> Usuarios
        {
            get { return _usuarios; }
            set
            {
                _usuarios = value;
                OnPropertyChanged();
            }
        }

        // Servicio para hablar con la BD
        private UsuarioService _usuarioService;

        public UsuariosViewModel()
        {
            _usuarioService = new UsuarioService();
            _usuarios = new ObservableCollection<Usuario>();

            CargarUsuarios();
        }

        // Carga la lista de usuarios desde la BD
        public void CargarUsuarios()
        {
            _usuarios.Clear();

            List<Usuario> lista = _usuarioService.ObtenerUsuarios();

            foreach (Usuario usuario in lista)
            {
                _usuarios.Add(usuario);
            }
        }

        // Elimina un usuario de la BD y lo quita de la lista
        public void EliminarUsuario(Usuario usuario)
        {
            _usuarioService.EliminarUsuario(usuario.IdUsuario);
            _usuarios.Remove(usuario);
        }

        // Crea un usuario nuevo y recarga la lista pa que se vea
        public void CrearUsuario(string nombre, string password, Enums.RolUsuario rol)
        {
            _usuarioService.CrearUsuario(nombre, password, rol);
            CargarUsuarios();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? nombre = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nombre));
        }
    }
}