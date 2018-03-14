using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace IGGGamesURLResolver
{
    class Start
    {
        static public WebClient webC = new WebClient();
        static public string host;
        static public string data;

        public static void Main()
        {
            bool urlCheck = false;
            string url;

            do
            {
                Console.WriteLine("Enter URL");
                url = Console.ReadLine();

                urlCheck = ExceptionHandles.WebExceptionHandle(url);

                if (!url.Contains("http://igg-games.com/")) //Change
                {
                    Console.WriteLine("Enter an igg-games link");
                    Console.ReadKey();
                    Console.Clear();
                    urlCheck = false;
                }
                else
                {
                    urlCheck = true;
                }
            } while (urlCheck == false);

            data = webC.DownloadString(url);

            Console.WriteLine(" ");
            Console.WriteLine("Please enter the character below for the hoster");

            string[] hosters = new string[7] { "1. MegaUp.net", "2. Mega.co.nz", "3. TusFiles", "4. Rapidgator", "5. Uptobox", "6. Uploaded", "7. Google Drive" };

            foreach (string i in hosters)
            {
                Console.WriteLine(i);
            }

            int hosterChoice = Convert.ToInt32(Console.ReadLine());

            HostSelection(hosterChoice, hosters);

            Console.ReadKey();
        }

        static void HostSelection(int hosterChoice, string[] hosts)
        {
            host = hosts[hosterChoice - 1];
            host = host.Substring(3);
            host = "Link " + host + ":";
            GetURLs.FindURLs();
        }
    }
}
