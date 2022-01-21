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
                    funCommands.Add(commandClass);
                    break;

                case Categories.Misc:
                    miscCommands.Add(commandClass);
                    break;

                case Categories.Moderation:
                    moderationCommands.Add(commandClass);
                    break;

                case Categories.Debug:
                    debugCommands.Add(commandClass);
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

        private static List<Type> funCommands = new();
        private static List<Type> miscCommands = new();
        private static List<Type> moderationCommands = new();
        private static List<Type> debugCommands = new();
        
        internal static List<Type> classes = new();
        internal static Dictionary<Categories, List<Type>> categoryComands = new Dictionary<Categories, List<Type>>();
    }
}
