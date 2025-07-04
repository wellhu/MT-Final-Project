using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json;
using System.Text;

public class DeepSeekClient : MonoBehaviour
{
    [Header("DeepSeek API Key")]
    public string apiKey = "sk-95e8a5792da54fc8b12d9ded384267db";

    private string apiUrl = "https://api.deepseek.com/v1/chat/completions";
    public AzureTTSPlayer azureTTSPlayer;

    public void GetResponseFromDeepSeek(string message)
    {
        StartCoroutine(SendRequest(message));
    }

    private IEnumerator SendRequest(string message)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        var payload = new
        {
            model = "deepseek-chat",
            messages = new[]
            {
                new { role = "user", content = message }
            }
        };
        string jsonBody = JsonConvert.SerializeObject(payload);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        var req = new UnityWebRequest(apiUrl, "POST")
        {
            uploadHandler = new UploadHandlerRaw(bodyRaw),
            downloadHandler = new DownloadHandlerBuffer()
        };
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apiKey.Trim());
        req.timeout = 100;

        Debug.Log("[DeepSeek] 请求开始: " + jsonBody);
        yield return req.SendWebRequest();

        Debug.Log("[DeepSeek] 响应码: " + req.responseCode);
        Debug.Log("[DeepSeek] 响应结果: " + req.downloadHandler.text);

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("[DeepSeek] 错误: " + req.error);
        }
        else
        {
            // 解析 DeepSeek 返回内容
            var response = JsonConvert.DeserializeObject<DeepSeekResponse>(req.downloadHandler.text);
            string reply = response.choices[0].message.content;

            // 播放语音
            MainThreadDispatcher.Enqueue(() => {
                azureTTSPlayer.Speak(reply);
            });
            Debug.Log("[DeepSeek] 成功回复");
        }
        req.Dispose();
    }
}
