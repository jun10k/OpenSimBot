using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSimBot.OMVWrapper.Command
{
    public delegate void CmdExecuted();
    interface ICommand
    {
        bool Execute();
        event CmdExecuted OnCmdExecuted;
    }
}
