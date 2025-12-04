using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    [Header(" 移动配置")]
    [Tooltip("角色/物体左右移动的速度。")]
    public float movementSpeed = 5f;

    [Tooltip("指定用于向左移动的按键。")]
    public KeyCode moveLeftKey = KeyCode.A; // 默认使用 'A' 键

    [Tooltip("指定用于向右移动的按键。")]
    public KeyCode moveRightKey = KeyCode.D; // 默认使用 'D' 键

    private Rigidbody rb;

    // 建议在 Start 或 Awake 中获取 Rigidbody 组件
    void Awake()
    {
        // 尝试获取 Rigidbody 组件，以便使用物理方式移动（推荐）
        rb = GetComponent<Rigidbody>();

        
    }

    // 使用 FixedUpdate 处理物理相关的移动操作
    void FixedUpdate()
    {
        // 1. 获取输入方向
        float horizontalInput = GetInputDirection();

        // 2. 计算目标速度/移动向量
        Vector3 targetVelocity = new Vector3(horizontalInput * movementSpeed, 0, 0);

        // 3. 执行移动
        MoveObject(targetVelocity);
    }

    /// <summary>
    /// 根据配置的按键返回当前水平输入值 (-1 为左, 1 为右, 0 为静止)。
    /// </summary>
    private float GetInputDirection()
    {
        float input = 0f;

        // 检查向左移动键是否被按下
        if (Input.GetKey(moveLeftKey))
        {
            input -= 1f;
        }

        // 检查向右移动键是否被按下
        if (Input.GetKey(moveRightKey))
        {
            input += 1f;
        }

        // 如果同时按下了左右键，input 会是 0f
        return input;
    }

    /// <summary>
    /// 根据 Rigidbody 或 Transform 执行移动。
    /// </summary>
    /// <param name="velocity">目标移动向量（速度）。</param>
    private void MoveObject(Vector3 velocity)
    {
        if (rb != null)
        {
            // 使用 Rigidbody 移动（推荐用于物理/碰撞）
            // 保持 Y 轴速度不变，只修改 X 轴速度
            rb.velocity = new Vector3(velocity.x, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            // 使用 Transform 移动（简单，但不处理碰撞）
            transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
        }
    }
}