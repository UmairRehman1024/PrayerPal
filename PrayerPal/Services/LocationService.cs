using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayerPal.Services
{
    public class LocationService
    {

        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public LocationService() { }

        public async Task<Location> GetCurrentLocation()
        {
            try
            {
                _isCheckingLocation = true;

                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();

                Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

                if (location != null)
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                return location;
            }
            // Catch one of the following exceptions:
            //   FeatureNotSupportedException
            //   FeatureNotEnabledException
            //   PermissionException
            catch (Exception ex)
            {
                // Unable to get location
                Debug.WriteLine($"Unable to get location {ex}");
                await Shell.Current.DisplayAlert("Error!", "Unable to get location. Make sure you have your location on.", "OK");
                return null;
            }
            finally
            {
                _isCheckingLocation = false;
            }
        }
    }
}
