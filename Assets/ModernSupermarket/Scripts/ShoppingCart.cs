using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ShoppingCart : MonoBehaviour
{
    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    public float fastMovementSpeed = 10f;
    public float rotationSpeed = 100f;
    
    [Header("Camera Settings")]
    public Transform cameraMount; // 相机挂载点
    public Camera cartCamera; // 购物车相机
    public float cameraHeight = 0f; // 相机高度
    public float cameraDistance = 0f; // 相机距离购物车后方的距离
    
    [Header("Cart Physics")]
    public float cartMass = 10f; // 购物车质量 - 建议设置小一点
    public float dragForce = 0.5f; // 阻力 - 建议设置小一点
    
    [Header("Movement Mode")]
    public bool useRigidbodyMovement = false; // 是否用刚体物理移动
    
    private Rigidbody cartRigidbody;
    
    void Start()
    {
        // 获取或添加Rigidbody组件
        cartRigidbody = GetComponent<Rigidbody>();
        if (cartRigidbody == null)
        {
            cartRigidbody = gameObject.AddComponent<Rigidbody>();
        }
        
        // 设置Rigidbody属性
        cartRigidbody.mass = cartMass;
        cartRigidbody.drag = dragForce;
        cartRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        // 确保不是Kinematic
        cartRigidbody.isKinematic = false;
        
        // 调试信息
        Debug.Log($"ShoppingCart initialized - Mass: {cartRigidbody.mass}, Drag: {cartRigidbody.drag}");
        Debug.Log($"Rigidbody constraints: {cartRigidbody.constraints}");
    }
    
    void Update()
    {
        var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        var speed = fastMode ? fastMovementSpeed : movementSpeed;

        // 仅在未启用刚体物理移动时，使用transform.position直接移动
        if (!useRigidbodyMovement)
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += -transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += -transform.right * speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += transform.right * speed * Time.deltaTime;
            }
        }

        // 右键旋转
        if (Input.GetKey(KeyCode.Mouse1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            transform.Rotate(0, mouseX, 0);
        }

        HandleCameraDistance();
    }
    
    void FixedUpdate()
    {
        if (useRigidbodyMovement)
        {
            Vector3 moveDirection = Vector3.zero;
            bool fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            float currentSpeed = fastMode ? fastMovementSpeed : movementSpeed;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.UpArrow))
                moveDirection += transform.right;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.DownArrow))
                moveDirection += -transform.right;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                moveDirection += -transform.forward;
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                moveDirection += transform.forward;

            if (moveDirection.magnitude > 0)
            {
                moveDirection.Normalize();
                cartRigidbody.velocity = moveDirection * currentSpeed;
            }
            else
            {
                cartRigidbody.velocity = Vector3.zero;
            }
        }
    }
    
    void HandleCameraDistance()
    {
        // 鼠标滚轮控制相机距离
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
        if (scrollWheel != 0 && cameraMount != null)
        {
            Vector3 currentPos = cameraMount.localPosition;
            float newDistance = currentPos.z + scrollWheel * 2f; // 调整滚动灵敏度
            newDistance = Mathf.Clamp(newDistance, -10f, -1f); // 限制距离范围
            cameraMount.localPosition = new Vector3(currentPos.x, currentPos.y, newDistance);
        }
    }
    
} 