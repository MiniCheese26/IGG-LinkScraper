using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;

namespace IGGGamesURLResolver
{
    class GetURLs
    {
        public static List<string> childNodeText = new List<string>();

        public static void FindURLs()
        {
            childNodeText.Clear();

            if (CheckIfHosterExists() != true)
            {
                Console.WriteLine("Sorry, the hoster chosen doesn't exist on this page");
                Console.ReadKey();
                Console.Clear();
                Start.Main();
            }

            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(Start.data);

            var div = document.DocumentNode.SelectSingleNode("//div[@class=\"post-content clear-block\"]");

            HtmlNodeCollection childDivNodes = div.ChildNodes;

            foreach (HtmlNode node in div.Descendants())
            {
                string t = node.InnerHtml;

                if (t.Contains(Start.host) == true)
                {
                    foreach (HtmlNode secondChild in node.Descendants())
                    {
                        var y = secondChild.OuterHtml;

                        if (y.Contains("url=s://") == true)
                        {
                            childNodeText.Add(y);
                        }
                        else if (y.Contains("xurl=://") == true)
                        {
                            childNodeText.Add(y);
                        }
                    }
                }
            }

            string[] childNodeTextArray = childNodeText.ToArray();

            FoundNode(childNodeTextArray);
        }

        static bool CheckIfHosterExists()
        {
            bool checkIfExists = Start.data.Contains(Start.host);
            Start.host = ExceptionHandles.FixDifferentURLs(Start.host);
            return checkIfExists;
        }

        static void FoundNode(string[] childNodeArray)
        {
            string[] split = new string[2];
            string[] linksFinal = new string[childNodeArray.Length];

            for (int i = 0; i < childNodeArray.Length; i++)
            {
                if (childNodeArray[i].Contains("xurl=://") == true)
                {
                    split = Regex.Split(childNodeArray[i], "xurl=://");
                }
                else if (childNodeArray[i].Contains("s://") == true)
                {
                    split = Regex.Split(childNodeArray[i], "s://");
                }

                string[] splitB = Regex.Split(split[1], " ");
                linksFinal[i] = splitB[0].ToString();
                linksFinal[i].Trim();
            }

            if (Start.host == "Link Mega.co.nz:")
            {
                for (int i = 0; i < childNodeArray.Length; i++)
                {
                    string t = linksFinal[i].Replace("%23", "#"); //Weird i have to do this
                    linksFinal[i] = t;
                }
            }

            for (int i = 0; i < childNodeArray.Length; i++)
            {
                linksFinal[i] = "http://" + linksFinal[i];
            }

            LinksGrabbed(linksFinal);
        }

        static void LinksGrabbed(string[] finalLinks)
        {
            Console.WriteLine("");
            Console.WriteLine("Done");
            Console.WriteLine("Would you like to 1. display the links or 2. export them to a text file?");
            int userChoice = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("");

            if (userChoice == 1)
            {
                foreach (string i in finalLinks)
                {
                    Console.WriteLine(i);
                }

                Console.WriteLine("");
                Console.WriteLine("Press enter to paste a new link");
                Console.ReadKey();
                Console.Clear();
                Start.Main();
            }
            if (userChoice == 2)
            {
                string root = Path.GetPathRoot(Directory.GetCurrentDirectory());
                string dir = root + @"IGG Links\" + "Links.txt";

                if (!Directory.Exists(root + "IGG Links"))
                {
                    Console.WriteLine("A");
                    Directory.CreateDirectory(root + "IGG Links");
                    File.Create(dir).Close();
                }

                foreach (string j in finalLinks)
                {
                    File.AppendAllText(dir, j + Environment.NewLine + Environment.NewLine);
                }

                Console.WriteLine("Links exported to {0}", root + @"IGG Links\" + "Links.txt");
                Console.WriteLine("Press enter to paste a new link");
                Console.ReadKey();
                Console.Clear();
                Start.Main();
            }
            else
            {
                Console.WriteLine("Enter A or B Please");
                Console.ReadKey();
                Console.Clear();
                LinksGrabbed(finalLinks);
            }
        }
    }
}
