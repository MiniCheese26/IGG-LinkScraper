using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Timers;

namespace IGGGamesURLResolver
{
    class ExceptionHandles
    {
        public static bool WebExceptionHandle(string url)
        {
            try
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
                req.Method = "HEAD";
                HttpWebResponse response = req.GetResponse() as HttpWebResponse;
                response.Close();
                return true;
            }
            catch
            {
                Console.Clear();
                Console.WriteLine("URL is not valid");
                Console.ReadKey();
                Console.Clear();
                return false;
            }
        }

        public static string FixDifferentURLs(string host)
        {
            if (host == "Link Uploaded:")
            {
                host = "ul.to";
                return host;
            }
            else if (host == "Link Rapidgator:")
            {
                host = "rapidgator.net";
                return host;
            }
            else if (host == "Link Uptobox:")
            {
                host = "uptobox.com";
                return host;
            }
            else if (host == "Link Upera:")
            {
                host = "public.upera.co";
                return host;
            }
            else
            {
                return host;
            }
        }

        public static int CountHosts(string[] hosts)
        {
            int j = 0;

            foreach (string i in hosts)
            {
                if (Start.data.Contains(i))
                {
                    j++;
                }
            }

            return j;
        }

        public static string[] DetectHosts(string[] hosters, int arrayLength)
        {
            int j = 0;
            string[] k = new string[arrayLength];

            foreach (string i in hosters)
            {
                if (Start.data.Contains(i))
                {
                    k[j] = i;
                    j++;
                }
            }

            return k;
        }
    }
}
