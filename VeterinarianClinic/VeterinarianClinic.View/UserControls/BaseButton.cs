using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VeterinarianClinic.View.UserControls
{
    /// <summary>
    /// Centralizes the shared settings for buttons in this application.
    /// Prevents the need for changing button's settings in every editor.
    /// </summary>
    public abstract class BaseButton : Button
    {
        public BaseButton()
        {
            HorizontalAlignment = HorizontalAlignment.Left;
            VerticalAlignment = VerticalAlignment.Top;
            Width = 150;
            Height = 30;
            FontSize = 16;
            Foreground = Brushes.White;
            FontWeight = FontWeight.FromOpenTypeWeight(600);
        }
    }
}
