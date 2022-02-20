using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot;
using Bot.Attributes;
using Discord.Commands;

namespace Bot.Commands.DebugCommand
{
    public class IsNaN : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".isnan <ur thing here>";
        private const string Description = "check is NaN?";
        [Command("isnan")]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Debug)]
        public async Task IsNaNAsync(string arg)
        {
            await Context.Channel.SendMessageAsync(Utils.Isnan(arg).ToString());

            return;
        }
    }
}
