using Discord;
using Discord.Commands;
using Bot.Attributes;
using System.Threading.Tasks;

namespace Bot.Commands.Misc
{
    public class Help : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".help <category> or <command>";
        private const string Description = "display this message";

        [Command("help")]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Misc)]
        public async Task HelpAsync(string arg = "default")
        {
            // very proud of this code very "clean"
            arg = arg.ToLower();
            try
            {
                var command = CommandList.listofcommands[arg];
                await ReplyAsync(embed: command);
            }
            catch
            {
                await ReplyAsync(message: "Cant find that command", messageReference: new MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            }

            return ;
        }
    }
}