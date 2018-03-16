using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace IGGGamesURLResolver
{
    class Extras : Start
    {
        public static string dir = Directory.GetCurrentDirectory() + @"\BatchLinks.txt";
        public static string[] linkSplit;
        public static string title;

        public static void CreateBatchFile()
        {
            File.Create(dir).Close();
            return;
        }

        public static void ReadBatchFile()
        {
            if (!File.Exists(dir))
            {
                CreateBatchFile();
                Console.WriteLine("File Created. Paste your links into the file now. This is a first time only thing");
                Console.ReadKey();

                string i = File.ReadAllText(dir);
                while (String.IsNullOrEmpty(i) == true)
                {
                    Console.WriteLine("No links detected, add them now. Enter A if you want to exit batch selection");
                    string response = Console.ReadLine().ToUpper();
                    if (response == "A")
                    {
                        Console.Clear();
                        Start.Main();
                    }
                }
            }

            string links = File.ReadAllText(dir);
            linkSplit = Regex.Split(links, "html");

            for (int i = 0; i < linkSplit.Length; i++)
            {
                linkSplit[i] = Regex.Replace(linkSplit[i], @"\s+", "");
                linkSplit[i] = linkSplit[i] + "html";
            }

            return;
        }

        public static bool HttpsList(string[] inputHost)
        {
            string[] usesHttps = new string[11] {"megaup.net",
                                                 "mega.nz",
                                                 "rapidgator.net",
                                                 "uptobox.com",
                                                 "drive.google.com",
                                                 "openload.co",
                                                 "ClicknUpload",
                                                 "go4up.com",
                                                 "multiup.org",
                                                 "filescdn.com",
                                                 "downace.com"};

            foreach (string i in usesHttps)
            {
                foreach (string j in inputHost)
                {
                    if (j.Contains(i))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
