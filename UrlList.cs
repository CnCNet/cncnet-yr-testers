using System;
using System.Collections.Generic;


namespace CnCNetTesters
{
    public partial class UrlList
    {
        // Structure for urls
        public class UrlUpdateList
        {
            public string uri { get; set; }
            public string fileName { get; set; }
            public string newName { get; set; }
            public string sha1 { get; set; }
        }

        // List of URLS to update
        public static List<UrlUpdateList> urls = new List<UrlUpdateList>
        {
            new UrlUpdateList() {
                uri = "http://rampastring.cncnet.org/yr/testupdates/ClientCore.new",
                fileName = "ClientCore.new",
                newName = "ClientCore.dll",
                sha1 = "6092c3054dc2f069e3a8918a0390f2efd9668f20"
            },
            new UrlUpdateList() {
                uri = "http://rampastring.cncnet.org/yr/testupdates/CnCNetClientYR.new",
                fileName = "CnCNetClientYR.new",
                newName = "CnCNetClientYR.exe",
                sha1 = "80ec053e94886f804871070e8c1cea58d983083f"
            },
            new UrlUpdateList() {
                uri = "http://rampastring.cncnet.org/yr/testupdates/ClientGUI.new",
                fileName = "ClientGUI.new",
                newName = "ClientGUI.dll",
                sha1 = "80ac6867f3f7c2bcb25b97b52bdeb78920392823"
            }
        };
    }
}
