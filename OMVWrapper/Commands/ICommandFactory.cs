using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Manager;

namespace OpenSimBot.OMVWrapper.Command
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(string cmdName, BotSessionMgr.BotSession owner);
    }
}
