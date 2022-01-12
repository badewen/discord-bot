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
                    Fun.Add(commandClass);
                    break;

                case Categories.Misc:
                    Misc.Add(commandClass);
                    break;

                case Categories.Moderation:
                    Moderation.Add(commandClass);
                    break;

                case Categories.Debug:
                    Debug.Add(commandClass);
                    break;
                default:
                    break;
            }
        }

        internal static Task PrepCat()
        {
            Category.Add(Categories.Fun, Fun);
            Category.Add(Categories.Misc, Misc);
            Category.Add(Categories.Moderation, Moderation);
            Category.Add(Categories.Debug, Debug);
            return Task.CompletedTask;
        }

        internal static List<Type> Fun = new();
        internal static List<Type> Misc = new();
        internal static List<Type> Moderation = new();
        internal static List<Type> Debug = new();
        
        internal static List<Type> classes = new();
        internal static Dictionary<Categories, List<Type>> Category = new Dictionary<Categories, List<Type>>();
    }
}