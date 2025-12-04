using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveGate3 : MonoBehaviour
{
    private bool FlagMove = false;

    [Header("移动距离，向下是正数，向上是负数")]
    public float moveDistance = 1f;

    [Header("移动速度")]
    public float moveSpeed = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;

    private void Awake()
    {
        startPos = transform.position;
        targetPos = startPos + Vector3.down * moveDistance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            FlagMove = true;
        }

        if (FlagMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // 只在 FlagMove 为 true 时检查是否到达
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                FlagMove = false; // 停止移动
            }
        }
    }
}
