using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace VideoDownloader;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly BotHandler _botHandler;

    public Worker(
        ILogger<Worker> logger,
        IConfiguration configuration,
        BotHandler botHandler)
    {
        _logger = logger;
        _configuration = configuration;
        _botHandler = botHandler;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bot = new TelegramBotClient(
            _configuration["TelegramToken"] ?? throw new FileNotFoundException("Телеграм токен не найден"),
            cancellationToken: stoppingToken);

        var receiverOptions = new ReceiverOptions()
        {
            AllowedUpdates = [UpdateType.Message]
        };
        
        bot.StartReceiving(
            _botHandler.HandleUpdateAsync,
            _botHandler.HandleErrorAsync,
            receiverOptions,
            stoppingToken);
        
        Console.WriteLine("Start");
        
    }
}