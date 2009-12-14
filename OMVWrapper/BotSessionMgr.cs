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
        private float m_simulatedLoss;
        private float m_simulatedDuplicateChance;
        private float m_simulatedMinimumLatency;
        private float m_simulatedLatencyVariance;

        private List<BotSession> m_sessionList = new List<BotSession>();
        private Simulator.SimStats m_simStats;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(BotSessionMgr));

        /*Attributes***********************************************************/
        #region Network Lantacy
        public float SimulatedLoss
        {
            get { return m_simulatedLoss; }
            set { m_simulatedLoss = value; }
        }

        public float SimulatedDuplicateChance
        {
            get { return m_simulatedDuplicateChance; }
            set { m_simulatedDuplicateChance = value; }
        }

        public float SimulatedMinimumLatency
        {
            get { return m_simulatedMinimumLatency; }
            set { m_simulatedMinimumLatency = value; }
        }

        public float SimulatedLatencyVariance
        {
            get { return m_simulatedLatencyVariance; }
            set { m_simulatedLatencyVariance = value; }
        }
        #endregion

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

        public BotSession FindBotSession(string firstname, string lastname, string servURI)
        {
            BotSession ret = null;
            foreach (BotSession sess in m_sessionList)
            {
                if (0 == string.Compare(sess.Bot.Info.Firstname, firstname, true) &&
                    0 == string.Compare(sess.Bot.Info.Lastname, lastname, true) &&
                    0 == string.Compare(sess.Bot.Info.Server, servURI, true))
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
                if (null != FindBotSession(bot.Info.Firstname, bot.Info.Lastname, bot.Info.Server))
                {
                    m_log.Info("The session for bot" + 
                               bot.Info.Firstname + " " + 
                               bot.Info.Lastname + "in server[" + bot.Info.Server + "]" +
                               "is already existed.");
                    ret = sess;
                }
            }

            if (null == ret)
            {
                ret = new BotSession(bot);
                ret.Client.Network.SimulatedDuplicateChance = m_simulatedDuplicateChance;
                ret.Client.Network.SimulatedLatencyVariance = m_simulatedLatencyVariance;
                ret.Client.Network.SimulatedLoss = m_simulatedLoss;
                ret.Client.Network.SimulatedMinimumLatency = m_simulatedMinimumLatency;

                m_sessionList.Add(ret);
            }

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
                    switch (cmdInfo.Status)
                    {
                        case UpdateInfo.CommandStatus.CMD_SUCCESS:
                            m_botAgent.Assignment.GetStepByID(cmdInfo.StepID).Status =
                                BotAgent.BotAssignment.TestStep.TestStatus.TESTSTEP_SUCESS;
                            m_botAgent.RegisterStatus(cmdInfo.Owner);
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
