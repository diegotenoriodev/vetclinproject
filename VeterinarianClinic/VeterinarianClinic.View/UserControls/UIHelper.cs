using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace VeterinarianClinic.View.UserControls
{
    /// <summary>
    /// Centralizing some shared behavior used in views.
    /// </summary>
    public class UIHelper
    {
        private static BrushConverter converter;

        public static Brush GetBrushFromString(string str)
        {
            if (converter == null)
            {
                converter = new BrushConverter();
            }

            return (Brush)converter.ConvertFromString(str);
        }

        private static ImageSourceConverter imgConverter;

        public static ImageSource GetImageSourceFromString(string source)
        {
            if (imgConverter == null)
            {
                imgConverter = new ImageSourceConverter();
            }

            return (ImageSource)imgConverter.ConvertFromString(source);
        }

        public static string GetStringFromList(List<string> list)
        {
            StringBuilder str = new StringBuilder();

            for (int i = 0; i < list.Count; i++)
            {
                str.Append("- ").Append(list[i]).Append("\n");
            }

            return str.ToString();
        }
    }
}
