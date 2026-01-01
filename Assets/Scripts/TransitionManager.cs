using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DefaultStartSceneManager : MonoBehaviour
{
    public static DefaultStartSceneManager Instance;
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    public void DoorTransition(PlayerController pc, DoorLogic dl)
    {
        StartCoroutine(DoorSequence(pc, dl));
    }
    IEnumerator DoorSequence(PlayerController pc, DoorLogic dl)
    {
        float fadeDuration = 1f;
        pc.inAnimation = true;

        pc.FadeOut(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        yield return StartCoroutine(dl.ExpandCircleAnimation());

        SceneManager.LoadScene("World_1");

        yield return null;

        pc.FadeIn(fadeDuration);
        yield return new WaitForSeconds(fadeDuration);

        pc.inAnimation = false;
    }

}
