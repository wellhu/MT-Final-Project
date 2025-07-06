using UnityEngine;
using TMPro;

public class HideListToggle : MonoBehaviour
{
    public GameObject listObject;
    public TextMeshProUGUI buttonText;

    private bool isListVisible = true;

    void Start()
    {
        isListVisible = false;
        listObject.SetActive(false);
        buttonText.text = "Show List";
    }

    public void ToggleList()
    {
        isListVisible = !isListVisible;
        listObject.SetActive(isListVisible);
        buttonText.text = isListVisible ? "Hide List" : "Show List";
    }
}