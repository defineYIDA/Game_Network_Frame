using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{

    class GetPlayerStatusResMsg : MsgBase
    {
        public string game_key;  // 对局key

        public string id;

        public int hp;  // 玩家血量

        public int bullet_amount;  // 当前弹夹子弹数

        public int total_bullet;  // 所有子弹数

        public string status;


        public override int GetCommand()
        {
            return Command.GET_PLAYER_STATUS_RESPONSE;
        }
    }
}
