using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper.Manager;
using OpenMetaverse;
using log4net;

namespace OpenSimBot.OMVWrapper.Command
{
    class Cmd_RandomChating : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(Cmd_RandomMoving));
        public const string CMD_NAME = "RandomChating";
        private readonly Guid m_stepID = Guid.Empty;
        private readonly BotSessionMgr.BotSession m_owner = null;
        private bool isToQuitRandomChating = false;
        private Random random = new Random(Environment.TickCount);

        /*Attributes***********************************************************/
        public string Name
        {
            get { return CMD_NAME; }
        }

        /*Functions************************************************************/
        public Cmd_RandomChating(Guid stepID, BotSessionMgr.BotSession owner)
        {
            m_stepID = stepID;
            m_owner = owner;
        }

        public bool Execute()
        {
            return false;
        }

        public void PostExecute()
        {
            isToQuitRandomChating = true;
        }

        private void RandomMovingRoutin(Object threadContext)
        {
            if (null == m_owner) return;

            UpdateInfo result = new UpdateInfo(m_stepID, this);
            OnCmdUpdated.Invoke(result);
            while (!isToQuitRandomChating)
            {

            }
        }
    }
}
