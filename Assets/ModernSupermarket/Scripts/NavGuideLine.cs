using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavGuideLine : MonoBehaviour
{
    public Transform startPoint; // 购物车
    public Transform endPoint;   // 目标商品
    private LineRenderer lineRenderer;
    private NavMeshPath path;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        path = new NavMeshPath();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
    }

    void Update()
    {
        if (startPoint && endPoint)
        {
            // 每帧用最新的购物车和目标位置计算路径
            NavMesh.CalculatePath(startPoint.position, endPoint.position, NavMesh.AllAreas, path);
            lineRenderer.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, path.corners[i] + Vector3.up * 0.1f); // 稍微抬高，避免穿地
            }
        }
    }
}
