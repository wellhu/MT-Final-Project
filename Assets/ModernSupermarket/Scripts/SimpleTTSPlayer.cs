using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class SimpleTTSPlayer : MonoBehaviour
{
    [Header("UI")]
    public Button recordButton;

    [Header("Azure TTS")]
    public string azureKey = "你的AzureKey";
    public string azureRegion = "你的AzureRegion";
    [TextArea]
    public string replyText = "Hello, this is my answer!";

    [Header("Avatar")]
    public ReadyPlayerMe.Core.VoiceHandler voiceHandler;

    private bool isRecording = false;
    private Microsoft.CognitiveServices.Speech.SpeechRecognizer recognizer;

    void Start()
    {
        recordButton.onClick.AddListener(OnRecordButtonClicked);
    }

    async void OnRecordButtonClicked()
    {
        if (!isRecording)
        {
            isRecording = true;
            await StartRecognition();
        }
    }

    async Task StartRecognition()
    {
        var config = Microsoft.CognitiveServices.Speech.SpeechConfig.FromSubscription(azureKey, azureRegion);
        config.SpeechRecognitionLanguage = "en-US";
        recognizer = new Microsoft.CognitiveServices.Speech.SpeechRecognizer(config);

        Debug.Log("开始语音识别，请说话...");
        var result = await recognizer.RecognizeOnceAsync();
        Debug.Log("识别结果: " + result.Text);

        recognizer.Dispose();
        recognizer = null;
        isRecording = false;

        // 这里不用 result.Text，直接用 replyText 进行TTS
        await PlayTTSAndLipSync(replyText);
    }

    async Task PlayTTSAndLipSync(string text)
    {
        // 1. 用 Azure TTS 合成语音，直接获取 AudioClip
        var audioClip = await AzureTTSUtility.TextToSpeechToClipAsync(text, azureKey, azureRegion);
        Debug.Log(audioClip == null ? "AudioClip 加载失败" : "AudioClip 加载成功: " + audioClip.name);

        // 2. 赋值给 VoiceHandler 并播放
        voiceHandler.AudioClip = audioClip;
        voiceHandler.PlayCurrentAudioClip();
    }
}