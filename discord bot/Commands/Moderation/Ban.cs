using Bot.Attributes;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Commands.Moderation
{
    public class Ban : ModuleBase<SocketCommandContext>
    {
        [RequireBotPermission(Discord.GuildPermission.BanMembers)]
        [RequireUserPermission(Discord.GuildPermission.BanMembers)]
        [Command("ban")]
        public async Task BanAsync([Remainder] string arg)
        {
            if (arg.Length == 0)
            {
                await ReplyAsync("Invalid usage", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }
            

            return;
        }

        public Ban()
        {
            CommandList.register(new CommandData(
                ".ban <id / mention>", 
                "ban people", 
                "BanAsync", 
                typeof(Ban), 
                Categories.Moderation));            
        }
    }
}