using UnityEngine;

public class Background_controller : MonoBehaviour
{
    private float startPos, length;
    public GameObject cam;
    public float parallaxEffect;
    public float offset = 0.2f;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x - offset;
    }

    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        float movement = cam.transform.position.x * (1 - parallaxEffect);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);
        if (movement > startPos + length)
        {
            startPos += length;
        }
        else if (movement < startPos - length)
        {
            startPos -= length;
        }
    }
}
