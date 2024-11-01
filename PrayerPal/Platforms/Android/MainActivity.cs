using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;

using PrayerPal.Services;
using Plugin.LocalNotification;
using Android.Content.PM;
using System.Globalization;
using Android.App.Job;
using Java.Util;


namespace PrayerPal
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        //set an activity on main application to get the reference on the service
        public static MainActivity ActivityCurrent { get; set; }
        public MainActivity()
        {
            ActivityCurrent = this;
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (!AlarmHelper.IsAlarmSet(this))
            {
                AlarmHelper.SetDailyAlarm(this);
            }
        }


    }

    public static class AlarmHelper
    {
        public static void SetDailyAlarm(Context context)
        {
            AlarmManager alarmManager = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(context, typeof(NotificationBroadcastReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            // Calculate the time for 1 AM
            var now = DateTime.Now;
            var alarmTime = new DateTime(now.Year, now.Month, now.Day, 1, 0, 0);
            if (now > alarmTime)
            {
                // If the current time is past 1 AM, schedule for the next day
                alarmTime = alarmTime.AddDays(1);
            }

            long triggerTime = (long)(alarmTime - DateTime.Now).TotalMilliseconds;

            // Log for debugging
            Console.WriteLine($"Current time: {now}");
            Console.WriteLine($"Scheduled alarm time: {alarmTime}");
            Console.WriteLine($"Trigger time in milliseconds from now: {triggerTime}");
            Console.WriteLine($"SystemClock.ElapsedRealtime() + triggerTime: {SystemClock.ElapsedRealtime() + triggerTime}");



            // Schedule the exact alarm to trigger at 1 AM every day
            alarmManager.SetExactAndAllowWhileIdle(AlarmType.RtcWakeup, SystemClock.ElapsedRealtime() + triggerTime, pendingIntent);
        }

        public static bool IsAlarmSet(Context context)
        {
            Intent intent = new Intent(context, typeof(NotificationBroadcastReceiver));
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(context, 0, intent, PendingIntentFlags.NoCreate | PendingIntentFlags.Immutable);
            return pendingIntent != null;
        }
    }



    [BroadcastReceiver(Enabled = true, Exported = true)]
    public class NotificationBroadcastReceiver : BroadcastReceiver
    {
        private InternalNotificationsService _internalNotificationsService;

        public NotificationBroadcastReceiver()
        {
            _internalNotificationsService = new InternalNotificationsService(new PrayerTimeService(new LocationService()));
        }

        public override void OnReceive(Context context, Intent intent)
        {
            Task.Run(async () =>
            {
                Console.WriteLine("-------------Alarm received-----------");

                try
                {
                    await _internalNotificationsService.AddNotifictations();
                    Console.WriteLine("Notifications added");
                    if (!AlarmHelper.IsAlarmSet(context))
                    {
                        AlarmHelper.SetDailyAlarm(context);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding notifications: {ex.Message}");
                }
            });
        }
    }



}

