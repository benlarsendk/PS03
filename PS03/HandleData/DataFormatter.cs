using PS03.PostOperations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03
{
    public abstract class DataFormatter
    {

        public abstract string Format(List<Profile> Profiles);
    }

    public class ReportFormatter : DataFormatter
    {

        public override string Format(List<Profile> Profiles)
        {
            var counter = PasswordCounter.Instance;
            counter.AddProfiles(Profiles);

            List<KeyValuePair<string, int>> pwList = counter.CountPasswords();
            List<KeyValuePair<string, int>> usrList = counter.CountUsers();
            var allprofiles = counter.GetAll();

            var doc = File.ReadAllText("reportTemplate.html");
            var alls = doc.IndexOf("[ALL]");
            doc = doc.Remove(alls, 5);
            doc = doc.Insert(alls, allprofiles.Count.ToString());

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
                    var table = @"<tr><td>" + pw.Key + @"</td><td>" + String.Format("{0:P2}.", (double)pw.Value / (double)allprofiles.Count)  + @"</td></tr>";
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
                    var table = @"<tr><td>" + usr.Key + @"</td><td>" + String.Format("{0:P2}.", (double)usr.Value / (double)allprofiles.Count) +
                                @"</td></tr>";
                    doc = doc.Insert(tableend, (table));
                    usrcnt++;
                }
            }

            foreach (var log in Profiles)
            {
                var tableend = doc.IndexOf("</tbody><!--ALL-->");
                var table = @"<tr><td>" + log.Specification + @"</td><td>" + log.Password +
                          @"</td><td><a href=" + "\"" + log.Password + "\"" + @">" + log.Action + "</a></td></tr>";
                doc = doc.Insert(tableend, (table));
                usrcnt++;
            }
            return doc;
        }

    }

    public class ConsoleFormatter : DataFormatter
    {


        public override string Format(List<Profile> Profiles)
        {
            foreach(var p in Profiles)
            {
                PrintColor(p.Action, p.Specification, p.Password, p.AppName);
            }
            return "";
        }

        private void PrintColor(string action, string user, string pass,string app)
        {
  
            Console.WriteLine();
            printCenterWhite("# ---------------------- "+ app + " Profile ---------------------- #");


            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Action:\t\t");
            printRed(action);
            Console.Write("Specification:\t");
            printRed(user);
            Console.Write("Password:\t");
            printRed(pass);
            Console.WriteLine();

            Console.ResetColor();

        }

        private void printRed(string data)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;
        }

        private void printCenterWhite(string data)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition((Console.WindowWidth - data.Length) / 2, Console.CursorTop);
            Console.WriteLine(data);
            Console.ForegroundColor = ConsoleColor.Green;
        }
    }
}
