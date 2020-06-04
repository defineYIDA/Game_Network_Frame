using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.NetWork
{
    class Codec
    {

        // 自定义 Msg 协议：
        // +-------+----------+-------+--------+------------------+
        // | 魔数   | 协议版本  |   指令 | 数据长度|     数据         |
        // +-------+----------+-------+--------+------------------+
        //  4byte     4byte    4byte    4byte      N byte
        //
        // 协议头部长度16字节
        // 协议的数据部分是由对象的json字符串的字节数组


        /// <summary>
        /// 协议编码
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static byte[] Encode(MsgBase msg)
        {
            // 处理协议头部
            byte[] header = new byte[MsgBase.HEADER_LEN];
            int index = 0;
            // 魔数，用来做报文校验
            IntToBytes(header, index, MsgBase.MAGIC_NUMBER);
            index += 4;

            // 协议版本
            IntToBytes(header, index, MsgBase.VERSION);
            index += 4;

            // 协议指令，用于事件分发
            IntToBytes(header, index, msg.GetCommand());
            index += 4;

            // 处理协议数据部分
            string s = JsonUtility.ToJson(msg);
            byte[] data = Encoding.UTF8.GetBytes(s);

            // 协议指令，用于事件分发
            IntToBytes(header, index, data.Length);

            // 拼接协议头部和数据部分

            byte[] bytes = new byte[header.Length + data.Length];
            Array.Copy(header, 0, bytes, 0, header.Length);
            Array.Copy(data, 0, bytes, header.Length, data.Length);

            return bytes;

        }

        /// <summary>
        /// 解码协议
        /// 缓冲区数据长度： 小于协议头部长度 || 数据部分长度小于数据长度
        ///                return null 继续接收
        /// </summary>
        /// <param name="buff">接收缓冲区</param>
        /// <returns></returns>
        public static MsgBase Decode(ByteBuffer buff)
        {
            if (buff.length < MsgBase.HEADER_LEN)
            {
                return null;
            }

            int readIdx = buff.readIdx;

            // 验证魔数
            if (BytesToInt(buff.bytes, readIdx) != MsgBase.MAGIC_NUMBER)
            {
                throw new Exception("协议不支持!");
            }
            readIdx += 4;

            // 验证协议版本
            if (BytesToInt(buff.bytes, readIdx) != MsgBase.VERSION)
            {
                throw new Exception("协议版本不支持!");
            }
            readIdx += 4;

            // 指令
            int command = BytesToInt(buff.bytes, readIdx);
            if (!Command.msgDic.ContainsKey(command))
            {
                throw new Exception("未知协议指令!");
            }
            readIdx += 4;

            int len = BytesToInt(buff.bytes, readIdx);

            if (buff.length - MsgBase.HEADER_LEN < len)
            {
                return null;
            }
            buff.readIdx += MsgBase.HEADER_LEN;

            string data = Encoding.UTF8.GetString(buff.bytes, buff.readIdx, len);
            buff.readIdx += len;

            return (MsgBase)JsonUtility.FromJson(data, Type.GetType(Command.msgDic[command]));
        }

        /// <summary>
        /// 将int存入byte[]
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <param name="num">存入的数字</param>
        public static void IntToBytes(byte[] bytes, int offset, int num)
        {
            if (bytes.Length - offset < 4)
            {
                Debug.Log("IntToBytes fail !");
                return;
            }
            bytes[offset] = (byte)((num >> 24) & 0xFF);
            bytes[offset + 1] = (byte)((num >> 16) & 0xFF);
            bytes[offset + 2] = (byte)((num >> 8) & 0xFF);
            bytes[offset + 3] = (byte)(num & 0xFF);
        }

        /// <summary>
        /// byte转int
        /// 使用大端字节序（网络字节序）
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static int BytesToInt(byte[] bytes, int offset)
        {
            if (bytes.Length - offset < 4)
            {
                Debug.Log("BytesToInt fail !");
                return 0;
            }
            int value;
            // 
            value = (int)((bytes[offset + 3] & 0xFF)
                | ((bytes[offset + 2] & 0xFF) << 8)
                | ((bytes[offset + 1] & 0xFF) << 16)
                | ((bytes[offset] & 0xFF) << 24));
            return value;
        }
    }
}
