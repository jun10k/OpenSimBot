using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Manager;
using OpenMetaverse;
using log4net;

namespace OpenSimBot.OMVWrapper.Command
{
    class Cmd_ToFly : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        public const string CMD_NAME = "ToFly";
        private readonly Guid m_stepID = Guid.Empty;
        private readonly BotSessionMgr.BotSession m_owner = null;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(Cmd_ToFly));

        /*Attributes***********************************************************/
        public string Name
        {
            get { return CMD_NAME; }
        }

        /*Functions************************************************************/
        public Cmd_ToFly(Guid stepID, BotSessionMgr.BotSession owner)
        {
            m_stepID = stepID;
            m_owner = owner;
        }

        public bool Execute() 
        {
            if (null != m_owner.Client)
            {
                bool isCancel = (bool)m_owner.Bot.Assignment.GetStepByID(m_stepID).Params["isCancel"];
                m_owner.Client.Self.Fly(!isCancel);
                UpdateInfo info = new UpdateInfo(m_stepID, this);
                info.Description = "Fly:" + isCancel.ToString();
                info.Status = UpdateInfo.CommandStatus.CMD_SUCCESS;
                OnCmdUpdated.Invoke(info);
            }

            return false;
        }

        public void PostExecute() {}
    }
}
