using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Manager;

namespace OpenSimBot.OMVWrapper.Command
{
    public class OMVCommandFactory : Singleton<OMVCommandFactory>, ICommandFactory
    {
        public ICommand CreateCommand(Guid stepID, 
                                      BotSessionMgr.BotSession owner)
        {
            ICommand cmd = null;
            switch (owner.Bot.Assignment.GetStepByID(stepID).Name)
            {
                case "login":
                    cmd = new Cmd_Login(stepID, owner);
                    break;

                case "chat":
                    cmd = new Cmd_Chat(stepID, owner);
                    break;

            }

            return cmd;
        }
    }
}
