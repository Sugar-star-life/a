using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PendulumObject : MonoBehaviour
{
    [Header("摆动幅度（最大旋转角度）")]
    public float swingAngle = 45f;  // 角度范围 例如 -45~45

    [Header("摆动速度")]
    public float swingSpeed = 2f;

    private float startAngle;

    private void Start()
    {
        // 记录初始角度
        startAngle = transform.localEulerAngles.z;
    }

    void Update()
    {
        // 计算摆动角度（正弦波）
        float angle = swingAngle * Mathf.Sin(Time.time * swingSpeed);

        // 应用摆动
        transform.localRotation = Quaternion.Euler(0, 0, startAngle + angle);
    }
}
