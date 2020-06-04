using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class RegisterResMsg : MsgBase
    {
        public string id = "";

        // 注册状态："true"/"false"
        public string status = "";

        // 消息
        public string msg = "";

        public override int GetCommand()
        {
            return Command.REGISTER_RESPONSE;
        }

        public override string ToString()
        {
            return "{command: " + GetCommand() + ", id: " + id + ", status: " + status + ", msg: " + msg + "}";
        }
    }
}
