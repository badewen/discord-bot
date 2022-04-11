using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord;
using CliWrap;

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
        
        //return message if attribute is not found
        // ??? what does the comment above me means? 
        public static bool IsNull<T>(T a)
        {
            return a == null;
        }

        public static ProcessStartInfo PrepareFFmpeg(string url = "", UInt64 from = 0, UInt64 to = 15, bool stdin = true)
        {
            if (!stdin)
            {
                var ffmpeg = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments =
                        $"-hide_banner -loglevel panic -ss {from} -t {to} -i \"{url}\" -ac 2 -f s16le -ar 48000 pipe:1",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
                return ffmpeg;
            }
            else
            {
                var ffmpeg = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments =
                        $"-hide_banner -loglevel panic -i pipe:  -ac 2 -f s16le -ar 48000 pipe:1",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false
                };
                return ffmpeg;
            }
        }
        
        //youtube link
        public static bool IsLivestreamLink(string link)
        {
            if (link.EndsWith("index.m3u8")) return true;
            else return false;
        }
        
        public static ProcessStartInfo PrepareYoutubedl(string arg)
        {
            return new ProcessStartInfo
            {
                RedirectStandardOutput = true,
                FileName = "youtube-dl",
                Arguments = $"{arg} -q -o -",
                UseShellExecute = false,
            };
        }
    }
}
