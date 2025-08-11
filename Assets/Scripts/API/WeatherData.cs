using System;
using Newtonsoft.Json;

namespace WeatherApp.Data
{
    /// <summary>
    /// Main weather data structure matching OpenWeatherMap API response
    /// TODO: Students will complete the JsonProperty attributes
    /// </summary>
    [Serializable]
    public class WeatherData
    {
        [JsonProperty("main")]
        public MainWeatherInfo Main { get; set; }
        
        [JsonProperty("weather")]
        public WeatherDescription[] Weather { get; set; }
        
        [JsonProperty("name")]
        public string CityName { get; set; }
        
        [JsonProperty("cod")]
        public int StatusCode { get; set; }
        
        // Helper properties for easier data access
        public float TemperatureInCelsius => Main?.Temperature - 273.15f ?? 0f;
        public float FeelsLikeInCelsius => Main?.FeelsLike - 273.15f ?? 0f;
        public string PrimaryDescription => Weather?.Length > 0 ? Weather[0].Description : "Unknown";
        
        // Validation method
        public bool IsValid => StatusCode == 200 && Main != null && !string.IsNullOrEmpty(CityName);
    }

    /// <summary>
    /// Main weather information (temperature, humidity, etc.)
    /// TODO: Students will complete the JsonProperty attributes
    /// </summary>
    [Serializable]
    public class MainWeatherInfo
    {
        [JsonProperty("temp")]
        public float Temperature { get; set; }
        
        [JsonProperty("feels_like")]
        public float FeelsLike { get; set; }
        
        [JsonProperty("humidity")]
        public int Humidity { get; set; }
        
        [JsonProperty("pressure")]
        public int Pressure { get; set; }
    }

    /// <summary>
    /// Weather description information
    /// TODO: Students will complete the JsonProperty attributes
    /// </summary>
    [Serializable]
    public class WeatherDescription
    {
        [JsonProperty("main")]
        public string Main { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
        
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}