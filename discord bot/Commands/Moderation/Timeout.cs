using Bot.Attributes;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Commands.Moderation
{
    public class Timeout : ModuleBase<SocketCommandContext>
    {
        private const string _usage = ".timeout <user>";
        private const string _description = "timeout user";

        // in progress
        [Command("timeout", RunMode = RunMode.Async)]
        [Alias("mute")]
        [Usage(_usage)]
        [Description(_description)]
        public async Task TimeoutAsync([Remainder] string input)
        {
            Console.WriteLine("im invoked");
            List<Task> timeoutTasks = new();
            var downloadTask = Context.Guild.DownloadUsersAsync();
            string[] args = input.Split(' ');
            List<ulong> ids = new();
            TimeSpan duration;
            int seconds = 0;
            int minutes = 0;
            int hours = 0;
            int days = 0;
            foreach (var arg in args)
            {
                if (!Utils.Isnan(arg) && arg.Length == 18)
                {
                    ids.Add(Convert.ToUInt64(arg));
                    continue;
                }
                (int parsed, char durT) = Utils.ParseDuration(arg.AsSpan());  
                
                if (durT == 'e')
                {
                    continue;
                }
                else if (durT == 's')
                {
                    seconds = parsed;
                }
                else if (durT == 'h')
                {
                    hours = parsed;
                }
                else if (durT == 'd')
                {
                    days = parsed;
                }
            }

            if (seconds == 0 && hours == 0 && days == 0)
            {
                seconds = 10;
            }

            duration = new(days, hours, minutes, seconds);
            foreach (var mentioned in Context.Message.MentionedUsers)
            {
                ids.Add(mentioned.Id);
            }
            int aboveAuthor = 0;
            int aboveBot = 0;
            int doesntExist = 0;
            await downloadTask;
            foreach (var id in ids)
            {
                Discord.WebSocket.SocketGuildUser target = Context.Guild.GetUser(id);
                if (target == null) { doesntExist++; continue; }
                if (!Utils.isHigher(Context.Guild, Context.User, id)) { aboveAuthor++; continue; }
                if (!Utils.isHigher(Context.Guild, Program.client.CurrentUser, id)) { aboveBot++; continue; }
                timeoutTasks.Add(target.SetTimeOutAsync(duration));
                Console.WriteLine($"{target.Username} timeouted for {duration.Days}D , {duration.Hours}H, {duration.Minutes}M, {duration.Seconds}S");
            }
            await Task.WhenAll(timeoutTasks);
            await ReplyAsync($"done timeout-ing member \n cant timeout {aboveAuthor} user because above you\n cant timeout {aboveBot} user because above me\n {doesntExist} users doesn't exist", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));

            return;
        }
    }
}