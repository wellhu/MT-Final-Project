// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class AppleInspector : MonoBehaviour
// {
//     [Header("Apple Properties")]
//     public string appleType = "Red Delicious";
//     public float price = 2.99f;
    
//     [Header("UI References")]
//     public GameObject infoPanel;
//     public TextMeshProUGUI typeText;
//     public TextMeshProUGUI priceText;

//     private Camera mainCamera;
//     private Collider appleCollider;
//     private bool isPanelActive = false;

//     void Start()
//     {
//         mainCamera = Camera.main;
//         appleCollider = GetComponent<Collider>();
        
//         if (infoPanel == null)
//             CreateInfoPanel();
            
//         if (infoPanel != null)
//             infoPanel.SetActive(false);
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0) && !isPanelActive)
//         {
//             RaycastHit hit;
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            
//             if (Physics.Raycast(ray, out hit) && hit.collider == appleCollider)
//             {
//                 ShowAppleInfo();
//             }
//         }
//         else if (Input.GetMouseButtonDown(0) && isPanelActive)
//         {
//             HideAppleInfo();
//         }
//     }

//     void ShowAppleInfo()
//     {
//         if (infoPanel == null || typeText == null || priceText == null)
//         {
//             Debug.LogError("UI references not set, attempting to create automatically...");
//             if (infoPanel == null) CreateInfoPanel();
//             if (typeText == null || priceText == null) SetupTextComponents();
//         }

//         if (infoPanel != null && typeText != null && priceText != null)
//         {
//             typeText.text = $"Type: {appleType}";
//             priceText.text = $"Price: ${price:F2}/lb"; // 使用美元符号和lb(磅)
            
//             Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
//             screenPos.y += 100;
//             infoPanel.GetComponent<RectTransform>().position = screenPos;
            
//             infoPanel.SetActive(true);
//             isPanelActive = true;
//         }
//     }

//     void HideAppleInfo()
//     {
//         if (infoPanel != null)
//         {
//             infoPanel.SetActive(false);
//             isPanelActive = false;
//         }
//     }

//     void CreateInfoPanel()
//     {
//         GameObject canvasObj = GameObject.Find("AppleInfoCanvas");
//         if (canvasObj == null)
//         {
//             canvasObj = new GameObject("AppleInfoCanvas");
//             Canvas canvas = canvasObj.AddComponent<Canvas>();
//             canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//             canvasObj.AddComponent<CanvasScaler>();
//             canvasObj.AddComponent<GraphicRaycaster>();
//         }

//         infoPanel = new GameObject("AppleInfoPanel");
//         infoPanel.transform.SetParent(canvasObj.transform, false);
        
//         RectTransform panelRect = infoPanel.AddComponent<RectTransform>();
//         panelRect.sizeDelta = new Vector2(200, 100);
        
//         Image panelImage = infoPanel.AddComponent<Image>();
//         panelImage.color = new Color(0.9f, 0.9f, 0.9f, 0.9f);
        
//         GameObject border = new GameObject("Border");
//         border.transform.SetParent(infoPanel.transform, false);
//         RectTransform borderRect = border.AddComponent<RectTransform>();
//         borderRect.anchorMin = Vector2.zero;
//         borderRect.anchorMax = Vector2.one;
//         borderRect.offsetMin = Vector2.zero;
//         borderRect.offsetMax = Vector2.zero;
        
//         Image borderImage = border.AddComponent<Image>();
//         borderImage.color = Color.black;
        
//         SetupTextComponents();
//     }

//     void SetupTextComponents()
//     {
//         if (infoPanel == null) return;

//         GameObject typeTextObj = new GameObject("TypeText");
//         typeTextObj.transform.SetParent(infoPanel.transform, false);
        
//         RectTransform typeRect = typeTextObj.AddComponent<RectTransform>();
//         typeRect.anchorMin = new Vector2(0.1f, 0.6f);
//         typeRect.anchorMax = new Vector2(0.9f, 0.9f);
//         typeRect.offsetMin = Vector2.zero;
//         typeRect.offsetMax = Vector2.zero;
        
//         typeText = typeTextObj.AddComponent<TextMeshProUGUI>();
//         typeText.fontSize = 24;
//         typeText.alignment = TextAlignmentOptions.Center;
//         typeText.color = Color.white;
//         typeText.text = "Type: Red Delicious";

