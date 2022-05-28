using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using Discord.Rest;
using Discord.Net;
using System.Linq;

namespace aurtest
{
    //change 2
    public class Function1
    {
        private const string PublicKey = "8f32935a699deed492cdf6610888babebbbaef5c9c20930fea2ca3b24df331db";
        private readonly DiscordRestClient client;

        public Function1(DiscordRestClient client)
        {
            this.client = client;
        }

        [FunctionName("Function4")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");




            string timestamp = req.Headers["X-Signature-Timestamp"];
            string signature = req.Headers["X-Signature-Ed25519"];
            //string body = await req.ReadAsStringAsync();

            MemoryStream ms = new MemoryStream();
            req.Body.CopyTo(ms);
            byte[] bytes = ms.ToArray();

            RestInteraction restInteraction = await client.ParseHttpInteractionAsync(PublicKey, signature, timestamp, bytes);
            try
            {
                if (restInteraction.Type == InteractionType.Ping)
                {
                    string v = ((RestPingInteraction)restInteraction).AcknowledgePing();
                    return new ContentResult() {Content = v, StatusCode = 200, ContentType = "application/json" };
                }
            }
            catch (Exception)
            {

                throw;
            }


            /*if (restInteraction.IsDMInteraction)
            {
                await restInteraction.User.SendMessageAsync($"{restInteraction.User.Mention}gay");
            }
            else
            {
                
                await restInteraction.Channel.SendMessageAsync("gay");
            }*/

            string value = (string)(((RestSlashCommand)restInteraction).Data.Options.First().Value);
            if (value == "delay")
            {
                await Task.Delay(TimeSpan.FromSeconds(10));
            }
            string v1 = restInteraction.Respond("stuff");
         

            return new ContentResult() { Content = v1, StatusCode = 200, ContentType = "application/json" };
        }

        [FunctionName("Function5")]
        public async Task<IActionResult> Run2(
       [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
       ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            await Client_Ready();


            return new OkResult();
        }

        public async Task Client_Ready()
        {
            RestGuild restGuild = await client.GetGuildAsync(538880298830790686);

            var guildCommand = new SlashCommandBuilder()
                .WithName("bad")
                .WithDescription("bad")
                .AddOption(new SlashCommandOptionBuilder()
                    .WithName("badoption")
                    .WithDescription("bad")
                    .WithType(ApplicationCommandOptionType.SubCommand)
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithName("teststring")
                        .WithDescription("bad")
                        .WithType(ApplicationCommandOptionType.String))
                    .AddOption(new SlashCommandOptionBuilder()
                        .WithName("user")
                        .WithDescription("bad")
                        .WithType(ApplicationCommandOptionType.User))
                );

            await restGuild.CreateApplicationCommandAsync(guildCommand.Build());
        }
    }
}
