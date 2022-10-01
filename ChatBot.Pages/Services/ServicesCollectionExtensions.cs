using ChatBot.Core.Interfaces;
using ChatBot.Core.Models;
using ChatBot.Pages.Areas.Identity;
using ChatBot.Pages.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Pages.Services
{
    public static class ServicesCollectionExtensions
    {
        public static IServiceCollection AddSqlServerDocker(this IServiceCollection services, ConfigurationManager configuration)
        {


            string connectionString = GetDatabaseConnectionString(configuration);

            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddLocalServices(this IServiceCollection services)
        {

            // Add services to the container.


            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<ChatUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddSignalR();

            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
      
            services.AddSingleton<ICommandService, CommandService>();
            
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddSingleton<IBotCommandRequest, BotCommandRequest>();
            services.AddHostedService<BotResponseCommunication>();
            return services;
        }

        private static string GetDatabaseConnectionString(IConfiguration configuration)
        {
            var database = configuration["DBDATABASE"];
            var host = configuration["DBHOST"];
            var password = configuration["DBPASSWORD"];
            var port = configuration["DBPORT"];
            var user = configuration["DBUSER"];

            return string.IsNullOrEmpty(database) 
                   || string.IsNullOrEmpty(host) 
                   || string.IsNullOrEmpty(password) 
                   || string.IsNullOrEmpty(port) 
                   || string.IsNullOrEmpty(user)
                ? configuration.GetConnectionString("DefaultConnection") 
                : $"Server={host}, {port};Database={database};User Id={user};Password={password};";
         
        }
    }
}
