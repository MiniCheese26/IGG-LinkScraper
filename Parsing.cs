using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace IGGLinksScraper
{
    internal static class Parsing
    {
        public static List<HostsData> GetHosts(string content)
        {
            HtmlDocument htmlDocument = new HtmlDocument();

            htmlDocument.LoadHtml(content);

            List<HostsData> hostsDataList = new List<HostsData>();

            int childNodeCount = htmlDocument.DocumentNode
                .SelectNodes("/html/body/div/div/div[4]/div/div/div/article/div/*").Count(x => x.Name == "p");

            for (int i = 0; i < childNodeCount; i++)
            {
                HostsData hostsData = new HostsData();

                var hostString = string.Empty;
                HtmlNode hostNode = htmlDocument.DocumentNode.SelectSingleNode($"/html/body/div/div/div[4]/div/div/div/article/div/p[{i + 1}]/b");
                hostsData.Url =
                    htmlDocument.DocumentNode.SelectNodes(
                        $"/html/body/div/div/div[4]/div/div/div/article/div/p[{i + 1}]").Descendants("a")
                        .Select(x => x.Attributes["href"].Value).Where(x => x.Contains("http://bluemediafiles"))
                        .Select(x => HttpUtility.UrlDecode(
                            Regex.Match(x, @"xurl=s?:\/\/([\w\W]+)").Groups[1].Captures[0].Value)).ToList();

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
    }
}
