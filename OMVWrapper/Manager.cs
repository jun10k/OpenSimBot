using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSimBot.OMVWrapper.Manager
{
    interface IManager
    {
        bool Initialize();
        void Reset();
        void SetLogLevel();
    }
}
