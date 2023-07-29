using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JetManager : MonoBehaviour
{
    [Header("タイムスケール")]
    [Range(0.2f, 1f)] public float timeScale;
    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;

    [Header("JetHud")]
    public float distance;
    public float duration;
    public Ease startupType;
    public Ease shutDownType;

    public bool buttonDownFlag;

    GrypsController grypsCrl;
    Tween time;
    RectTransform rectTransform;

    private void Awake()
    {
        grypsCrl = transform.root.GetComponent<StageCtrl>().grypsCrl;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnButtonDown()
    {
        time.Kill(true);
        buttonDownFlag = true;
        time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, slowDuration).SetEase(slowType);
    }

    public void OnButtonUp()
    {
        if (buttonDownFlag)
        {
            buttonDownFlag = false;
            time.Kill(true);
            time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType);
            grypsCrl.ForceJet(0);
            ShutDownJetHud();
        }
    }

    public void StartUpJetHud()
    {
        rectTransform.DOKill(true);
        rectTransform.DOAnchorPosY(distance, duration).SetEase(startupType);
    }

    public void ShutDownJetHud()
    {
        rectTransform.DOKill(true);
        rectTransform.DOAnchorPosY(0f, duration).SetEase(startupType);
    }
}