//         GameObject priceTextObj = new GameObject("PriceText");
//         priceTextObj.transform.SetParent(infoPanel.transform, false);
        
//         RectTransform priceRect = priceTextObj.AddComponent<RectTransform>();
//         priceRect.anchorMin = new Vector2(0.1f, 0.1f);
//         priceRect.anchorMax = new Vector2(0.9f, 0.4f);
//         priceRect.offsetMin = Vector2.zero;
//         priceRect.offsetMax = Vector2.zero;
        
//         priceText = priceTextObj.AddComponent<TextMeshProUGUI>();
//         priceText.fontSize = 24;
//         priceText.alignment = TextAlignmentOptions.Center;
//         priceText.color = Color.white;
//         priceText.text = "Price: $2.99/lb";
//     }
// }



// using UnityEngine;
// using TMPro;
// using UnityEngine.UI;

// public class AppleInspector : MonoBehaviour
// {
//     [Header("Item Info")]
//     public ItemInfo itemInfo;

//     [Header("UI References")]
//     public GameObject infoPanel;
//     public TextMeshProUGUI nameText;
//     public TextMeshProUGUI priceText;
//     public TextMeshProUGUI descText;
//     public Image itemImage;

//     private Camera mainCamera;
//     private Collider itemCollider;
//     private bool isPanelActive = false;

//     void Start()
//     {
//         mainCamera = Camera.main;
//         itemCollider = GetComponent<Collider>();
//         if (infoPanel == null)
//             CreateInfoPanel();
//         if (infoPanel != null)
//             infoPanel.SetActive(false);
//     }

//     void Update()
//     {
//         if (Input.GetMouseButtonDown(0) && !isPanelActive)
//         {
//             RaycastHit hit;
//             Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out hit) && hit.collider == itemCollider)
//             {
//                 ShowItemInfo();
//             }
//         }
//         else if (Input.GetMouseButtonDown(0) && isPanelActive)
//         {
//             // 点击面板外部才关闭
//             if (!RectTransformUtility.RectangleContainsScreenPoint(
//                 infoPanel.GetComponent<RectTransform>(),
//                 Input.mousePosition,
//                 mainCamera))
//             {
//                 HideItemInfo();
//             }
//         }
//     }

//     void ShowItemInfo()
//     {
//         if (infoPanel == null || nameText == null || priceText == null || descText == null || itemImage == null)
//         {
//             Debug.LogError("UI references not set, attempting to create automatically...");
//             if (infoPanel == null) CreateInfoPanel();
//             if (nameText == null || priceText == null || descText == null || itemImage == null) SetupTextComponents();
//         }
//         if (infoPanel != null && nameText != null && priceText != null && descText != null && itemImage != null)
//         {
//             nameText.text = $"Name: {itemInfo.itemName}";
//             priceText.text = $"Price: ${itemInfo.price:F2}/lb";
//             descText.text = $"Advantage: {itemInfo.description}";
//             itemImage.sprite = itemInfo.itemSprite;
//             itemImage.color = new Color(1, 1, 1, 0.8f); // 半透明

//             Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
//             screenPos.y += 100;
//             infoPanel.GetComponent<RectTransform>().position = screenPos;
//             infoPanel.SetActive(true);
//             isPanelActive = true;
//         }
//     }

//     void HideItemInfo()
//     {
//         if (infoPanel != null)
//         {
//             infoPanel.SetActive(false);
//             isPanelActive = false;
//         }
//     }

//     void CreateInfoPanel()
//     {
//         GameObject canvasObj = GameObject.Find("ItemInfoCanvas");
//         if (canvasObj == null)
//         {
//             canvasObj = new GameObject("ItemInfoCanvas");
//             Canvas canvas = canvasObj.AddComponent<Canvas>();
//             canvas.renderMode = RenderMode.ScreenSpaceOverlay;
//             canvasObj.AddComponent<CanvasScaler>();
//             canvasObj.AddComponent<GraphicRaycaster>();
//         }
//         infoPanel = new GameObject("ItemInfoPanel");
//         infoPanel.transform.SetParent(canvasObj.transform, false);
//         RectTransform panelRect = infoPanel.AddComponent<RectTransform>();
//         panelRect.sizeDelta = new Vector2(300, 220);
//         Image panelImage = infoPanel.AddComponent<Image>();
//         panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.7f); // 半透明深色背景

