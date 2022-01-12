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
        public async Task BanAsync([Remainder] string arg)
        {
            if (arg.Length == 0)
            {
                await ReplyAsync("Invalid usage", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                return;
            }
            var s = Task.Run(() =>
            {
                string asd = arg;
                List<ulong> membersId = new();
                List<SocketUser> mentionedUsers = new();
                // loop through members id in the arg
                foreach (var id in asd.Split(' '))
                {
                    if (id.StartsWith("<@")) continue;
                    try { membersId.Add(Convert.ToUInt64(id)); }
                    catch { continue; }
                }

                try
                {
                    foreach (var mentioned in Context.Message.MentionedUsers)
                    {
                        mentionedUsers.Add(mentioned);
                        Console.WriteLine(mentioned);
                    }
                }
                catch { }
                try
                {
                    foreach (var id in membersId)
                    {
                        var target = Context.Guild.GetUser(id);
                        var author = Context.Guild.GetUser(Context.User.Id);
                        if (target == null)
                        {
                            Console.WriteLine(id);
                            ReplyAsync("that people doesnt exist", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                            return;
                        }
                        if (target.Hierarchy > author.Hierarchy)
                        {
                            ReplyAsync("Cant ban people above you", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                            break;
                        }
                        else 
                        { 
                            Context.Guild.AddBanAsync(id);
                            ReplyAsync($"{target.Username}#{target.Discriminator} has been banned");
                        }
                    }
                    foreach (var mentioned in mentionedUsers)
                    {
                        var target = Context.Guild.GetUser(mentioned.Id);
                        var author = Context.Guild.GetUser(Context.User.Id);
                        Console.WriteLine(mentioned.Id);
                        if (target == null)
                        {
                            ReplyAsync("that people doesnt exist", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                            return;
                        }
                        if (target.Hierarchy > author.Hierarchy)
                        {
                            ReplyAsync("Cant ban people above you", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
                            break;
                        }
                        else
                        { 
                            Context.Guild.AddBanAsync(mentioned);
                            ReplyAsync($"{target.Username}#{target.Discriminator} has been banned");
                        }
                    }
                }
                catch { ReplyAsync("Something went wrong", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id)); return; }
            });
            s.Wait(10);
            return;
        }

        public Ban()
        {
            CommandData.register(typeof(Ban), Categories.Moderation);
        }
    }
}