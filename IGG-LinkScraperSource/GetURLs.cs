using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using ClipboardForPaste = System.Windows.Forms;

namespace IGGGamesURLResolver
{
    class GetURLs
    {
        public static List<string> childNodeText = new List<string>();

        public static void FindURLs()
        {
            childNodeText.Clear();

            HtmlDocument document = new HtmlDocument();

            document.LoadHtml(Start.data);

            var div = document.DocumentNode.SelectSingleNode("//div[@class=\"post-content clear-block\"]");

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

                string[] splitB = Regex.Split(split[1], @"""");
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
                bool httpsCheck = Extras.HttpsList(linksFinal);

                if (httpsCheck == true)
                {
                    linksFinal[i] = "https://" + linksFinal[i];
                }
                else
                {
                    linksFinal[i] = "http://" + linksFinal[i];
                }
            }

            LinksGrabbed(linksFinal);
        }

        static void LinksGrabbed(string[] finalLinks)
        {
            Console.WriteLine("");
            bool wLoop = false;

            do
            {
                Console.WriteLine("Would you like to A. display the links, B. copy them to your clipboard or C. export them to a text file?");
                string userChoice = Console.ReadLine().ToUpper();

                Console.WriteLine("");

                if (userChoice == "A")
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
                if (userChoice == "C")
                {
                    string path = (Directory.GetCurrentDirectory());
                    string dir = path + @"\Links.txt";

                    int i = 0;

                    foreach (string j in finalLinks)
                    {
                        string title = GetTitle();

                        if (i == 0)
                        {
                            File.AppendAllText(dir, Environment.NewLine +
                                                    "---------------------------" +
                                                    Environment.NewLine +
                                                    title +
                                                    Environment.NewLine +
                                                    "---------------------------" +
                                                    Environment.NewLine +
                                                    j +
                                                    Environment.NewLine);
                            i++;
                        }

                        File.AppendAllText(dir, j + Environment.NewLine);
                    }

                    Console.WriteLine("Links exported to {0}", path + @"\Links.txt");
                    Console.WriteLine("Press enter to paste a new link");
                    Console.ReadKey();
                    Console.Clear();
                    Start.Main();
                }
                if (userChoice == "B")
                {
                    string[] eachString = new string[finalLinks.Length];

                    for (int i = 0; i < finalLinks.Length; i++)
                    {
                        eachString[i] = finalLinks[i];
                    }

                    string s = String.Join(Environment.NewLine, eachString);

                    ClipboardForPaste.Clipboard.SetText(s);

                    Console.WriteLine("Clipboard set");
                    Console.WriteLine("Press enter to paste a new link");
                    Console.ReadKey();
                    Console.Clear();
                    Start.Main();
                }
                else
                {
                    Console.WriteLine("Enter A or B Please");
                    Console.ReadKey();
                    wLoop = true;
                }
            } while (wLoop == true);
        }

        public static string GetTitle()
        {
            string stringForTitle = Start.url.Substring(21);

            stringForTitle = stringForTitle.Replace("-", " ");

            string[] split = Regex.Split(stringForTitle, " free download");

            stringForTitle = split[0];
            stringForTitle.Trim();

            stringForTitle = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringForTitle);

            return stringForTitle;
        }
    }
}
