﻿using ChatBot.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatBot.Web
{
    public static class ServicesExtra
    {
        public static IServiceCollection AddSqlServerDocker(this IServiceCollection services, ConfigurationManager configuration)
        {


            string connectionString = GetDatabaseConnectionString(configuration);

            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }


        private static string GetDatabaseConnectionString(ConfigurationManager configuration)
        {
            // The next lines will help us to connect to a docker database instance
            string connectionString = "";

            string database = configuration["DBDATABASE"];
            string host = configuration["DBHOST"];
            string password = configuration["DBPASSWORD"];
            string port = configuration["DBPORT"];
            string user = configuration["DBUSER"];

            // If any of the variables is null, get connectionString from appSettings.json
            if (new List<string>() { database, host, password, port }.Any(s => s == null))
            {
                connectionString = configuration.GetConnectionString("DefaultConnection");
            }
            else
            {
                connectionString = $"Server={host}, {port};Database={database};User Id={user};Password={password};";
            }
            return connectionString;
        }
    }
}