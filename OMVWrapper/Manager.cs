using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenSimBot.OMVWrapper.Manager
{
    public interface IManager
    {
        bool Initialize();
        void Reset();
        void SetLogLevel();
    }
}
