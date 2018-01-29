using System.Windows;

namespace VeterinarianClinic.View.Views
{
    public delegate void Action();

    /// <summary>
    /// Generic interface that defines actions for any editor.
    /// </summary>
    public interface IEditor<T>
    {
        T Entity { get; set; }

        Visibility Visibility { get; set; }

        event Action OnSave;

        event Action OnCancel;

        void Clean();

        void Reload();
    }
}
