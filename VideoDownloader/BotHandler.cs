using Telegram.Bot;
using Telegram.Bot.Types;

namespace VideoDownloader;

public class BotHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            if (update.Message is null) return;
            var message = update.Message;
            var chatId = message.Chat.Id;

            switch (message.Text?.ToLower())
            {
                case "/start":
                    await botClient.SendMessage(chatId, "привет я твой помощник в скачивании видео из Ютуб",
                        cancellationToken: cancellationToken);
                    break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Произошла ошибка - {exception.Message}");
    }
}