using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace iggGamesLinksScraper
{
    internal static class Parsing
    {
        private static readonly HtmlDocument HtmlDocument = new HtmlDocument();

        /// <summary>
        /// Parses page source to find valid links and hosters
        /// </summary>
        /// <param name="content">Page source</param>
        /// <returns>List as type HostsData</returns>
        public static List<HostsData> GetHosts(string content)
        {
            HtmlDocument.LoadHtml(content);

            List<HostsData> hostsDataList = new List<HostsData>();

            int childNodeCount = HtmlDocument.DocumentNode
                .SelectNodes("/html/body/div/div/div[4]/div/div/div/article/div/*").Count(x => x.Name == "p");

            for (int i = 0; i < childNodeCount; i++)
            {
                HostsData hostsData = new HostsData();
                var hostString = string.Empty;
                HtmlNode hostNode = HtmlDocument.DocumentNode.SelectSingleNode($"/html/body/div/div/div[4]/div/div/div/article/div/p[{i + 1}]/b");
                hostsData.Url =
                    HtmlDocument.DocumentNode.SelectNodes(
                        $"/html/body/div/div/div[4]/div/div/div/article/div/p[{i + 1}]").Descendants("a")
                        .Select(x => x.Attributes["href"].Value).Where(x => x.Contains("http://bluemediafiles"))
                        .Select(x => HttpUtility.UrlDecode(
                            Regex.Match(x, @"xurl=s?:\/\/([\w\W]+)").Groups[1].Captures[0].Value));

                if (hostNode != null)
                {
                    hostString = hostNode.InnerText.Trim();
                }

                if (hostString.Length == 0 || !hostString.Contains("Link ") || hostString.Contains("TORRENT")) continue;

                hostString = hostString
                    .Split(new[] { "Link " }, StringSplitOptions.RemoveEmptyEntries).Last()
                    .Replace(":", "");

                hostsData.Host = hostString;

                hostsDataList.Add(hostsData);
            }

            return hostsDataList;
        }


        /// <summary>
        /// Grabs title from page source
        /// </summary>
        /// <param name="content">Page source</param>
        /// <returns>Game title as string</returns>
        public static string GetTitle(string content)
        {
            HtmlDocument.LoadHtml(content);

            return HtmlDocument.DocumentNode.SelectSingleNode("/html/body/div/div/div[4]/div/div/div/article/h1")
                .InnerText
                .Split(new[] { " Free Download" }, StringSplitOptions.None).First();
        }
    }
}
