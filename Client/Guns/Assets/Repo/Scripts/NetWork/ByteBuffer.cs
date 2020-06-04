using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class ByteBuffer
    {
        // 缓冲区默认大小
        const int DEFAULT_SIZE = 1024;

        // 初始大小
        int initSize = 0;

        // 缓冲区
        public byte[] bytes;

        // 读写指针
        public int readIdx = 0;
        public int writeIdx = 0;

        // 容量
        private int capacity = 0;

        // 剩余空间
        public int remain { get { return capacity - writeIdx; } }

        // 数据长度
        public int length { get { return writeIdx - readIdx; } }

        /// <summary>
        /// 初始化Buffer
        /// </summary>
        /// <param name="size">缓冲区大小</param>
        public ByteBuffer(int size = DEFAULT_SIZE)
        {
            // 初始化字节数组
            bytes = new byte[size];
            capacity = size;
            initSize = size;

            // 初始化读写指针
            readIdx = 0;
            writeIdx = 0;
        }

        /// <summary>
        /// 字节数组的方式初始化
        /// </summary>
        /// <param name="oriBytes">给定的字节数组</param>
        public ByteBuffer(byte[] oriBytes)
        {
            // 初始化字节数组
            bytes = oriBytes;
            capacity = oriBytes.Length;
            initSize = oriBytes.Length;

            // 初始化读写指针
            readIdx = 0;
            writeIdx = oriBytes.Length;
        }

        /// <summary>
        /// 扩容
        /// </summary>
        /// <param name="size"></param>
        public void Resize(int size)
        {
            if (size < length || size < initSize)
                return;

            // 扩容到2的倍数
            int n = 1;
            while (n < size)
                n *= 2;

            capacity = n;
            byte[] newBytes = new byte[capacity];
            Array.Copy(bytes, readIdx, newBytes, 0, length);
            bytes = newBytes;
            readIdx = 0;
            writeIdx = length;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="sourceBytes"></param>
        /// <param name="offset"></param>
        /// <param name="count">写入长度</param>
        /// <returns>写入的字节长度</returns>
        public int Write(byte[] sourceBytes, int offset, int count)
        {
            // 判断是否需要扩容
            if (remain < count)
            {
                Resize(length + count);
            }
            Array.Copy(sourceBytes, offset, bytes, writeIdx, count);
            writeIdx += count;
            return count;
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="outBytes"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns>读取到的字节长度</returns>
        public int Read(byte[] outBytes, int offset, int count)
        {
            // 读取长度受buffer的未读数据长度限定
            count = Math.Min(count, length);

            Array.Copy(bytes, writeIdx, outBytes, offset, count);

            readIdx += count;

            return count;
        }

        /// <summary>
        /// 清理已读数据
        /// </summary>
        public void MoveBytes()
        {
            if (length > 0)
            {
                Array.Copy(bytes, readIdx, bytes, 0, length);
            }
            writeIdx = length;
            readIdx = 0;
        }

        public override string ToString()
        {
            return string.Format("readIdx({0}) writeIdx({1}) bytes({2})", readIdx, writeIdx, BitConverter.ToString(bytes, 0, capacity));
        }
    }
}
