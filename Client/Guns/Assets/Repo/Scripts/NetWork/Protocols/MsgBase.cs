using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    public abstract class MsgBase
    {
        public const int HEADER_LEN = 16;  // 协议头部长度

        public const int MAGIC_NUMBER = 0x12345678;  // 魔数，用来做报文校验

        public const int VERSION = 2;  // 协议版本

        public abstract int GetCommand();
    }
}
