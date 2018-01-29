using System;
using System.Windows.Media;
using System.Globalization;
using System.Windows.Data;

namespace VeterinarianClinic.View.Converter
{
    [ValueConversion(typeof(DateTime), typeof(Brush))]
    public class ChangeColorAppointmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return Brushes.Black;

            DateTime.TryParse(value.ToString(), out DateTime appDate);

            if (appDate < DateTime.Now)
                return Brushes.Gray;
            
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        
    }
}