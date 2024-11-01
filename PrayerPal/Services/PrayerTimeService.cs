using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PrayerPal.Services
{
    public class PrayerTimeService
    {
        HttpClient httpClient;
        LocationService LocationService;


        public PrayerTimeService(LocationService locationService)
        {
            httpClient = new HttpClient();
            LocationService = locationService;  
        }

        public async Task<PrayerTimesResponse> GetPrayerTimesFromAPI(DateTime date, double latitude, double longitude, int method = 2)
        {
            try
            {
                string formattedDate = date.ToString("dd-MM-yyyy");
                Console.WriteLine($"Date: {formattedDate}");
                string apiUrl = $"http://api.aladhan.com/v1/timings/{formattedDate}?latitude={latitude}&longitude={longitude}&method={method}";

                Console.WriteLine(apiUrl);


                using HttpClient client = new HttpClient();

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    var prayerTimesResponse = JsonSerializer.Deserialize<PrayerTimesResponse>(responseBody, options);
                    //Console.WriteLine(prayerTimesResponse);
                    return prayerTimesResponse;
                }
                else
                {
                    // Handle unsuccessful response
                    return null;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                return null;
            }
        }


        public async Task<Collection<PrayerTime>> GetPrayerTimesAsync()
        {
            try
            {
                bool test = false;
                Collection<PrayerTime> PrayerTimes = new();

                if (test)
                {

                    return testNotifications();
                }

                //if dont need to get todays praye times, return the stored prayer times
                if (!CheckNewPrayerTimesNeeded())
                {

                    PrayerTimes.Add(new PrayerTime { PrayerName = "Fajr", Time = (Preferences.Default.Get<DateTime>("Fajr", DateTime.Now)).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Dhuhr", Time = Preferences.Default.Get("Dhuhr", DateTime.Now).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Asr", Time = Preferences.Default.Get("Asr", DateTime.Now).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Maghrib", Time = Preferences.Default.Get("Maghrib", DateTime.Now).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Isha", Time = Preferences.Default.Get("Isha", DateTime.Now).ToString("HH:mm") });
                }
                //get the new prayer times
                else
                {
                    

                    //get location
                    var location = await LocationService.GetCurrentLocation();
                    DateTime today = DateTime.Today;

                    //get response from api
                    var response = await GetPrayerTimesFromAPI(today, location.Latitude, location.Longitude);

                    //add to collection
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Fajr", Time = response.Data.Timings.Fajr });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Dhuhr", Time = response.Data.Timings.Dhuhr });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Asr", Time = response.Data.Timings.Asr });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Maghrib", Time = response.Data.Timings.Maghrib });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Isha", Time = response.Data.Timings.Isha });


                    //store in preferences for cache
                    Preferences.Default.Clear();
                    foreach (var prayer in PrayerTimes)
                    {
                        Preferences.Default.Set(prayer.PrayerName, DateTime.Parse(prayer.Time));
                    }
                }

                foreach (var prayerTime in PrayerTimes)
                {
                    Console.WriteLine("__PrayerTimes: {0} ---- {1}", prayerTime.PrayerName, prayerTime.Time);
                }
                return PrayerTimes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get Prayer Times: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!","Unable to get Prayer Times", "OK");
                return null;
            }
        }

        private bool CheckNewPrayerTimesNeeded()
        {


            // If any prayer time is missing, return true
            if (!Preferences.Default.ContainsKey("Fajr") ||
                !Preferences.Default.ContainsKey("Dhuhr") ||
                !Preferences.Default.ContainsKey("Asr") ||
                !Preferences.Default.ContainsKey("Maghrib") ||
                !Preferences.Default.ContainsKey("Isha"))
            {
                return true;
            }

            // Get the date of today
            DateTime today = DateTime.Today;

            // Check if any of the prayer times are from yesterday
            foreach (var prayerTimeKey in new[] { "Fajr", "Dhuhr", "Asr", "Maghrib", "Isha" })
            {
                DateTime prayerTime = Preferences.Default.Get(prayerTimeKey, DateTime.Now);
                if (prayerTime.Date < today)
                {
                    return true;
                }
            }

            // If none of the prayer times are from yesterday, return false
            return false;
        }

        private Collection<PrayerTime> testNotifications()
        {

            try
            {
                Collection<PrayerTime> PrayerTimes = new();


                //if preferences is empty or it has gone passed Isha time, 
                if (!Preferences.Default.ContainsKey("Fajr") ||
                !Preferences.Default.ContainsKey("Dhuhr") ||
                !Preferences.Default.ContainsKey("Asr") ||
                !Preferences.Default.ContainsKey("Maghrib") ||
                !Preferences.Default.ContainsKey("Isha") ||
                Preferences.Default.Get("Isha", DateTime.Now) <= DateTime.Now)
                //get new prayer times and add to preferences
                {

                    PrayerTimes.Add(new PrayerTime { PrayerName = "Fajr", Time = DateTime.Now.AddMinutes(1).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Dhuhr", Time = DateTime.Now.AddMinutes(2).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Asr", Time = DateTime.Now.AddMinutes(3).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Maghrib", Time = DateTime.Now.AddMinutes(4).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Isha", Time = DateTime.Now.AddMinutes(5).ToString("HH:mm") });


                    Preferences.Default.Clear();

                    foreach (var prayer in PrayerTimes)
                    {
                        Preferences.Default.Set(prayer.PrayerName, DateTime.Parse(prayer.Time));
                    }
                }

                //else return prayer times stored in preferences
                else
                {


                    PrayerTimes.Add(new PrayerTime { PrayerName = "Fajr", Time = (Preferences.Default.Get<DateTime>("Fajr", DateTime.Now)).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Dhuhr", Time = Preferences.Default.Get("Dhuhr", DateTime.Now).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Asr", Time = Preferences.Default.Get("Asr", DateTime.Now).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Maghrib", Time = Preferences.Default.Get("Maghrib", DateTime.Now).ToString("HH:mm") });
                    PrayerTimes.Add(new PrayerTime { PrayerName = "Isha", Time = Preferences.Default.Get("Isha", DateTime.Now).ToString("HH:mm") });
                }

                //write prayertimes to console
                foreach (var prayerTime in PrayerTimes)
                {
                    Console.WriteLine("__PrayerTimes: {0} ---- {1}", prayerTime.PrayerName, prayerTime.Time);
                }

                return PrayerTimes;

            }
            catch (Exception)
            {

                throw;
            }
            
        }

    }
}
