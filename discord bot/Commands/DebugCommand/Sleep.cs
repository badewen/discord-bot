using Bot.Attributes;
using Discord.Commands;
using System.Threading.Tasks;

namespace Bot.Commands.DebugCommand
{
    public class Sleep : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".sleep <duration(ms)>";
        private const string Description = "sleep";

        [Command("sleep", RunMode = RunMode.Async)]
        [Usage(Usage)]
        [Description(Description)]
        public async Task SleepAsync(int duration)
        {
            await Task.Delay(duration);
            await ReplyAsync("Done sleeping");
            return;
        }
    }
}