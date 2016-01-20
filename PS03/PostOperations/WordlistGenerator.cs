using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.PostOperations
{
    public class WordlistGenerator
    {
        public void Generate(List <Profile> profiles)
        {


            List<string> usernames = new List<string>();
            List<string> passwords = new List<string>();

            FilterProfiles(profiles, ref passwords, ref usernames);

            using (StreamWriter PwFile = new StreamWriter("Passwords.txt", true))
            using (StreamWriter UsrFile = new StreamWriter("Usernames.txt", true))
            {
                foreach(var p in passwords)
                {
                    PwFile.WriteLine(p);
                }
                foreach (var u in usernames)
                {
                    UsrFile.WriteLine(u);
                }
            }
        }

        private void FilterProfiles(List <Profile> profiles, ref List<string> pws, ref List<string> users)
        {
            foreach (var p in profiles)
            {
                if (p.AppName != "WiFi" && p.Specification != "EMPTY")
                    users.Add(p.Specification);
                if (p.Password != "EMPTY")
                    pws.Add(p.Password);
            }

            pws = pws.Distinct().ToList();
            users = users.Distinct().ToList();

        }
    }
}
