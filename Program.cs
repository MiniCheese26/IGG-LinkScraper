using System;
using System.Collections.Generic;
using System.Net;

namespace iggGamesLinksScraper
{
    public class HostsData
    {
        public string Host;
        public IEnumerable<string> Url;
    }

    internal static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayHelp();
                Environment.Exit(1);
            }

            if (args[0] == "--help")
            {
                DisplayHelp();
                Environment.Exit(1);
            }

            Console.WriteLine("\nIGG Games LinkScraper Version: 2.0.1");

            foreach (var arg in args)
            {
                Uri url = new Uri(arg);

                WebClient webClient = new WebClient();

                var content = webClient.DownloadString(url);

                Console.WriteLine(Environment.NewLine + Parsing.GetTitle(content));

                List<HostsData> hostersList = Parsing.GetHosts(content);

                Console.WriteLine("\nHosts Available\n");

                for (var index = 0; index < hostersList.Count; index++)
                {
                    var host = hostersList[index];

                    Console.WriteLine($"[{index + 1}] {host.Host}");
                }

                Console.Write("\nWhich hoster would you like to use: ");
                string hosterResponseString = Console.ReadLine();

                while (!int.TryParse(hosterResponseString, out int n))
                {
                    Console.WriteLine("\nPlease enter a number");
                    Console.Write("\nWhich hoster would you like to use: ");
                    hosterResponseString = Console.ReadLine();
                }

                while (Convert.ToInt32(hosterResponseString) - 1 > hostersList.Count - 1 || Convert.ToInt32(hosterResponseString) - 1 < 0)
                {
                    Console.WriteLine("\nPlease enter a number within the correct range");
                    Console.Write("\nWhich hoster would you like to use: ");
                    hosterResponseString = Console.ReadLine();
                }

                int hosterResponse = Convert.ToInt32(hosterResponseString) - 1;

                Console.Write("\n");

                foreach (var hostUrl in hostersList[hosterResponse].Url)
                {
                    Console.WriteLine(hostUrl);
                }
            }

            Console.WriteLine("\nDone");
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("\nUsage : dotnet iggLinkScraper.dll <url1> <ur2> <url3>...");
        }
    }
}
