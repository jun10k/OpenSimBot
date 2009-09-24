using System;
using System.Collections.Generic;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using LuaInterface;

namespace OpenSimBot.BotFramework
{
    class LuaAgent : Singleton<LuaAgent>
    {
        /*Members**************************************************************/
        private Lua m_luaInterface = new Lua();

        /*Functions************************************************************/
        public LuaAgent()
        {
            // API_Base Interface
            m_luaInterface.RegisterFunction("InitializeAll", 
                                            API_Base.Instance, 
                                            API_Base.Instance.GetType().GetMethod("InitializeAll"));
            m_luaInterface.RegisterFunction("Reset",
                                            API_Base.Instance,
                                            API_Base.Instance.GetType().GetMethod("Reset"));
            m_luaInterface.RegisterFunction("Logout",
                                            API_Base.Instance,
                                            API_Base.Instance.GetType().GetMethod("Logout"));
            m_luaInterface.RegisterFunction("LogoutAll",
                                            API_Base.Instance,
                                            API_Base.Instance.GetType().GetMethod("LogoutAll"));
            m_luaInterface.RegisterFunction("BeginAssignment",
                                            API_Base.Instance,
                                            API_Base.Instance.GetType().GetMethod("BeginAssignment"));
            m_luaInterface.RegisterFunction("EndAssignment",
                                            API_Base.Instance,
                                            API_Base.Instance.GetType().GetMethod("EndAssignment"));

            // API_Movement Interface
            m_luaInterface.RegisterFunction("MoveTo",
                                            API_Movement.Instance,
                                            API_Movement.Instance.GetType().GetMethod("MoveTo"));
            m_luaInterface.RegisterFunction("RandomMoving",
                                            API_Movement.Instance,
                                            API_Movement.Instance.GetType().GetMethod("RandomMoving"));
            m_luaInterface.RegisterFunction("ToFly",
                                            API_Movement.Instance,
                                            API_Movement.Instance.GetType().GetMethod("ToFly"));

            // API_InstanceMsg Interface
            m_luaInterface.RegisterFunction("Chat",
                                            API_InstanceMsg.Instance,
                                            API_InstanceMsg.Instance.GetType().GetMethod("Chat"));
            m_luaInterface.RegisterFunction("RandomChating",
                                            API_InstanceMsg.Instance,
                                            API_InstanceMsg.Instance.GetType().GetMethod("RandomChating"));
        }

        public void ProcessScripts()
        {
            string dir = ConfigAgent.Instance.GetScriptsDirectory();
            if (!string.IsNullOrEmpty(dir))
            {

            }
        }

        public void ProcessScript(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {

            }
        }

        public void ProcessCommandLine(string cmd)
        {
            if (string.IsNullOrEmpty(cmd)) return;
            m_luaInterface.DoString(cmd);
        }
    }
}
