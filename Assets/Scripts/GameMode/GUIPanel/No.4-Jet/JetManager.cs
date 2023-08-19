using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class JetManager : MonoBehaviour
{
    [Header("StartUp")]
    public float distance;
    public float duration;
    public Ease startupType;

    [Header("TimeScale")]
    [Range(0.2f, 1f)] public float timeScale;
    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;

    [Header("Count")]
    public MemoryCountManager countMg;
    public int jetPower;
    public bool isLimitRelease;

    const int MAX_POWER = 3;

    GrypsController grypsCrl;
    Tween time;
    RectTransform rectTransform;

    bool buttonDownFlag;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        grypsCrl = transform.root.GetComponent<StageCtrl>().grypsCrl;
        countMg.InitializeMemoryCount(0);
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
            if (!isLimitRelease) ShutDownJetHud();

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
        rectTransform.DOAnchorPosY(-distance, duration).SetEase(startupType);
    }



}
