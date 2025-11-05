using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class Camera2D : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("要跟隨的目標")]
    public Transform target;
    [Tooltip("鏡頭相對目標的位移（世界座標）")]
    public Vector2 offset = Vector2.zero;

    [Header("Smooth Follow")]
    [Range(0.1f, 1f)]
    public float smoothTime = 0.15f;
    [Tooltip("最大跟隨速度（無上限用 Mathf.Infinity）")]
    public float maxSpeed = Mathf.Infinity;
    [Tooltip("使用 FixedUpdate（物理節奏）還是 LateUpdate（建議一般用 LateUpdate）")]
    public bool useFixedUpdate = false;

    private Vector2 followVelocity; // 給 SmoothDamp 用

    void LateUpdate()
    {
        if (!useFixedUpdate) TickUpdate(Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (useFixedUpdate) TickUpdate(Time.fixedDeltaTime);
    }

    private void TickUpdate(float dt)
    {
        if (target == null) return;

        // 1) 計算目標位置（含 offset）
        Vector3 desired = target.position;
        desired.x += offset.x;
        desired.y += offset.y;
        desired.z = transform.position.z; // 保持原本的 Z

        // 2) 平滑跟隨
        Vector2 current = new Vector2(transform.position.x, transform.position.y);
        Vector2 target2 = new Vector2(desired.x, desired.y);
        Vector2 smoothed = Vector2.SmoothDamp(current, target2, ref followVelocity, smoothTime, maxSpeed, dt);

        transform.position = new Vector3(smoothed.x, smoothed.y, desired.z);
    }
}
