using Discord;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
                foreach (char num in Nums)
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

        public static bool Isnan(ReadOnlySpan<char> str)
        {
            if (str == null || str.Length <= 0)
            {
                return true;
            }

            foreach(char c in str)
            {
                bool found = false;
                foreach(char num in Nums)
                {
                    if (c == num)
                    {
                        found = true;
                        continue;
                    }
                }
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
            foreach (var item in list)
            {
                a += item;
                a += " ";
            }
            a.Remove(a.Length - 1);
            return a;
        }

        //return message if attribute is not found
        // ??? what does the comment above me means?
        public static bool IsNull<T>(T a)
        {
            return a == null;
        }

        public static (int, char) ParseDuration(ReadOnlySpan<char> duration)
        {

            char end = duration[duration.Length - 1];
            
            if (end == 's' || end == 'S' || end == 'h' || end == 'H' || end == 'd' || end == 'D')
            {
                if(Isnan(duration.Slice(0, duration.Length - 1)))
                {
                    return (-1, 'e');
                }
                // now that we have sanitized everything, it's safe to assume that it's going to be a number
                else
                {
                    if (end == 's' || end == 'S')
                    {
                        return (Convert.ToInt32(duration.Slice(0, duration.Length - 1).ToString()), 's');
                    }
                    else if (end == 'h' || end == 'H')
                    {
                        return (Convert.ToInt32(duration.Slice(0, duration.Length - 1).ToString()), 'h');
                    }
                    else if (end == 'd' || end == 'D')
                    {
                        return (Convert.ToInt32(duration.Slice(0, duration.Length - 1).ToString()), 'd');
                    }
                }

            }
            else
            {
                return (-1, 'e');     
            }
            // there is no way this code is reached
            return (-1, 'e');
        }


    }
}