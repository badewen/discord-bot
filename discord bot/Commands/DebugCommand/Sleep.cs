using Bot.Attributes;
using System.Threading.Tasks;
using Discord.Commands;

namespace Bot.Commands.DebugCommand
{
    public class Sleep : ModuleBase<SocketCommandContext>
    {//3126
        private const string Usage = ".sleep <duration(ms)>";
        private const string Description = "sleep";
        [Command("sleep", RunMode = RunMode.Async)]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Debug)]
        public async Task SleepAsync(int duration)
        {
            await Task.Delay(duration);
            await ReplyAsync("Done sleeping");
            return ;
        }
    }
}
