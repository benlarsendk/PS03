using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.PostOperations
{
    public class PasswordCounter
    {

        private List<Profile> plist = new List<Profile>();
        private static PasswordCounter _instance;


        public void AddProfiles(List<Profile> profiles)
        {
            foreach(var p in profiles)
            {
                plist.Add(p);
            }
        }

        public List<Profile> GetAll()
        {
            return plist;
        }

        public static PasswordCounter Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new PasswordCounter();
                }
                return _instance;
            }
        }
        public List<KeyValuePair<string, int>> CountPasswords()
        {
            var pwDict = new Dictionary<string, int>();

            foreach (var pw in plist)
            {
                if (!pwDict.ContainsKey(pw.Password))
                {
                    pwDict.Add(pw.Password, 1);
                }
                else
                {
                    pwDict[pw.Password]++;
                }
            }

            List<KeyValuePair<string, int>> pwList = pwDict.ToList();
            pwList.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            });
            pwList.Reverse();

            return pwList;
        }

        public List<KeyValuePair<string, int>> CountUsers()
        {
            var usrDict = new Dictionary<string, int>();
            List<KeyValuePair<string, int>> usrList = new List<KeyValuePair<string, int>>();


            foreach (var usr in plist)
            {
                if (!usrDict.ContainsKey(usr.Specification))
                {
                    usrDict.Add(usr.Specification, 1);
                }
                else
                {
                    usrDict[usr.Specification]++;
                }
            }
            List<KeyValuePair<string, int>> newusrList = usrDict.ToList();
            newusrList.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            });
            newusrList.Reverse();
            return newusrList;

        }

    }
}
