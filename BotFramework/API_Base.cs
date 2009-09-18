using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using OpenSimBot.OMVWrapper.Manager;
using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper;

namespace OpenSimBot.BotFramework
{
    public class API_Base : Singleton<API_Base>
    {
        /*Members**************************************************************/

        /*Functions************************************************************/
        public bool InitializeAll()
        {
            bool returnVal = true;

            returnVal &= BotSessionMgr.Instance.Initialize();
            returnVal &= CommandMgr.Instance.Initialize();

            return returnVal;
        }

        public void Reset()
        {
            BotSessionMgr.Instance.Reset();
            CommandMgr.Instance.Reset();
        }

        public void SetNetworkLatency()
        {

        }

        public bool Login(string firstname, 
                          string lastname, 
                          string password, 
                          string servURI,
                          string region,
                          int x, // start location.
                          int y,
                          int z)
        {
            BotAgent.BotInfo info = new BotAgent.BotInfo(firstname, lastname, password);
            BotAgent bot = new BotAgent(info);
            Hashtable paramList = new Hashtable();
            paramList["region"] = region;
            paramList["x"] = x.ToString();
            paramList["y"] = y.ToString();
            paramList["z"] = z.ToString();
            bot.Assignment.AddStep(new BotAgent.BotAssignment.TestStep("login", paramList));
            BotSessionMgr.Instance.CreateBotSession(bot);

            return false;
        }

        public void Logout(string firstname, string lastname)
        {
            BotSessionMgr.Instance.RemoveBotSession(firstname, lastname, false);
        }

        public void LogoutAll()
        {
            BotSessionMgr.Instance.RemoveBotSession(null, null, true);
        }

        public bool BeginAssignment(string firstname, string lastname)
        {

            return false;
        }

        public void EndAssignment()
        {

        }
    }
}
