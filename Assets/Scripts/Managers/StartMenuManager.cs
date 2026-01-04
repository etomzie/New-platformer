using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    public PlayerInput playerInput;

    void Start()
    {
        StartCoroutine(DiablePlayerAction());
    }



    IEnumerator DiablePlayerAction()
    {
        playerInput.actions.FindActionMap("Player").Disable();
        yield return new WaitForSeconds(1.5f);
        playerInput.actions.FindActionMap("Player").Enable();
    }
}
