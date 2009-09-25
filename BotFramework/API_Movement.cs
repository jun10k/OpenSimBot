using System;
using System.Collections.Generic;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;

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
                                 string lastname, 
                                 bool isCancel)
        {

        }

        public void ToFly(string firstname, string lastname, bool isCancel)
        {

        }
   }
}