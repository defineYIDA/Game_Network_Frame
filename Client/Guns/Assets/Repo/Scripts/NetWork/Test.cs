using UnityEngine;
using System.Collections;
using Scripts.NetWork;

public class Test : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        // 注册
        Dispatcher.RegisterHandler(1, OnLoginMsgResponse);
        //TestCodec();
        TestSend();
    }

    /// <summary>
    /// 对协议的编解码测试
    /// </summary>
    private void TestCodec()
    {
        LoginReqMsg msg = new LoginReqMsg("123", "pwd");
        ByteBuffer buff = new ByteBuffer(Codec.Encode(msg));

        LoginReqMsg login = (LoginReqMsg)Codec.Decode(buff);

        Debug.Log(login.GetCommand());

        Debug.Log(login.id);

        Debug.Log(login.pwd);
    }

    private void TestSend()
    {
        LoginReqMsg msg = new LoginReqMsg("666", "pwd");
        ClientSocket.Instance().Send(msg);
    }
    void OnLoginMsgResponse(MsgBase msg)
    {
        LoginReqMsg loginMsg = (LoginReqMsg)msg;
        Debug.Log(msg.GetCommand().ToString() + "/" + loginMsg.id + "/" + loginMsg.pwd);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
