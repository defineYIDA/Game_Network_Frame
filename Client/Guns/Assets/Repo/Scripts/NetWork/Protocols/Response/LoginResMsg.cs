using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    /// <summary>
    /// 登陆响应消息
    /// </summary>
    class LoginResMsg : MsgBase
    {
        public string id = "";

        // 登陆状态："true"/"false"
        public string status = "";

        // 消息
        public string msg = "";

        public override int GetCommand()
        {
            return Command.LOGIN_RESPONSE;
        }

        public override string ToString()
        {
            return "{command: " + GetCommand() + ", id: " + id + ", status: " + status + ", msg: " + msg + "}";
        }
    }
}