//         SetupTextComponents();
//     }

//     void SetupTextComponents()
//     {
//         if (infoPanel == null) return;

//         // 名称
//         GameObject nameTextObj = new GameObject("NameText");
//         nameTextObj.transform.SetParent(infoPanel.transform, false);
//         RectTransform nameRect = nameTextObj.AddComponent<RectTransform>();
//         nameRect.anchorMin = new Vector2(0.1f, 0.75f);
//         nameRect.anchorMax = new Vector2(0.9f, 0.95f);
//         nameRect.offsetMin = Vector2.zero;
//         nameRect.offsetMax = Vector2.zero;
//         nameText = nameTextObj.AddComponent<TextMeshProUGUI>();
//         nameText.fontSize = 24;
//         nameText.alignment = TextAlignmentOptions.Center;
//         nameText.color = Color.white;

//         // 价格
//         GameObject priceTextObj = new GameObject("PriceText");
//         priceTextObj.transform.SetParent(infoPanel.transform, false);
//         RectTransform priceRect = priceTextObj.AddComponent<RectTransform>();
//         priceRect.anchorMin = new Vector2(0.1f, 0.6f);
//         priceRect.anchorMax = new Vector2(0.9f, 0.75f);
//         priceRect.offsetMin = Vector2.zero;
//         priceRect.offsetMax = Vector2.zero;
//         priceText = priceTextObj.AddComponent<TextMeshProUGUI>();
//         priceText.fontSize = 20;
//         priceText.alignment = TextAlignmentOptions.Center;
//         priceText.color = Color.white;

//         // 描述
//         GameObject descTextObj = new GameObject("DescText");
//         descTextObj.transform.SetParent(infoPanel.transform, false);
//         RectTransform descRect = descTextObj.AddComponent<RectTransform>();
//         descRect.anchorMin = new Vector2(0.1f, 0.35f);
//         descRect.anchorMax = new Vector2(0.9f, 0.6f);
//         descRect.offsetMin = Vector2.zero;
//         descRect.offsetMax = Vector2.zero;
//         descText = descTextObj.AddComponent<TextMeshProUGUI>();
//         descText.fontSize = 18;
//         descText.alignment = TextAlignmentOptions.Top;
//         descText.color = Color.white;

//         // 图片
//         GameObject imageObj = new GameObject("ItemImage");
//         imageObj.transform.SetParent(infoPanel.transform, false);
//         RectTransform imageRect = imageObj.AddComponent<RectTransform>();
//         imageRect.anchorMin = new Vector2(0.35f, 0.05f);
//         imageRect.anchorMax = new Vector2(0.65f, 0.35f);
//         imageRect.offsetMin = Vector2.zero;
//         imageRect.offsetMax = Vector2.zero;
//         itemImage = imageObj.AddComponent<Image>();
//         itemImage.color = new Color(1, 1, 1, 0.8f); // 半透明
//     }
// }


