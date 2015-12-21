using System;
using System.Diagnostics;
using System.Text;

namespace WiPS.CmdLineExecutor
{
    public class CmdCommand : ICommand
    {
        public delegate void OutputReadyHandler(string data);
        public event OutputReadyHandler OnOutputReady;
        public void HandleOutputReady(string output)
        {
            OnOutputReady?.Invoke(output);
        }

        private string _arg;
        private ProcessStartInfo _startinfo = new ProcessStartInfo();

        public CmdCommand(string argument)
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
        public void Execute()
        {
            string output;
            using (var process = Process.Start(_startinfo))
            {
                var StdOutput = new StringBuilder();

                while (!process.HasExited)
                {
                    StdOutput.Append(process.StandardOutput.ReadToEnd());
                }
                StdOutput.Append(process.StandardOutput.ReadToEnd());

                output = StdOutput.ToString();
            }

            HandleOutputReady(output);
        }
    }
}