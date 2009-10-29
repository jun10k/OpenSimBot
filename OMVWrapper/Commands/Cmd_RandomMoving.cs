using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using OpenSimBot.OMVWrapper.Manager;
using OpenMetaverse;
using log4net;

namespace OpenSimBot.OMVWrapper.Command
{
    class Cmd_RandomMoving : ICommand
    {
        /*Members**************************************************************/
        public event CmdUpdated OnCmdUpdated;
        protected static readonly ILog m_log =
            LogManager.GetLogger(typeof(Cmd_RandomMoving));
        public const string CMD_NAME = "RandomMoving";
        private readonly Guid m_stepID = Guid.Empty;
        private readonly BotSessionMgr.BotSession m_owner = null;
        private bool isToQuitRandomMoving = false;
        private Random random = new Random(Environment.TickCount);

        /*Attributes***********************************************************/
        public string Name
        {
            get { return CMD_NAME; }
        }

        /*Functions************************************************************/
        public Cmd_RandomMoving(Guid stepID, BotSessionMgr.BotSession owner)
        {
            m_stepID = stepID;
            m_owner = owner;
        }

        public bool Execute() 
        {
            try
            {
                isToQuitRandomMoving = false;
                WaitCallback randomMovingRoutin =
                        new WaitCallback(RandomMovingRoutin);
                ThreadPool.QueueUserWorkItem(randomMovingRoutin);
            }
            catch
            {
                m_log.Error("Fail to Execute the random moving for bot:" +
                            m_owner.Bot.Info.Firstname + " " +
                            m_owner.Bot.Info.Lastname);
            }

            return false;
        }

        public void PostExecute()
        {
            isToQuitRandomMoving = true;
        }

        private void RandomMovingRoutin(Object threadContext)
        {
            if (null == m_owner) return;

            while (!isToQuitRandomMoving)
            {
                if (0 == (random.Next() % 2))
                {
                    m_owner.Client.Self.Movement.AlwaysRun = true;
                }
                else
                {
                    m_owner.Client.Self.Movement.AlwaysRun = false;
                }

                Vector3 pos = new Vector3(random.Next(100), random.Next(100), random.Next(100));
                m_owner.Client.Self.Movement.TurnToward(pos);
                m_owner.Client.Self.Movement.AtPos = true;
                Thread.Sleep(random.Next(2000, 100000));
                m_owner.Client.Self.Movement.AtPos = false;

                UpdateInfo result = new UpdateInfo(m_stepID, this);
                OnCmdUpdated.Invoke(result);
            }
        }
    }
}
