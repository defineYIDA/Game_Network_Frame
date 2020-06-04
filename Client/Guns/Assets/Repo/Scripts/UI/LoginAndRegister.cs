using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Scripts.NetWork;
using System.Collections;

public class LoginAndRegister : MonoBehaviour
{
    public GameObject loginPlane;
    public GameObject registerPlane;
    public GameObject freePlane;
    public InputField userName;
    public InputField userPass;
    public InputField registerName;
    public InputField registerPass;
    public InputField registerConfirmPass;

    public Text tip;

    private void Awake()
    {
        // 注册消息处理方法
        Dispatcher.RegisterHandler(Command.LOGIN_RESPONSE, OnLoginMsgResponse);
        Dispatcher.RegisterHandler(Command.REGISTER_RESPONSE, OnRegisterMsgResponse);

        loginPlane.SetActive(true);
        registerPlane.SetActive(false);
        freePlane.SetActive(false);
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (!ClientSocket.Instance().Connected())
        {
            tip.text = "连接服务器失败！";
        }
    }

    #region Click 事件

    public void OnClickLogin()
    {
        freePlane.SetActive(true);
        tip.text = "";
        if ((userName.text == "") || (userPass.text == ""))
        {
            tip.text = "请输入正确用户名或密码!";
        }
        LoginReqMsg reqMsg = new LoginReqMsg(userName.text, userPass.text);
        // 发送登陆消息
        ClientSocket.Instance().Send(reqMsg);
        Debug.Log("Send: " + reqMsg.ToString());
        freePlane.SetActive(false);
    }

    public void OnClickRegister()
    {
        freePlane.SetActive(true);
        loginPlane.SetActive(false);
        registerPlane.SetActive(true);
        freePlane.SetActive(false);
        tip.text = "";
    }

    public void OnClickRegisterSubmit()
    {
        freePlane.SetActive(true);
        tip.text = "";
        string id = registerName.text;
        string pwd = registerPass.text;
        if (id == "")
        {
            tip.text = "请输入用户名";
            freePlane.SetActive(false);
            return;
        }
        if (pwd != registerConfirmPass.text)
        {
            tip.text = "请输入确保两次输入的密码一致";
            freePlane.SetActive(false);
            return;
        }
        RegisterReqMsg reqMsg = new RegisterReqMsg(id, pwd);
        // 发送注册消息
        ClientSocket.Instance().Send(reqMsg);
        Debug.Log("Send: " + reqMsg.ToString());

    }
    public void OnReToLogin()
    {
        freePlane.SetActive(true);
        loginPlane.SetActive(true);
        registerPlane.SetActive(false);
        freePlane.SetActive(false);
        tip.text = "";
    }

    #endregion


    #region 响应消息的处理事件
    /// <summary>
    /// 登陆消息响应
    /// </summary>
    /// <param name="message"></param>
    private void OnLoginMsgResponse(MsgBase msg)
    {
        LoginResMsg resMsg = (LoginResMsg)msg;
        Debug.Log("Recv: " + resMsg.ToString());
        if (resMsg.status == "true")
        {
            tip.text = "登录成功";
            // SceneManager.LoadScene("Repo/Scene/MainScene");
            // 设置玩家id
            PlayerStatus.id = resMsg.id;
            // 生成对局key
            PlayerStatus.gamekey = resMsg.msg;
        }
        else
        {
            tip.text = "登录失败: " + resMsg.msg;
            print("登录失败: " + resMsg.msg);
            freePlane.SetActive(false);
        }

    }

    /// <summary>
    /// 注册消息响应
    /// </summary>
    /// <param name="message"></param>
    private void OnRegisterMsgResponse(MsgBase msg)
    {
        RegisterResMsg resMsg = (RegisterResMsg)msg;
        Debug.Log("Recv: " + resMsg.ToString());
        if (resMsg.status == "true")
        {
            tip.text = "注册成功";
            OnReToLogin();
        }
        else
        {
            tip.text = resMsg.msg;
            print("注册失败: " + resMsg.msg);
            freePlane.SetActive(false);
        }
    }
    #endregion

}
