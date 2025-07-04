using UnityEngine;
using TMPro; // 如果用TextMeshPro
using UnityEngine.UI; // 如果用UI Image

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager Instance;
    public TextMeshProUGUI subtitleText;
    public Image subtitleBg; // 如果需要背景图片

    void Awake()
    {
        Instance = this;
        subtitleText.text = "";
        subtitleBg.enabled = false; // 初始化时隐藏背景图片
    }

    public void ShowSubtitle(string text)
    {
        subtitleText.text = text;
        subtitleBg.enabled = true; // 显示背景图片
    }

    public void HideSubtitle()
    {
        subtitleText.text = "";
        subtitleBg.enabled = false; // 隐藏背景图片，而不是将其置为 null
    }
}
