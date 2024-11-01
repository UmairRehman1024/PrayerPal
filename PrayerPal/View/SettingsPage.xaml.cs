namespace PrayerPal.View;

public partial class SettingsPage : ContentPage
{
    //IService Services;
	public SettingsPage()
    {
        InitializeComponent();
        //Services = Services_;
    }

    //method to start manually foreground service
    private void OnServiceStartClicked(object sender, EventArgs e)
    {
        //Services.Start();
    }

    //method to stop manually foreground service
    private void Button_Clicked(object sender, EventArgs e)
    {
        //Services.Stop();
    }

    private void Button_Clicked_1(object sender, EventArgs e)
    {
        Vibration.Vibrate();
    }
}
