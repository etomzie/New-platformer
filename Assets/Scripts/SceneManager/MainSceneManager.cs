using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class MainSceneManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("N pressed");
            SceneManager.LoadScene("AnotherScene");
        }
    }
}
