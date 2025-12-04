using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    [Header("给玩家施加的向上力")]
    public float upwardForce = 1000f;

    [Header("玩家的 Tag 名称")]
    public string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        // 确认碰撞到的是玩家
        if (collision.collider.CompareTag(playerTag))
        {
            Rigidbody rb = collision.collider.attachedRigidbody;

            if (rb != null)
            {
                // 清除玩家现有的垂直速度，使弹跳更稳定
                Vector3 vel = rb.velocity;
                vel.y = 0f;
                rb.velocity = vel;

                // 给玩家添加向上力
                rb.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
            }
        }
    }
}
