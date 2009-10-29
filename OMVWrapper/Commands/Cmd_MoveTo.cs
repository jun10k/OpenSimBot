using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Manager;
using OpenMetaverse;
using log4net;

namespace OpenSimBot.OMVWrapper.Command
{
    class Cmd_MoveTo : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        public const string CMD_NAME = "MoveTo";
        private readonly Guid m_stepID = Guid.Empty;
        private readonly BotSessionMgr.BotSession m_owner = null;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(Cmd_MoveTo));

        /*Attributes***********************************************************/
        public string Name
        {
            get { return CMD_NAME; }
        }

        /*Functions************************************************************/
        public Cmd_MoveTo(Guid stepID, BotSessionMgr.BotSession owner)
        {
            m_stepID = stepID;
            m_owner = owner;
        }

        public bool Execute() 
        {
            if (null != m_owner.Client)
            {
                ulong globalX = (ulong)m_owner.Bot.Assignment.GetStepByID(m_stepID).Params["globalX"];
                ulong globalY = (ulong)m_owner.Bot.Assignment.GetStepByID(m_stepID).Params["globalY"];
                float z = (float)m_owner.Bot.Assignment.GetStepByID(m_stepID).Params["z"];

                m_owner.Client.Self.AutoPilot(globalX, globalY, z);
                UpdateInfo info = new UpdateInfo(m_stepID, this);
                info.Description = "Bot{" + m_owner.Bot.Info.Firstname + " " +
                                   m_owner.Bot.Info.Lastname + "}" + "move to {" +
                                   globalX.ToString() + "," + globalY.ToString() + "," +
                                   z.ToString() + "}";
                info.Status = UpdateInfo.CommandStatus.CMD_SUCCESS;
                OnCmdUpdated.Invoke(info);
            }

            return false;
        }

        public void PostExecute()
        {

        }
    }
}
