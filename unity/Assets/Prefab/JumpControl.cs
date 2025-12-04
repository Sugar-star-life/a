using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpControl : MonoBehaviour
{
    public float jumpForce = 6f;        // 跳跃力度（球体不需要太大）
    public float groundCheckRadius =0.1f; // 检测地面的小范围
    public float extraGravity = 20f;    // 自定义重力，让球下落更快、更真实
    public KeyCode JumpKey = KeyCode.J;
    private Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.angularDrag = 0.05f;  // 减少阻力，让球更容易滚动
    }

    void FixedUpdate()
    {
        ApplyExtraGravity();
    }

    void Update()
    {
        if (Input.GetKeyDown(JumpKey) && IsGrounded())
        {
            Jump();
        }
    }

    void Jump()
    {
        // 防止球体跳跃时出现“向下压地”导致跳不起来
        Vector3 vel = rb.velocity;
        vel.y = 0;
        rb.velocity = vel;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        float r = GetComponent<SphereCollider>().radius *
                  Mathf.Max(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        // 最低点
        Vector3 bottom = transform.position - Vector3.up * r;
        // 再往下给一点容错
        return Physics.CheckSphere(bottom, groundCheckRadius,1 << LayerMask.NameToLayer("Ground"),QueryTriggerInteraction.Ignore);//要将地面的图层设置成Ground，
    }

    void ApplyExtraGravity()
    {
        
        rb.AddForce(Vector3.down * extraGravity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
    }
}