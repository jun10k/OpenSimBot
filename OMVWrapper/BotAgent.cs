using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSimBot.OMVWrapper
{
    class BotAgent
    {
        /*Members**************************************************************/
        Guid m_botGUID = Guid.NewGuid();

        /*Attributes***********************************************************/
        public Guid botGUID
        {
            get { return m_botGUID; }
        }

    }
}
