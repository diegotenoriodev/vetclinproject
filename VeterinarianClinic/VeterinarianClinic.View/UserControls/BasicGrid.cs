using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace VeterinarianClinic.View.UserControls
{
    /// <summary>
    /// This is a customized grid, that inherits from the WPF's DataGrid, thus it presents the 
    /// same structure.
    /// However, in order to optimize the development and centralize the settings and behaviors,
    /// we created this class that is used in the same fashion as the WPF's DataGrid, but
    /// it presents controls for firing edit and delete actions, color, font and tooltips.
    /// About the usage, the framework allows the developer to only add the columns that will
    /// be shown in the grid, as well as specific settings.
    /// </summary>
    public class BasicGrid : DataGrid
    {
        public delegate void ItemAction(object item);

        /// <summary>
        /// This event is called when a user clicks on edit.
        /// It sends the selected object, so the client can do what is necessary.
        /// </summary>
        public event ItemAction OnEdit;

        /// <summary>
        /// This event is called when a user clicks on delete.
        /// It sends the selected object, so the client can do what is necessary.
        /// </summary>
        public event ItemAction OnDelete;

        /// <summary>
        /// It is used to control the actions.
        /// </summary>
        public enum Action
        {
            None,
            Edit,
            Delete
        }

        private Action action;
        
        public BasicGrid()
        {
            RowHeaderWidth = 0;
            AutoGenerateColumns = false;
            SelectionChanged += BasicGrid_SelectionChanged;
            AlternatingRowBackground = UIHelper.GetBrushFromString("#FFDADDF3");
            RowBackground = UIHelper.GetBrushFromString("#FFBAC7F5");
            CanUserResizeRows = false;
            RowDetailsVisibilityMode = DataGridRowDetailsVisibilityMode.Visible;
            CanUserResizeColumns = false;
            UseLayoutRounding = true;
            CanUserAddRows = false;

            AddColumnImageButton("", Properties.Resources.edit, 
                new MouseButtonEventHandler(ImgEdit_MouseDown), "Click to edit this item.");
            AddColumnImageButton("", Properties.Resources.remove, 
                new MouseButtonEventHandler(ImgRemove_MouseDown), "Click to remove this item.");
        }

        public ImageSource ImageSourceForBitmap(System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally { GC.SuppressFinalize(handle); }
        }

        private void AddColumnImageButton(string header, System.Drawing.Bitmap img, MouseButtonEventHandler evt, string toolTip)
        {
            DataGridTemplateColumn col1 = new DataGridTemplateColumn
            {
                Header = header
            };

            FrameworkElementFactory factory1 = new FrameworkElementFactory(typeof(Image));

            factory1.SetValue(Image.WidthProperty, 20.0);
            factory1.SetValue(Image.HeightProperty, 20.0);
            factory1.SetValue(Image.CursorProperty, Cursors.Hand); 
            factory1.SetValue(Image.ToolTipProperty, toolTip);
            factory1.SetValue(Image.SourceProperty, ImageSourceForBitmap(img));
            factory1.AddHandler(Image.MouseDownEvent, evt);


            DataTemplate cellTemplate1 = new DataTemplate
            {
                VisualTree = factory1
            };
            
            col1.CellTemplate = cellTemplate1;

            Columns.Add(col1);
        }

        private void ImgEdit_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Sets the action based on the image that was clicked
            action = Action.Edit;
        }

        private void ImgRemove_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //Sets the action based on the image that was clicked
            action = Action.Delete;
        }

        /// <summary>
        /// This event is called every time that the user clicks on some cell in the grid.
        /// The Img Mouse Down is called first, determining what action caused the event.
        /// If it was caused by a click on some information, no action should be taken.
        /// </summary>
        private void BasicGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (action != Action.None)
            {
                if (CurrentItem != null)
                {
                    switch (action)
                    {
                        case Action.Edit:
                            OnEdit?.Invoke(CurrentItem);
                            break;
                        case Action.Delete:
                            OnDelete?.Invoke(CurrentItem);
                            break;
                        default:
                            break;
                    }
                }
            }

            action = Action.None;
            SelectedItem = null;
        }
    }
}
