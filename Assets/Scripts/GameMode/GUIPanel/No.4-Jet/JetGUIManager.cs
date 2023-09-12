using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JetGUIManager : MonoBehaviour
{
    //*ƒ{ƒ^ƒ“‚ðo‚µ“ü‚ê‚·‚éˆ—=>GUIMg <-‚±‚±

    [Header("Ring")]
    public Image circleImage;
    [Range(0.1f, 0.5f)] public float longTapTime;
    public Image limitRingImage;
    public CanvasGroup ringCanvas;
    public int jetNumber;

    [Header("Button")]
    public RectTransform buttonLeft;
    public RectTransform buttonRight;
    public float distance;
    public float duration;
    public Ease startupType;

    private bool isTapMode;
    [HideInInspector] public bool isCharge;
    private float tapTime;
    private bool isDown;

    const int MAXJETNUMBER = 3;

    StageCtrl stageCrl;
    JetManager jetMg;


    /*
    private void Start()
    {
        stageCrl = transform.root.GetComponent<StageCtrl>();
        jetMg = transform.parent.GetComponent<JetManager>();
    }
    private void Update()
    {
        if (isDown)
        {
            tapTime += Time.deltaTime;
            circleImage.fillAmount = tapTime / longTapTime;
            if (tapTime >= longTapTime)
            {
                isDown = false;
                isCharge = true;
                //Debug.Log("LongTap");
                circleImage.color = completeColor;
            }
        }

    }

    public void PushDown()
    {
        isDown = true;
        tapTime = 0f;
        circleImage.fillAmount = 0f;
        circleImage.color = fillingColor;
    }
    public void PushUp()
    {
        isDown = false;
        tapTime = 0f;
        circleImage.fillAmount = 0f;
        circleImage.color = fillingColor;

    }

    public void DisplayJetCount(int jetNum)
    {
        if (jetNum > MAXJETNUMBER) return;
        limitRingImage.fillAmount = (float)jetNum / (float)MAXJETNUMBER;
        jetNumber = jetNum;
        JugeTapMode();
    }

    public void JugeTapMode()
    {
        if (jetNumber == 3 && stageCrl.controlStatus != StageCtrl.ControlStatus.unControl && !isTapMode)
        {
            jetMg.StartUpJetHud();
            isTapMode = true;
            ringCanvas.gameObject.transform.DOScale(1.2f, 0.25f).SetEase(Ease.InQuint);
            ringCanvas.DOFade(1f, 0.25f).SetEase(Ease.InQuint);
        }
    }

    public void ResetJetRing()
    {
        jetMg.ShutDownJetHud();

        jetNumber = 0;
        limitRingImage.fillAmount = 0f;

        isTapMode = false;
        ringCanvas.gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InQuint);
        ringCanvas.DOFade(0f, 0.25f).SetEase(Ease.InQuint);
    }

    public void StartUpJetHud()
    {
        pushRect.DOKill(true);
        pushRect.DOAnchorPosY(0f, duration).SetEase(startupType);
    }

    public void ShutDownJetHud()
    {
        pushRect.DOKill(true);
        pushRect.DOAnchorPosY(-distance, duration).SetEase(startupType);
    }

    */
}
