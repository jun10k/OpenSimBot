using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Manager;
using OpenMetaverse;
using log4net;
using log4net.Config;

namespace OpenSimBot.OMVWrapper.Command
{
    class Cmd_Login : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        private readonly BotSessionMgr.BotSession m_owner = null;
        protected static readonly ILog m_log = 
            LogManager.GetLogger(typeof(Cmd_Login));

        /*Functions************************************************************/
        public Cmd_Login(BotSessionMgr.BotSession owner)
        {
            m_owner = owner;
            log4net.Config.XmlConfigurator.Configure();
        }

        public bool Execute()
        {
            bool ret = false;
            if (m_owner != null)
            {
                m_owner.client.Network.OnConnected += 
                    new NetworkManager.ConnectedCallback(OnConnected);
                if (m_owner.client.Network.Login(m_owner.Bot.Info.Firstname,
                                                 m_owner.Bot.Info.Lastname,
                                                 m_owner.Bot.Info.Password,
                                                 "Client application name",
                                                 "Client application version"))
                {
                    ret = true;
                    m_log.Info("BOT:" + m_owner.Bot.Info.Firstname + " " +
                               m_owner.Bot.Info.Lastname +
                               "Successfully to connect with the remote simulator.");
                }
                else
                {
                    m_log.Error("BOT:" + m_owner.Bot.Info.Firstname + " " +
                                m_owner.Bot.Info.Lastname +
                                "Fail to connect with the remote simulator.");
                }
            }

            return ret;
        }

        public void OnConnected(object sender)
        {
            if (m_owner != null)
            {
                UpdateInfo info = new UpdateInfo();
                OnCmdUpdated.Invoke(info);
            }
            else
            {
                // Log
            }
        }
    }
}