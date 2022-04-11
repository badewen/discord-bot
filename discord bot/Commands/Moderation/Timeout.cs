using Bot.Attributes;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Commands.Moderation
{
    public class Timeout : ModuleBase<SocketCommandContext>
    {
        private const string usage = ".timeout <user>";
        private const string description = "timeout user";

        // in progress
        [Command("timeout", RunMode = RunMode.Async)]
        [Alias("mute")]
        [Usage(usage)]
        [Description(description)]
        [Category(Category.Moderation)]
        public async Task TimeoutAsync([Remainder] string input)
        {
            await Context.Guild.DownloadUsersAsync();
            string[] args = input.Split(' ');
            List<ulong> ids = new();
            TimeSpan Duration;
            int Seconds = 10;
            int Minutes = 0;
            int Hours = 0;
            int Days = 0;
            foreach (var arg in args)
            {
                if (!Utils.Isnan(arg) && arg.Length == 18)
                {
                    ids.Add(Convert.ToUInt64(arg));
                    continue;
                }
                if (arg.EndsWith('s') || arg.EndsWith('m') || arg.EndsWith('d') || arg.EndsWith('h'))
                {
                    if (Utils.Isnan(arg.Substring(0, arg.Length - 1)))
                    {
                        continue;
                    }
                    else
                    {
                        if (arg.EndsWith('s'))
                        {
                            Seconds = Convert.ToInt32(arg.Substring(0, arg.Length - 1));
                        }
                        else if (arg.EndsWith('m'))
                        {
                            Minutes = Convert.ToInt32(arg.Substring(0, arg.Length - 1));
                        }
                        else if (arg.EndsWith('d'))
                        {
                            Days = Convert.ToInt32(arg.Substring(0, arg.Length - 1));
                        }
                        else
                        {
                            Hours = Convert.ToInt32(arg.Substring(0, arg.Length - 1));
                        }
                    }
                }
            }
            Duration = new(Days, Hours, Minutes, Seconds);
            foreach (var mentioned in Context.Message.MentionedUsers)
            {
                ids.Add(mentioned.Id);
            }
            int aboveAuthor = 0;
            int aboveBot = 0;
            int doesntExist = 0;
            foreach (var id in ids)
            {
                Discord.WebSocket.SocketGuildUser target = Context.Guild.GetUser(id);
                if (target == null) { doesntExist++; continue; }
                if (!Utils.isHigher(Context.Guild, Context.User, id)) { aboveAuthor++; continue; }
                if (!Utils.isHigher(Context.Guild, Program.client.CurrentUser, id)) { aboveBot++; continue; }
                await target.SetTimeOutAsync(Duration);
                Console.WriteLine($"{target.Username} timeouted for {Duration.Days}D , {Duration.Hours}H, {Duration.Minutes}M, {Duration.Seconds}S");
            }
            await ReplyAsync($"done timeout-ing member \n cant timeout {aboveAuthor} user because above you\n cant timeout {aboveBot} user because above me\n {doesntExist} users doesn't exist", messageReference: new Discord.MessageReference(Context.Message.Id, Context.Channel.Id, Context.Guild.Id));

            return;
        }
    }
}