using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingMachine : MonoBehaviour
{
    public float force = 20; //吹力
    public float range = 5f; //范围
    public float coneAngle = 30; //吹风机夹角
    private Collider[] colliders = new Collider[64]; //存储检测到的碰撞体
    [Header("调试")]
    public bool drawGizmos = true;
    public Color gizmoFillColor = new Color(0.2f, 0.6f, 1f, 0.12f);
    public Color gizmoEdgeColor = new Color(0.2f, 0.6f, 1f, 0.9f);
    private void FixedUpdate()
    {
       int count =  Physics.OverlapSphereNonAlloc(transform.position, range, colliders);//获得检测到的碰撞体数量和物体s
       for (int i = 0; i < count; i++)
       {
           Collider collider = colliders[i];
           if (collider == null)
           {
                continue;//跳过空的碰撞体  
           }
           Rigidbody rb = collider.GetComponent<Rigidbody>();//获得刚体

           if (rb == null)
           {
                continue;//跳过没有刚体的物体
           } 
           Vector3 toTarget = (rb.transform.position - transform.position).normalized;//获得方向
           float distance = Vector3.Distance(rb.transform.position, transform.position);//获得距离
           float angle = Vector3.Angle(transform.forward, toTarget);
           if (angle > coneAngle) continue; // 超出扇形范围
           float force1 = Mathf.Clamp01(1f - (distance / Mathf.Max(0.0001f, range))); // 衰减：距离影响最终力度
           rb.AddForce(toTarget * force1 * force, ForceMode.Force);          // 将力应用到刚体中心（AddForce 会自动应用到中心），使用选择的 ForceMode force1鉴于01之间

       }
    }
    void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;

        Gizmos.color = gizmoFillColor;
        // 画一个简化的表示范围（球形）
        Gizmos.DrawSphere(transform.position, 0.02f);

        // 画扇形边线
        Gizmos.color = gizmoEdgeColor;
        Vector3 forward = transform.forward;
        Quaternion leftRot = Quaternion.AngleAxis(-coneAngle, transform.up);
        Quaternion rightRot = Quaternion.AngleAxis(coneAngle, transform.up);
        Vector3 leftDir = leftRot * forward * range;
        Vector3 rightDir = rightRot * forward * range;
        Gizmos.DrawLine(transform.position, transform.position + leftDir);
        Gizmos.DrawLine(transform.position, transform.position + rightDir);

        // 画几条中间的线来更好地表现扇形
        int steps = Mathf.Clamp(Mathf.CeilToInt(coneAngle / 5f), 1, 36);
        for (int i = 0; i <= steps; i++)
        {
            float t = (float)i / steps;
            float a = Mathf.Lerp(-coneAngle, coneAngle, t);
            Quaternion rot = Quaternion.AngleAxis(a, transform.up);
            Vector3 dir = rot * forward * range;
            Gizmos.DrawLine(transform.position, transform.position + dir);
        }
    }

}
