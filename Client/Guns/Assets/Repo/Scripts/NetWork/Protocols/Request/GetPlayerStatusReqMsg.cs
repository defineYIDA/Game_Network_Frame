using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{

    class GetPlayerStatusReqMsg : MsgBase
    {
        public string key = "";

        public string id = "";

        public GetPlayerStatusReqMsg(string key, string id)
        {
            this.key = key;
            this.id = id;
        }

        public override int GetCommand()
        {
            return Command.GET_PLAYER_STATUS_REQUEST;
        }
    }
}
