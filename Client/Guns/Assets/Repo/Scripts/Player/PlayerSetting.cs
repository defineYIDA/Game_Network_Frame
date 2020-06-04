using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 玩家的初始参数
/// </summary>
public class PlayerSetting
{
    // 从服务端获取的玩家参数
    public int hp;  // 玩家血量
    public int speed;  // 移动速度
    public int damage;  // 伤害值
    public int magazine_amout;  // 弹夹容量
    public int total_bullet;  // 子弹数
}
