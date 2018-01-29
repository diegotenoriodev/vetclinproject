namespace VeterinarianClinic.View.UserControls
{
    public class ButtonSave : BaseButton
    {
        /// <summary>
        /// All editors will have a button to save.
        /// This button should be used because it presents the settings in a centralized place.
        /// </summary>
        public ButtonSave()
        {
            Content = "Save";
            Background = UIHelper.GetBrushFromString("#FF007ACC");
        }
    }
}
