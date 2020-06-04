using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class EventReqMsg : MsgBase
    {
        public string key;

        public string id;

        // 事件类型：0-被攻击；1-开枪；2-换弹
        public int type;

        public EventReqMsg(string key, string id, int type)
        {
            this.key = key;
            this.id = id;
            this.type = type;
        }

        public override int GetCommand()
        {
            return Command.EVENT_REQUEST;
        }
    }
}
