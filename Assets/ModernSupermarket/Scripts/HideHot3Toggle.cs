using UnityEngine;
using TMPro;

public class HideHot3Toggle : MonoBehaviour
{
    public GameObject listObject;
    public TextMeshProUGUI buttonText;

    private bool isListVisible = true;

    void Start()
    {
        isListVisible = false;
        listObject.SetActive(false);
        buttonText.text = "Hot 3";
    }

    public void ToggleList()
    {
        isListVisible = !isListVisible;
        listObject.SetActive(isListVisible);
        buttonText.text = isListVisible ? "Close Hot 3" : "Hot 3";
    }
}