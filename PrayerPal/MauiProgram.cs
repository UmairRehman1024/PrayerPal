using Microsoft.Extensions.Logging;
using PrayerPal.Services;
using PrayerPal.View;
using PrayerPal.ViewModel;
using Syncfusion.Maui.Core.Hosting;
using Plugin.LocalNotification;
using CommunityToolkit.Maui.Maps;
using System.Reflection.PortableExecutable;
using CommunityToolkit.Maui;

namespace PrayerPal
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit()
                ;
            
#if ANDROID || IOS
            builder.UseLocalNotification()
            .UseMauiMaps();
#elif WINDOWS
            builder.UseMauiCommunityToolkitMaps("eNtrzWLxIMYSZzsngKfH~C28wR0Ee4yejr9ssPXXHzw~AsNYmFOKYSJzMc-q9rJnWZ4IRLqoaOi_zVjdEmKJY2YlAFgkU2Hp6J1gFzwj8nrM");
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif

            //Views
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<QiblahPage>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddSingleton<MapPage>();
            builder.Services.AddSingleton<CalanderPage>();
            

            //ViewModels
            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<MapViewModel>();
            builder.Services.AddSingleton<QiblahViewModel>();


            //Services
            builder.Services.AddSingleton<PrayerTimeService>();
            builder.Services.AddSingleton<LocationService>();
            builder.Services.AddSingleton<NearestMosquesService>();
            builder.Services.AddSingleton<InternalNotificationsService>();



            builder.Services.AddSingleton<Microsoft.Maui.Controls.Maps.Map>();

#if ANDROID
            //builder.Services.AddTransient<IService, NotificationService>();

            //builder.Services.AddSingleton<AlarmService>();

#endif

        

            return builder.Build();
        }
    }
}
