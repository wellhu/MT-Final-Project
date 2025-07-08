using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Info : MonoBehaviour
{
    public TMP_InputField usernameInputField; // 拖入TMP_InputField组件
    public Button submitButton; // 拖入提交按钮组件
    public TextMeshProUGUI resultText; // 用于显示结果的TextMeshProUGUI组件

    private string userName;

    // 定义一个事件，当用户名被提交时触发
    public delegate void UsernameSubmitted(string username);
    public static event UsernameSubmitted OnUsernameSubmitted;

    // Start is called before the first frame update
    void Start()
    {
        submitButton.onClick.AddListener(SubmitUsername);
    }

    // Update is called once per frame
    public void SubmitUsername()
    {
        // 获取输入框中的内容
        userName = usernameInputField.text;
        // 显示结果
        resultText.text = "Hello, " + userName + "!";
        // 触发事件
        OnUsernameSubmitted?.Invoke(userName);
    }

    public string GetUsername()
    {
        return userName;
    }
}