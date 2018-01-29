using System;
using System.Globalization;
using System.Text;
using System.Windows.Data;
using VeterinarianClinic.Domain;

namespace VeterinarianClinic.View.Converter
{
    [ValueConversion(typeof(Address), typeof(string))]
    class AddressConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Address && value != null)
            {
                Address address = (Address)value;
                StringBuilder str = new StringBuilder();
                str.Append(address.Line1).Append(", ");

                if (!string.IsNullOrEmpty(address.Apartment))
                {
                    str.Append("Apt: ").Append(address.Apartment).Append(", ");
                }

                str.Append(address.City).Append(", ").Append(address.Province.ToString());
                str.Append(" - ").Append(address.PostalCode);

                return str.ToString();
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
