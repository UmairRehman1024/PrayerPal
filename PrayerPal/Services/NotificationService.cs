

#if ANDROID || IOS 
using Plugin.LocalNotification;

#elif WINDOWS
using Microsoft.Toolkit.Uwp.Notifications;

#endif

namespace PrayerPal.Services
{

   enum PrayerNames
    {
        Fajr,
        Dhuhr,
        Asr,
        Maghrib,
        Isha
    }
    public partial class InternalNotificationsService 
    {
        PrayerTimeService prayerTimeService;
        public InternalNotificationsService(PrayerTimeService prayerTimeService)
        {
            this.prayerTimeService = prayerTimeService;
            
        }

        

        public async Task AddNotifictations()
        {
#if ANDROID || IOS
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }
#endif

            Collection<PrayerTime> PrayerTimes = await prayerTimeService.GetPrayerTimesAsync();

            int id = 100;
            foreach (var prayerTime in PrayerTimes)
            {
                Console.WriteLine("PrayerTime from NotificationService: {0}:{1}:{2}",prayerTime.PrayerName, prayerTime.Time, id);
                createPrayerNotification(prayerTime, id);
                id += 100;

            }

            

            






        }


//        public async void createNotification(string Title, DateTime NotifyTime, string Description)
//        {
//            try {
//#if ANDROID || IOS

//                var notification = new NotificationRequest
//                {
//                    NotificationId = Guid.NewGuid(),
//                    Title = Title,
//                    Description = Description,
//                    Schedule =
//                    {
//                        NotifyTime = NotifyTime
//                    },



//                };
//                Console.WriteLine(Title + " set at " + notification.Schedule.NotifyTime.Value);
//                await LocalNotificationCenter.Current.Show(notification);

//#elif WINDOWS
//                if (NotifyTime > DateTime.Now)
//                {
//                    new ToastContentBuilder()
//                    .AddText(Title)
//                    .Schedule(NotifyTime);
//                    //.Schedule(DateTime.Now.AddSeconds(5));

//                    Debug.WriteLine("Toast for {0} Added", Title);
//                }
//                else
//                {
//                    Debug.WriteLine("Toast for {0} Skipped", Title);
//                }
//#endif
//            }
//            catch (Exception err)
//            {
//                Debug.WriteLine($"Unable to schedule notification for {0} : {1}", Title, err.Message);
//                await Shell.Current.DisplayAlert("Unable to schedule notification for " + Title, err.Message, "OK");
//            }
//        }


        private async void createPrayerNotification(PrayerTime prayer, int id)
        {
            try
            {

#if ANDROID || IOS
                var notification = new NotificationRequest
                {
                    NotificationId = id,
                    Title = prayer.PrayerName,
                    Description = $"Time for {prayer.PrayerName}",
                    Schedule =
                    {
                        NotifyTime = DateTime.Parse(prayer.Time)
                    },



                };
                Console.WriteLine(prayer.PrayerName + "-------" + notification.Schedule.NotifyTime.Value);
                await LocalNotificationCenter.Current.Show(notification);




#elif WINDOWS
            if (DateTime.Parse(prayer.Time) > DateTime.Now)
            {
                new ToastContentBuilder()
                .AddText("It's time for " + prayer.PrayerName)
                .Schedule(DateTime.Parse(prayer.Time));
                //.Schedule(DateTime.Now.AddSeconds(5));

                Debug.WriteLine("Toast for {0} Added", prayer.PrayerName);
            }else{
                Debug.WriteLine("Toast for {0} Skipped", prayer.PrayerName);
            }

#endif



            }
            catch (Exception err)
            {
                Debug.WriteLine($"Unable to schedule notification for {0} : {1}", prayer.PrayerName, err.Message);
                await Shell.Current.DisplayAlert("Unable to schedule notification for " + prayer.PrayerName, err.Message, "OK");
            }



        }
    }
}
