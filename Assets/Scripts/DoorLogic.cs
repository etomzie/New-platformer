using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorLogic: MonoBehaviour
{
    public bool startExpanded = false;

    public bool CanEnter = true;
    //public GameObject textUI;
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
        if (startExpanded) {
            circle.localScale = new Vector3(10f, 10f, 1f);
            StartCoroutine(ShrinkSequence());
        }
        else circle.localScale = new Vector3(0f, 0f, 1f);
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
        if (other.CompareTag("Player") && CanEnter && !startExpanded)
        {
            PlayerController pc = other.GetComponent<PlayerController>();
            DoorLogic dl = GetComponent<DoorLogic>();
            if (pc != null)
            {
                DefaultStartSceneManager.Instance.DoorTransition(pc, dl);
            }
            //CanEnter = true;
        }
    }
    public IEnumerator ShrinkSequence()
    {
        float fadeDuration = 1f;
        PlayerController pc = FindAnyObjectByType<PlayerController>();
        Color c = pc.sr.color;
        c.a = 0f;
        pc.sr.color = c;

        pc.inAnimation = true;
 
        yield return StartCoroutine(ShrinkCircleAnimation());
        yield return new WaitForSeconds(0.1f);
        
        if (pc == null) Debug.LogError("PlayerController not found!");
        
        pc.FadeIn(fadeDuration);
        yield return new WaitForSeconds(fadeDuration + 0.1f);
        CanEnter = false;

        pc.inAnimation = false;
    }

    public IEnumerator ExpandCircleAnimation()
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
    public IEnumerator ShrinkCircleAnimation()
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
