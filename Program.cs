﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace IGGLinksScraper
{
    public class HostsData
    {
        public string Host;
        public List<string> Url;
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

            Console.WriteLine("IGG Games LinkScraper Version: 2.0.0");

            Uri url = new Uri(args[0]);

            WebClient webClient = new WebClient();

            var content = webClient.DownloadString(url);

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

            Console.ReadKey();
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("\nUsage : dotnet iggLinkScraper.dll <url-here>");
        }
    }
}