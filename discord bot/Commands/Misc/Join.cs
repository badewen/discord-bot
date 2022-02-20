using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using Discord;
using Bot.Attributes;
using System.Threading.Tasks;

namespace Bot.Commands.Misc
{
    public class Join : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".join <channel (optional)>";
        private const string Description = "join voice channel. If no channel is provided, it will join your voice channel";
        [Command("join",RunMode = RunMode.Async)]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Misc)]
        public async Task JoinAsync(IVoiceChannel channel = null)
        {

        }

    }
}
