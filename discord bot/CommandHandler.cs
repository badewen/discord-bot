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

        private readonly CommandService _commandsService;

        public CommandHandler(CommandService commands)
        {
            _commandsService = commands;
        }

        public Task SetupCommandsAsync()
        {
            Program.client.MessageReceived += HandleCommandAsync;
             _commandsService.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(),
                services: null);
            return Task.CompletedTask;
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {

            Console.WriteLine($"{arg.Author.Username}#{arg.Author.Username} : {arg.Content}");

            SocketUserMessage message = arg as SocketUserMessage;

            if (message.Author.IsBot) return;
            if (message == null) return;
            if (message.Content.Substring(0, Program.PREFIX.Length) != Program.PREFIX) return;
            
            // note: d doesnt mean anything. i ran out of names
            int argpos = Program.PREFIX.Length;
            CommandData dCommand;
            string InvokedCommand = message.Content.Trim();
            {
                int pos = InvokedCommand.IndexOf(' ');
                // only prefix and command
                if (pos == -1)
                {
                    InvokedCommand = InvokedCommand[Program.PREFIX.Length..];
                }
                // contain args
                else
                {
                    InvokedCommand = InvokedCommand[Program.PREFIX.Length..new Index(pos)];
                }
            }
            var context = new SocketCommandContext(Program.client, message);

            if (!CommandList.CommandsDic.TryGetValue(InvokedCommand, out dCommand))
            {
                await context.Channel.SendMessageAsync(text : "invalid command", messageReference: context.Message.Reference);
                return;
            }

            if (!Cooldown.IsUserExist(InvokedCommand, message.Author.Id))
            {
                Cooldown.AddUser(InvokedCommand, message.Author.Id);
            }
            else
            {
                (bool done, UInt64 secPassed) = Cooldown.IsCooldownDone(dCommand.CooldownSec, InvokedCommand, message.Author.Id);
                if (!done)
                {
                    await context.Channel.SendMessageAsync($"that command is on cooldown try again in {dCommand.CooldownSec - secPassed}s", messageReference: new Discord.MessageReference(message.Id, message.Channel.Id, context.Guild.Id));
                    return;
                }
                else
                {
                    Cooldown.UpdateUserCooldown(InvokedCommand, message.Author.Id);
                }
            }

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