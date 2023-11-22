namespace MPCMobile.Helpers
{
    interface IDisplayAlert
    {
        void ShowAlert(string title, string message);
    }

    public class DisplayAlert : IDisplayAlert
    {
        public void ShowAlert(string title, string message)
        {
            App.Current.MainPage.DisplayAlert(title, message, "Ok");
        }
    }
}
