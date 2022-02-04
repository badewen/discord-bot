using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Commands.DebugCommand
{
    public class Sleep : ModuleBase<SocketCommandContext>
    {
        [Command("sleep")]
        public async Task SleepAsync(int duration)
        {
            await Task.Delay(duration);
            return;
        }

        Sleep()
        {
            CommandList.register(new CommandData(".sleep <duraiton ms>", "you know ,you know", "SleepAsync", typeof(Sleep), Categories.Debug));
        }
    }
}
