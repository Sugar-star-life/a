using UnityEngine;

/// <summary>
/// 极简版：绕自身中心旋转（仅编辑器切换方向，游戏中不交互）
/// </summary>
public class SimpleRotation : MonoBehaviour
{
    [Tooltip("旋转轴（自身坐标系）：(0,1,0)=Y轴，(1,0,0)=X轴，(0,0,1)=Z轴")]
    public Vector3 rotationAxis = Vector3.up; // 默认绕自身Y轴

    [Tooltip("旋转速度（度/秒）")]
    public float rotationSpeed = 90f; // 固定速度，不中途调节

    [Tooltip("旋转方向：正方向/反方向（运行前设置）")]
    public RotationDirection rotationDirection = RotationDirection.Forward; // 默认正方向

    // 旋转方向枚举（编辑器下拉选择，直观易懂）
    public enum RotationDirection
    {
        Forward = 1,   // 正方向
        Reverse = -1   // 反方向
    }

    void Update()
    {
        // 核心旋转逻辑（根据枚举值确定方向）
        transform.Rotate(
            rotationAxis.normalized,
            (int)rotationDirection * rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }
}