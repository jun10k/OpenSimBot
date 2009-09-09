using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Command;
using OpenSimBot.OMVWrapper.Manager;
using log4net;

namespace OpenSimBot.OMVWrapper.Manager
{
    public delegate void Cmd_Login();
    public delegate void Cmd_Logout();

    public sealed class CommandMgr : Singleton<CommandMgr>, IManager
    {
        /*Members**************************************************************/
        private const string INSTRUCTION_LOGIN = "login";

        protected static readonly ILog m_log = 
            LogManager.GetLogger(typeof(CommandMgr));
        private System.Object m_queueLock = new System.Object();
        private readonly List<string> m_instructionList = new List<string>();
        private Queue<ICommand> m_cmdQueue = new Queue<ICommand>();

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

        public bool IsValidInstruction(string name)
        {
            return m_instructionList.Contains(name.ToLower());
        }

        public bool ProcessTestSteps(BotSessionMgr.BotSession owner,
                                     CmdUpdated cmdUpdatedHandler)
        {
            bool ret = false;
            if (owner != null)
            {
                BotAgent.BotAssignment.TestStep step = owner.Bot.Assignment.GetNextStep();
                if (null == step)
                {
                    m_log.Info("SESSION: (" + owner.Bot.Info.Firstname + " " +
                               owner.Bot.Info.Lastname + ") finished its assignment.");
                    cmdUpdatedHandler.Invoke(null);
                }

                if (null != step && IsValidInstruction(step.Name))
                {
                    ICommand cmd = OMVCommandFactory.Instance.CreateCommand(step.Name, owner);
                    if (cmd != null)
                    {
                        lock (m_queueLock)
                        {
                            cmd.OnCmdUpdated += cmdUpdatedHandler;
                            m_cmdQueue.Enqueue(cmd);
                            ret = true;
                        }
                    }
                    else
                    {
                        m_log.Error("No command object is generated for the legal intruction" + step.Name);
                    }
                }
                else
                {
                    m_log.Info("SESSION: (" + owner.Bot.Info.Firstname + " " +
                               owner.Bot.Info.Lastname + ") finished its assignment.");
                    UpdateInfo info = new UpdateInfo();
                    info.Status = UpdateInfo.CommandStatus.CMD_FAIL;
                    info.Description = "Illegal instruction for the bot (" +
                                       owner.Bot.Info.Firstname + " " +
                                       owner.Bot.Info.Lastname + ")";
                    cmdUpdatedHandler.Invoke(info);
                }
            }

            return ret;
        }

        private bool ProcessCommands()
        {
            bool ret = false;
            lock (m_queueLock)
            {
                if (0 < m_cmdQueue.Count)
                {
                    ICommand cmd = m_cmdQueue.Dequeue();
                    if (null != cmd)
                    {
                        ret = cmd.Execute();
                    }
                }
            }

            return ret;
        }
    }
}
