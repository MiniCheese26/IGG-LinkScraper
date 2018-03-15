using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace IGGGamesURLResolver
{
    class Start
    {
        static public WebClient webC = new WebClient();
        static public string host;
        static public string data;
        static public string url;
        static public string[] hosters = new string[17] { "MegaUp.net",
                                                "Mega.co.nz",
                                                "TusFiles",
                                                "Rapidgator",
                                                "Uptobox",
                                                "Uploaded",
                                                "Google Drive",
                                                "Openload.co",
                                                "ClicknUpload",
                                                "Go4Up (Multi Links)",
                                                "MultiUp (Multi Links)",
                                                "OwnDrives",
                                                "Upera",
                                                "KumpulBagi",
                                                "UpFile",
                                                "FilesCDN",
                                                "DownACE"};

        public static void Main()
        {
            bool urlCheck = false;

            do
            {
                Console.WriteLine("Enter URL");
                url = Console.ReadLine();

                urlCheck = ExceptionHandles.WebExceptionHandle(url);

                if (urlCheck != false)
                {
                    if (!url.Contains("http://igg-games.com/")) //Change
                    {
                        Console.WriteLine("");
                        Console.WriteLine("!!!Enter an igg-games link");
                        Console.ReadKey();
                        Console.Clear();
                        urlCheck = false;
                    }
                    else
                    {
                        urlCheck = true;
                    }
                }
            } while (urlCheck == false);

            data = webC.DownloadString(url);

            int detectedCount = ExceptionHandles.CountHosts(hosters);
            string[] detectedHosts = new string[detectedCount];

            detectedHosts = ExceptionHandles.DetectHosts(hosters, detectedCount);

            detectedHosts = CreateDisplayStrings(detectedCount, detectedHosts);

            bool detectedHostsLoop = false;

                Console.WriteLine(" ");
                Console.WriteLine("Detected hosts");
                for (int i = 0; i < detectedCount; i++)
                {
                    Console.WriteLine(detectedHosts[i]);
                }
                string hostNumberStr = Console.ReadLine();

            do
            {
                while (!Int32.TryParse(hostNumberStr, out int n))
                {
                    Console.WriteLine("");
                    Console.WriteLine("Try again");

                    hostNumberStr = Console.ReadLine();
                }

                int hostCheck = Convert.ToInt32(hostNumberStr);

                if (hostCheck - 1 > detectedCount)
                {
                    Console.WriteLine("");
                    Console.WriteLine("Try again");

                    hostNumberStr = Console.ReadLine();
                }

                host = detectedHosts[Convert.ToInt32(hostNumberStr) - 1];

                host = CreateSearchStrings(host);
            } while (detectedHostsLoop == true);

            GetURLs.FindURLs();
        }

        public static string[] CreateDisplayStrings(int detectedCount, string[] detectedHosts)
        {
            int listNumber = 1;

            for (int i = 0; i < detectedCount; i++)
            {
                detectedHosts[i] = listNumber + ". " + detectedHosts[i];
                listNumber++;
            }

            return detectedHosts;
        }

        public static string CreateSearchStrings(string host)
        {
            host = host.Substring(3);
            host = "Link " + host + ":";
            return host;
        }
    }
}
