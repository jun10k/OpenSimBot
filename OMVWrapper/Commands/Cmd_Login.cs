using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Manager;

namespace OpenSimBot.OMVWrapper.Command
{
    class Cmd_Login : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        private readonly BotSessionMgr.BotSession m_owner = null;

        /*Functions************************************************************/
        public Cmd_Login(BotSessionMgr.BotSession owner)
        {
            m_owner = owner;
        }

        public bool Execute()
        {
            if (m_owner != null)
            {

            }

            return false;
        }

        public void OnConnected(object sender)
        {
            UpdateInfo info = new UpdateInfo();
            OnCmdUpdated.Invoke(info);
        }
    }
}