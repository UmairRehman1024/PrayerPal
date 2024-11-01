using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrayerPal.Model
{
    public class PrayerTimesResponse
    {
        public int Code { get; set; }
        public string Status { get; set; }
        public PrayerData Data { get; set; }
    }

    public class PrayerData
    {
        public DateTimeReadable Date { get; set; }
        public Timings Timings { get; set; }
    }

    public class DateTimeReadable
    {
        public string Readable { get; set; }
    }

    public class Timings
    {
        public string Fajr { get; set; }
        public string Sunrise { get; set; }
        public string Dhuhr { get; set; }
        public string Asr { get; set; }
        public string Sunset { get; set; }
        public string Maghrib { get; set; }
        public string Isha { get; set; }
        public string Imsak { get; set; }
        public string Midnight { get; set; }
        public string Firstthird { get; set; }
        public string Lastthird { get; set; }

    }
}
