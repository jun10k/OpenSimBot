using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper.Utility;
using OpenSimBot.OMVWrapper.Command;
using OpenSimBot.OMVWrapper.Manager;
using log4net;

namespace OpenSimBot.OMVWrapper.Manager
{
    public class CommandMgr : Singleton<CommandMgr>, IManager
    {
        /*Members**************************************************************/
        protected static readonly ILog m_log = 
            LogManager.GetLogger(typeof(CommandMgr));
        private System.Object m_queueLock = new System.Object();
        private readonly List<string> m_instructionList = new List<string>();
        private Queue<ICommand> m_cmdQueue = new Queue<ICommand>();
        private AutoResetEvent m_loopEvent = new AutoResetEvent(false);
        private bool m_isToQuit = false;

        /*Functions************************************************************/
        public bool Initialize()
        {
            m_instructionList.Add(Cmd_Chat.CMD_NAME);
            m_instructionList.Add(Cmd_Login.CMD_NAME);
            m_instructionList.Add(Cmd_MoveTo.CMD_NAME);
            m_instructionList.Add(Cmd_RandomMoving.CMD_NAME);
            m_instructionList.Add(Cmd_ToFly.CMD_NAME);

            return false;
        }

        public void Uninitialize()
        {
            lock (m_queueLock)
            {
                m_isToQuit = true;
                m_cmdQueue.Clear();
                m_instructionList.Clear();
            }
        }

        public void Reset()
        {
            lock (m_queueLock)
            {
                m_cmdQueue.Clear();
            }
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
                    ICommand cmd = OMVCommandFactory.Instance.CreateCommand(step.ID, owner);
                    if (cmd != null)
                    {
                        lock (m_queueLock)
                        {
                            cmd.OnCmdUpdated += cmdUpdatedHandler;
                            bool toWakeQueueThread = (0 == m_cmdQueue.Count) ? true : false;
                            m_cmdQueue.Enqueue(cmd);
                            if (toWakeQueueThread) m_loopEvent.Set();
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
                    UpdateInfo info = new UpdateInfo(step.ID, null);
                    info.Status = UpdateInfo.CommandStatus.CMD_FAIL;
                    info.Description = "Illegal instruction for the bot (" +
                                       owner.Bot.Info.Firstname + " " +
                                       owner.Bot.Info.Lastname + ")";
                    cmdUpdatedHandler.Invoke(info);
                }
            }

            return ret;
        }

        private void ProcessCommands()
        {
            lock (m_queueLock)
            {
                while (0 < m_cmdQueue.Count && ! m_isToQuit)
                {
                    ICommand cmd = m_cmdQueue.Dequeue();
                    if (null != cmd)
                    {
                        cmd.Execute();
                    }

                    if (0 == m_cmdQueue.Count)
                    {
                        m_loopEvent.WaitOne();
                    }
                }
            }
        }
    }
}
