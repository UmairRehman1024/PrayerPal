




using Microsoft.Toolkit.Uwp.Notifications;
using Syncfusion.Maui.Core.Carousel;

namespace PrayerPal.View;

public partial class HomePage : ContentPage
{
	HomePageViewModel ViewModel;




	
    public HomePage(HomePageViewModel viewModel)
		{
			InitializeComponent();
			this.ViewModel = viewModel;
			BindingContext = ViewModel;

        //Task.Run(async() =>
        //{
        //    await viewModel.GetPrayerTimesOnStart();
        //}).Wait();

        Appearing += OnShellAppearing;

    }

    private async void  OnShellAppearing(object sender, EventArgs e)
    {
        Preferences.Default.Clear();

        await ViewModel.GetPrayerTimesOnStart();
        Appearing -= OnShellAppearing;
    }

    protected async override void OnAppearing()
    {
        //#if ANDROID
        //		Preferences.Default.Clear();
        //        await ViewModel.GetPrayerTimes();

        //        //await ViewModel.AddNotifications();
        //        alarmService.ScheduleAlarm(DateTime.Now.AddMinutes(1));
        //#else
        //        Preferences.Default.Clear();
        //        await ViewModel.GetPrayerTimes();
        //        //await ViewModel.AddNotifications();
        //#endif

        //        await ViewModel.GetPrayerTimes();
        //#if ANDROID || IOS
        //        await ViewModel.AddNotifications();
        //#endif



    }

}