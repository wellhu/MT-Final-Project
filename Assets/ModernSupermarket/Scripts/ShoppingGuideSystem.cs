using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; // Added for List

public class ShoppingGuideSystem : MonoBehaviour
{
    [Header("Guide Line")]
    public NavGuideLine guideLine;
    
    [Header("Shopping Cart")]
    public Transform shoppingCart;
    
    [Header("Product Selection")]
    public Transform[] availableProducts; // 可选择的商品
    public Transform currentTargetProduct; // 当前选中的商品
    
    [Header("UI")]
    public Button[] productButtons; // 商品选择按钮
    public Text distanceText; // 显示距离的文本
    public Text pathInfoText; // 显示路径信息的文本
    
    [Header("Outline Settings")]
    public bool enableOutline = true; // 是否启用描边功能
    public Color outlineColor = Color.green; // 描边颜色
    public float outlineWidth = 2f; // 描边宽度
    public Outline.Mode outlineMode = Outline.Mode.OutlineVisible; // 描边模式
    
    [Header("Settings")]
    public bool showGuideLine = true;
    public bool autoUpdateDistance = true;
    
    private int currentProductIndex = -1;
    private Outline currentOutline; // 当前激活的描边组件
    
    void Start()
    {
        InitializeGuideLine();
        SetupProductButtons();
        // Inspector赋值后，运行时同步到NavGuideLine
        if (guideLine != null && currentTargetProduct != null)
        {
            SetGuideLineEndPointToProduct(currentTargetProduct);
            ApplyOutlineToProduct(currentTargetProduct);
        }
        UpdateUI();
    }
    
    void InitializeGuideLine()
    {
        // 如果没有指定guideLine，尝试在当前对象上找到
        if (guideLine == null)
        {
            guideLine = GetComponent<NavGuideLine>();
        }
        
        // 如果还是没有，创建一个
        if (guideLine == null)
        {
            GameObject guideLineObj = new GameObject("ShoppingGuideLine");
            guideLine = guideLineObj.AddComponent<NavGuideLine>();
        }
        
        // 设置购物车为起点
        if (shoppingCart != null)
        {
            guideLine.SetStartPoint(shoppingCart);
        }
        
        // 设置引导线显示
        guideLine.enabled = showGuideLine;
    }
    
    void SetupProductButtons()
    {
        if (productButtons == null || productButtons.Length == 0)
            return;
            
        // 为每个按钮设置点击事件
        for (int i = 0; i < productButtons.Length && i < availableProducts.Length; i++)
        {
            int productIndex = i; // 捕获变量
            productButtons[i].onClick.AddListener(() => SelectProduct(productIndex));
        }
    }
    
    void Update()
    {
        if (autoUpdateDistance)
        {
            UpdateDistanceDisplay();
        }
    }
    
    // 选择商品
    public void SelectProduct(int productIndex)
    {
        if (productIndex < 0 || productIndex >= availableProducts.Length)
        {
            Debug.LogWarning($"Invalid product index: {productIndex}");
            return;
        }
        
        // 移除之前商品的描边
        RemoveCurrentOutline();
        
        currentProductIndex = productIndex;
        currentTargetProduct = availableProducts[productIndex];
        
        // 设置引导线终点（吸附到最近的NavMesh上）
        SetGuideLineEndPointToProduct(currentTargetProduct);
        
        // 为新选中的商品添加描边
        ApplyOutlineToProduct(currentTargetProduct);
        
        UpdateUI();
        
        Debug.Log($"Selected product: {currentTargetProduct.name}");
    }

