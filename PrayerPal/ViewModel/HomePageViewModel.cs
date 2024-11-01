using PrayerPal.Services;
using Plugin.LocalNotification;




namespace PrayerPal.ViewModel
{
    public partial class HomePageViewModel : BaseViewModel
    {
        [ObservableProperty]
        bool isRefreshing;

        public ObservableCollection<PrayerTime> PrayerTimes { get; } = new();

        InternalNotificationsService internalNotificationsService;
        PrayerTimeService PrayerTimeService;

        //IService Services;
        //public HomePageViewModel(PrayerTimeService prayerTimeService, IService Services_, InternalNotificationsService internalNotificationsService)
        public HomePageViewModel(PrayerTimeService prayerTimeService, InternalNotificationsService internalNotificationsService)

        {
            Title = "Home";


            this.PrayerTimeService = prayerTimeService;
            //this.Services = Services_;
            this.internalNotificationsService = internalNotificationsService;
        }





        public async Task GetPrayerTimes()
        {
            //if (IsBusy) return;
            try
            {
                //IsBusy = true;

                Collection<PrayerTime> PrayerTimesTemp = await PrayerTimeService.GetPrayerTimesAsync();

                if (PrayerTimes.Count() > 0) { PrayerTimes.Clear(); }

                foreach (var prayerTime in PrayerTimesTemp)
                {
                    PrayerTimes.Add(prayerTime);

                }


                //if (!Services.IsRunning)
                //{
                //    Services.Start();

                //}
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get Prayer Times: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Unable to get Prayer Times", "OK");
            }
            finally { IsRefreshing = false; }
        }

        [RelayCommand]
        async Task GetPrayerTimesCommand()
        {
            await GetPrayerTimes();

            await AddNotifications();
        }

        public async Task AddNotifications()
        {
            //await internalNotificationsService.AddNotifictations();


        }

        public async Task GetPrayerTimesOnStart()
        {
            try
            {
                IsBusy = true;
                await GetPrayerTimes();
                await AddNotifications();
                //await Task.Delay(5000);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get Prayer Times: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Unable to get Prayer Times", "OK");
            }
            finally { IsBusy = false; }


        }

        [RelayCommand]
        async Task SaySpeech(PrayerTime prayer)
        {
            try
            {
                await TextToSpeech.Default.SpeakAsync(prayer.PrayerName + " is at " + prayer.Time);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to emit speech to text: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", "Unable to emit speech to text!", "OK");

            }
        }



    }

   



}
