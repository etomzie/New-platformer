using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public TMP_Text titleText;
    public Collider2D player;
    public Collider2D checkPoint;
    public GameObject willDisableCanvas;
    public GameObject wall;
    public GameObject door;

    void Start()
    {
        StartCoroutine(DiablePlayerAction());
    }

    void Update()
    {
        if (player.IsTouching(checkPoint))
        {
            willDisableCanvas.SetActive(false);
            wall.transform.position = new Vector3(30, 1.4f, 0);
            door.SetActive(true);
        }
    }




    IEnumerator DiablePlayerAction()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        yield return new WaitForSeconds(1.5f);
        playerInput.actions.FindActionMap("Player").Enable();
        yield return StartCoroutine(FadeOutTitle());
    }

    IEnumerator FadeOutTitle(float duration = 0.5f)
    {
        float elapsedTime = 0f;
        Color originalColor = titleText.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            titleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        titleText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }
}
