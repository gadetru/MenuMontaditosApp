using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MenuMontaditosApp.Models;


namespace MenuMontaditosApp.Converters
{
    // Esta clase la usa el XAML para convertir un EstadoPedido en un Color de fondo.
    // Le pasas un estado, te devuelve el color que toca pa esa tarjeta.
    public class EstadoAColorConverter : IValueConverter
    {
        // Este método lo llama el XAML solito cuando le pasamos un valor por binding.
        // value es lo que llega del binding (en nuestro caso un EstadoPedido).
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Si lo que nos llega no es un EstadoPedido, devolvemos blanco por defecto y au.
            if (value is Enums.EstadoPedido estado)
            {
                if (estado == Enums.EstadoPedido.Proceso)
                {
                    // Naranjita pa los pedidos en proceso
                    return Color.FromArgb("#FFE0B2");
                }

                if (estado == Enums.EstadoPedido.Completado)
                {
                    // Verdecito pa los completados
                    return Color.FromArgb("#C8E6C9");
                }
            }

            // Pa los pendientes o cualquier otra cosa, blanco
            return Colors.White;
        }

        // Este método es pa la conversión inversa, pero como no la vamos a usar,
        // lo dejamos sin hacer nada. MAUI lo pide igual aunque no lo usemos.
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
