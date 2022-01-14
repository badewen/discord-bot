using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot
{
    public class CommandData
    {
        public static void register(Type commandClass, Categories cat)
        {
            classes.Add(commandClass);
            switch (cat)
            {
                case Categories.Fun:
                    s_funCommands.Add(commandClass);
                    break;

                case Categories.Misc:
                    s_miscCommands.Add(commandClass);
                    break;

                case Categories.Moderation:
                    s_moderationCommands.Add(commandClass);
                    break;

                case Categories.Debug:
                    s_debugCommands.Add(commandClass);
                    break;
                default:
                    break;
            }
        }

        internal static Task PrepCat()
        {
            Category.Add(Categories.Fun, s_funCommands);
            Category.Add(Categories.Misc, s_miscCommands);
            Category.Add(Categories.Moderation, s_moderationCommands);
            Category.Add(Categories.Debug, s_debugCommands);
            return Task.CompletedTask;
        }

        private static List<Type> s_funCommands = new();
        private static List<Type> s_miscCommands = new();
        private static List<Type> s_moderationCommands = new();
        private static List<Type> s_debugCommands = new();
        
        internal static List<Type> classes = new();
        // this need a new name
        // s_commandsCategory?
        internal static Dictionary<Categories, List<Type>> Category = new Dictionary<Categories, List<Type>>();
    }
}
