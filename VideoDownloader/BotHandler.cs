using Telegram.Bot;
using Telegram.Bot.Types;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;

namespace VideoDownloader;

public class BotHandler
{
    private readonly YoutubeClient _youtubeClient = new YoutubeClient();
    
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
                default:
                    var manifest = await _youtubeClient.Videos.Streams.GetManifestAsync(message.Text ?? string.Empty, cancellationToken);
                    var streamInfo = manifest.GetMuxedStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .GetWithHighestVideoQuality();
                    await using (var stream =
                                 await _youtubeClient.Videos.Streams.GetAsync(streamInfo, cancellationToken))
                    {
                        using var memoryStream = new MemoryStream();
                        await stream.CopyToAsync(memoryStream, cancellationToken);
                        memoryStream.Position = 0;
                        
                        await botClient.SendVideo(chatId, new InputFileStream(memoryStream, "video.mp4"));
                        break;
                    }
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