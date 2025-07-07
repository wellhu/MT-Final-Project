using System.Threading.Tasks;
using Microsoft.CognitiveServices.Speech;
using System.IO;

public static class AzureTTSUtility
{
    // 新方法：直接返回 AudioClip
    public static async Task<UnityEngine.AudioClip> TextToSpeechToClipAsync(string text, string azureKey, string azureRegion)
    {
        var config = SpeechConfig.FromSubscription(azureKey, azureRegion);
        config.SpeechSynthesisLanguage = "en-US";
        config.SpeechSynthesisVoiceName = "en-US-JennyNeural";

        using (var synthesizer = new SpeechSynthesizer(config, null))
        {
            var result = await synthesizer.SpeakTextAsync(text);
            UnityEngine.Debug.Log($"TTS Result Reason: {result.Reason}");
            UnityEngine.Debug.Log($"TTS Audio Data Length: {result.AudioData?.Length ?? 0}");
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                UnityEngine.Debug.Log("TTS 合成成功, AudioData 长度: " + result.AudioData.Length);
                var audioClip = WavUtility.ToAudioClip(result.AudioData, 0, "AzureTTSClip");
                return audioClip;
            }
            else
            {
                UnityEngine.Debug.LogError("TTS 合成失败: " + result.Reason);
                return null;
            }
        }
    }
}