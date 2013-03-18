using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace ChallengeParty
{
    public class RequestParsing
    {
        public static Tree GetTreeFromUrl(string url)
        {
            WebClient wc = new WebClient();

            string strJson = wc.DownloadString(url);

            return JSonParsingExtension.FromJson(strJson);
        }
    }
}
