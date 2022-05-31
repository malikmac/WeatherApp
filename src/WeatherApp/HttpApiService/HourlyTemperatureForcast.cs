namespace WeatherApp.WeatherApi
{
    public class HourlyTemperatureForcast
    {
        public double generationtime_ms { get; set; }
        public HourlyUnits? Hourly_units { get; set; }
        public Hourly? Hourly { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Utc_offset_seconds { get; set; }
        public double Elevation { get; set; }
    }
    public class Hourly
    {
        public IList<DateTimeOffset>? Time { get; set; }
        public IList<double>? Temperature_2m { get; set; }
    }

    public class HourlyUnits
    {
        public string? Time { get; set; }
        public string? Temperature_2m { get; set; }
    }
}
