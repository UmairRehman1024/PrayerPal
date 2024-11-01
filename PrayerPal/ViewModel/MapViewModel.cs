

using PrayerPal.Services;


namespace PrayerPal.ViewModel
{
    public partial class MapViewModel : BaseViewModel
    {
        public ObservableCollection<Mosque> Mosques { get; } = new();

        [ObservableProperty]
        Mosque selectedMosque;

        [ObservableProperty]
        bool isReady;

        [ObservableProperty]
        bool isRefreshing;

        private int PROXIMITY_RADIUS = 5000;
        LocationService LocationService;
        NearestMosquesService MosquesService;
        public MapViewModel(LocationService locationService, NearestMosquesService mosquesService)
        {
            Title = "Map";
            this.LocationService = locationService;
            this.MosquesService = mosquesService;
            
        }

        [RelayCommand]
        public async Task GetNearestMosques()
        {
            //if (IsBusy) return;
            try
            {
                //IsBusy = true;
                IsReady = false;
                
                var location = await LocationService.GetCurrentLocation();

                //double lat = 62.40048592777711;
                //double lng = 58.17413117320772;

                var MosquesResult = await MosquesService.GetMosques(location.Latitude, location.Longitude, PROXIMITY_RADIUS);

                //var MosquesResult = await MosquesService.GetMosques(lat,lng, PROXIMITY_RADIUS);

                if (MosquesResult.Status == "ZERO_RESULTS")
                {
                    await Shell.Current.DisplayAlert("Error!", "No mosques found in your area", "OK");
                    return;
                }

                if (Mosques.Count != 0) { Mosques.Clear(); }

                foreach (var mosque in MosquesResult.Results) 
                { 
                    Location templocation = new Location();
                    templocation.Latitude = mosque.Geometry.Location.Lat;
                    templocation.Longitude = mosque.Geometry.Location.Lng;

                    Mosques.Add(new Mosque { Location = templocation, Name = mosque.Name });
                }
                

                //add new pin for current location



            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");

            }
            finally
            {
                //IsBusy = false;
                IsRefreshing = false;
                IsReady = true;
                SelectedMosque = Mosques.First();
            }
        }

        [RelayCommand]
        void UpdateSelectedMosque(Mosque mosque)
        {
            if (mosque == null) { return; }

            SelectedMosque = mosque;
            HapticFeedback.Perform(HapticFeedbackType.LongPress);
        }

        //private void UpdateMapWithMosques()
        //{
        //    // Clear existing pins from the map
        //    map.Pins.Clear();
        
        //    // Add pins for each mosque in the collection
        //    foreach (var mosque in Mosques)
        //    {
        //        var pin = new Pin
        //        {
        //            Location = mosque.Location,
        //            Label = mosque.Name
        //        };
        //        map.Pins.Add(pin);
        //    }
        //}

        public async Task GetNearestMosquesOnStart()
        {
            try
            {
                IsBusy = true;
                await GetNearestMosques();
             
                //UpdateSelectedMosque(Mosques.First());
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Unable to get nearby mosques: {error.Message}");
                await Shell.Current.DisplayAlert("Error!","Unable to get nearby mosques", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
