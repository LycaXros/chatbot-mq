﻿// See https://aka.ms/new-console-template for more information
using ChatBot.Bot.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

CancellationTokenSource cts = new();
using var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddFilter("Microsoft", LogLevel.Warning)
        .AddFilter("System", LogLevel.Warning)
        .AddFilter("ChatBot.Program", LogLevel.Debug)
        .AddConsole();
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogInformation("Probando el logger");

var rabbitConnection = Environment.GetEnvironmentVariable("RabbitConnectionString");
if (string.IsNullOrWhiteSpace(rabbitConnection))
{
    var builder =
        new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

    var configuration = builder.Build();
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
consumer.BotMessageToUsers("Use following format to Request stock data '/stock=[Stock Name]'");
await Task.Delay(Timeout.Infinite, cts.Token).ConfigureAwait(false);