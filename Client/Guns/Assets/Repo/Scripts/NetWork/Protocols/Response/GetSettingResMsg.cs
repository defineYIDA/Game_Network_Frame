using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.NetWork
{
    class GetSettingResMsg : MsgBase
    {
        // 注册状态："true"/"false"
        public string status = "";

        public string data = "{}";

        public string type = "";

        public override int GetCommand()
        {
            return Command.GET_SETTING_RESPONSE;
        }

        public PlayerSetting GetPlayerSetting()
        {
            return (PlayerSetting)JsonUtility.FromJson(data, Type.GetType("PlayerSetting"));
        }

        public EnemySetting GetEnemySetting()
        {
            return (EnemySetting)JsonUtility.FromJson(data, Type.GetType("EnemySetting"));
        }
    }
}
