using Telegram.Bot;
using Telegram.Bot.Types;
using TestWorkCRMPark.Interface;

namespace TestWorkCRMPark.Class
{
    public class BotService : IBotService
    {
        private readonly TelegramBotClient _client;
        private readonly IApiService _apiService;
        private readonly BotUpdateService _botUpdateService;
        private readonly ErrorService _errorService;

        public BotService(string botToken, IApiService apiService, BotUpdateService botUpdateService, ErrorService errorService)
        {
            _client = new TelegramBotClient(botToken);
            _apiService = apiService;
            _botUpdateService = botUpdateService;
            _errorService = errorService;
        }

        public async Task StartBot()
        {
            var me = await _client.GetMeAsync();
            Console.WriteLine($"Бот Telegram. Мой ID: {me.Id}.");

            _client.StartReceiving(_botUpdateService.Update, _errorService.Error);
            Console.ReadLine();
        }

        public async Task<string> ProcessMessage(Message message)
        {
            return await _botUpdateService.ProcessMessage(_client, message);
        }
    }
}
