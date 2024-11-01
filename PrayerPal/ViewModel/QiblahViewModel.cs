using PrayerPal.Services;

namespace PrayerPal.ViewModel
{
    public partial class QiblahViewModel:BaseViewModel
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(directionString))]
        double direction;

        [ObservableProperty]
        bool isToggled = true;

        public string directionString => ((int)Direction).ToString() + "°";

        LocationService LocationService;

        [ObservableProperty]
        double qiblahDirection;

        // Kaaba's coordinates
        private const double KaabaLatitude = 21.4224779;
        private const double KaabaLongitude = 39.8251832;


        private double qiblaAngle;
        public QiblahViewModel(LocationService locationService)
        {
            Title = "Qiblah";
            this.LocationService = locationService;
        }


        public void TurnOnCompass()
        {
            if (Compass.Default.IsSupported)
            {
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                Compass.Default.Start(SensorSpeed.UI, applyLowPassFilter: true);
            }
        }
        public void TurnOffCompass()
        {
            if (Compass.Default.IsSupported)
            {
                Compass.Default.Stop();
                Compass.Default.ReadingChanged -= Compass_ReadingChanged;
            }
        }


        private async void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {

            Direction = e.Reading.HeadingMagneticNorth;

            

            QiblahDirection =  qiblaAngle - Direction;

            if (QiblahDirection < 0)
            {
                QiblahDirection += 360;
            }

            //if ((QiblahDirection >= 359 && QiblahDirection <= 360) || (QiblahDirection <= 1 && QiblahDirection >= 0))
            //if (QiblahDirection == 0 ) 
            if (IsToggled)
            {
                //if (QiblahDirection == 359 ||  QiblahDirection == 360 || QiblahDirection == 0 || QiblahDirection == 1)]
                if ((QiblahDirection >= 359 && QiblahDirection <= 360) || (QiblahDirection <= 1 && QiblahDirection >= 0))
                {
                    Vibration.Vibrate();
                }
            }
            
        }


        //public void OnToggled(object sender, ToggledEventArgs e)
        //{
        //    // Perform an action after examining e.Value
        //    IsToggled = !IsToggled;
        //}

        public async Task CalculateQiblaDirection()
        {
            Location location = await LocationService.GetCurrentLocation();

            double longitudeDifference = KaabaLongitude - location.Longitude;
            double y = Math.Sin(longitudeDifference) * Math.Cos(KaabaLatitude);
            double x = Math.Cos(location.Latitude) * Math.Sin(KaabaLatitude) -
                       Math.Sin(location.Latitude) * Math.Cos(KaabaLatitude) * Math.Cos(longitudeDifference);
            double qiblaAngle = Math.Atan2(y, x);

            // Convert radians to degrees
            qiblaAngle = qiblaAngle * (180 / Math.PI);

            // Adjust the angle to be between 0 and 360 degrees
            if (qiblaAngle < 0)
            {
                qiblaAngle += 360;
            }
            this.qiblaAngle = qiblaAngle;

            Console.WriteLine(qiblaAngle);
            
        }

     

    }
}
