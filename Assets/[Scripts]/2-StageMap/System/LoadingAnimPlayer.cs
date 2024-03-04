using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingAnimPlayer : MonoBehaviour
{
    private const float DURATION = 0.4f;
    private const float RADIUS = 50f;
    Image[] circles;
    CanvasGroup canvasGroup;

    void Start()
    {
        circles = GetComponentsInChildren<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        Initialize();
    }

    void Initialize()
    {
        for (var i = 0; i < circles.Length; i++)
        {
            var angle = -2 * Mathf.PI * i / circles.Length;
            circles[i].rectTransform.anchoredPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * RADIUS;
        }
        canvasGroup.alpha = 0f;
    }

    public void PlayLoadAnim()
    {
        KillLoadAnim();
        canvasGroup.DOFade(1f, DURATION);
        for (var i = 0; i < circles.Length; i++)
        {
            circles[i].DOFade(0f, DURATION).SetLoops(-1, LoopType.Yoyo).SetDelay(DURATION * i / circles.Length);
        }
    }

    public void KillLoadAnim()
    {
        canvasGroup.DOFade(0f, DURATION);
        for (var i = 0; i < circles.Length; i++)
        {
            circles[i].DOKill();
            circles[i].color = new Vector4(1f, 1f, 1f, 1f);
        }
    }

}
