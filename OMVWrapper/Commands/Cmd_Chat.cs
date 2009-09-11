using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Manager;
using OpenMetaverse;
using log4net;

namespace OpenSimBot.OMVWrapper.Command
{ 
    class Cmd_Chat : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        private readonly Guid m_stepID = Guid.Empty;
        private readonly BotSessionMgr.BotSession m_owner = null;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(Cmd_Chat));

        /*Functions************************************************************/
        public Cmd_Chat(Guid stepID, BotSessionMgr.BotSession owner)
        {
            m_stepID = stepID;
            m_owner = owner;
        }
        public bool Execute()
        {
            bool ret = false;
            if (null != m_owner)
            {
                if (null != m_owner.Client)
                {
                    string msg = (string)m_owner.Bot.Assignment.GetStepByID(m_stepID).Params["message"];
                    m_owner.Client.Self.Chat(msg, 0, ChatType.Shout);
                    UpdateInfo info = new UpdateInfo(m_stepID);
                    info.Description = "Chat:" + msg;
                    info.Status = UpdateInfo.CommandStatus.CMD_SUCCESS;
                    OnCmdUpdated.Invoke(info);
                }
            }

            return ret;
        }
    }
}
