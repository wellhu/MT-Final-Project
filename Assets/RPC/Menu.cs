using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间
using System.Collections; // 引入协程相关命名空间

public class CanvasDisappearing : MonoBehaviour
{
    public Button button; // 按钮引用
    public GameObject canvas; // Canvas 引用
    void Start()
    {
        // 给按钮添加点击事件
        button.onClick.AddListener(StartDisappearing);
    }

    void StartDisappearing()
    {
        // 启动协程
        StartCoroutine(DisappearAfterDelay());
    }

    IEnumerator DisappearAfterDelay()
    {
        // 等待 2 秒
        yield return new WaitForSeconds(2.0f);

        // 2 秒后让 Canvas 消失
        canvas.SetActive(false);
    }
}