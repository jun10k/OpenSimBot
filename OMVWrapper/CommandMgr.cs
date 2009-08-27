using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Command;
using OpenSimBot.OMVWrapper.Manager;

namespace OpenSimBot.OMVWrapper.Manager
{
    public delegate void Cmd_Login();
    public delegate void Cmd_Logout();

    public sealed class CommandMgr : Singleton<CommandMgr>, IManager
    {
        /*Members**************************************************************/
        private const string INSTRUCTION_LOGIN = "login";

        private readonly List<string> m_instructionList = new List<string>();

        /*Functions************************************************************/
        public bool Initialize()
        {
            m_instructionList.Add(INSTRUCTION_LOGIN);

            return false;
        }

        public void Reset()
        {
            m_instructionList.Clear();
        }

        public void SetLogLevel()
        {

        }

        public bool IsValidInstruction(string name)
        {
            return m_instructionList.Contains(name.ToLower());
        }
    }
}
