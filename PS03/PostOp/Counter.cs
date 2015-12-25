using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace PS03.PostOp
{
    public class DataCounter
    {
        private Mutex mtx = new Mutex();
        private Mutex usrMutex = new Mutex();
        private Mutex loginMtx = new Mutex();


        private List<string> pws = new List<string>();
        private List<string> users = new List<string>();
        public ReportGenerator ReportGenerator = new ReportGenerator();

        private static DataCounter _pwc = null;

        public static DataCounter Instance
        {
            get
            {
                if (_pwc == null)
                {
                    _pwc = new DataCounter();
                }
                return _pwc;
            }
            
        }
        public void AddPassword(string pw)
        {
            mtx.WaitOne();
            pws.Add(pw);
            Count(pws);
            mtx.ReleaseMutex();
        }

        public void AddLogin(KeyValuePair<string, string> login)
        {
            loginMtx.WaitOne();
            ReportGenerator.allGathered.Add(login);
            loginMtx.ReleaseMutex();
        }

        public void AddUsername(string user)
        {
            usrMutex.WaitOne();
            users.Add(user);
            CountUsers(users);
            usrMutex.ReleaseMutex();
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
            ReportGenerator.pwList = pwList;

        }

        public void CountUsers(List<string> usersList)
        {
            var usrDict = new Dictionary<string, int>();

            foreach (var usr in usersList)
            {
                if (!usrDict.ContainsKey(usr))
                {
                    usrDict.Add(usr, 1);
                }
                else
                {
                    usrDict[usr]++;
                }
            }
            List<KeyValuePair<string, int>> newusrList = usrDict.ToList();
            newusrList.Sort((firstPair, nextPair) =>
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            });
            newusrList.Reverse();
            ReportGenerator.usrList = newusrList;

        }


        private void Save(List<KeyValuePair<string, int>> pwList, string filename)
        {
            mtx.WaitOne();
            string path = @"C:\Users\benla\Desktop\git\"+ filename + ".txt";

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (var pw in pwList)
                    {
                        double percent = (double)pw.Value / (double)pws.Count * 100;
                        string sP = String.Format("{0:0.0}", percent);
                        sw.WriteLine(filename + ": " + pw.Key + " is used " + pw.Value + " time(s) (" + sP + "%)");
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
