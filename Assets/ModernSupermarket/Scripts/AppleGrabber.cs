using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleGrabber : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody rb;
    private bool isGrabbed = false;
    private Vector3 grabOffset;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 抓取逻辑
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
            {
                isGrabbed = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                // 计算鼠标和物体的偏移
                Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.position);
                grabOffset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
            }
        }

        // 松开鼠标释放
        if (Input.GetMouseButtonUp(0) && isGrabbed)
        {
            isGrabbed = false;
            rb.useGravity = true;
        }

        // 拖拽物体
        if (isGrabbed)
        {
            Vector3 screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z);
            Vector3 newWorldPos = mainCamera.ScreenToWorldPoint(screenPoint) + grabOffset;
            rb.MovePosition(newWorldPos);
        }

        // X/Y键旋转
        if (isGrabbed)
        {
            if (Input.GetKey(KeyCode.X))
            {
                transform.Rotate(Vector3.up, 100 * Time.deltaTime, Space.World); // 左右旋转
            }
            if (Input.GetKey(KeyCode.Y))
            {
                transform.Rotate(Vector3.right, 100 * Time.deltaTime, Space.World); // 上下旋转
            }
        }

        // 按1失去重力
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            rb.useGravity = false;
        }
    }
}