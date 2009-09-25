using System;
using System.Collections.Generic;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;

namespace OpenSimBot.BotFramework
{
    public class API_InstanceMsg : Singleton<API_InstanceMsg>
    {
        public bool UserChat(string firstname, string lastname, string msg)
        {
            return false;
        }

        public bool Chat(string msg)
        {
            return false;
        }

        public bool RandomChating()
        {
            return false;
        }
    }
}
