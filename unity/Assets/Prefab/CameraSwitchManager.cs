using UnityEngine;
using System.Collections.Generic;

public class CameraSwitchManager : MonoBehaviour
{
    [Header("摄像头列表")]
    public List<Camera> cameras = new List<Camera>(); // 摄像头列表
    public List<string> cameraNames = new List<string>(); // 摄像头名称（可选）

    [Header("切换设置")]
    public KeyCode switchKey = KeyCode.V; // 切换按键
    public bool loopCameras = true; // 是否循环切换
    public bool showCameraName = true; // 是否显示当前摄像头名称
    public float nameDisplayTime = 2f; // 名称显示时间

    [Header("过渡效果")]
    public float transitionSpeed = 5f; // 过渡速度
    public bool smoothTransition = true; // 是否平滑过渡
    public CameraTransitionType transitionType = CameraTransitionType.Instant; // 过渡类型

    [Header("UI显示")]
    public UnityEngine.UI.Text cameraNameText; // UI文本显示摄像头名称
    public GameObject cameraNamePanel; // 显示摄像头名称的面板

    private int currentCameraIndex = 0;
    private float nameDisplayTimer = 0f;
    private Camera previousCamera;

    // 过渡类型枚举
    public enum CameraTransitionType
    {
        Instant,    // 瞬间切换
        Smooth,     // 平滑移动
        Fade        // 淡入淡出（需要额外的设置）
    }

    void Start()
    {
        // 验证摄像头列表
        ValidateCameras();

        // 初始化摄像头状态
        InitializeCameras();

        // 显示第一个摄像头
        ActivateCamera(currentCameraIndex);
    }

    void Update()
    {
        // 检测切换按键
        if (Input.GetKeyDown(switchKey))
        {
            SwitchToNextCamera();
        }

        // 可选：数字键直接切换
        CheckNumberKeys();

        // 更新UI显示
        UpdateCameraNameDisplay();

        // 处理平滑过渡
        if (smoothTransition && transitionType == CameraTransitionType.Smooth)
        {
            HandleSmoothTransition();
        }
    }

    void ValidateCameras()
    {
        // 移除列表中的空引用
        cameras.RemoveAll(cam => cam == null);
        cameraNames.RemoveAll(name => string.IsNullOrEmpty(name));

        // 如果名称列表长度不匹配摄像头列表，进行调整
        while (cameraNames.Count < cameras.Count)
        {
            cameraNames.Add($"Camera {cameraNames.Count + 1}");
        }

        // 如果摄像头列表为空，尝试自动查找
        if (cameras.Count == 0)
        {
            FindAllCamerasInScene();
        }

        // 如果仍然为空，使用主摄像头
        if (cameras.Count == 0 && Camera.main != null)
        {
            cameras.Add(Camera.main);
            cameraNames.Add("Main Camera");
        }
    }

    void FindAllCamerasInScene()
    {
        Camera[] allCameras = FindObjectsOfType<Camera>();
        foreach (Camera cam in allCameras)
        {
            cameras.Add(cam);
            cameraNames.Add(cam.gameObject.name);
        }

        Debug.Log($"找到 {cameras.Count} 个摄像头");
    }

