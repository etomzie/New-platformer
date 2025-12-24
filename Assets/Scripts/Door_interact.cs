using UnityEngine;




public class Door_interact : MonoBehaviour
{
    public bool CanTrigger = false;
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
            CanTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            textUI.SetActive(false);
            CanTrigger = false;
        }
    }
}
