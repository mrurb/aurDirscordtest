using aurtest.Startup;
using Discord.Rest;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Bootstrapper))]

namespace aurtest.Startup
{
    internal class Bootstrapper : FunctionsStartup
    {
        private static readonly string ApplicationGoogleDriveServiceAccountId = Environment.GetEnvironmentVariable(nameof(ApplicationGoogleDriveServiceAccountId), EnvironmentVariableTarget.Process);
        private static readonly string ApplicationGoogleDriveServiceAccountPrivateKey = Environment.GetEnvironmentVariable(nameof(ApplicationGoogleDriveServiceAccountPrivateKey), EnvironmentVariableTarget.Process);

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
               .AddSingleton<DiscordRestClient>(c => { 
                   DiscordRestClient discordRestClient = new();
                   discordRestClient.LoginAsync(Discord.TokenType.Bot, "OTc5ODQ2NDIwNDU2NjczMzYx.GF3WDT.OYXqupAV7crpU9U4I72ONSLrYVQav9O0Loyo4k").Wait();
                   return discordRestClient; 
               });
        }
    }
}