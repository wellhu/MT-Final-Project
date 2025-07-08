using UnityEngine;
using TMPro; // 引入 TextMeshPro 命名空间
using System.Text.RegularExpressions; // 引入正则表达式命名空间
using System.Text;

public class MessageDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // 指定 TextMeshPro 组件

    void OnEnable()
    {
        // 订阅 UnityClient 的事件
        UnityClient.MessageReceived += DisplayMessage;
    }

    void OnDisable()
    {
        // 取消订阅事件
        UnityClient.MessageReceived -= DisplayMessage;
    }

    void DisplayMessage(UnityClient.UserInfo userInfo)
    {
        // 提取以数字序号开头的内容
        string extractedContent = ExtractNumberedContent(userInfo.Prompt);

        // 去掉内容中的 *
        extractedContent = extractedContent.Replace("*", "");

        // 将提取到的内容写入 TextMeshPro 组件
        textMeshPro.text = extractedContent;
    }

    // 使用正则表达式提取以数字序号开头的内容
    string ExtractNumberedContent(string input)
    {
        // 正则表达式匹配以数字序号开头的内容，例如 "1. 内容" 或 "2. 内容"
        string pattern = @"\d+\..*?(?=\n\d+\.|$)";
        MatchCollection matches = Regex.Matches(input, pattern, RegexOptions.Singleline);

        // 将匹配到的内容拼接成一个字符串
        StringBuilder result = new StringBuilder();
        foreach (Match match in matches)
        {
            result.AppendLine(match.Value);
        }

        return result.ToString();
    }
}