using System;
using System.Net.Sockets;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.NetWork
{
    class ClientSocket
    {
        // 服务端ip
        string host = "127.0.0.1";

        // 服务端口
        int port = 8080;

        // 客户端socket
        Socket clientSocket;

        // 接收缓冲区
        ByteBuffer readBuffer;

        // 发送队列
        Queue<ByteBuffer> writeQueue = new Queue<ByteBuffer>();

        // 待处理消息列表
        public List<MsgBase> msgList = new List<MsgBase>();

        public int msgCount = 0;

        // 连接超时
        float connTimeLimit = 5;


        // 是否正在连接
        bool isConnecting = false;

        // 是否正在关闭
        bool isClosing = false;

        // 单例
        private static ClientSocket m_Instance;

        /// <summary>
        /// 直接静态构造饥汉式生成单例
        /// </summary>
        static ClientSocket()
        {
            m_Instance = new ClientSocket("127.0.0.1", 8080);
        }

        public static ClientSocket Instance()
        {
            return m_Instance;
        }

        public ClientSocket()
        {
            InitSocket();
        }

        public ClientSocket(string host, int port)
        {
            this.host = host;

            this.port = port;

            InitSocket();
        }

        void InitSocket()
        {
            clientSocket = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            clientSocket.NoDelay = true;  // 不使用Nagle算法，保证实时性

            readBuffer = new ByteBuffer();

            isConnecting = false;

            isClosing = false;
        }

        /// <summary>
        /// 连接到Server
        /// </summary>
        public void Connect()
        {
            if (clientSocket.Connected)
            {
                Debug.Log("Connect fail, already connected!");
                return;
            }
            if (isConnecting)
            {
                Debug.Log("Connect fail, is connecting!");
                return;
            }

            isConnecting = true;
            // 连接Server
            clientSocket.BeginConnect(host, port, ConnectCallback, clientSocket);
        }

        /// <summary>
        /// connect函数回调
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                socket.EndConnect(ar);
                isConnecting = false;
                Debug.Log("Connect success:" + host + " " + port);

                // 开始接收数据
                clientSocket.BeginReceive(readBuffer.bytes, readBuffer.writeIdx, readBuffer.remain, 0, ReceiveCallback, clientSocket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Connect fail, " + ex.ToString());
                isConnecting = false;
            }

        }

        /// <summary>
        /// recv 回调
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket)ar.AsyncState;
                // 接收数据长度
                int count = socket.EndReceive(ar);

                if (count == 0)
                {
                    Close();
                    return;
                }

                readBuffer.writeIdx += count;

                // 处理接收的数据
                OnRecvData();

                if (readBuffer.remain < MsgBase.HEADER_LEN)
                {
                    readBuffer.Resize(readBuffer.length * 2);
                }
                clientSocket.BeginReceive(readBuffer.bytes, readBuffer.writeIdx, readBuffer.remain, 0, ReceiveCallback, clientSocket);
            }
            catch (SocketException ex)
            {
                Debug.Log("Socket Receive fail " + ex.ToString());
            }
        }

        /// <summary>
        /// 处理接收到的数据
        /// </summary>
        private void OnRecvData()
        {
            try
            {
                MsgBase msg = Codec.Decode(readBuffer);
                // 判断协议是否接收完全
                if (msg == null)
                {
                    return;
                }

                // 添加到消息队列
                lock (msgList)
                {
                    msgList.Add(msg);
                }
                msgCount++;

                // 整理buff，移除已处理数据
                if (readBuffer.length < MsgBase.HEADER_LEN)
                {
                    readBuffer.MoveBytes();
                }
                else
                {
                    OnRecvData();
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Decode fail " + ex.ToString());
            }
        }

        /// <summary>
        /// 关闭Client
        /// </summary>
        public void Close()
        {
            if (clientSocket == null || !clientSocket.Connected)
            {
                return;
            }
            if (isConnecting)
            {
                return;
            }

            if (writeQueue.Count > 0)
            {
                isClosing = true;
            }
            else
            {
                clientSocket.Close();
                Debug.Log("Socket colse !");
            }
        }

        /// <summary>
        /// 发送协议报文
        /// </summary>
        /// <param name="msg"></param>
        public void Send(MsgBase msg)
        {
            // 判断当前状态
            if (clientSocket == null || isClosing)
            {
                Debug.Log("Socket is not ready !");
                return;
            }
            if (!clientSocket.Connected && !isConnecting)
            {
                Debug.Log("Socket is not ready !");
                return;
            }
            float t = Time.time;
            while (isConnecting)
            {
                if (Time.time - t > connTimeLimit)
                {
                    Debug.Log("Connection timed out !");
                    return;
                }
            }
            // 添加消息到队列
            ByteBuffer buff = new ByteBuffer(Codec.Encode(msg));
            lock (writeQueue)
            {
                writeQueue.Enqueue(buff);
            }

            if (writeQueue.Count != 0)
            {
                clientSocket.BeginSend(buff.bytes, buff.readIdx, buff.length, 0, SendCallback, clientSocket);
            }
        }

        /// <summary>
        /// send 回调
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            // 获取state、Endstate
            Socket socket = (Socket)ar.AsyncState;
            // 状态判断
            if (clientSocket == null || !clientSocket.Connected)
            {
                return;
            }
            int count = clientSocket.EndSend(ar);

            // 获取队列第一条数据
            ByteBuffer buff;
            lock (writeQueue)
            {
                buff = writeQueue.First();
            }

            // 判断是否发送完全
            buff.readIdx += count;
            if (buff.length == 0)
            {
                lock (writeQueue)
                {
                    writeQueue.Dequeue();
                    buff = writeQueue.First();
                }
            }

            // 继续发送
            if (buff != null)
            {
                clientSocket.BeginSend(buff.bytes, buff.readIdx, buff.length, 0, SendCallback, clientSocket);
            }
            // 这种关闭
            else if (isClosing)
            {
                clientSocket.Close();
            }

        }
        /// <summary>
        /// 返回当前客户端的连接状态
        /// </summary>
        /// <returns></returns>
        public bool Connected()
        {
            return clientSocket.Connected;
        }
    }
}