using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AppleInspector : MonoBehaviour
{
    [Header("两个物品的信息")]
    public ItemInfo item1;
    public ItemInfo item2;

    [Header("UI References")]
    public GameObject infoPanel;

    // UI组件
    private TextMeshProUGUI nameText1, priceText1, descText1;
    private Image image1;
    private TextMeshProUGUI nameText2, priceText2, descText2;
    private Image image2;

    private Camera mainCamera;
    private Collider itemCollider;
    private bool isPanelActive = false;

    void Start()
    {
        mainCamera = Camera.main;
        itemCollider = GetComponent<Collider>();
        if (infoPanel == null)
            CreateInfoPanel();
        if (infoPanel != null)
            infoPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isPanelActive)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.collider == itemCollider)
            {
                ShowItemsInfo();
            }
        }
        else if (Input.GetMouseButtonDown(0) && isPanelActive)
        {
            // 点击面板外部才关闭
            if (!RectTransformUtility.RectangleContainsScreenPoint(
                infoPanel.GetComponent<RectTransform>(),
                Input.mousePosition,
                mainCamera))
            {
                HideItemsInfo();
            }
        }
    }

    void ShowItemsInfo()
    {
        if (infoPanel == null)
            CreateInfoPanel();

        // 设置物品1信息
        nameText1.text = item1.itemName;
        priceText1.text = $"Price: ${item1.price:F2}";
        descText1.text = item1.description;
        image1.sprite = item1.itemSprite;
        image1.color = new Color(1, 1, 1, 0.85f);

        // 设置物品2信息
        nameText2.text = item2.itemName;
        priceText2.text = $"Price: ${item2.price:F2}";
        descText2.text = item2.description;
        image2.sprite = item2.itemSprite;
        image2.color = new Color(1, 1, 1, 0.85f);

        // 面板位置
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        screenPos.y += 100;
        infoPanel.GetComponent<RectTransform>().position = screenPos;
        infoPanel.SetActive(true);
        isPanelActive = true;
    }

    void HideItemsInfo()
    {
        if (infoPanel != null)
        {
            infoPanel.SetActive(false);
            isPanelActive = false;
        }
    }

    void CreateInfoPanel()
    {
        GameObject canvasObj = GameObject.Find("ItemInfoCanvas");
        if (canvasObj == null)
        {
            canvasObj = new GameObject("ItemInfoCanvas");
            Canvas canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObj.AddComponent<CanvasScaler>();
            canvasObj.AddComponent<GraphicRaycaster>();
        }
        infoPanel = new GameObject("ItemInfoPanel");
        infoPanel.transform.SetParent(canvasObj.transform, false);
        RectTransform panelRect = infoPanel.AddComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(400, 350);
        Image panelImage = infoPanel.AddComponent<Image>();
        panelImage.color = new Color(0.1f, 0.1f, 0.1f, 0.7f); // 半透明深色背景

        // 物品1
        nameText1 = CreateTMP(infoPanel.transform, new Vector2(0.05f, 0.8f), new Vector2(0.45f, 0.95f), 22, TextAlignmentOptions.Left);
        priceText1 = CreateTMP(infoPanel.transform, new Vector2(0.05f, 0.7f), new Vector2(0.45f, 0.8f), 18, TextAlignmentOptions.Left);
        descText1 = CreateTMP(infoPanel.transform, new Vector2(0.05f, 0.55f), new Vector2(0.45f, 0.7f), 16, TextAlignmentOptions.TopLeft);
        image1 = CreateImage(infoPanel.transform, new Vector2(0.48f, 0.65f), new Vector2(0.68f, 0.95f));

        // 分割线
        GameObject line = new GameObject("Line");
        line.transform.SetParent(infoPanel.transform, false);
        RectTransform lineRect = line.AddComponent<RectTransform>();
        lineRect.anchorMin = new Vector2(0.05f, 0.5f);
        lineRect.anchorMax = new Vector2(0.95f, 0.52f);
        lineRect.offsetMin = Vector2.zero;
        lineRect.offsetMax = Vector2.zero;
        Image lineImg = line.AddComponent<Image>();
        lineImg.color = Color.gray;

        // 物品2
        nameText2 = CreateTMP(infoPanel.transform, new Vector2(0.05f, 0.3f), new Vector2(0.45f, 0.45f), 22, TextAlignmentOptions.Left);
        priceText2 = CreateTMP(infoPanel.transform, new Vector2(0.05f, 0.2f), new Vector2(0.45f, 0.3f), 18, TextAlignmentOptions.Left);
        descText2 = CreateTMP(infoPanel.transform, new Vector2(0.05f, 0.05f), new Vector2(0.45f, 0.2f), 16, TextAlignmentOptions.TopLeft);
        image2 = CreateImage(infoPanel.transform, new Vector2(0.48f, 0.05f), new Vector2(0.68f, 0.35f));
    }

    // 工具方法：创建TMP
    TextMeshProUGUI CreateTMP(Transform parent, Vector2 anchorMin, Vector2 anchorMax, int fontSize, TextAlignmentOptions align)
    {
        GameObject go = new GameObject("TMP");
        go.transform.SetParent(parent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        TextMeshProUGUI tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.fontSize = fontSize;
        tmp.alignment = align;
        tmp.color = Color.white;
        return tmp;
    }

    // 工具方法：创建Image
    Image CreateImage(Transform parent, Vector2 anchorMin, Vector2 anchorMax)
    {
        GameObject go = new GameObject("Image");
        go.transform.SetParent(parent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        Image img = go.AddComponent<Image>();
        img.color = new Color(1, 1, 1, 0.85f);
        return img;
    }
}