using System.Collections.Generic;

namespace Bot
{
    public enum Category
    {
        Fun = 1,
        Misc = 2,
        Moderation = 3,
        Debug = 4,
    }

    public class CategoryTable{
        public static Dictionary<string, Category> Table = new();
        public static void Init(){
            foreach(Category category in System.Enum.GetValues<Category>()){
                Table.Add(System.Enum.GetName<Category>(category), category);
            }
        }
    }

}