using System.Net;
using Newtonsoft.Json;

namespace lr5;

public class WeatherHttpClient
{
    private readonly string ApiKey;
    private readonly HttpClient _HttpClient;

    public WeatherHttpClient(string apiKey)
    {
        ApiKey = apiKey;
        _HttpClient = new HttpClient { BaseAddress = new Uri("http://api.weatherstack.com/") };
    }

    public async Task<HttpClientCustomResponse<CurrentWeatherResponseDto>> Get()
    {
        try
        {
            var response = await _HttpClient.GetAsync($"current?access_key={ApiKey}&query=Kyiv&units=m");
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
                    ? new List<CurrentWeatherResponseDto>()
                    : new List<CurrentWeatherResponseDto> { data }
            };
        }
        catch (HttpRequestException e)
        {
            return new HttpClientCustomResponse<CurrentWeatherResponseDto>
            {
                Message = e.Message,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = new List<CurrentWeatherResponseDto>()
            };
        }
    }

    public async Task<HttpClientCustomResponse<CurrentWeatherResponseDto>> Post(CurrentWeatherOptions options)
    {
        try
        {
            var response = await _HttpClient.GetAsync(
                $"current?access_key={ApiKey}&query={options.Query}&units={options.Units}"
            );
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
                    ? new List<CurrentWeatherResponseDto>()
                    : new List<CurrentWeatherResponseDto> { data }
            };
        }
        catch (HttpRequestException e)
        {
            return new HttpClientCustomResponse<CurrentWeatherResponseDto>
            {
                Message = e.Message,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = new List<CurrentWeatherResponseDto>()
            };
        }
    }
}

public class CurrentWeatherOptions
{
    public string Query { get; set; }
    public string Units { get; set; }
}

public class HttpClientCustomResponse<T>
{
    public string Message { get; set; }
    public List<T> Data { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}

public class CurrentWeatherResponseDto
{
    [JsonProperty("request")]
    public Request Request { get; set; }

    [JsonProperty("location")]
    public Location Location { get; set; }

    [JsonProperty("current")]
    public Current Current { get; set; }
}

public class Request
{
    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("query")]
    public string Query { get; set; }

    [JsonProperty("language")]
    public string Language { get; set; }

    [JsonProperty("unit")]
    public string Unit { get; set; }
}

public class Location
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }

    [JsonProperty("lat")]
    public string Lat { get; set; }

    [JsonProperty("lon")]
    public string Lon { get; set; }

    [JsonProperty("timezone_id")]
    public string TimezoneId { get; set; }

    [JsonProperty("localtime")]
    public string Localtime { get; set; }

    [JsonProperty("localtime_epoch")]
    public long LocaltimeEpoch { get; set; }

    [JsonProperty("utc_offset")]
    public string UtcOffset { get; set; }
}

public class Current
{
    [JsonProperty("observation_time")]
    public string ObservationTime { get; set; }

    [JsonProperty("temperature")]
    public int Temperature { get; set; }

    [JsonProperty("weather_code")]
    public int WeatherCode { get; set; }

    [JsonProperty("weather_icons")]
    public List<string> WeatherIcons { get; set; }

    [JsonProperty("weather_descriptions")]
    public List<string> WeatherDescriptions { get; set; }

    [JsonProperty("wind_speed")]
    public int WindSpeed { get; set; }

    [JsonProperty("wind_degree")]
    public int WindDegree { get; set; }

    [JsonProperty("wind_dir")]
    public string WindDir { get; set; }

    [JsonProperty("pressure")]
    public int Pressure { get; set; }

    [JsonProperty("precip")]
    public int Precip { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    [JsonProperty("cloudcover")]
    public int Cloudcover { get; set; }

    [JsonProperty("feelslike")]
    public int FeelsLike { get; set; }

    [JsonProperty("uv_index")]
    public int UvIndex { get; set; }

    [JsonProperty("visibility")]
    public int Visibility { get; set; }
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