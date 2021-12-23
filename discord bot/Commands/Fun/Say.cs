using Bot.Attributes;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Bot.Commands.Fun
{
    public class Say : ModuleBase<SocketCommandContext>
    {
        public Say()
        {
            CommandData.register(typeof(Say), Categories.Fun);
        }

        [RequireBotPermission(Discord.ChannelPermission.SendMessages)]
        [Command("say")]
        [Usage(".Say <message>")]
        [Summary("make bot say what you want it to say")]
        public Task SayAsync([Remainder] string message)
        {
            var arg = message;
            arg = arg.Trim();
            if (arg.Length == 0) return Task.CompletedTask;
            foreach (var mentionedMember in Context.Message.MentionedUsers)
            {
                arg = arg.Replace($"<@{mentionedMember.Id}>", $"@{mentionedMember.Username}");
            }
            foreach (var mentionedRole in Context.Message.MentionedRoles)
            {
                arg = arg.Replace($"<@&{mentionedRole.Id}>", $"@{mentionedRole.Name}");
            }
            if (Context.Message.MentionedEveryone) arg = arg.Replace("@everyone", "@‎everyone");
            ReplyAsync(arg.ToString());
            return Task.CompletedTask;
        }
    }
}