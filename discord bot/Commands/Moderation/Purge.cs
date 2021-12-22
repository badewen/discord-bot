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
        [RequireBotPermission(Discord.ChannelPermission.ManageMessages)]
        [Command("purge")]
        [Usage(".purge <amount>")]
        [Summary("Delete message under 14 days (ask discord why only under 14 days)")]
        [RequireUserPermission(Discord.GuildPermission.ManageMessages)]
        public async Task PurgeAsync(string message)
        {
            int amount;
            try
            {
                amount = Convert.ToInt32(message);
            }
            catch
            {
                await ReplyAsync("enter ONLY number please.", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }

            List<Discord.IMessage> a = new();

            _ = Task.Run(() => // used "_" to make ugly green linting gone
              {
                  var idk = Context.Channel.GetMessagesAsync(fromMessage: Context.Message, Discord.Direction.Before, limit: amount).ToListAsync().Result;
                  foreach (var c in idk)
                  {
                      foreach (var f in c)
                      {
                          a.Add(f);
                      }
                  }
                  (Context.Channel as SocketTextChannel).DeleteMessagesAsync(a);
                  ReplyAsync($"messages has been removed", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Message.Channel.Id, Context.Guild.Id));
              });
            return;
        }
        public Purge()
        {
            CommandData.register(typeof(Purge), Categories.Moderation);
        }
    }
}