    // 修改：为商品找到最近的可行走地面点作为导航终点
    void SetGuideLineEndPointToProduct(Transform product)
    {
        if (guideLine == null || product == null) return;
        
        Vector3 productPos = product.position;
        UnityEngine.AI.NavMeshHit hit;
        
        // 在商品位置附近寻找最近的可行走地面点
        // 增加采样半径，确保能找到合适的地面点
        float sampleRadius = 3.0f;
        
        if (UnityEngine.AI.NavMesh.SamplePosition(productPos, out hit, sampleRadius, UnityEngine.AI.NavMesh.AllAreas))
        {
            // 设置导航终点为找到的地面点，但不改变商品位置
            guideLine.SetEndPoint(hit.position);
            Debug.Log($"Found walkable point near {product.name}: {hit.position}");
        }
        else
        {
            // 如果找不到可行走点，尝试在更大的范围内搜索
            if (UnityEngine.AI.NavMesh.SamplePosition(productPos, out hit, 5.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                guideLine.SetEndPoint(hit.position);
                Debug.Log($"Found walkable point (extended search) near {product.name}: {hit.position}");
            }
            else
            {
                Debug.LogWarning($"No walkable NavMesh found near product: {product.name}");
                guideLine.ClearPath();
            }
        }
    }
    
    // 清除选择
    public void ClearSelection()
    {
        // 移除当前商品的描边
        RemoveCurrentOutline();
        
        currentProductIndex = -1;
        currentTargetProduct = null;
        
        if (guideLine != null)
        {
            guideLine.ClearPath();
        }
        
        UpdateUI();
    }
    
    // 设置购物车
    public void SetShoppingCart(Transform cart)
    {
        shoppingCart = cart;
        if (guideLine != null)
        {
            guideLine.SetStartPoint(cart);
        }
    }
    
    // 切换引导线显示
    public void ToggleGuideLine()
    {
        showGuideLine = !showGuideLine;
        if (guideLine != null)
        {
            guideLine.enabled = showGuideLine;
        }
    }
    
    // 更新距离显示
    void UpdateDistanceDisplay()
    {
        if (distanceText == null || guideLine == null)
            return;
            
        if (guideLine.IsPathValid())
        {
            float distance = guideLine.GetPathDistance();
            distanceText.text = $"Distance: {distance:F1}m";
        }
        else
        {
            distanceText.text = "Distance: N/A";
        }
    }
    
    // 更新UI显示
    void UpdateUI()
    {
        UpdateDistanceDisplay();
        UpdatePathInfo();
        UpdateButtonStates();
    }
    
    // 更新路径信息
    void UpdatePathInfo()
    {
        if (pathInfoText == null || guideLine == null)
            return;
            
        if (guideLine.IsPathValid())
        {
            int corners = guideLine.GetPathCornerCount();
            pathInfoText.text = $"Path: {corners} waypoints";
        }
        else
        {
            pathInfoText.text = "Path: No valid path";
        }
    }
    
    // 更新按钮状态
    void UpdateButtonStates()
    {
        if (productButtons == null)
            return;
            
        for (int i = 0; i < productButtons.Length; i++)
        {
            if (productButtons[i] != null)
            {
                // 高亮当前选中的按钮
                ColorBlock colors = productButtons[i].colors;
                if (i == currentProductIndex)
                {
                    colors.normalColor = Color.green;
                    colors.selectedColor = Color.green;
                }
                else
                {
                    colors.normalColor = Color.white;
                    colors.selectedColor = Color.white;
                }
                productButtons[i].colors = colors;
            }
        }
    }
    
    // 获取当前选中的商品
    public Transform GetCurrentTarget()
    {
        return currentTargetProduct;
    }
    
    // 检查是否有有效的路径
    public bool HasValidPath()
    {
        return guideLine != null && guideLine.IsPathValid();
    }
    
    // 获取到目标的距离
    public float GetDistanceToTarget()
    {
        if (guideLine != null && guideLine.IsPathValid())
        {
            return guideLine.GetPathDistance();
        }
        return -1f;
    }
    
    // 公共方法：通过商品名称选择
    public void SelectProductByName(string productName)
    {
        for (int i = 0; i < availableProducts.Length; i++)
        {
            if (availableProducts[i] != null && availableProducts[i].name.Contains(productName))
            {
                SelectProduct(i);
                return;
            }
        }
        Debug.LogWarning($"Product with name '{productName}' not found");
    }
    
    // 公共方法：通过商品Transform选择
    public void SelectProductByTransform(Transform productTransform)
    {
        for (int i = 0; i < availableProducts.Length; i++)
        {
            if (availableProducts[i] == productTransform)
            {
                SelectProduct(i);
                return;
            }
        }
        Debug.LogWarning($"Product transform not found in available products");
    }
    
    // 新增：为商品及其所有有网格的子物体添加描边
    void ApplyOutlineToProduct(Transform product)
    {
        if (!enableOutline || product == null) return;

        RemoveCurrentOutline(); // 先移除旧的

        // 记录所有新加的Outline，方便后续移除
        currentOutlines = new List<Outline>();

        foreach (var renderer in product.GetComponentsInChildren<Renderer>(true))
        {
            // 只给MeshRenderer或SkinnedMeshRenderer加
            if (renderer is MeshRenderer || renderer is SkinnedMeshRenderer)
            {
                var outline = renderer.gameObject.GetComponent<Outline>();
                if (outline == null)
                    outline = renderer.gameObject.AddComponent<Outline>();

                outline.OutlineColor = outlineColor;
                outline.OutlineWidth = outlineWidth;
                outline.OutlineMode = outlineMode;

                currentOutlines.Add(outline);
            }
        }
    }

    // 新增：移除所有描边
    List<Outline> currentOutlines = new List<Outline>();
    void RemoveCurrentOutline()
    {
        if (currentOutlines != null)
        {
            foreach (var outline in currentOutlines)
            {
                if (outline != null)
                    DestroyImmediate(outline);
            }
            currentOutlines.Clear();
        }
    }
    
    // 新增：更新描边设置
    public void UpdateOutlineSettings(Color color, float width, Outline.Mode mode)
    {
        outlineColor = color;
        outlineWidth = width;
        outlineMode = mode;
        
        // 如果当前有激活的描边，更新其设置
        if (currentOutlines != null)
        {
            foreach (var outline in currentOutlines)
            {
                if (outline != null)
                {
                    outline.OutlineColor = outlineColor;
                    outline.OutlineWidth = outlineWidth;
                    outline.OutlineMode = outlineMode;
                }
            }
        }
    }
    
    // 新增：切换描边功能
    public void ToggleOutline()
    {
        enableOutline = !enableOutline;
        if (!enableOutline)
        {
            RemoveCurrentOutline();
        }
        else if (currentTargetProduct != null)
        {
            ApplyOutlineToProduct(currentTargetProduct);
        }
    }
    
    void OnValidate()
    {
        // 在编辑器中Inspector赋值时自动同步终点
        if (guideLine != null && currentTargetProduct != null)
        {
            SetGuideLineEndPointToProduct(currentTargetProduct);
        }
        
        // 在编辑器中更新描边设置
        if (currentOutlines != null)
        {
            foreach (var outline in currentOutlines)
            {
                if (outline != null)
                {
                    outline.OutlineColor = outlineColor;
                    outline.OutlineWidth = outlineWidth;
                    outline.OutlineMode = outlineMode;
                }
            }
        }
    }
    
    void OnDrawGizmos()
    {
        // 在Scene视图中绘制商品位置
        if (availableProducts != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var product in availableProducts)
            {
                if (product != null)
                {
                    Gizmos.DrawWireSphere(product.position, 0.5f);
                }
            }
        }
        
        // 绘制当前目标
        if (currentTargetProduct != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(currentTargetProduct.position, 0.8f);
        }
        
        // 绘制购物车
        if (shoppingCart != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(shoppingCart.position, Vector3.one);
        }
    }
} 