using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot;
using Discord.Commands;

namespace Bot.Commands.DebugCommand
{
    public class IsNaN : ModuleBase<SocketCommandContext>
    {
        [Command("isnan")]
        public async Task IsNaNAsync(string arg)
        {
            await Context.Channel.SendMessageAsync(Utils.Isnan(arg).ToString());

            return;
        }

        IsNaN()
        {
            CommandList.register(new CommandData(".isnan <param>", "check if is nan", "IsNaNAsync", typeof(IsNaN), Categories.Debug));
        }
    }
}
