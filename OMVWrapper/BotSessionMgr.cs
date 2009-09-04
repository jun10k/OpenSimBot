using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Command;
using OpenMetaverse;

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

        public void SetLogLevel()
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
            private BotAgent m_botAgent;
            private GridClient m_omvClient = new GridClient();
            private Thread m_exeAssignment;
            private AutoResetEvent m_loopEvent = new AutoResetEvent(false);


            /*Attributes*******************************************************/
            public BotAgent Bot
            {
                get { return m_botAgent; }
            }

            public GridClient client
            {
                get { return m_omvClient; }
            }

            /*Functions********************************************************/
            public BotSession(BotAgent bot)
            {
                m_botAgent = bot;
                m_exeAssignment = new Thread(SesstionThreadRoutin);
            }

            private void OnSessionUpdated(UpdateInfo cmdInfo)
            {
                m_loopEvent.Set();
            }

            private void SesstionThreadRoutin()
            {
                if (m_botAgent != null)
                {
                    CommandMgr.Instance.ProcessTestSteps(this, new CmdUpdated(OnSessionUpdated));
                }
            }
        }
    }
}
