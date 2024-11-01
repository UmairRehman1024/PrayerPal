
namespace PrayerPal.Model
{
    public class Mosque
    {
        public string Name { get; set; }
        public Location Location { get; set; }
    }

    public class MapPlace
    {
        public Location Location { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
