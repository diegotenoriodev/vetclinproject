namespace VeterinarianClinic.View.UserControls
{
    public class ButtonCancel : BaseButton
    {
        /// <summary>
        /// All editors will have a button to cancel.
        /// This button should be used because it presents the settings in a centralized place.
        /// </summary>
        public ButtonCancel()
        {
            Content = "Cancel";
            Background = UIHelper.GetBrushFromString("#FFFF262A");
        }
    }
}
