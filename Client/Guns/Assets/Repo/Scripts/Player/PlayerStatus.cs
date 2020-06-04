using UnityEngine;
using Scripts.NetWork;

/// <summary>
/// 玩家状态，在登陆时初始化，存放从服务端获取到的玩家状态
/// </summary>
class PlayerStatus
{

    public static string gamekey;  // 对局key

    public static string id;

    public static int hp;  // 玩家血量

    public static int bulletAmount;  // 当前弹夹子弹数

    public static int totalBullet;  // 所有子弹数

    //public static Vector3 position;  // 玩家位置


    /// <summary>
    /// 同步玩家状态
    /// </summary>
    public static void SyncPlayerStatus()
    {
        ClientSocket.Instance().Send(new GetPlayerStatusReqMsg(gamekey, id));
    }

    /// <summary>
    /// 设置玩家状态
    /// </summary>
    /// <param name="resMsg"></param>
    public static void SetPlsyerStatus(GetPlayerStatusResMsg resMsg)
    {
        hp = resMsg.hp;
        bulletAmount = resMsg.bullet_amount;
        totalBullet = resMsg.total_bullet;
    }

    public static string String()
    {
        return "{playerstatus key: " + gamekey + ", id: " + id + ", hp: " + hp + ", bulletAmount: " + bulletAmount + ", totalBullet: " + totalBullet + "}"; ;
    }
}
