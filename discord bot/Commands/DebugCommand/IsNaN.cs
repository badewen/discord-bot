using Bot.Attributes;
using Discord.Commands;
using System.Threading.Tasks;
using System;

namespace Bot.Commands.DebugCommand
{
    public class IsNaN : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".isnan <ur thing here>";
        private const string Description = "check is NaN?";

        [Command("isnan")]
        [Usage(Usage)]
        [Description(Description)]
        public async Task IsNaNAsync(string arg)
        {
            await Context.Channel.SendMessageAsync(Utils.Isnan(arg).ToString());
            return;
        }
    }
}