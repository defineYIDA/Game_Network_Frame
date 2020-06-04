using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class RegisterReqMsg : MsgBase
    {
        public string id = "";

        public string pwd = "";

        public RegisterReqMsg(string id, string pwd)
        {
            this.id = id;
            this.pwd = pwd;
        }

        public override int GetCommand()
        {
            return Command.REGISTER_REQUEST;
        }

        public override string ToString()
        {
            return "{command: " + GetCommand() + ", id: " + id + ", pwd: " + pwd + "}";
        }
    }
}
