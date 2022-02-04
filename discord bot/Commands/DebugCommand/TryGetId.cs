using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Commands.Debug
{
    public class TryGetId : ModuleBase<SocketCommandContext>
    {
        [Command("trygetid")]
        [Alias("getid")]
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

        public TryGetId()
        {
            CommandList.register(new CommandData(".trygetid <user>", "get user id", "TryGetIdAsync", typeof(TryGetId), Categories.Debug));
        }
    }
}
