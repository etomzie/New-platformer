using UnityEngine;

public class Interact_Trigger : MonoBehaviour
{
    public GameObject textUI;

    void Start()
    {
        textUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textUI.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textUI.SetActive(false);
        }
    }
}
