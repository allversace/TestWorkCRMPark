using Microsoft.Extensions.Configuration;
using TestWorkCRMPark.Class;

class Program
{
    private static HttpClient httpClient;

    static async Task Main(string[] args)
    {
        var config = new ConfigurationBuilder().AddJsonFile("C:\\Users\\artur\\RiderProjects\\TestWorkCRMPark\\TestWorkCRMPark\\appsettings.json").Build();
        var botToken = config["BotToken"];

        httpClient = new HttpClient();
        var apiService = new ApiService(httpClient, config);
        var botUpdateService = new BotUpdateService(apiService);
        var errorService = new ErrorService();
        var botService = new BotService(botToken, apiService, botUpdateService, errorService);

        await botService.StartBot();
    }
}