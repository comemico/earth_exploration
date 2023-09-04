using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class JetManager : MonoBehaviour
{
    [Header("StartUp")]
    public RectTransform pushRect;
    public float distance;
    public float duration;
    public Ease startupType;

    [Header("TimeScale")]
    [Range(0.2f, 1f)] public float timeScale;
    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;

    GrypsController grypsCrl;
    [HideInInspector] public JetCountManager jetCountMg;
    Tween time;

    /*
    [Header("Count")]
    public MemoryCountManager countMg;
    public int jetPower;
    public bool isLimitRelease;
     */


    private void Awake()
    {
        grypsCrl = transform.root.GetComponent<StageCtrl>().grypsCrl;
        jetCountMg = GetComponentInChildren<JetCountManager>();
        //countMg.InitializeMemoryCount(0);
    }

    public void OnButtonDown()
    {
        time.Kill(true);
        time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, slowDuration).SetEase(slowType);

        jetCountMg.PushDown();
    }

    public void OnButtonUp()
    {
        time.Kill(true);
        time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType);

        jetCountMg.PushUp();

        if (jetCountMg.isCharge)
        {
            grypsCrl.ForceJet(0);
            jetCountMg.ResetJetRing();
            //if (!isLimitRelease) ShutDownJetHud();
            jetCountMg.isCharge = false;
        }
    }



    public void StartUpJetHud()
    {
        pushRect.DOKill(true);
        pushRect.DOAnchorPosY(distance, duration).SetEase(startupType);
    }

    public void ShutDownJetHud()
    {
        pushRect.DOKill(true);
        pushRect.DOAnchorPosY(-distance, duration).SetEase(startupType);
    }



}
