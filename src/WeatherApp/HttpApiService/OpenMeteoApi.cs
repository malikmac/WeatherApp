using Microsoft.AspNetCore.Http.Extensions;
using WeatherApp.WeatherApi;

namespace WeatherApp.ApiService
{
    public class OpenMeteoApi : IOpenMeteoApi
    {
        private readonly HttpClient _httpClient;

        public OpenMeteoApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private string CreateRequest(double latitude, double longitude)
        {
            UriBuilder uriBuilder = new UriBuilder();       

            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.open-meteo.com";
            uriBuilder.Path = "/v1/forecast";

            QueryBuilder queryBuilder = new QueryBuilder();

            queryBuilder.Add("latitude", latitude.ToString().Replace(',', '.'));
            queryBuilder.Add("longitude", longitude.ToString().Replace(',', '.'));
            queryBuilder.Add("hourly", "temperature_2m");

            uriBuilder.Query = queryBuilder.ToString();

            return uriBuilder.Uri.ToString();
        }

        public async Task<ServiceResponse<HourlyTemperatureForcast>> GetForcast(double latitude, double longitude)
        {
            try
            {
                var request = CreateRequest(latitude, longitude);
                var response = await _httpClient.GetFromJsonAsync<HourlyTemperatureForcast>(request);

                return ServiceResponse<HourlyTemperatureForcast>.Success(response);
            }
            catch(Exception ex)
            {
                return ServiceResponse<HourlyTemperatureForcast>.Failed("Error", "Something went wrong.\nThe values entered may not be correct.");
            }      
        }

        public Dictionary<string, string> GetAverageTemperatures(HourlyTemperatureForcast forcast)
        {
            int i = 0;
            double sum = 0;
            double avg = 0;
            string date;
            var averageTemperatures = new Dictionary<string, string>();

            foreach (double item in forcast.Hourly.Temperature_2m)
            {
                sum += item;

                if ((i + 1) % 24 == 0)
                {
                    date = forcast.Hourly.Time[i].Date.ToString("dd.MM.yyyy");
                    avg = sum / 24;
                    avg = Math.Round(avg, 2);
                    averageTemperatures.Add(date, avg.ToString());

                    sum = 0;
                }

                i++;
            }

            return averageTemperatures;
        }
    }

    public interface IOpenMeteoApi
    {
        Task<ServiceResponse<HourlyTemperatureForcast>> GetForcast(double latitude, double longitude);
        Dictionary<string, string> GetAverageTemperatures(HourlyTemperatureForcast forcast);
    }
}
