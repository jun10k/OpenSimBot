using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper;

namespace OpenSimBot.OMVWrapper
{
    class BotSessionMgr
    {
        /*Members**************************************************************/
        private List<BotAgent> m_botList = new List<BotAgent>();

        /*Attributes***********************************************************/

        /*Functions************************************************************/
        public bool AddBotSession(BotAgent bot) 
        {
            return false;
        }

        public void RemoveBotSession(BotAgent bot)
        {
        }
    }
}
