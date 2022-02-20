using System;
using System.Collections.Generic;
using Discord;
using System.Linq;
using System.Reflection;

namespace Bot
{
    public sealed class Utils
    {
        public static List<char> Nums = new(new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' });
        // true if nan 
        public static bool Isnan(string s)
        {
            if (s == null || s == "")
            {
                return true;
            }
            foreach (char c in s)
            {
                bool found = false;
                foreach(char num in Nums)
                {
                    if (c == num)
                    {
                        found = true;
                        break;
                    } 
                }
                // if a char is not number
                if (!found) return true;
            }
            return false;
        }
        
        public static bool isHigher(IGuild guild, IUser author, ulong target)
        {
            var authorHierarchy = guild.GetUserAsync(author.Id).Result.Hierarchy;
            var targetHierarchy = guild.GetUserAsync(target).Result.Hierarchy;

            return authorHierarchy > targetHierarchy;

        }

        public static string ToString<T>(IList<T> list)
        {
            string a = "";
            foreach(var item in list)
            {
                a += item;
                a += " ";
            }
            a.Remove(a.Length-1);
            return a;
        }
        public static List<string> Split (string str , char delim)
        {
            List<string> token = new();
            Int32 pos;
            while((pos = str.IndexOf(delim)) != -1)
            {
                token.Add(str.Substring(0,pos));
                str.Remove(0, pos + 1);
            }
            return token;
        }
        //return message if attribute is not found
        public static bool IsNull<T>(T a)
        {
            return a == null;
        }
    }
}
