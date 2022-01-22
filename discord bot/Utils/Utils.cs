using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot
{
    public class Utils
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
    }
}
