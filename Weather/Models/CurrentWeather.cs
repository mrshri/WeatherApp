namespace Weather.Models
{
    public class CurrentWeather
    {
        public double Temperature { get; set; }
        public double Windspeed { get; set; }
        public double Winddirection { get; set; }
        public int Weathercode { get; set; }
        public string Time { get; set; }
    }
}
