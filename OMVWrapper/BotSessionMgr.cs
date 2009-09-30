using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Command;
using OpenMetaverse;
using log4net;

namespace OpenSimBot.OMVWrapper.Manager
{
    public class BotSessionMgr : Singleton<BotSessionMgr>, IManager
    {
        /*Members**************************************************************/
        private List<BotSession> m_sessionList = new List<BotSession>();
        private Simulator.SimStats m_simStats;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(BotSessionMgr));

        /*Attributes***********************************************************/

        /*Functions************************************************************/
        public BotSessionMgr()
        {
        }

        public bool Initialize()
        {
            return false;
        }

        public void Reset()
        {

        }

        public BotSession FindBotSession(string firstname, string lastname)
        {
            BotSession ret = null;
            foreach (BotSession sess in m_sessionList)
            {
                if (0 == string.Compare(sess.Bot.Info.Firstname, firstname, true) &&
                    0 == string.Compare(sess.Bot.Info.Lastname, lastname, true))
                {
                    return ret = sess;
                }
            }

            return ret; 
        }

        public BotSession CreateBotSession(BotAgent bot) 
        {
            BotSession ret = null;
            foreach (BotSession sess in m_sessionList)
            {
                if (0 == string.Compare(sess.Bot.Info.Firstname, bot.Info.Firstname, true) &&
                    0 == string.Compare(sess.Bot.Info.Lastname, bot.Info.Lastname, true))
                {
                    m_log.Info("The session for bot" + 
                               bot.Info.Firstname + " " + 
                               bot.Info.Lastname +
                               "is already existed.");
                    ret = sess;
                }
            }

            ret = new BotSession(bot);
            m_sessionList.Add(ret);
            return ret;
        }

        public void RemoveBotSession(string firstname, string lastname, bool isAll)
        {
            foreach (BotSession sess in m_sessionList)
            {
                if (0 == string.Compare(sess.Bot.Info.Firstname, firstname, true) &&
                    0 == string.Compare(sess.Bot.Info.Lastname, lastname, true) || isAll)
                {
                    sess.Terminate();
                    m_sessionList.Remove(sess);
                }
            }
        }

        public class BotSession
        {
            /*Members**********************************************************/
            public enum SessionStatus
            {
                SESS_WAIT = 0,
                SESS_FAIL,
                SESS_RUNNING,
            }

            protected static readonly ILog m_log =
                LogManager.GetLogger(typeof(BotSession));
            private BotAgent m_botAgent = null;
            private GridClient m_omvClient = new GridClient();
            private AutoResetEvent m_loopEvent = new AutoResetEvent(false);
            private SessionStatus m_status = SessionStatus.SESS_WAIT;
            private bool isToQuitSession = false;

            /*Attributes*******************************************************/
            public SessionStatus Status
            {
                get { return m_status; }
            }

            public BotAgent Bot
            {
                get { return m_botAgent; }
            }

            public GridClient Client
            {
                get { return m_omvClient; }
            }

            /*Functions********************************************************/
            public BotSession(BotAgent bot)
            {
                m_botAgent = bot;
                if (m_botAgent != null)
                {
                    WaitCallback sessRoutin = 
                        new WaitCallback(SesstionThreadRoutin);
                    ThreadPool.QueueUserWorkItem(sessRoutin);
                    m_status = SessionStatus.SESS_RUNNING;
                    m_log.Info("SESSION: (" + bot.Info.Firstname + " " +
                               bot.Info.Lastname + ") is running.");
                }
                else
                {
                    m_status = SessionStatus.SESS_FAIL;
                    m_log.Debug("SESSION: The session contains null bot.");
                }
            }

            public void Terminate()
            {
                isToQuitSession = true;
                Client.Network.Logout();
                m_status = SessionStatus.SESS_WAIT;
            }

            private void OnSessionUpdated(UpdateInfo cmdInfo)
            {
                if (null == cmdInfo) return;
                if (null != cmdInfo.Owner)
                {
                    if (Cmd_RandomMoving.CMD_NAME == cmdInfo.Owner.Name)
                    {
                        cmdInfo.Owner.PostExecute();
                    }

                    switch (cmdInfo.Status)
                    {
                        case UpdateInfo.CommandStatus.CMD_SUCCESS:
                            m_botAgent.Assignment.GetStepByID(cmdInfo.StepID).Status =
                                BotAgent.BotAssignment.TestStep.TestStatus.TESTSTEP_SUCESS;
                            break;

                        case UpdateInfo.CommandStatus.CMD_FAIL:
                        case UpdateInfo.CommandStatus.CMD_ERROR:
                            m_botAgent.Assignment.GetStepByID(cmdInfo.StepID).Status =
                                BotAgent.BotAssignment.TestStep.TestStatus.TESTSTEP_FAILE;
                            break;
                    }
                }

                m_loopEvent.Set();
            }

            private void SesstionThreadRoutin(Object threadContext)
            {
                if (m_botAgent != null)
                {
                    while (!isToQuitSession)
                    {
                        CommandMgr.Instance.ProcessTestSteps(this, new CmdUpdated(OnSessionUpdated));
                        m_loopEvent.WaitOne();
                    }
                }
            }
        }
    }
}
