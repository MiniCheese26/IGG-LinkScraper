using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace iggGamesLinksScraper
{
    internal class HostsData
    {
        public string Host;
        public IEnumerable<string> Url;
    }

    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "--help")
            {
                DisplayHelp();
                Environment.Exit(1);
            }
            
            WebClient webClient = new WebClient();

            Console.WriteLine("\nIGG Games LinkScraper Version: 2.0.1");

            foreach (var arg in args)
            {
                var content = webClient.DownloadString(new Uri(arg));

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
                
                while (!int.TryParse(hosterResponseString, out int n) || !Enumerable.Range(1, hostersList.Count).Contains(n))
                {
                    Console.WriteLine("\nInvalid input");
                    Console.Write("\nWhich hoster would you like to use: ");
                    hosterResponseString = Console.ReadLine();
                }

                int hosterResponse = int.Parse(hosterResponseString) - 1;

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
