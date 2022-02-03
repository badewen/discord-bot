using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Rest;
using Bot;

namespace Bot.Commands.Moderation 
{
    public class Timeout : ModuleBase<SocketCommandContext>
    {
        // in progress
        [Command("timeout")]
        [Alias("mute")]
        public Task TimeoutAsync([Remainder] string input)
        {
            var downloadTask = Task.Run(() => { Context.Guild.DownloadUsersAsync(); });
            List<string> args = input.Split(' ').ToList<string>();
            List<ulong> ids = new();
            TimeSpan Duration;
            int Seconds = 10;
            int Minutes = 0;
            int Hours = 0;
            int Days = 0;
            foreach(var arg in args)
            {
                if (arg.StartsWith("<@") && arg.EndsWith(">"))
                {
                    args.Remove(arg);
                    continue;
                }
                else if (!Utils.Isnan(arg) && arg.Length == 18)
                {
                    ids.Add(Convert.ToUInt64(arg));
                    continue;
                }
                if (arg.EndsWith('s') || arg.EndsWith('m') || arg.EndsWith('d') || arg.EndsWith('h'))
                {
                    if (Utils.Isnan(arg.Remove(arg.Length - 1)))
                    {
                        continue;
                    }
                    else
                    {
                        if (arg.EndsWith('s'))
                        {
                            Seconds = Convert.ToInt32(arg);
                        }
                        else if (arg.EndsWith('m'))
                        {
                            Minutes = Convert.ToInt32(arg);
                        }
                        else if (arg.EndsWith('d'))
                        {
                            Days = Convert.ToInt32(arg);
                        }
                        else
                        {
                            Hours = Convert.ToInt32(arg);
                        }
                    }
                }
            }
            Duration = new(Days, Hours, Minutes, Seconds);
            foreach(var mentioned in Context.Message.MentionedUsers)
            {
                ids.Add(mentioned.Id);
            }
            int aboveAuthor = 0;
            int aboveBot = 0;
            int doesntExist = 0;
            foreach(var id in ids)
            {
                var target = Context.Guild.GetUser(id);
                if (target == null) { doesntExist++; continue; }
                if (!Utils.isHigher(Context.Guild, Context.User, id)) { aboveAuthor++; continue; }
                if (!Utils.isHigher(Context.Guild, Program.client.CurrentUser, id)) { aboveBot++; continue; }
                target.SetTimeOutAsync(Duration);
            }
            ReplyAsync($"done banning member \n cant ban {aboveAuthor} user because above you\n cant ban {aboveBot} user because above me\n {doesntExist} users doesn't exist", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));
            return Task.CompletedTask;
        }

        public Timeout()
        {
            CommandList.register(new CommandData(".timeout <user>", "timeout user", "TimeoutAsync", typeof(Timeout) ,Categories.Moderation));
        }
    }
}
