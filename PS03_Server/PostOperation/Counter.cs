using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PS03.PostOperation
{
    public class PasswordCounter
    {
        private Mutex mtx = new Mutex();
        private List<string> pws = new List<string>(); 
        public void Add(string pw)
        {
            mtx.WaitOne();
            pws.Add(pw);
            Count(pws);
            mtx.ReleaseMutex();
        }
        public void Count(List<string> passwords)
        {
            var pwDict = new Dictionary<string, int>();

            foreach (var pw in passwords)
            {
                if (!pwDict.ContainsKey(pw))
                {
                    pwDict.Add(pw, 1);
                }
                else
                {
                    pwDict[pw]++;
                }
            }
            List<KeyValuePair<string, int>> pwList = pwDict.ToList();
            pwList.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            });
            pwList.Reverse();
            Save(pwList);

        }


        private void Save(List<KeyValuePair<string, int>> pwList)
        {
            mtx.WaitOne();
            string path = @"C:\Report.txt";
     
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (var pw in pwList)
                    {
                        double percent = (double) pw.Value/(double) pws.Count*100;
                        string sP = String.Format("{0:0.0}",percent);
                        sw.WriteLine("Password: " + pw.Key + " is used "+ pw.Value +" time(s) (" +sP + "%)" );
                    }
                }
            mtx.ReleaseMutex();
        }
    }

    }
