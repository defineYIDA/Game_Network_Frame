using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    /// <summary>
    /// 登陆请求消息
    /// </summary>
    class LoginReqMsg : MsgBase
    {

        public string id = "";

        public string pwd = "";

        public LoginReqMsg(string id, string pwd)
        {
            this.id = id;
            this.pwd = pwd;
        }

        public override int GetCommand()
        {
            return Command.LOGIN_REQUEST;
        }

        public override string ToString()
        {
            return "{command: " + GetCommand() + ", id: " + id + ", pwd: " + pwd + "}";
        }
    }
}
