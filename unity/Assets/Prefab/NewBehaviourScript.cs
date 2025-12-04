using UnityEngine;

public class SphereSpeedFix : MonoBehaviour
{
    private Rigidbody rb;
    private Collider sphereCollider; // 球体碰撞体（用于检测接触）
    private bool isAtCorner = false; // 是否处于直角边缘

    public float minSpeed = 5f; // 最低速度阈值
    public float speedCompensate = 0.1f; // 补偿值

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<Collider>(); // 获取球体碰撞体

        // 校验必要组件
        if (rb == null) Debug.LogError("未挂载Rigidbody组件！");
        if (sphereCollider == null) Debug.LogError("未挂载Collider组件！");
    }

    // 持续碰撞时检测接触点数量（判断是否在直角边）
    void OnCollisionStay(Collision collision)
    {
        // 接触点≥2 → 判定为直角边缘（单一面接触时只有1个接触点）
        isAtCorner = collision.contactCount >= 2;
    }

    // 离开碰撞时重置状态
    void OnCollisionExit(Collision collision)
    {
        isAtCorner = false;
    }

    void FixedUpdate()
    {
        // 空引用防护
        if (rb == null || sphereCollider == null) return;

        // 判定条件：速度过低 + 处于直角边缘
        if (rb.velocity.magnitude < minSpeed && isAtCorner)
        {
            // 避免速度为0时normalized报错
            if (rb.velocity.magnitude > 0.1f)
            {
                rb.velocity = rb.velocity.normalized * (minSpeed + speedCompensate);
            }
            else
            {
                // 速度为0时给默认方向（沿球体自身前方向）
                rb.velocity = transform.forward * minSpeed;
            }
        }
    }
}