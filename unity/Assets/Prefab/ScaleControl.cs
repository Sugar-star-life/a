using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleControl : MonoBehaviour
{
    public float scaleSpeed = 1f;   // 每秒缩放速度
    public float minScale = 0.5f;   // 最小尺寸限制
    public float maxScale = 3f;     // 最大尺寸限制
    public KeyCode ScaleCon = KeyCode.K;
   
    void Update()
    {
        // 当前缩放
        Vector3 scale = transform.localScale;//1.放大缩小

        if (Input.GetKey(ScaleCon))
        {
            // 持续放大
            scale += Vector3.one * scaleSpeed * Time.deltaTime;
        }
        else
        {
            // 松开时持续缩小
            scale -= Vector3.one * scaleSpeed * Time.deltaTime;
        }

        // 限制缩放范围，避免变负数或太大
        scale.x = Mathf.Clamp(scale.x, minScale, maxScale);
        scale.y = Mathf.Clamp(scale.y, minScale, maxScale);
        scale.z = Mathf.Clamp(scale.z, minScale, maxScale);

        transform.localScale = scale;
    }

}
