using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    /// <summary>
    /// 协议所有包对应的Command常量
    /// </summary>
    class Command
    {
        // command和协议实例的映射
        public static Dictionary<int, string> msgDic = new Dictionary<int, string>()
        {
            {HEARTBEAT, "Scripts.NetWork.HeartBeatMsg"},
            {LOGIN_REQUEST, "Scripts.NetWork.LoginReqMsg"},
            {LOGIN_RESPONSE, "Scripts.NetWork.LoginResMsg"},
            {REGISTER_REQUEST, "Scripts.NetWork.RegisterReqMsg"},
            {REGISTER_RESPONSE, "Scripts.NetWork.RegisterResMsg"},
            {GET_SETTING_REQUEST, "Scripts.NetWork.GetSettingReqMsg"},
            {GET_SETTING_RESPONSE, "Scripts.NetWork.GetSettingResMsg"},
            {GET_PLAYER_STATUS_REQUEST, "Scripts.NetWork.GetPlayerStatusReqMsg"},
            {GET_PLAYER_STATUS_RESPONSE, "Scripts.NetWork.GetPlayerStatusResMsg"},
            {EVENT_REQUEST, "Scripts.NetWork.EventReqMsg"},
            {EVENT_RESPONSE, "Scripts.NetWork.EventResMsg"},
        };

        // 心跳
        public const int HEARTBEAT = 0;
        // 登陆请求
        public const int LOGIN_REQUEST = 1;
        // 登陆响应
        public const int LOGIN_RESPONSE = 2;
        // 注册请求
        public const int REGISTER_REQUEST = 3;
        // 注册响应
        public const int REGISTER_RESPONSE = 4;
        // 获取设置参数请求
        public const int GET_SETTING_REQUEST = 5;
        // 获取设置参数响应
        public const int GET_SETTING_RESPONSE = 6;
        // 获取玩家状态请求
        public const int GET_PLAYER_STATUS_REQUEST = 7;
        // 获取玩家状态响应
        public const int GET_PLAYER_STATUS_RESPONSE = 8;
        // 玩家事件请求
        public const int EVENT_REQUEST = 9;
        // 玩家事件请求响应
        public const int EVENT_RESPONSE = 10;
    }
}
