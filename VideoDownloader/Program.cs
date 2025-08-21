using VideoDownloader;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Secret.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddHostedService<Worker>();
builder.Services.AddTransient<BotHandler>();

var host = builder.Build();
host.Run();