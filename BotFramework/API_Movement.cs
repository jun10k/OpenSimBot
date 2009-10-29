using System;
using System.Collections.Generic;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Manager;
using OpenSimBot.OMVWrapper;

namespace OpenSimBot.BotFramework
{
    public class API_Movement : Singleton<API_Movement>
    {

        public bool MoveTo(string firstname, 
                           string lastname, 
                           ulong globalX,
                           ulong globalY,
                           float z)
        {
            return false;
        }

        public void RandomMoving(string firstname, 
                                 string lastname)
        {
            BotSessionMgr.BotSession sess = 
                BotSessionMgr.Instance.FindBotSession(firstname, lastname);
            if (null != sess)
            {
                sess.Bot.Assignment.AddStep(new BotAgent.BotAssignment.TestStep("RandomMoving", null));
            }
        }

        public void ToFly(string firstname, string lastname, bool isCancel)
        {

        }
   }
}