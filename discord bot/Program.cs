using Bot.Commands.Misc;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Bot
{
    public class Program
    {
        private static DiscordSocketConfig discordConfig = new();
        public static DiscordSocketClient client;
        private static CommandServiceConfig CommandServiceConfig = new CommandServiceConfig();
        private static readonly CommandService commandService = new();
        public static CommandHandler commandHandler;
        public static readonly JObject ClientConfig = JObject.Parse(File.ReadAllText(@"./config.json"));

        private static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();
        
        public async Task MainAsync()
        {
            discordConfig.GatewayIntents = (GatewayIntents)32447;
            client = new DiscordSocketClient(discordConfig);
            client.Log += Log;
            client.Ready += OnReady;
            await client.LoginAsync(TokenType.Bot, ClientConfig["token"].ToString());
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            CommandServiceConfig.CaseSensitiveCommands = false;
            commandHandler = new CommandHandler(client, commandService);
            await commandHandler.SetupCommandsAsync(); // class command cons called
            await CommandList.RegisterAllCommands();
            await CommandList.PrepareCategories();
            await CommandList.BuildCommandsEmbed();
            Console.WriteLine($"{client.CurrentUser} is ready");
            await client.SetGameAsync(".help");

            List<Task> downloadTasks = new();
            foreach(var guild in client.Guilds)
            {
                downloadTasks.Add(Task.Run(() => { guild.DownloadUsersAsync(); }));
            }
            await Task.WhenAll(downloadTasks);
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}