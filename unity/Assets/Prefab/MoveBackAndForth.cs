using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    [Header("移动起点（可选，不填则使用物体初始位置）")]
    public Transform pointA;

    [Header("移动终点")]
    public Transform pointB;

    [Header("移动速度")]
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 endPos;

    private void Start()
    {
        // 如果没有设置点则使用物体当前位置作为起点
        if (pointA == null)
            startPos = transform.position;
        else
            startPos = pointA.position;

        endPos = pointB.position;
    }

    void Update()
    {
        // 使用 PingPong 来回移动
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        transform.position = Vector3.Lerp(startPos, endPos, t);
    }
}
