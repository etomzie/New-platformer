using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Camera))]
public class Camera2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Vector2 offset = Vector2.zero;
    public float x_bias = 0;

    [Header("Smooth Follow")]
    [Range(0.1f, 1f)]
    public float smoothTime = 0.15f;

    public float maxSpeed = Mathf.Infinity;

    public bool useFixedUpdate = false;

    public bool freezeY = false;

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

        float facing = Mathf.Sign(target.localScale.x);
        PlayerController pc = target.GetComponent<PlayerController>();
        if (facing > 0 && !pc.inAnimation) // left and right camera bias
        {
            desired.x += x_bias;
        }
        else
        {
            desired.x += -x_bias;
        }

        if (freezeY)
        {
            desired.y = transform.position.y;
        }

        // 2) 平滑跟隨
        Vector2 current = new Vector2(transform.position.x, transform.position.y);
        Vector2 target2 = new Vector2(desired.x, desired.y);
        Vector2 smoothed = Vector2.SmoothDamp(current, target2, ref followVelocity, smoothTime, maxSpeed, dt);

        transform.position = new Vector3(smoothed.x, smoothed.y, desired.z);
    }
}
