using System;
using CommandLine;
using CommandLine.Text;

namespace PS03.CommandLineOptions
{
    public class Options
    {
        [Option('t', "transmit", DefaultValue = false,
            HelpText = "Sets PS03 to transmit results.")]
        public bool Transmit { get; set; }

        [Option('l', "log", DefaultValue = false,
            HelpText = "Creates a report file.")]
        public bool Log { get; set; }

        [Option('r', "receive", DefaultValue = false,
            HelpText = "Sets PS03 to receive results.")]
        public bool Receive { get; set; }

        [Option('v', "verbose", DefaultValue = false,
            HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; }

        [Option('i', "ip", Required = false,
            HelpText = "IP to transmit to.")]
        public string Ip { get; set; }

        [Option('p', "port", Required = false,
            HelpText = "port to transmit/receive through")]
        public int Port { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
                (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }

        public void HandleArgs(string[] args)
        {
            var options = this;
            if (Parser.Default.ParseArguments(args, options))
            {
                if (options.Transmit)
                {
                    if (string.IsNullOrWhiteSpace(options.Ip))
                    {
                        Console.WriteLine("You must enter an IP for the receiver");
                        return;
                    }
                    if (options.Port <= 0 || options.Port > 65535)
                    {
                        Console.WriteLine("You must enter a valid portnumber");
                        return;
                    }

                    if (options.Verbose)
                    {
                        Console.WriteLine("PS03 is transmitting to " + options.Ip + " port " + options.Port);
                    }
                }

                if (options.Receive)
                {
                    if (options.Port <= 0 || options.Port > 65535)
                    {
                        Console.WriteLine("You must enter a valid portnumber");
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}