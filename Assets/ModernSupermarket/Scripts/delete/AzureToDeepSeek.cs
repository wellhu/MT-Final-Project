using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using System.Threading.Tasks;

public class AzureToDeepSeek : MonoBehaviour
{
    private SpeechRecognizer recognizer;
    private string azureKey = "9WaruVVhVpH12JDGHgocahqzxBW9NJm0UmBhExyZk55DgOCfN79SJQQJ99BFACHYHv6XJ3w3AAAAACOGutex";
    private string azureRegion = "eastus2";

    [Header("拖拽 DeepSeekClient 脚本所在的 GameObject 到这里")]
    public DeepSeekClient deepSeekClient;

    public bool IsRecognizing { get; private set; } = false;

    async void Start()
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

        var config = SpeechConfig.FromSubscription(azureKey, azureRegion);
        config.SpeechRecognitionLanguage = "en-US";
        recognizer = new SpeechRecognizer(config);

        recognizer.Recognized += async (s, e) =>
        {
            if (e.Result.Reason == ResultReason.RecognizedSpeech)
            {
                string resultText = e.Result.Text;
                Debug.Log("Azure识别到: " + resultText);

                // 立刻暂停识别
                await PauseRecognition();

                MainThreadDispatcher.Enqueue(() =>
                {
                    deepSeekClient.GetResponseFromDeepSeek(resultText); 
                });
            }
        };

        await ResumeRecognition();
    }

    private async void OnDestroy()
    {
        if (recognizer != null)
        {
            await recognizer.StopContinuousRecognitionAsync();
            recognizer.Dispose();
            recognizer = null;
        }
    }

    public async Task PauseRecognition()
    {
        if (recognizer != null && IsRecognizing)
        {
            Debug.Log("暂停识别");
            await recognizer.StopContinuousRecognitionAsync();
            IsRecognizing = false;
        }
    }

    public async Task ResumeRecognition()
    {
        if (recognizer != null && !IsRecognizing)
        {
            Debug.Log("恢复识别");
            await recognizer.StartContinuousRecognitionAsync();
            IsRecognizing = true;
        }
    }
}
