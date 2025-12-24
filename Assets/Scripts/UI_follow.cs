using UnityEngine;
using TMPro;

public class UIFollowObject : MonoBehaviour
{
    public Transform target;     // object to follow
    public Vector3 offset;       // screen offset
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {   
        
        transform.position = target.position + offset;
    }
}