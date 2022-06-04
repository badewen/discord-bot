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
        public static readonly string PREFIX = ".";
        public static readonly JObject ClientConfig = JObject.Parse(File.ReadAllText(@"./config.json"));

        private static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            Console.WriteLine(ClientConfig["token"].ToString());
            discordConfig.GatewayIntents = (GatewayIntents)32447;
            client = new DiscordSocketClient(discordConfig);
            CommandServiceConfig.CaseSensitiveCommands = false;
            client.Log += Log;
            await client.LoginAsync(TokenType.Bot, ClientConfig["token"].ToString());
            client.Ready += OnReady;
            await client.StartAsync();
            await Task.Delay(-1);
        }

        private async Task OnReady()
        {
            List<Task> allTasks = new();
            commandHandler = new CommandHandler(commandService);
            allTasks.Add(commandHandler.SetupCommandsAsync()); 
            CategoryTable.Init();
            CommandList.RegisterAllCommands();
            //oh god im confused
            CommandList.RegisterCommandListCategories();
            CommandList.RegisterCommandsCategories();
            CommandList.BuildCommandsEmbed();
            Cooldown.PrepareCooldown();
            Console.WriteLine($"{client.CurrentUser} is ready");
            allTasks.Add(client.SetGameAsync(".help"));

            foreach (var guild in client.Guilds)
            {
                allTasks.Add(guild.DownloadUsersAsync());
            }
            await Task.WhenAll(allTasks);
        }

        private Task Log(LogMessage message)
        {
            Console.WriteLine(message);
            return Task.CompletedTask;
        }
    }
}