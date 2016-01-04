using PS03.PostOperations;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Reflection;
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

            string doc;
            var allprofilesWithDubs = counter.GetAll();
            var allprofiles = allprofilesWithDubs.Distinct().ToList();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PS03.ReportTemplate.html";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                doc = reader.ReadToEnd();
            }

            var alls = doc.IndexOf("[ALL]");

            var pwind = doc.IndexOf("[PW]");
            doc = doc.Remove(pwind, 4);
            doc = doc.Insert(pwind, pwList.Count.ToString());

            var usrind = doc.IndexOf("[USR]");
            doc = doc.Remove(usrind, 5);
            doc = doc.Insert(usrind, usrList.Count.ToString());

            doc = SetStatistics(allprofiles, doc);
            var pwcnt = 0;
            foreach (var pw in pwList)
            {
                if (pwcnt == 10) break;
                if (pw.Key != "EMPTY")
                {
                    var tableend = doc.IndexOf("<!--PASSWORDS-->");
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
                    var tableend = doc.IndexOf("<!--USERNAMES-->");
                    var table = @"<tr><td>" + usr.Key + @"</td><td>" + String.Format("{0:P2}.", (double)usr.Value / (double)allprofiles.Count) +
                                @"</td></tr>";
                    doc = doc.Insert(tableend, (table));
                    usrcnt++;
                }
            }

            foreach (var log in Profiles)
            {
                var tableend = doc.IndexOf("<!--" + log.AppName + "-->");
                var table = @"<tr><td>" + log.Specification + @"</td><td>" + log.Password +
                          @"</td><td><a href=" + "\"" + log.Action + "\"" + @">" + GetHost(log) + "</a></td></tr>";
                doc = doc.Insert(tableend, (table));
                usrcnt++;
            }
            return doc;
        }

        private string GetHost(Profile link)
        {
            if (link.AppName != "WiFi")
            {
                if (string.IsNullOrWhiteSpace(link.Action)) return link.Action;
                Uri myUri = new Uri(link.Action);
                return myUri.Host;
            }
            return link.Action;

        }

        private Dictionary<string,Statistic> GetNumbers(List<Profile> profiles)
        {
            Statistic Chrome = new Statistic() { AppName = "Chrome", pw = 0, usr = 0 };
            Statistic Firefox = new Statistic() { AppName = "Firefox", pw = 0, usr = 0 };
            Statistic WiFi = new Statistic() { AppName = "WiFi", pw = 0, usr = 0 };

            foreach (var p in profiles)
            {
                if (p.AppName == "Google Chrome")
                {
                    if (p.Password != "EMPTY" || string.IsNullOrEmpty(p.Password))
                        Chrome.pw++;
                    if (p.Specification != "EMPTY" || string.IsNullOrEmpty(p.Specification))
                        Chrome.usr++;
                }
                else if (p.AppName == "Firefox")
                {
                    if (p.Password != "EMPTY" || string.IsNullOrEmpty(p.Password))
                        Firefox.pw++;
                    if (p.Specification != "EMPTY" || string.IsNullOrEmpty(p.Specification))
                        Firefox.usr++;
                }
                else if (p.AppName == "WiFi")
                {
                    if (p.Password != "EMPTY" || string.IsNullOrEmpty(p.Password))
                        Firefox.pw++;
                }


                }

            var ret =  new Dictionary<string, Statistic>();
            ret.Add("Firefox", Firefox);
            ret.Add("Chrome", Chrome);
            ret.Add("WiFi", WiFi);
            return ret;
        }

        private string SetStatistics(List<Profile> profiles, string doc)
        {
            var stats = GetNumbers(profiles);

            var fpw = doc.IndexOf("[FPW]");
            doc = doc.Remove(fpw, 5);
            doc = doc.Insert(fpw, stats["Firefox"].usr.ToString());

            var fpa = doc.IndexOf("[FPA]");
            doc = doc.Remove(fpa, 5);
            doc = doc.Insert(fpa, stats["Firefox"].pw.ToString());

            var cpw = doc.IndexOf("[CPW]");
            doc = doc.Remove(cpw, 5);
            doc = doc.Insert(cpw, stats["Chrome"].usr.ToString());

            var cpa = doc.IndexOf("[CPA]");
            doc = doc.Remove(cpa, 5);
            doc = doc.Insert(cpa, stats["Chrome"].pw.ToString());

            var wpa = doc.IndexOf("[WPA]");
            doc = doc.Remove(wpa, 5);
            doc = doc.Insert(wpa, stats["WiFi"].pw.ToString());

            return doc;
        }


    }

    internal class Statistic
    {
        public string AppName;
        public int pw = 0;
        public int usr = 0;
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
