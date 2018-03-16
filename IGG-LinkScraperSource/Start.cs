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
        public static bool batchLinksEnabled;
        public static int linkSplitIndex = 0;

        public static void Main()
        {
            string startChoice;
            startChoice = "";

            if (batchLinksEnabled == true)
            {
                bool batchCheck = false;
                int linkSplitLength = Extras.linkSplit.Length - 1;

                if (linkSplitLength == linkSplitIndex)
                {
                    Console.WriteLine("Batch links done");
                    Console.ReadKey();
                    Console.Clear();
                    batchLinksEnabled = false;
                    Main();
                }

                url = Extras.linkSplit[linkSplitIndex];

                batchCheck = LinkCheck();

                if (batchCheck != true)
                {
                    Console.WriteLine("Your link on line {0} is invalid", linkSplitIndex + 1);
                    Console.ReadKey();
                    Console.Clear();
                    batchLinksEnabled = false;
                    Main();
                }

                linkSplitIndex++;
                Extras.title = GetURLs.GetTitle();
                AfterStart();
            }

            Console.WriteLine("IGG Games LinkScraper Version: 1.4");
            Console.WriteLine("Enter A. to enter a URL or enter B. to load links from batchfile");
            startChoice = Console.ReadLine().ToUpper();

            if (startChoice == "B")
            {
                bool batchCheck = false;
                batchLinksEnabled = true;
                Console.Clear();
                Extras.ReadBatchFile();
                url = Extras.linkSplit[linkSplitIndex];

                batchCheck = LinkCheck();

                if (batchCheck != true)
                {
                    Console.WriteLine("Your link on line {0} is invalid", linkSplitIndex + 1);
                    Console.ReadKey();
                    Console.Clear();
                    batchLinksEnabled = false;
                    Main();
                }


                Extras.title = GetURLs.GetTitle();
                linkSplitIndex++;
                AfterStart();
            }

            bool urlValid = false;

            while (urlValid == false)
            {
                Console.WriteLine("Enter URL");
                url = Console.ReadLine();

                urlValid = LinkCheck();

                if (urlValid == false)
                {
                    Console.WriteLine("Try again");
                    url = Console.ReadLine();
                }
            }

            AfterStart();
        }

        static bool LinkCheck()
        {
            bool urlCheck;

            urlCheck = ExceptionHandles.WebExceptionHandle(url);

            if (urlCheck == false)
            {

                return false;
            }

            if (!url.Contains("http://igg-games.com/"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        static void AfterStart()
        {
            data = webC.DownloadString(url);

            int detectedCount = ExceptionHandles.CountHosts(hosters);
            string[] detectedHosts = new string[detectedCount];

            detectedHosts = ExceptionHandles.DetectHosts(hosters, detectedCount);

            detectedHosts = CreateDisplayStrings(detectedCount, detectedHosts);

            Console.WriteLine(" ");
            if (batchLinksEnabled == true)
            {
                Console.WriteLine(Extras.title);
                Console.WriteLine("-----------------");
            }
            Console.WriteLine("Detected hosts");
            for (int i = 0; i < detectedCount; i++)
            {
                Console.WriteLine(detectedHosts[i]);
            }
            string hostNumberStr = Console.ReadLine();

            while (!Int32.TryParse(hostNumberStr, out int n))
            {
                Console.WriteLine("");
                Console.WriteLine("Try again");

                hostNumberStr = Console.ReadLine();
            }

            int hostCheck = Convert.ToInt32(hostNumberStr);

            while (hostCheck > detectedCount)
            {
                Console.WriteLine("");
                Console.WriteLine("Try again");

                hostNumberStr = Console.ReadLine();
                hostCheck = Convert.ToInt32(hostNumberStr);
            }

            host = detectedHosts[Convert.ToInt32(hostNumberStr) - 1];

            host = CreateSearchStrings(host);

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
