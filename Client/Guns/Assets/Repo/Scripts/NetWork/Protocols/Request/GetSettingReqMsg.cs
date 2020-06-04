using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class GetSettingReqMsg : MsgBase
    {
        public string id = "";

        public string type = "";

        public GetSettingReqMsg(string id, string type)
        {
            this.id = id;
            // 获取参数类型：player（玩家的初始参数），enemy（敌人的初始参数）
            this.type = type;
        }

        public override int GetCommand()
        {
            return Command.GET_SETTING_REQUEST;
        }
    }
}
