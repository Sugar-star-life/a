using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTeleport : MonoBehaviour
{
    [Header("传送门设置")]
    public string playerTag = "Player";     // 玩家 Tag
    public Transform targetGate;            // 目标传送门位置

    [Header("传送效果")]
    public bool keepVelocity = false;       // 是否保持速度
    public float teleportDelay = 0.1f;      // 传送延迟，防止立即传回

    private bool canTeleport = true;
    private Vector3 storedVelocity;

    // 触发事件（3D）
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && canTeleport)
        {
            TeleportPlayer(other.transform);
        }
    }

    private void TeleportPlayer(Transform player)
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        // 存储速度
        if (playerRb != null && keepVelocity)
        {
            storedVelocity = playerRb.velocity;
        }

        // 暂时禁用传送，防止立即传回
        canTeleport = false;
        if (targetGate != null)
        {
            GateTeleport targetScript = targetGate.GetComponent<GateTeleport>();
            if (targetScript != null)
                targetScript.canTeleport = false;
        }

        // 执行传送
        player.position = targetGate.position;

        // 恢复速度
        if (playerRb != null && keepVelocity)
        {
            playerRb.velocity = storedVelocity;
        }

        // 延迟重新启用传送
        Invoke(nameof(EnableTeleport), teleportDelay);

        Debug.Log($"玩家从 {gameObject.name} 传送到 {targetGate.name}");
    }

    private void EnableTeleport()
    {
        canTeleport = true;
        if (targetGate != null)
        {
            GateTeleport targetScript = targetGate.GetComponent<GateTeleport>();
            if (targetScript != null)
                targetScript.canTeleport = true;
        }
    }
}
