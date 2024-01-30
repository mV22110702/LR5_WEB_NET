using lr5;
using Newtonsoft.Json;

var apiConfig = File.ReadAllText("api-data.json");
var config = JsonConvert.DeserializeObject<ApiConfig>(apiConfig);
ArgumentException.ThrowIfNullOrEmpty(config?.ApiKey);

var weatherHttpClient = new WeatherHttpClient(config.ApiKey);
var responseGet = await weatherHttpClient.Get();
Console.WriteLine(responseGet.Message);
Console.WriteLine(responseGet.StatusCode);
Console.WriteLine(responseGet.Data);
var responsePost = await weatherHttpClient.Post(new CurrentWeatherOptions(){Query = "Kyiv", Units = "s"});
Console.WriteLine(responsePost.Message);
Console.WriteLine(responsePost.StatusCode);
Console.WriteLine(responsePost.Data);
public class ApiConfig
{
    public string ApiKey { get; set; }
}