using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot
{
    public class CommandData
    {
        public static void register(Type commandClass, Categories category)
        {
            CommandClasses.Add(commandClass);
            switch (category)
            {
                case Categories.Fun:
                    FunCommands.Add(commandClass);
                    break;

                case Categories.Misc:
                    MiscCommands.Add(commandClass);
                    break;

                case Categories.Moderation:
                    ModerationCommands.Add(commandClass);
                    break;

                case Categories.Debug:
                    DebugCommands.Add(commandClass);
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

        private static List<Type> FunCommands = new();
        private static List<Type> MiscCommands = new();
        private static List<Type> ModerationCommands = new();
        private static List<Type> DebugCommands = new();
        
        internal static List<Type> CommandClasses = new();
        internal static Dictionary<Categories, List<Type>> CategoryCommands = new Dictionary<Categories, List<Type>>();
    }
}
