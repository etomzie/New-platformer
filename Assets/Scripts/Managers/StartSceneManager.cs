using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName = "MainScene";


    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
