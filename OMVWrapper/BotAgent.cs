using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OpenSimBot.OMVWrapper
{
    public class BotAgent
    {
        /*Members**************************************************************/
        private readonly Guid m_botGUID = Guid.NewGuid();
        private readonly BotInfo m_botInfo;
        private BotAssignment m_assignment = new BotAssignment();

        /*Attributes***********************************************************/
        public Guid ID
        {
            get { return m_botGUID; }
        }

        public BotInfo Info
        {
            get { return m_botInfo; }
        }

        public BotAssignment Assignment
        {
            get { return m_assignment; }
        }

        /*Functions************************************************************/
        public BotAgent(BotInfo botInfo)
        {
            m_botInfo = botInfo;
        }

        /*Class****************************************************************/
        public class BotInfo
        {
            /*Members**********************************************************/
            private readonly string m_firstname;
            private readonly string m_lastname;
            private readonly string m_password;

            /*Attributes*******************************************************/
            public string Firstname
            {
                get { return m_firstname; }
            }

            public string Lastname
            {
                get { return m_lastname; }
            }

            public string Password
            {
                get { return m_password; }
            }
            /*Functions********************************************************/
            public BotInfo(string firstname,
                           string lastname,
                           string password)
            {
                m_firstname = firstname;
                m_lastname = lastname;
                m_password = password;
            }

            // The command will return the exceptions and errors while proceeding.
            private void HandleException()
            {

            }

            private void LogExcetption(string msg)
            {


            }

        }

        public class BotAssignment
        {
            /*Members**********************************************************/
            private List<TestStep> m_stepList = new List<TestStep>();
            private bool m_isFinished = false;

            /*Attributes*******************************************************/
            public bool IsFinished
            {
                get { return m_isFinished; }
                set { m_isFinished = value; }
            }

            /*Functions********************************************************/
            public void AddStep(TestStep step)
            {
                if (step != null)
                {
                    m_stepList.Add(step);
                }
            }

            public void ResetAssignment()
            {
                foreach (TestStep step in m_stepList)
                {
                    if (step.Status != TestStep.TestStatus.TESTSTEP_WAIT)
                    {
                        step.Status = TestStep.TestStatus.TESTSTEP_WAIT;
                    }
                }
            }

            public TestStep GetNextStep()
            {
                TestStep ret = null;
                foreach (TestStep step in m_stepList)
                {
                    if (step.Status == TestStep.TestStatus.TESTSTEP_WAIT)
                    {
                        ret = step;
                        break; 
                    }
                }

                return ret;
            }

            /*Class************************************************************/
            public class TestStep
            {
                /*Members******************************************************/
                public enum TestStatus
                {
                    TESTSTEP_WAIT = 0,
                    TESTSTEP_FAILE,
                    TESTSTEP_PROCESSING,
                    TESTSTEP_SUCESS,
                }
                private readonly string m_name; 
                private readonly Hashtable m_paramList;
                private TestStatus m_status = TestStatus.TESTSTEP_WAIT;

                /*Attributes***************************************************/
                public string Name
                {
                    get { return m_name; }
                }

                public Hashtable Params
                {
                    get { return m_paramList; }
                }

                public TestStatus Status
                {
                    get { return m_status; }
                    set { m_status = value; }
                }

                /*Functions****************************************************/
                public TestStep(string name, Hashtable paramList)
                {
                    m_name = name;
                    m_paramList = paramList;
                }
            }
        }
    }
}
