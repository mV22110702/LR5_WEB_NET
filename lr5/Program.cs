using Newtonsoft.Json;

var apiConfig = File.ReadAllText("api-data.json");
var config = JsonConvert.DeserializeObject<ApiConfig>(apiConfig);
ArgumentException.ThrowIfNullOrEmpty(config?.ApiKey);


internal class ApiConfig
{
    public string ApiKey { get; set; }
}