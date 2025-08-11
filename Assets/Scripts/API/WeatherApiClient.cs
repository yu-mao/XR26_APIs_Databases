using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WeatherApp.Data;
using WeatherApp.Config;

namespace WeatherApp.Services
{
    /// <summary>
    /// Modern API client for fetching weather data
    /// Students will complete the implementation following async/await patterns
    /// </summary>
    public class WeatherApiClient : MonoBehaviour
    {
        [Header("API Configuration")]
        [SerializeField] private string baseUrl = "http://api.openweathermap.org/data/2.5/weather";
        
        /// <summary>
        /// Fetch weather data for a specific city using async/await pattern
        /// TODO: Students will implement this method
        /// </summary>
        /// <param name="city">City name to get weather for</param>
        /// <returns>WeatherData object or null if failed</returns>
        public async Task<WeatherData> GetWeatherDataAsync(string city)
        {
            // Validate input parameters
            if (string.IsNullOrWhiteSpace(city))
            {
                Debug.LogError("City name cannot be empty");
                return null;
            }
            
            // Check if API key is configured
            if (!ApiConfig.IsApiKeyConfigured())
            {
                Debug.LogError("API key not configured. Please set up your config.json file in StreamingAssets folder.");
                return null;
            }
            
            // TODO: Build the complete URL with city and API key
            string url = $"{baseUrl}?q={city}&&appid={ApiConfig.OpenWeatherMapApiKey}";
            
            // TODO: Create UnityWebRequest and use modern async pattern
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                // TODO: Use async/await, send the request and wait for response
                await request.SendWebRequest();
                
                // TODO: Implement proper error handling for different result types
                // Check request.result for Success, ConnectionError, ProtocolError, DataProcessingError
                switch (request.result)
                {
                    case UnityWebRequest.Result.Success:
                        // TODO: Parse JSON response using Newtonsoft.Json
                        // TODO: Return the parsed WeatherData object
                        // Debug.Log(request.downloadHandler.text);
                        return ParseWeatherData(request.downloadHandler.text);
                    case UnityWebRequest.Result.ConnectionError:
                        Debug.LogError("Connection error: " + request.error);
                        break;
                    case UnityWebRequest.Result.ProtocolError:
                        Debug.LogError("Protocol error " + request.responseCode + ": " + request.error);
                        break;
                    case UnityWebRequest.Result.DataProcessingError:
                        Debug.LogError("Data processing error: " + request.error);
                        break;
                }
                
                return null;
            }
        }
        
        private WeatherData ParseWeatherData(string jsonString)
        {
            try
            {
                var settings = new JsonSerializerSettings
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
        
                return JsonConvert.DeserializeObject<WeatherData>(jsonString, settings);
            }
            catch (JsonException exception)
            {
                Debug.LogError("JSON parsing error: " + exception.Message);
                return null;
            }
        }

        /// <summary>
        /// Example usage method - students can use this as reference
        /// </summary>
        private async void Start()
        {
            // Example: Get weather for London
            var weatherData = await GetWeatherDataAsync("London");
            
            if (weatherData != null && weatherData.IsValid)
            {
                Debug.Log($"Weather in {weatherData.CityName}: {weatherData.TemperatureInCelsius:F1}°C");
                Debug.Log($"Description: {weatherData.PrimaryDescription}");
            }
            else
            {
                Debug.LogError("Failed to get weather data");
            }
        }
    }
}