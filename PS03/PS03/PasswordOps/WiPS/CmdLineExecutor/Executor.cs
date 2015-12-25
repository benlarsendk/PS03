using System;
using System.Collections.Generic;

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
            foreach (ICommand cmd in _cmds)
            {
                cmd.Execute();
            }
        }

    };
}
