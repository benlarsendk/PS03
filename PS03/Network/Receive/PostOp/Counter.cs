using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PS03.Network.Receive.PostOp
{
    public class PasswordCounter
    {
        private Mutex mtx = new Mutex();
        private List<string> pws = new List<string>();
        private static PasswordCounter _pwc = null;

        public static PasswordCounter Instance
        {
            get
            {
                if (_pwc == null)
                {
                    _pwc = new PasswordCounter();
                }
                return _pwc;
            }
            
        }
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
            string path = @"C:\Users\benla\Desktop\git\Report.txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (var pw in pwList)
                    {
                        double percent = (double)pw.Value / (double)pws.Count * 100;
                        string sP = String.Format("{0:0.0}", percent);
                        sw.WriteLine("Password: " + pw.Key + " is used " + pw.Value + " time(s) (" + sP + "%)");
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Error: Couldn't access report file (Are you running server and client on same machine with the verbose option?)");
                mtx.ReleaseMutex();
                throw;
            }
                
            mtx.ReleaseMutex();
        }
    }

    }
