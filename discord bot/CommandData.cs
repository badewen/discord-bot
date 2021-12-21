using System;
using System.Threading.Tasks;
using Bot.Attributes;
using System.Collections.Generic;

namespace Bot
{
    internal class CommandData
    {
        public static void register(Type a, Categories cat)
        {
            classes.Add(a);
            switch (cat)
            {
                case Categories.Fun:
                    Fun.Add(a);
                    break;
                case Categories.Misc:
                    Misc.Add(a);
                    break;
                case Categories.Moderation:
                    Moderation.Add(a);
                    break;
                default:
                    break;
            }
        }

        public static Task PrepCat()
        {
            Category.Add(Categories.Fun, Fun);
            Category.Add(Categories.Misc, Misc);
            Category.Add(Categories.Moderation, Moderation);
            return Task.CompletedTask;
        }

        public static List<Type> Fun = new();
        public static List<Type> Misc = new();
        public static List<Type> Moderation = new();

        public static List<Type> classes = new();
        public static Dictionary<Categories, List<Type>> Category = new Dictionary<Categories, List<Type>>();
    }
}