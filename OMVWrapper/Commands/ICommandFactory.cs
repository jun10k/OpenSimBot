using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Manager;

namespace OpenSimBot.OMVWrapper.Command
{
    public interface ICommandFactory
    {
        ICommand CreateCommand(Guid stepID, 
                               BotSessionMgr.BotSession owner);
    }
}
