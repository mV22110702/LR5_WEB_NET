using System.Net;

namespace lr5;

public class WeatherHttpClient
{
    private readonly HttpClient HttpClient;
    private readonly string ApiKey; 

    public WeatherHttpClient(string apiKey)
    {
        ApiKey = apiKey;
        HttpClient = new HttpClient { BaseAddress = new Uri("http://api.weatherstack.com/") };
    }

    public async Task<CurrentWeatherResponseDto> GetCapitalCurrentWeatherAsync()
    {
        var response = await HttpClient.GetAsync($"access_key={ApiKey}&query=Kyiv&units=m");
        try
        {
        response.EnsureSuccessStatusCode();
        } catch (HttpRequestException e)
        {
            
            return null;
        }

        return await response.Content.ReadAsStringAsync();
    }
}

class HttpClientCustomResponse<T>
{
    public T Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    
}
class CurrentWeatherResponseDto
{
    public string InitialQuery;
    public double Lat;
    public double Lon;
    public string ObservationTime;
    public string Temperature;
    public string FeelsLike;
    public List<string> WeatherDescriptions;
    public Wind WindData;
}

class Wind
{
    public double Speed;
    public string Dir;
    public double Degree;
}