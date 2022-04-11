using System;

namespace Bot.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class CategoryAttribute : Attribute
    {
        public Category category;

        public CategoryAttribute(Category category)
        {
            this.category = category;
        }
    }
}