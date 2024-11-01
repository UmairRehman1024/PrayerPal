using System.Collections.Generic;

namespace PrayerPal.Model
{

    public class MosqueApiResponse
    {
        public List<string> Html_Attributions { get; set; }
        public string Next_Page_Token { get; set; }
        public List<Place> Results { get; set; }
        public string Status { get; set; }
    }

    public class Place
    {
        public string BusinessStatus { get; set; }
        public Geometry Geometry { get; set; }
        public string Icon { get; set; }
        public string IconBackgroundColor { get; set; }
        public string IconMaskBaseUri { get; set; }
        public string Name { get; set; }
        public OpeningHours OpeningHours { get; set; }
        public List<PlacePhoto> Photos { get; set; }
        public string PlaceId { get; set; }
        public PlusCode PlusCode { get; set; }
        public double Rating { get; set; }
        public string Reference { get; set; }
        public string Scope { get; set; }
        public List<string> Types { get; set; }
        public int UserRatingsTotal { get; set; }
        public string Vicinity { get; set; }
    }

    public class Geometry
    {
        public LatLngLiteral Location { get; set; }
        public Bounds Viewport { get; set; }
    }

    public class LatLngLiteral
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class Bounds
    {
        public LatLngLiteral Northeast { get; set; }
        public LatLngLiteral Southwest { get; set; }
    }

    public class OpeningHours
    {
        public bool OpenNow { get; set; }
    }

    public class PlacePhoto
    {
        public int Height { get; set; }
        public List<string> HtmlAttributions { get; set; }
        public string PhotoReference { get; set; }
        public int Width { get; set; }
    }

    public class PlusCode
    {
        public string CompoundCode { get; set; }
        public string GlobalCode { get; set; }
    }
}
