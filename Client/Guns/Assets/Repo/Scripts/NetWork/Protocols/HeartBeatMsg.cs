using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class HeartBeatMsg : MsgBase
    {
        public override int GetCommand()
        {
            return Command.HEARTBEAT;
        }
    }
}
