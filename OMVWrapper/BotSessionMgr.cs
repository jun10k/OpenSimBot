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

        public bool CreateBotSession(BotAgent bot) 
        {
            foreach (BotSession sess in m_sessionList)
            {
                if (0 == string.Compare(sess.Bot.Info.Firstname, bot.Info.Firstname, true) &&
                    0 == string.Compare(sess.Bot.Info.Lastname, bot.Info.Lastname, true))
                {
                    return false;
                }
            }

            m_sessionList.Add(new BotSession(bot));
            return true;
        }

        public void RemoveBotSession(BotAgent bot)
        {
            foreach (BotSession sess in m_sessionList)
            {
                if (0 == string.Compare(sess.Bot.Info.Firstname, bot.Info.Firstname, true) &&
                    0 == string.Compare(sess.Bot.Info.Lastname, bot.Info.Lastname, true))
                {
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

            private void OnSessionUpdated(UpdateInfo cmdInfo)
            {
                m_loopEvent.Set();
            }

            private void SesstionThreadRoutin(Object threadContext)
            {
                if (m_botAgent != null)
                {
                    if (!CommandMgr.Instance.ProcessTestSteps(this, new CmdUpdated(OnSessionUpdated)))
                    {
                            m_loopEvent.WaitOne();
                    }
                    
                }
            }
        }
    }
}
