using UnityEngine;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using System.Threading.Tasks;
using System.Net;
using System.Collections;

public class AzureTTSPlayer : MonoBehaviour
{
    public string azureKey = "9WaruVVhVpH12JDGHgocahqzxBW9NJm0UmBhExyZk55DgOCfN79SJQQJ99BFACHYHv6XJ3w3AAAAACOGutex";
    public string azureRegion = "eastus2";
    private AudioSource audioSource;
    public AzureToDeepSeek azureToDeepSeek; // 在 Inspector 里拖拽

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    public async void Speak(string text)
    {
        var config = SpeechConfig.FromSubscription(azureKey, azureRegion);
        config.SpeechSynthesisLanguage = "en-US"; // 设置为英文
        config.SpeechSynthesisVoiceName = "en-US-JennyNeural"; // 选择一个英文女声（或其他你喜欢的英文声音）

        // 暂停识别（确保异步完成）
        await azureToDeepSeek.PauseRecognition();
        SubtitleManager.Instance.ShowSubtitle(text);

        using (var synthesizer = new SpeechSynthesizer(config, null))
        {
            var result = await synthesizer.SpeakTextAsync(text);
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                var audioData = result.AudioData;
                var clip = WavUtility.ToAudioClip(audioData, 0, "AzureTTSClip");
                audioSource.clip = clip;
                audioSource.Play();
                StartCoroutine(HideSubtitleAfterPlay());
            }
            else
            {
                Debug.LogError("TTS报错: " + result.Reason);
                SubtitleManager.Instance.HideSubtitle();
                await azureToDeepSeek.ResumeRecognition();
            }
        }
    }

    private IEnumerator HideSubtitleAfterPlay()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        SubtitleManager.Instance.HideSubtitle();
        // 恢复识别（确保异步完成）
        var task = azureToDeepSeek.ResumeRecognition();
        while (!task.IsCompleted) yield return null;
    }
}