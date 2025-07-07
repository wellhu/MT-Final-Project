using UnityEngine;

public class TestWavPlay : MonoBehaviour
{
    public AudioSource audioSource;
    void Start()
    {
        var clip = WavUtility.ToAudioClip(System.IO.Path.Combine(Application.persistentDataPath, "tts.wav"));
        audioSource.clip = clip;
        audioSource.Play();
    }
}