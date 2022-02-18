using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Commands.DebugCommand
{
    public class Sleep : ModuleBase<SocketCommandContext>
    {//3126
        [Command("sleep")]
        public Task SleepAsync(int duration)
        {
            _= Task.Run(() => { Task.Delay(duration); });
            return Task.CompletedTask;
        }

        Sleep()
        {
            CommandList.register(new CommandData(".sleep <duraiton ms>", "you know ,you know", "SleepAsync", typeof(Sleep), Categories.Debug));
        }
    }
}
