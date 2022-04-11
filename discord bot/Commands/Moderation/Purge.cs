using Bot.Attributes;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bot.Commands.Moderation
{
    public class Purge : ModuleBase<SocketCommandContext>
    {
        private const string usage = ".purge <1-100>";
        private const string description = "delete message \n note: this only delete message that is less than 14 days old";

        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        [RequireBotPermission(Discord.ChannelPermission.ManageMessages)]
        [Command("purge")]
        [Usage(usage)]
        [Description(description)]
        [Category(Category.Moderation)]
        public async Task PurgeAsync(string message)
        {
            int amount;
            if (Utils.Isnan(message))
            {
                await ReplyAsync("enter ONLY number please.", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }
            amount = Convert.ToInt32(message);

            List<Discord.IMessage> messages = new();

            var idk = Context.Channel.GetMessagesAsync(fromMessage: Context.Message, Discord.Direction.Before, limit: amount).ToListAsync().Result;
            foreach (var c in idk)
            {
                foreach (var f in c)
                {
                    messages.Add(f);
                }
            }
            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages);
            await ReplyAsync($"messages has been removed", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Message.Channel.Id, Context.Guild.Id));

            return;
        }
    }
}