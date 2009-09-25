using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using OpenSimBot.BotFramework;

namespace BotMonitor
{
    public partial class BotMonitor : Form
    {
        public BotMonitor()
        {
            InitializeComponent();
            API_Base.Instance.InitializeAll();
            LuaAgent.Instance.ProcessScripts();
        }
    }
}
