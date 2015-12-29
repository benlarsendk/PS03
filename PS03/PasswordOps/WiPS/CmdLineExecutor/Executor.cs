using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PS03.PasswordOps.WiPS.CmdLineExecutor
{
    public class Executor
    {
        private List<ICommand> _cmds = new List<ICommand>();

        public void AddCommand(ICommand cmd)
        {
            if (cmd != null)
                _cmds.Add(cmd);
            else throw new Exception("CMD is null");
        }

        public void ExecuteCommands()
        {
            Parallel.For(0, _cmds.Count, i =>
             {
                 _cmds[i].Execute();
             });
        }

    };
}
