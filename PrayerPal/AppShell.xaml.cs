namespace PrayerPal
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Appearing += OnShellAppearing;




        }


      
        private void OnShellAppearing(object sender, System.EventArgs e)
        {
            // Set the CurrentItem after the Shell has appeared
#if ANDROID || IOS
CurrentItem = HomePhone;
#else
CurrentItem = HomeDesktop;

#endif

        

            // You may want to remove the event handler after it's used once, 
            // if you don't need to set CurrentItem dynamically afterwards
            Appearing -= OnShellAppearing;
        }

    }
}
