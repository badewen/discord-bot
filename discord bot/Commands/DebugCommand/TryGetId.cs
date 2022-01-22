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
        public Task TryGetIdAsync(string id)
        {
            Context.Guild.DownloadUsersAsync();
            var allowed = Program.ClientConfig["debugAllowedUser"].ToObject<string[]>();

            foreach(var i in allowed)
            {
                if (Context.User.Id.ToString() != i)
                {
                    return Task.CompletedTask;
                }
            }
            var e = Convert.ToUInt64(id);

            var result = Context.Guild.GetUser(e);
            if (result == null)
            {
                ReplyAsync("failed to get user", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return Task.CompletedTask;
            }
            ReplyAsync("success", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            return Task.CompletedTask;
        }

        public TryGetId()
        {
            CommandList.register(new CommandData(".trygetid <user>", "get user id", "TryGetIdAsync", typeof(TryGetId), Categories.Debug));
        }
    }
}
