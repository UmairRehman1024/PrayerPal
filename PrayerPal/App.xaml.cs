namespace PrayerPal
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NAaF1cVGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEFjXX1ZcXBXQWFZVEZxXw==");

            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
