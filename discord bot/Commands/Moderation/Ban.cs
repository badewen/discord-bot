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
        [Summary("ban people (no time support me laptop potato)")]
        [Usage(".ban <mention a member / member Id> ")]
        public async Task BanAsync([Remainder] string member)
        {
            if (member.Length == 0)
            {
                await ReplyAsync("Invalid usage", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }
            List<ulong> membersId = new();
            List<SocketUser> mentionedUsers = new();
            _ = Task.Run(() =>
            {
                foreach (var id in member.Split(' '))
                {
                    try { membersId.Add(Convert.ToUInt64(id)); }
                    catch { continue; }
                }

                try
                {
                    foreach (var mentioned in Context.Message.MentionedUsers)
                    {
                        mentionedUsers.Add(mentioned);
                    }
                }
                catch { }
                try
                {
                    foreach (var id in membersId)
                    {
                        var ika = Context.Guild.GetUser(id);
                        var guh = Context.Guild.GetUser(Context.User.Id);
                        if (ika.Hierarchy > guh.Hierarchy)
                        {
                            ReplyAsync("Cant ban people above you", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                            break;
                        }
                        else Context.Guild.AddBanAsync(id);
                    }
                    foreach (var mentioned in mentionedUsers)
                    {
                        var targ = mentioned as SocketGuildUser;
                        var auth = Context.User as SocketGuildUser;
                        if (targ.Hierarchy > auth.Hierarchy)
                        {
                            ReplyAsync("Cant ban people above you", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                            break;
                        }
                        else Context.Guild.AddBanAsync(mentioned);
                    }
                }
                catch { ReplyAsync("Something went wrong", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id)); return; }
            });
            return;
        }

        public Ban()
        {
            CommandData.register(typeof(Ban), Categories.Moderation);
        }
    }
}