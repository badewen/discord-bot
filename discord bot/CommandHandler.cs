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
        private readonly DiscordSocketClient client;
        private readonly CommandService _commandsService;

        public CommandHandler(DiscordSocketClient client, CommandService commands)
        {
            this.client = client;
            _commandsService = commands;
        }

        public async Task SetupCommandsAsync()
        {
            client.MessageReceived += HandleCommandAsync;
            await _commandsService.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: null);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            Console.WriteLine($"{arg.Author.Username}#{arg.Author.Username} : {arg.Content}");
            var message = arg as SocketUserMessage;
            if (message == null) return;
            int argpos = 0;

            if (!(message.HasCharPrefix('.', ref argpos) || message.HasMentionPrefix(client.CurrentUser, ref argpos)) || message.Author.IsBot) return;
            var context = new SocketCommandContext(client, message);

            await _commandsService.ExecuteAsync( //magic
                context: context,
                argPos: argpos,
                services: null
                );
        }

        public CommandService GetCommandService()
        {
            return _commandsService;
        }
    }
}
