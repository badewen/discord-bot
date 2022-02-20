using System;
using Bot.Attributes;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Commands.Debug
{
    public class TryGetId : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".trygetid <user>";
        private const string Description = "get user id";
        [Command("trygetid")]
        [Alias("getid")]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Debug)]
        public async Task TryGetIdAsync(string id)
        {
            await Context.Guild.DownloadUsersAsync();
            var e = Convert.ToUInt64(id);

            var result = Context.Guild.GetUser(e);
            if (result == null)
            {
                await ReplyAsync("failed to get user", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }
            await ReplyAsync("success", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            return ;
        }
    }
}
