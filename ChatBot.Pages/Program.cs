using ChatBot.Core.Models;
using ChatBot.Pages;
using ChatBot.Pages.Services;
using ChatBot.Pages.Areas.Identity;
using ChatBot.Pages.Data;
using ChatBot.Pages.Hubs;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);



builder.Services
    .AddSqlServerDocker(builder.Configuration)
    .AddLocalServices();

var app = builder.Build();


app.ConfigureApp();


app.Run();
