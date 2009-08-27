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

        /*Attributes***********************************************************/
        public Guid botGUID
        {
            get { return m_botGUID; }
        }

        public BotInfo botInfo
        {
            get { return m_botInfo; }
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

        }

        public class Assignment
        {
            /*Members**********************************************************/
            private List<TestStep> m_stepList = new List<TestStep>();


            /*Functions********************************************************/
            public void AddStep(TestStep step)
            {

            }

            public void RemoveStep(string stepName)
            {


            }

            /*Class************************************************************/
            public class TestStep
            {
                /*Members******************************************************/
                private readonly string m_name; 
                private readonly Hashtable m_paraList;

                /*Attributes***************************************************/
                public string name
                {
                    get { return m_name; }
                }

                public Hashtable Params
                {
                    get { return m_paraList; }
                }

                /*Functions****************************************************/
                public TestStep(string name, Hashtable paramList)
                {
                    m_name = name;
                    m_paraList = paramList;
                }
            }
        }
    }
}
