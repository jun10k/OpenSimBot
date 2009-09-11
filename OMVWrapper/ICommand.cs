using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSimBot.OMVWrapper.Command
{
    public delegate void CmdUpdated(UpdateInfo result);

    public interface ICommand
    {
        bool Execute();
        event CmdUpdated OnCmdUpdated;
    }

    public class UpdateInfo
    {
        /*Members**************************************************************/
        public enum CommandStatus
        {
            CMD_READY = 0,
            CMD_SUCCESS,
            CMD_FAIL,
            CMD_ERROR,
        }

        private CommandStatus m_status = CommandStatus.CMD_READY;
        private readonly Guid m_stepID = Guid.Empty;
        private string m_description = string.Empty;

        /*Attributes***********************************************************/
        public CommandStatus Status
        {
            get { return m_status; }
            set { m_status = value; }
        }

        public Guid StepID
        {
            get { return m_stepID; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        /*Functions************************************************************/
        public UpdateInfo(Guid stepID)
        {
            m_stepID = stepID;
        }
    }
}
