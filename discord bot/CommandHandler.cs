// i have no idea how this class work
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Bot
{
    public class CommandHandler
    {
        public DiscordSocketClient Client
        {
            get;
        }

        private readonly CommandService _commandsService;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this.Client = client;
            _commandsService = commands;
        }

        public async Task SetupCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await _commandsService.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: null);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            Console.WriteLine($"{arg.Author.Username}#{arg.Author.Username} : {arg.Content}");

            SocketUserMessage message = arg as SocketUserMessage;
            if (message == null) return;
            int argpos = 0;
            CommandData dCommand = new();
            if (!(message.HasCharPrefix('.', ref argpos) || message.HasMentionPrefix(Client.CurrentUser, ref argpos)) || message.Author.IsBot || !CommandList.CommandsDic.TryGetValue(message.Content[1..].Split(' ')[0], out dCommand)) return;
            var context = new SocketCommandContext(Client, message);
            if (dCommand.Category == Category.Debug)
            {
                var allowed = Program.ClientConfig["debugAllowedUser"].ToObject<string[]>();
                bool found = false;
                foreach (var i in allowed)
                {
                    if (message.Author.Id.ToString() == i)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return;
                }
            }
            await _commandsService.ExecuteAsync( //magic
                context: context,
                argPos: argpos,
                services: null);
        }
    }
}