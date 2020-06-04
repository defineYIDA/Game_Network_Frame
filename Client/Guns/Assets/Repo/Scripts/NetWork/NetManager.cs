using UnityEngine;
using System.Collections;
using Scripts.NetWork;
using System;

public class NetManager : MonoBehaviour
{
    // 客户端socket
    static ClientSocket clientSocket = ClientSocket.Instance();

    // 每一次Update处理的消息量
    readonly static int MAX_MESSAGE_FIRE = 10;

    // 是否开启心跳
    bool isHeartBeat = false;

    // 心跳间隔
    float heartBeatInterval = 5;

    // 上一次发送心跳的时间
    float lastSendHeartBeatTime = 0;

    // 上一次接收到心跳的时间
    float lastRecvHeartBeatTime = 0;

    void Awake()
    {
        // 连接server
        clientSocket.Connect();
        isHeartBeat = true;
    }

    // Use this for initialization
    void Start()
    {

        // add event handler
        
        // 服务端心跳
        Dispatcher.RegisterHandler(Command.HEARTBEAT, OnServerHeartBeat);

    }

    void OnServerHeartBeat(MsgBase msg)
    {
        lastRecvHeartBeatTime = Time.time;
        Debug.Log("Server HeartBeat");
    }


    // Update is called once per frame
    void Update()
    {
        MsgUpdate();
        HeartBeatUpdate();
    }

    public void Send(MsgBase msg)
    {
        clientSocket.Send(msg);
    }

    /// <summary>
    /// 断开与服务端连接
    /// </summary>
    public void Close()
    {
        // 关闭客户端心跳
        isHeartBeat = false;
        // 关闭客户端连接
        clientSocket.Close();
    }

    /// <summary>
    /// 处理消息
    /// </summary>
    private void MsgUpdate()
    {
        if (clientSocket.msgCount == 0)
        {
            // 没有要处理的消息
            return;
        }

        // 处理消息
        for (int i =0; i < MAX_MESSAGE_FIRE; i++)
        {
            MsgBase msg = null;
            lock (clientSocket.msgList)
            {
                if (clientSocket.msgList.Count > 0)
                {
                    msg = clientSocket.msgList[0];
                    clientSocket.msgList.RemoveAt(0);
                    clientSocket.msgCount--;
                }
            }
            // 消息分发
            if (msg != null)
            {
                Dispatcher.HandlerEvent(msg.GetCommand(), msg);
            }
            else
            {
                break;
            }
        }
    }

    /// <summary>
    /// 客户端心跳
    /// </summary>
    private void HeartBeatUpdate()
    {
        if (!isHeartBeat || !clientSocket.Connected())
        {
            return;
        }

        if (Time.time - lastSendHeartBeatTime > heartBeatInterval)
        {
            // 发送心跳包
            clientSocket.Send(new HeartBeatMsg());
            lastSendHeartBeatTime = Time.time;
            Debug.Log("Client HeartBeat");
        }

        if (Time.time - lastRecvHeartBeatTime > heartBeatInterval * 3)
        {
            Debug.Log("失去server连接！");
            clientSocket.Close();
        }
    }
}
