using System;

namespace Bot.Attributes
{ 
    [Obsolete]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class CategoryAttribute : Attribute
    {
        public CategoryAttribute(Categories cat)
        {
            Category = cat;
        }

        public Categories Category { get; }
    }
}