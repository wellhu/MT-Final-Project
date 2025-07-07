using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavGuideLine : MonoBehaviour
{
    [Header("Path Settings")]
    public Transform startPoint; // 购物车
    public Transform endPoint;   // 目标商品
    
    [Header("Line Settings")]
    public float lineWidth = 0.1f;
    public float lineHeight = 0.1f; // 线条离地面的高度
    public Color lineColor = Color.green;
    public Color pathNotFoundColor = Color.red;
    
    [Header("Performance")]
    public float updateInterval = 0.1f; // 路径更新间隔，避免每帧都计算
    public float maxPathDistance = 100f; // 最大路径距离
    
    private LineRenderer lineRenderer;
    private NavMeshPath path;
    private float lastUpdateTime;
    private bool pathFound = false;
    
    void Start()
    {
        InitializeLineRenderer();
        path = new NavMeshPath();
        lastUpdateTime = -updateInterval; // 确保第一帧会更新
    }
    
    void InitializeLineRenderer()
    {
        // 获取或添加LineRenderer组件
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
        
        // 设置LineRenderer属性
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.useWorldSpace = true;
        lineRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRenderer.receiveShadows = false;
        lineRenderer.allowOcclusionWhenDynamic = false;
        
        // 设置材质属性
        if (lineRenderer.material != null)
        {
            lineRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineRenderer.material.SetInt("_ZWrite", 0);
            lineRenderer.material.DisableKeyword("_ALPHATEST_ON");
            lineRenderer.material.EnableKeyword("_ALPHABLEND_ON");
            lineRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            lineRenderer.material.renderQueue = 3000;
        }
    }
    
    void Update()
    {
        // 检查是否需要更新路径
        if (Time.time - lastUpdateTime >= updateInterval)
        {
            UpdatePath();
            lastUpdateTime = Time.time;
        }
    }
    
    void UpdatePath()
    {
        // 检查起点和终点是否有效
        if (!IsValidStartPoint() || !IsValidEndPoint())
        {
            HideLine();
            return;
        }
        
        // 检查距离是否在合理范围内
        float distance = Vector3.Distance(startPoint.position, endPoint.position);
        if (distance > maxPathDistance)
        {
            Debug.LogWarning($"Path distance ({distance}) exceeds maximum ({maxPathDistance})");
            HideLine();
            return;
        }
        
        // 计算路径
        pathFound = NavMesh.CalculatePath(startPoint.position, endPoint.position, NavMesh.AllAreas, path);
        
        if (pathFound && path.corners.Length > 0)
        {
            ShowPath();
        }
        else
        {
            ShowDirectLine(); // 如果找不到路径，显示直线
        }
    }
    
    bool IsValidStartPoint()
    {
        return startPoint != null && startPoint.gameObject.activeInHierarchy;
    }
    
    bool IsValidEndPoint()
    {
        return endPoint != null && endPoint.gameObject.activeInHierarchy;
    }
    
    void ShowPath()
    {
        // 设置线条颜色为正常颜色
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        
        // 设置路径点
        lineRenderer.positionCount = path.corners.Length;
        for (int i = 0; i < path.corners.Length; i++)
        {
            Vector3 position = path.corners[i] + Vector3.up * lineHeight;
            lineRenderer.SetPosition(i, position);
        }
        
        // 启用线条
        lineRenderer.enabled = true;
    }
    
    void ShowDirectLine()
    {
        // 设置线条颜色为警告颜色
        lineRenderer.startColor = pathNotFoundColor;
        lineRenderer.endColor = pathNotFoundColor;
        
        // 显示直线连接
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint.position + Vector3.up * lineHeight);
        lineRenderer.SetPosition(1, endPoint.position + Vector3.up * lineHeight);
        
        // 启用线条
        lineRenderer.enabled = true;
        
        Debug.LogWarning("No valid path found, showing direct line");
    }
    
    void HideLine()
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }
    
    // 公共方法：设置起点（购物车）
    public void SetStartPoint(Transform newStartPoint)
    {
        startPoint = newStartPoint;
        lastUpdateTime = -updateInterval; // 强制立即更新
    }
    
    // 公共方法：设置终点（目标商品）
    public void SetEndPoint(Transform newEndPoint)
    {
        endPoint = newEndPoint;
        lastUpdateTime = -updateInterval; // 强制立即更新
    }

    // 公共方法：设置终点（通过Vector3位置）
    public void SetEndPoint(Vector3 position)
    {
        // 创建一个临时的空物体作为终点锚点（可选，也可以直接用一个私有变量存储位置）
        if (endPoint == null)
        {
            GameObject temp = new GameObject("NavGuideLine_EndPoint");
            temp.hideFlags = HideFlags.HideAndDontSave;
            endPoint = temp.transform;
        }
        endPoint.position = position;
        lastUpdateTime = -updateInterval; // 强制立即更新
    }
    
    // 公共方法：清除路径
    public void ClearPath()
    {
        startPoint = null;
        endPoint = null;
        HideLine();
    }
    
    // 公共方法：检查路径是否有效
    public bool IsPathValid()
    {
        return pathFound && path.corners.Length > 0;
    }
    
    // 公共方法：获取路径距离
    public float GetPathDistance()
    {
        if (!pathFound || path.corners.Length < 2)
            return Vector3.Distance(startPoint.position, endPoint.position);
        
        float distance = 0f;
        for (int i = 1; i < path.corners.Length; i++)
        {
            distance += Vector3.Distance(path.corners[i - 1], path.corners[i]);
        }
        return distance;
    }
    
    // 公共方法：获取路径点数量
    public int GetPathCornerCount()
    {
        return pathFound ? path.corners.Length : 0;
    }
    
    void OnDrawGizmosSelected()
    {
        // 在Scene视图中绘制调试信息
        if (startPoint != null && endPoint != null)
        {
            Gizmos.color = pathFound ? Color.green : Color.red;
            Gizmos.DrawLine(startPoint.position, endPoint.position);
            
            if (pathFound && path.corners.Length > 0)
            {
                Gizmos.color = Color.blue;
                for (int i = 1; i < path.corners.Length; i++)
                {
                    Gizmos.DrawLine(path.corners[i - 1], path.corners[i]);
                }
            }
        }
    }
}
