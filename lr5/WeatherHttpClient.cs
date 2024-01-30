using System.Net;
using Newtonsoft.Json;

namespace lr5;

public class WeatherHttpClient
{
    private readonly string ApiKey;
    private readonly HttpClient HttpClient;

    public WeatherHttpClient(string apiKey)
    {
        ApiKey = apiKey;
        HttpClient = new HttpClient { BaseAddress = new Uri("http://api.weatherstack.com/") };
    }
    public async Task<HttpClientCustomResponse<CurrentWeatherResponseDto>> Get()
    {
        try
        {
            var response = await HttpClient.GetAsync($"access_key={ApiKey}&query=Kyiv&units=m");
            response.EnsureSuccessStatusCode();
            var dataString = await response.Content.ReadAsStringAsync();
            if (dataString.Contains("error"))
            {
                var error = JsonConvert.DeserializeObject<ApiErrorDto>(dataString);
                throw new HttpRequestException(error?.Error?.Info, null, HttpStatusCode.InternalServerError);
            }

            var data = JsonConvert.DeserializeObject<CurrentWeatherResponseDto>(dataString);
            return new HttpClientCustomResponse<CurrentWeatherResponseDto>
            {
                Message = "Success",
                StatusCode = response.StatusCode,
                Data = data == null
                    ? new List<CurrentWeatherResponseDto>() { }
                    : new List<CurrentWeatherResponseDto> { data }
            };
        }
        catch (HttpRequestException e)
        {
            return new HttpClientCustomResponse<CurrentWeatherResponseDto>
            {
                Message = e.Message,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = new List<CurrentWeatherResponseDto>() { }
            };
        }
    }

    // public async Task<HttpClientCustomResponse<CurrentWeatherResponseDto>> Post{}
}

public class HttpClientCustomResponse<T>
{
    public string Message { get; set; }
    public List<T> Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class CurrentWeatherResponseDto
{
    public string FeelsLike;
    public string InitialQuery;
    public double Lat;
    public double Lon;
    public string ObservationTime;
    public string Temperature;
    public List<string> WeatherDescriptions;
    public Wind WindData;
}

public class ApiErrorDto
{
    public bool Success { get; set; }
    public ApiError? Error { get; set; }
}

public class ApiError
{
    public int Code { get; set; }
    public string Type { get; set; }
    public string Info { get; set; }
}

public class Wind
{
    public double Degree;
    public string Dir;
    public double Speed;
}