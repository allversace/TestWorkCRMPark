using Telegram.Bot.Types;
using Telegram.Bot;
using TestWorkCRMPark.Interface;

public class BotUpdateService
{
    private readonly Dictionary<long, string> InnRequests = new Dictionary<long, string>();
    private readonly IApiService _apiService;

    public BotUpdateService(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task Update(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
    {
        var message = update.Message;

        if (message.Text != null)
        {
            var result = await ProcessMessage(client, message);
            Console.WriteLine("Result: " + result);
        }
    }

    public async Task<string> ProcessMessage(ITelegramBotClient client, Message message)
    {
        var command = message.Text.Split(' ')[0];

        switch (command)
        {
            case "/start":
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Привет! Для помощи используйте команду /help"
                );
                break;
            case "/help":
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: "Доступные команды:\n/start - начать общение с ботом.\n/help - вывести справку о доступных командах.\n/hello - вывести ваше имя и фамилию, ваш email, и дату получения задания.\n/inn - получить наименования и адреса компаний по ИНН. Укажите ИНН компаний через пробел (Пример: /inn 5190405629 4234234231) или (Пример: /inn)"
                );
                break;
            case "/hello":
                var dateHomeWork = "04.11.2023";
                var name = "Артур";
                var lastName = "Хохлов";
                var email = "hohlovartur414@gmail.com";
                await client.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: $"Имя: {name}\nФамилия: {lastName}\nEmail: {email}\nДата получения задания: {dateHomeWork}"
                );
                break;
            case "/inn":
                if (message.Text.Split(' ').Length > 1 && (message.Text.All(char.IsDigit) && message.Text.Length >= 10 || message.Text.Length >= 10))
                {
                    var innList = message.Text.Split(' ')[1..];
                    foreach (var inn in innList)
                    {
                        var response = await _apiService.GetCompaniesByINN(inn);
                        if (response == "Результаты запроса:\n")
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Нет информации по введенному ИНН."
                            );
                        else
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: response
                            );
                    }
                }
                else
                {
                    InnRequests[message.Chat.Id] = command;
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Пожалуйста, укажите ИНН компании"
                    );
                }
                break;
            default:
                if (InnRequests.ContainsKey(message.Chat.Id) && InnRequests[message.Chat.Id] == "/inn" && message.Text.All(char.IsDigit) && message.Text.Length >= 10)
                {
                    var innList = message.Text.Split(' ')[0..];
                    foreach (var inn in innList)
                    {
                        var response = await _apiService.GetCompaniesByINN(inn);
                        if (response == "Результаты запроса:\n")
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: "Нет информации по введенному ИНН."
                            );
                        else
                            await client.SendTextMessageAsync(
                                chatId: message.Chat.Id,
                                text: response
                            );
                    }
                }
                else
                {
                    await client.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: "Неизвестная команда. Для получения справки используйте /help"
                    );
                }
                break;
        }
        return "Processing complete";
    }
}
