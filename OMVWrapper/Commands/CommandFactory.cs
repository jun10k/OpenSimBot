using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Manager;

namespace OpenSimBot.OMVWrapper.Command
{
    public class OMVCommandFactory : Singleton<OMVCommandFactory>, ICommandFactory
    {
        public ICommand CreateCommand(string cmdName, BotSessionMgr.BotSession owner)
        {
            ICommand cmd = null;
            switch (cmdName)
            {
                case "login":
                    cmd = new Cmd_Login(owner);
                    break;
            }

            return cmd;
        }
    }
}
