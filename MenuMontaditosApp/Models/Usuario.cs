using System;
using System.Collections.Generic;
using System.Text;

namespace MenuMontaditosApp.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = String.Empty; 
        public string Contraseña { get; set; } = String.Empty; 
        public Enums.RolUsuario  Rol { get; set; }  
    }
}
