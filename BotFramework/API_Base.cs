using System;
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

        }

        public void SetNetworkLatency()
        {

        }

        public bool Login(BotAgent.BotInfo info)
        {
            return false;
        }

        public void Logout(BotAgent.BotInfo info)
        {

        }

        public bool BeginAssignment(BotAgent.BotInfo info)
        {
            return false;
        }

        public void EndAssignment()
        {

        }
    }
}
