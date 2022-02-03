using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Bot.Commands.Moderation 
{
    public class Timeout : ModuleBase<SocketCommandContext>
    {
        // in progress
        [Command("timeout")]
        [Alias("mute")]
        public Task TimeoutAsync()
        {
            var downloadTask = Task.Run(() => { Context.Guild.DownloadUsersAsync(); });
            
            return Task.CompletedTask;
        }

        public Timeout()
        {
            CommandList.register(new CommandData(".timeout <user>", "timeout user", "TimeoutAsync", typeof(Timeout) ,Categories.Moderation));
        }
    }
    
}
