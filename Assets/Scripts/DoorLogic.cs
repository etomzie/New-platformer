using UnityEngine;
using System.Collections;

public class DoorLogic: MonoBehaviour
{
    public bool CanEnter = true;
    public GameObject textUI;
    public Transform circle;

    public Sprite OpenedSprite;
    public Sprite ClosedSprite;
    public SpriteRenderer sr;

    private bool lastState = true;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        textUI.SetActive(false);
    }

    void Update()
    {
        if (CanEnter != lastState)
        {
            sr.sprite = CanEnter ? OpenedSprite : ClosedSprite;
            lastState = CanEnter;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && CanEnter)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            if (pc != null)
            {
                StartCoroutine(ExpandDoorSequence(pc));
            }
            CanEnter = true;
        }
    }

    IEnumerator ExpandDoorSequence(PlayerController pc)
    {
        float fadeDuration = 1f;
        pc.inAnimation = true;
        pc.FadeOut(fadeDuration);

        yield return new WaitForSeconds(fadeDuration + 0.1f);
        pc.inAnimation = false;

        yield return StartCoroutine(ExpandCircleAnimation());
    }
    IEnumerator ShrinkDoorSequence(PlayerController pc)
    {
        float fadeDuration = 1f;
        pc.inAnimation = true;
        pc.FadeIn(fadeDuration);

        yield return new WaitForSeconds(fadeDuration + 0.1f);
        pc.inAnimation = false;

        yield return StartCoroutine(ShrinkCircleAnimation());
    }

    IEnumerator ExpandCircleAnimation()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 initialScale = circle.localScale;
        Vector3 targetScale = new Vector3(10f, 10f, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            circle.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }
        circle.localScale = targetScale;
    }
    IEnumerator ShrinkCircleAnimation()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 initialScale = new Vector3(10f, 10f, 1f);
        Vector3 targetScale = new Vector3(0f, 0f, 1f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            circle.localScale = Vector3.Lerp(initialScale, targetScale, t);
            yield return null;
        }
        circle.localScale = targetScale;
    }
}
