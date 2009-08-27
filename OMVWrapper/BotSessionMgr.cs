using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper.Utility;
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
                if (0 == string.Compare(sess.Bot.botInfo.Firstname, bot.botInfo.Firstname, true) &&
                    0 == string.Compare(sess.Bot.botInfo.Lastname, bot.botInfo.Lastname, true))
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
                if (0 == string.Compare(sess.Bot.botInfo.Firstname, bot.botInfo.Firstname, true) &&
                    0 == string.Compare(sess.Bot.botInfo.Lastname, bot.botInfo.Lastname, true))
                {
                    m_sessionList.Remove(sess);
                }
            }
        }

        private class BotSession
        {
            /*Members**********************************************************/
            private BotAgent m_botAgent;
            private GridClient m_omvClient;
            private Thread m_exeAssignment;

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
                m_omvClient = new GridClient();
                //m_exeAssignment = new Thread(
            }
        }
    }
}
