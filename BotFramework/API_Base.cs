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

        #region Network Simulation
        public void SimulatePackageLoss(float simulatedLoss)
        {
            BotSessionMgr.Instance.SimulatedLoss = simulatedLoss;
        }

        public void SimulateMinimumLatency(float simulatedMinimumLatency)
        {
            BotSessionMgr.Instance.SimulatedMinimumLatency = simulatedMinimumLatency;
        }

        public void SimulateLatencyVariance(float simulatgedLatencyVariance)
        {
            BotSessionMgr.Instance.SimulatedLatencyVariance = simulatgedLatencyVariance;
        }

        public void SimulatedDuplicateChance(float simulatedDuplicateChance)
        {
            BotSessionMgr.Instance.SimulatedDuplicateChance = simulatedDuplicateChance;
        }
        #endregion

        public bool Login(string firstname, 
                          string lastname, 
                          string password, 
                          string servURI)
        {
            BotAgent.BotInfo info = new BotAgent.BotInfo(firstname, lastname, password, servURI);
            BotAgent bot = new BotAgent(info);
            Hashtable paramList = new Hashtable();
            paramList["servURI"] = servURI;
            bot.Assignment.AddStep(new BotAgent.BotAssignment.TestStep("Login", paramList));
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
    }
}
