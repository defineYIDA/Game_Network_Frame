using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class EventResMsg : MsgBase
    {
        public string status;

        public override int GetCommand()
        {
            return Command.EVENT_RESPONSE;
        }
    }
}
