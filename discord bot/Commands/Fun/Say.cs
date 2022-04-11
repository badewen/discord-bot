using Bot.Attributes;
using Discord.Commands;
using System.Threading.Tasks;

namespace Bot.Commands.Fun
{
    public class Say : ModuleBase<SocketCommandContext>
    {
        private const string Usage = ".say <message>";
        private const string Description = "make bot what you want it to say";

        [RequireBotPermission(Discord.ChannelPermission.SendMessages)]
        [Command("say")]
        [Usage(Usage)]
        [Description(Description)]
        [Category(Category.Fun)]
        public async Task SayAsync([Remainder] string message)
        {
            var arg = message;
            arg = arg.Trim();
            if (arg.Length == 0) return;
            foreach (var mentionedMember in Context.Message.MentionedUsers)
            {
                arg = arg.Replace($"<@{mentionedMember.Id}>", $"@{mentionedMember.Username}");
            }
            foreach (var mentionedRole in Context.Message.MentionedRoles)
            {
                arg = arg.Replace($"<@&{mentionedRole.Id}>", $"@{mentionedRole.Name}");
            }
            if (Context.Message.MentionedEveryone) arg = arg.Replace("@everyone", "@‎everyone");
            await ReplyAsync(arg.ToString());
            return;
        }
    }
}