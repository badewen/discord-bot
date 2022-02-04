using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot
{
    public class CommandList
    {
        public static void register(CommandData Command)
        {
            Commands.Add(Command);
            switch (Command.Category)
            {
                case Categories.Fun:
                    FunCommands.Add(Command);
                    break;

                case Categories.Misc:
                    MiscCommands.Add(Command);
                    break;

                case Categories.Moderation:
                    ModerationCommands.Add(Command);
                    break;

                case Categories.Debug:
                    DebugCommands.Add(Command);
                    break;
                default:
                    break;
            }
        }

        internal static Task PrepareCategories()
        {
            CategoryCommands.Add(Categories.Fun, FunCommands);
            CategoryCommands.Add(Categories.Misc, MiscCommands);
            CategoryCommands.Add(Categories.Moderation, ModerationCommands);
            CategoryCommands.Add(Categories.Debug, DebugCommands);
            return Task.CompletedTask;
        }

        private static List<CommandData> FunCommands = new();
        private static List<CommandData> MiscCommands = new();
        private static List<CommandData> ModerationCommands = new();
        private static List<CommandData> DebugCommands = new();
        
        internal static List<CommandData> Commands = new();
        internal static Dictionary<string, CommandData> CommandsDic = new();
        internal static Dictionary<Categories, List<CommandData>> CategoryCommands = new Dictionary<Categories, List<CommandData>>();
    }
}
