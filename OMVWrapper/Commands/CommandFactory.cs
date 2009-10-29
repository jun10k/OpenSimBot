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
            if (Guid.Empty != stepID)
            {
                switch (owner.Bot.Assignment.GetStepByID(stepID).Name)
                {
                    case Cmd_Login.CMD_NAME:
                        cmd = new Cmd_Login(stepID, owner);
                        break;

                    case Cmd_Chat.CMD_NAME:
                        cmd = new Cmd_Chat(stepID, owner);
                        break;

                    case Cmd_MoveTo.CMD_NAME:
                        cmd = new Cmd_MoveTo(stepID, owner);
                        break;

                    case Cmd_RandomMoving.CMD_NAME:
                        cmd = new Cmd_RandomMoving(stepID, owner);
                        break;

                    case Cmd_ToFly.CMD_NAME:
                        cmd = new Cmd_ToFly(stepID, owner);
                        break;
                }
            }

            return cmd;
        }
    }
}
