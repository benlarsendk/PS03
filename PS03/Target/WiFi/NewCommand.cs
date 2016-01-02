using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS03.Target.WiFi.CmdLineExecutor
{
    public class NewCommand
    {
        private string _arg;
        private readonly ProcessStartInfo _startinfo = new ProcessStartInfo();

        public NewCommand(string argument)
        {
            if (String.IsNullOrWhiteSpace(argument))
                throw new Exception("Argument is invalid");

            _arg = "/C " + argument;
            _startinfo.WindowStyle = ProcessWindowStyle.Hidden;
            _startinfo.FileName = "cmd.exe";
            _startinfo.Arguments = _arg;
            _startinfo.RedirectStandardOutput = true;
            _startinfo.UseShellExecute = false;
        }
        public string Execute()
        {
            using (var process = Process.Start(_startinfo))
            {
                var stdOutput = new StringBuilder();

                while (process != null && !process.HasExited)
                {
                    stdOutput.Append(process.StandardOutput.ReadToEnd());
                }
                if (process != null) stdOutput.Append(process.StandardOutput.ReadToEnd());

                return stdOutput.ToString();
            }

        }
            
        
    }
}
