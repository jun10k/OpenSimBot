using System;
using System.Collections;
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
                           string servURI,
                           ulong globalX,
                           ulong globalY,
                           float globalZ)
        {
            BotSessionMgr.BotSession sess =
                BotSessionMgr.Instance.FindBotSession(firstname, lastname, servURI);
            if (null != sess)
            {
                Hashtable paramList = new Hashtable();
                paramList["globalX"] = globalX;
                paramList["globalY"] = globalY;
                paramList["globalZ"] = globalZ;
                sess.Bot.Assignment.AddStep(new BotAgent.BotAssignment.TestStep("MoveTo", null));
            }

            return true;
        }

        public void RandomMoving(string firstname, 
                                 string lastname,
                                 string servURI)
        {
            BotSessionMgr.BotSession sess = 
                BotSessionMgr.Instance.FindBotSession(firstname, lastname, servURI);
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