// See https://aka.ms/new-console-template for more information
using ChatBot.Bot.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

CancellationTokenSource Cts = new();
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("ChatBot.Program", LogLevel.Debug)
        .AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("PRobando el logger");

string? rabbitConnection = Environment.GetEnvironmentVariable("RabbitConnectionString");
if (string.IsNullOrWhiteSpace(rabbitConnection))
{
    var builder =
        new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    IConfigurationRoot configuration = builder.Build();
    rabbitConnection = configuration.GetConnectionString("RabbitConnectionString");
}

if (string.IsNullOrWhiteSpace(rabbitConnection))
{
    Console.WriteLine("RabbitsMQ Service connectionString is required!");
    return;
}

logger.LogInformation("Creating BotCommunication Service");
var consumer = new BotCommunication(rabbitConnection, loggerFactory.CreateLogger<BotCommunication>());
consumer.WaitForStockCode();
await Task.Delay(Timeout.Infinite, Cts.Token).ConfigureAwait(false);