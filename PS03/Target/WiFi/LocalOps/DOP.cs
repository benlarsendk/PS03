using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;

namespace PS03.PasswordOps.WiPS.WiFI
{
    internal class DOP
    {
        private readonly ConcurrentBag<Profile> ProfilesList;


        public List<Profile> ExtractProfiles(string data)
        {
            var localList = new List<Profile>();

            if (data.StartsWith("The Wireless AutoConfig Service"))
                return localList;

            var rawprofiles = GetRawProfileLines(GetLines(data));
            foreach (var rawpro in rawprofiles)
            {
                localList.Add(GetProfileNameSSID(rawpro));
            }
            return localList;

        }

        private List<string> GetLines(string data)
        {
            var lines = new List<string>();
            var line = "";
            foreach (var x in data)
            {
                if (x == '\n')
                {
                    if (!string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                    line = "";
                }
                else
                {
                    line += x;
                }
            }

            return lines;
        }

        private List<string> GetRawProfileLines(List<string> lines)
        {
            var rawprofiles = new List<string>();
            foreach (var line in lines)
            {
                if (line.StartsWith("    All User Profile"))
                {
                    rawprofiles.Add(line);
                }
            }
            return rawprofiles;
        }


        private Profile GetProfileNameSSID(string rawprofile)
        {
            var sep = rawprofile.IndexOf(": ") + ": ".Length;
            return new Profile { Action = rawprofile.Substring(sep, rawprofile.Length - sep - 1) };
        }

        private Profile SortLines(List<string> lines)
        {

            var p = new Profile();
            foreach (var line in lines)
            {
                if (line.StartsWith("    Authentication"))
                {
                    var sep = line.IndexOf(": ") + ": ".Length;
                    p.Specification = line.Substring(sep, line.Length - sep - 1);
                }
                else if (line.StartsWith("    Key Content"))
                {
                    var sep = line.IndexOf(": ") + ": ".Length;
                    p.Password = line.Substring(sep, line.Length - sep - 1);
                }
                else if (line.StartsWith("    Name"))
                {
                    var sep = line.IndexOf(": ") + ": ".Length;
                    p.Action = line.Substring(sep, line.Length - sep - 1);
                }
            }
            return p;
        }

        private List<string> Split(string data)
        {
            var clean = new List<string>();
            string del2 = "on interface Wi-Fi:";
           // string del2 = "Cost Source";

            int x = 0;

            while (true)
            {
                int end = data.IndexOf(del2);
                if (end == -1) break;
                clean.Add(data.Substring(0,end));
                data = data.Substring(end+del2.Count(), data.Length - (end+del2.Count()));
            }
            clean.RemoveAt(0);
            return clean;

        }

        private void Checkup(Profile p, List<Profile> Profiles)
        {
            foreach (var pro in Profiles)
            {
                if (pro.Action == p.Action)
                {
                    pro.Password = p.Password;
                    pro.Specification = p.Specification;
                    break;
                }
            }
        }

        public List<Profile> GetClearProfiles(string data, List<Profile> ps)
        {
            var all = Split(data);
            foreach (var VARIABLE in all)
            {
                var rawdata = SortLines(GetLines(VARIABLE));
                Checkup(rawdata,ps);
            }

            return ps;
        }
    }
}