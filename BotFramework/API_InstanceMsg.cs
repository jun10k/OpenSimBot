using System;
using System.Collections.Generic;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;

namespace OpenSimBot.BotFramework
{
    public sealed class API_InstanceMsg : Singleton<API_InstanceMsg>
    {
        public API_InstanceMsg()
        {

        }

        public bool Chat(string firstname, string lastname, string msg)
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
