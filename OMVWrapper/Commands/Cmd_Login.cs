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
    class Cmd_Login : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        protected const int LOGIN_TIMEOUT = 30000;
        public const string CMD_NAME = "Login";
        private AutoResetEvent m_loginEvent = new AutoResetEvent(false);
        private readonly Guid m_stepID = Guid.Empty;
        private readonly BotSessionMgr.BotSession m_owner = null;
        protected static readonly ILog m_log = 
            LogManager.GetLogger(typeof(Cmd_Login));

        /*Attributes***********************************************************/
        public string Name
        {
            get { return CMD_NAME; }
        }

        /*Functions************************************************************/
        public Cmd_Login(Guid stepID, BotSessionMgr.BotSession owner)
        {
            m_stepID = stepID;
            m_owner = owner;
            log4net.Config.XmlConfigurator.Configure();
        }

        public bool Execute()
        {
            bool ret = false;
            if (m_owner != null)
            {
                if (null != m_owner.Client)
                {
                    m_owner.Client.Network.OnConnected +=
                        new NetworkManager.ConnectedCallback(OnConnected);
                    UpdateInfo info = new UpdateInfo(m_stepID, this);
                    BotAgent.BotAssignment.TestStep step = m_owner.Bot.Assignment.GetStepByID(m_stepID);
                    NetworkManager.StartLocation(step.Params["region"].ToString(),
                                                 (int)step.Params["x"],
                                                 (int)step.Params["y"],
                                                 (int)step.Params["z"]);
                    if (m_owner.Client.Network.Login(m_owner.Bot.Info.Firstname,
                                                     m_owner.Bot.Info.Lastname,
                                                     m_owner.Bot.Info.Password,
                                                     "Client application name",
                                                     "Client application version"))
                    {
                        ret = true;
                        m_loginEvent.WaitOne(LOGIN_TIMEOUT);
                        m_log.Info("BOT:" + m_owner.Bot.Info.Firstname + " " +
                                   m_owner.Bot.Info.Lastname +
                                   "Successfully to connect with the remote simulator.");
                        info.Status = UpdateInfo.CommandStatus.CMD_SUCCESS;
                    }
                    else
                    {
                        m_log.Error("BOT:" + m_owner.Bot.Info.Firstname + " " +
                                    m_owner.Bot.Info.Lastname +
                                    "Fail to connect with the remote simulator.");
                        info.Status = UpdateInfo.CommandStatus.CMD_FAIL;
                    }

                    info.Description = m_owner.Client.Network.LoginMessage;
                    OnCmdUpdated.Invoke(info);
                }
            }

            return ret;
        }

        public void PostExecute()
        {
        }

        public void OnConnected(object sender)
        {
            m_loginEvent.Set();
        }
    }
}