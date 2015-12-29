using System;
using System.Collections.Generic;
using System.IO;

namespace PS03.PostOp
{
    public class ReportGenerator
    {
        public List<KeyValuePair<string, KeyValuePair<string, string>>> allGathered = new List<KeyValuePair<string, KeyValuePair<string, string>>>();
        public List<KeyValuePair<string, int>> pwList = new List<KeyValuePair<string, int>>();
        public List<KeyValuePair<string, int>> usrList = new List<KeyValuePair<string, int>>();

        public void HTML()
        {
            var doc = File.ReadAllText("reportTemplate.html");
            var pwind = doc.IndexOf("[PW]");
            doc = doc.Remove(pwind, 4);
            doc = doc.Insert(pwind, pwList.Count.ToString());

            var usrind = doc.IndexOf("[USR]");
            doc = doc.Remove(usrind, 5);
            doc = doc.Insert(usrind, usrList.Count.ToString());
            var pwcnt = 0;
            foreach (var pw in pwList)
            {
                if (pwcnt == 10) break;
                if (pw.Key != "EMPTY")
                {
                    var tableend = doc.IndexOf("</tbody><!--PASSWORDS-->");
                    var table = @"<tr><td>" + pw.Key + @"</td><td>" + pw.Value/(double) pwList.Count*100 + @"</td></tr>";
                    doc = doc.Insert(tableend, (table));
                    pwcnt++;
                }
            }

            var usrcnt = 0;
            foreach (var usr in usrList)
            {
                if (usrcnt == 10) break;
                if (usr.Key != "EMPTY")
                {
                    var tableend = doc.IndexOf("</tbody><!--USERNAMES-->");
                    var table = @"<tr><td>" + usr.Key + @"</td><td>" + usr.Value/(double) usrList.Count*100 +
                                @"</td></tr>";
                    doc = doc.Insert(tableend, (table));
                    usrcnt++;
                }
            }

            foreach (var log in allGathered)
            {
                var tableend = doc.IndexOf("</tbody><!--ALL-->");
                var table = @"<tr><td>" + log.Value.Key + @"</td><td>" + log.Value.Value +
                          @"</td><td><a href=" + "\"" + log.Key + "\"" + @">"+log.Key+ "</a></td></tr>";
                doc = doc.Insert(tableend, (table));
                usrcnt++;
            }


            File.WriteAllText("Report_PS03"+".html", doc);
        }
    }
}