    void InitializeCameras()
    {
        // 禁用所有摄像头
        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != null)
            {
                cameras[i].enabled = false;
                cameras[i].gameObject.SetActive(true); // 确保游戏对象是激活的
            }
        }
    }

    // 切换到下一个摄像头
    public void SwitchToNextCamera()
    {
        if (cameras.Count <= 1) return;

        // 保存当前摄像头
        previousCamera = cameras[currentCameraIndex];

        // 计算下一个摄像头索引
        int nextIndex = currentCameraIndex + 1;

        if (nextIndex >= cameras.Count)
        {
            if (loopCameras)
            {
                nextIndex = 0;
            }
            else
            {
                nextIndex = cameras.Count - 1;
                return; // 不循环，已到最后一个
            }
        }

        // 切换摄像头
        SwitchToCamera(nextIndex);
    }

    // 切换到上一个摄像头
    public void SwitchToPreviousCamera()
    {
        if (cameras.Count <= 1) return;

        previousCamera = cameras[currentCameraIndex];

        int prevIndex = currentCameraIndex - 1;

        if (prevIndex < 0)
        {
            if (loopCameras)
            {
                prevIndex = cameras.Count - 1;
            }
            else
            {
                prevIndex = 0;
                return; // 不循环，已到第一个
            }
        }

        SwitchToCamera(prevIndex);
    }

    // 切换到指定索引的摄像头
    public void SwitchToCamera(int index)
    {
        if (cameras.Count == 0 || index < 0 || index >= cameras.Count)
        {
            Debug.LogWarning($"摄像头索引 {index} 无效");
            return;
        }

        // 如果已经是当前摄像头，不执行切换
        if (index == currentCameraIndex) return;

        // 禁用当前摄像头
        if (cameras[currentCameraIndex] != null)
        {
            cameras[currentCameraIndex].enabled = false;
        }

        // 更新索引
        currentCameraIndex = index;

        // 激活新摄像头
        ActivateCamera(currentCameraIndex);

        // 显示摄像头名称
        ShowCameraName();

        Debug.Log($"切换到摄像头: {GetCurrentCameraName()}");
    }

    // 切换到指定名称的摄像头
    public void SwitchToCamera(string cameraName)
    {
        int index = cameraNames.IndexOf(cameraName);
        if (index >= 0)
        {
            SwitchToCamera(index);
        }
        else
        {
            Debug.LogWarning($"未找到名为 {cameraName} 的摄像头");
        }
    }

    void ActivateCamera(int index)
    {
        if (cameras[index] == null) return;

        // 根据过渡类型激活摄像头
        switch (transitionType)
        {
            case CameraTransitionType.Instant:
                cameras[index].enabled = true;
                break;

            case CameraTransitionType.Smooth:
                cameras[index].enabled = true;
                // 平滑过渡由 HandleSmoothTransition 处理
                break;

            case CameraTransitionType.Fade:
                cameras[index].enabled = true;
                // 这里可以添加淡入淡出效果
                break;
        }
    }

    void HandleSmoothTransition()
    {
        // 如果有上一个摄像头且与当前不同，可以在这里添加平滑过渡效果
        // 例如：位置、旋转的插值过渡
        // 这需要摄像头有移动的能力，通常适用于跟随摄像头
    }

    void CheckNumberKeys()
    {
        // 数字键1-9直接切换到对应摄像头
        for (int i = 0; i < Mathf.Min(9, cameras.Count); i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchToCamera(i);
            }
        }
    }

    void ShowCameraName()
    {
        nameDisplayTimer = nameDisplayTime;

        // 更新UI显示
        if (showCameraName)
        {
            if (cameraNameText != null)
            {
                cameraNameText.text = GetCurrentCameraName();
                cameraNameText.gameObject.SetActive(true);
            }

            if (cameraNamePanel != null)
            {
                cameraNamePanel.SetActive(true);
            }
        }
    }

    void UpdateCameraNameDisplay()
    {
        if (nameDisplayTimer > 0)
        {
            nameDisplayTimer -= Time.deltaTime;

            // 倒计时结束时隐藏名称
            if (nameDisplayTimer <= 0)
            {
                if (cameraNameText != null)
                {
                    cameraNameText.gameObject.SetActive(false);
                }

                if (cameraNamePanel != null)
                {
                    cameraNamePanel.SetActive(false);
                }
            }
        }
    }

    // 获取当前摄像头名称
    public string GetCurrentCameraName()
    {
        if (currentCameraIndex >= 0 && currentCameraIndex < cameraNames.Count)
        {
            return cameraNames[currentCameraIndex];
        }
        else if (cameras[currentCameraIndex] != null)
        {
            return cameras[currentCameraIndex].gameObject.name;
        }
        return "Unknown Camera";
    }

    // 获取当前摄像头索引
    public int GetCurrentCameraIndex()
    {
        return currentCameraIndex;
    }

    // 获取摄像头总数
    public int GetCameraCount()
    {
        return cameras.Count;
    }

    // 添加摄像头到列表
    public void AddCamera(Camera newCamera, string cameraName = "")
    {
        if (newCamera == null) return;

        cameras.Add(newCamera);

        if (string.IsNullOrEmpty(cameraName))
        {
            cameraName = newCamera.gameObject.name;
        }
        cameraNames.Add(cameraName);

        // 禁用新添加的摄像头（除非它是第一个）
        if (cameras.Count > 1)
        {
            newCamera.enabled = false;
        }
    }

    // 从列表中移除摄像头
    public void RemoveCamera(int index)
    {
        if (index < 0 || index >= cameras.Count) return;

        // 如果要移除的是当前摄像头，先切换到另一个
        if (index == currentCameraIndex)
        {
            if (cameras.Count > 1)
            {
                int nextIndex = (index + 1) % cameras.Count;
                SwitchToCamera(nextIndex);
            }
        }

        cameras.RemoveAt(index);
        cameraNames.RemoveAt(index);

        // 调整当前索引
        if (currentCameraIndex >= cameras.Count)
        {
            currentCameraIndex = cameras.Count - 1;
        }
    }

    // 在编辑器中选择摄像头时，在Scene视图中显示摄像头Frustum
    void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying)
        {
            for (int i = 0; i < cameras.Count; i++)
            {
                if (cameras[i] != null)
                {
                    Color gizmoColor = (i == currentCameraIndex) ? Color.green : Color.red;
                    gizmoColor.a = 0.3f;
                    Gizmos.color = gizmoColor;

                    // 绘制摄像头的视锥
                    Camera cam = cameras[i];
                    if (cam.enabled)
                    {
                        Gizmos.matrix = cam.transform.localToWorldMatrix;
                        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
                    }
                }
            }
        }
    }

    // 公共方法：外部调用切换
    public void NextCamera()
    {
        SwitchToNextCamera();
    }

    public void PreviousCamera()
    {
        SwitchToPreviousCamera();
    }
}