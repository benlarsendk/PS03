using PS03.CommandLineOptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.Operations
{
    public class Operations
    {
        private List<ITarget> _targets;

        public Operations(List<ITarget> targets)
        {
            _targets = targets;
        }

        public ConcurrentBag<Profile> ExecuteOperations(Options options)
        {
            var data = new List<Profile>();
            var bag = new ConcurrentBag<Profile>();
            var datahandler = new HandleData.DataHandler(options);

            Parallel.ForEach(_targets, (tg) =>
            {
                Parallel.ForEach(tg.ExectueModule(), (n) =>
                 {
                     bag.Add(n);
                 });
            });


            foreach(var d in bag)
            {
                data.Add(d);
            }
            if (options.Transmit)
                datahandler.Send(options.Port, options.Ip, data);
            datahandler.Print(data,new ConsolePrinter(), new ConsoleFormatter());


            return bag;
        }

    }
}
