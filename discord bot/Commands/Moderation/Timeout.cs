using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Bot.Attributes;

namespace Bot.Commands.Moderation 
{
    public class Timeout : ModuleBase<SocketCommandContext>
    {
        // in progress
        [Command("timeout")]
        [Usage(".timeout <user>")]
        [Alias("mute")]
        [Summary("timeout user from sending message (mute)")]
        public Task TimeoutAsync()
        {
            return Task.CompletedTask;
        }

        public Timeout()
        {
            CommandData.register(typeof(Timeout), Categories.Moderation);
        }
    }
    
}
