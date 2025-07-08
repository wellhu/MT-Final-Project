using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UnityClient : MonoBehaviour
{
    public struct UserInfo
    {
        public string Name; // 名字
        public string Prompt; // 从服务器获取的提示信息
    }

    public string serverIP = "127.0.0.1";
    public int serverPort = 25001;

    private Info infoInstance;
    public UserInfo MyUserInfo;

    TcpClient client;
    NetworkStream stream;

    void Start()
    {
        // 订阅 Info.cs 中的事件
        Info.OnUsernameSubmitted += UpdateUsername;
        ConnectToServer();
    }

    void UpdateUsername(string username)
    {
        // 更新用户名
        MyUserInfo.Name = username;
        Debug.Log("用户名已更新为: " + MyUserInfo.Name);

        // 用户名更新后自动发送到服务器
        SendMessage(MyUserInfo);
    }

    void ConnectToServer()
    {
        client = new TcpClient(serverIP, serverPort);
        stream = client.GetStream();
        Debug.Log("成功连接到服务器");
    }

    void SendMessage(UserInfo userInfo)
    {
        // 将 UserInfo 实例转换为 JSON 格式
        string json = JsonUtility.ToJson(userInfo);
        byte[] data = Encoding.UTF8.GetBytes(json);
        stream.Write(data, 0, data.Length);
        Debug.Log("用户名已发送到服务器: " + userInfo.Name);
    }

    void Update()
    {
        // 每帧检查是否有数据可读
        if (stream.DataAvailable)
        {
            ReceiveMessage();
        }
    }

    void ReceiveMessage()
    {
        byte[] responseData = new byte[1024];
        int bytesRead = stream.Read(responseData, 0, responseData.Length);
        string response = Encoding.UTF8.GetString(responseData, 0, bytesRead);
        DecodeJSON(response);
    }

    // 定义一个事件，用于通知其他脚本数据已接收
    public delegate void OnMessageReceived(UserInfo userInfo);
    public static event OnMessageReceived MessageReceived;

    public void DecodeJSON(string json)
    {
        // 使用 JsonUtility.FromJson<T> 解码 JSON 数据
        UserInfo userInfo = JsonUtility.FromJson<UserInfo>(json);
        Debug.Log("名字：" + userInfo.Name);
        Debug.Log("从服务器获取的提示信息：" + userInfo.Prompt);

        // 触发事件，通知其他脚本数据已接收
        MessageReceived?.Invoke(userInfo);
    }

    void OnDestroy()
    {
        // 取消订阅事件
        Info.OnUsernameSubmitted -= UpdateUsername;
        stream.Close();
        client.Close();
    }
}