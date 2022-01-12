using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Bot.Attributes;

namespace Bot.Commands.Debug
{
    public class TryGetId : ModuleBase<SocketCommandContext>
    {
        [Command("trygetid")]
        [Alias("getid")]
        [Summary("debug")]
        [Usage(".trygetid <id>")]
        public Task TryGetIdAsync(ulong id)
        {
            var allowed = Program.ClientConfig["debugAllowedUser"].ToObject<string[]>();

            foreach(var a in allowed)
            {
                if (Context.User.Id.ToString() != a)
                {
                    return Task.CompletedTask;
                }
            }

            var result = Context.Guild.GetUser(id);

            if (result == null)
            {//741846750867750972
                ReplyAsync("failed to get user", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return Task.CompletedTask;
            }
            ReplyAsync("success", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            return Task.CompletedTask;
        }

        public TryGetId()
        {
            CommandData.register(typeof(TryGetId), Categories.Debug);
        }
    }
}
