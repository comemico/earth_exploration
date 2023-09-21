using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JetGUIManager : MonoBehaviour
{
    //*ƒ{ƒ^ƒ“‚ðo‚µ“ü‚ê‚·‚éˆ—=>GUIMg <-‚±‚±

    [Header("ChargeRing")]
    public Transform chargeRing;
    public CanvasGroup chargeRingCanGrp;

    public float startupValue;
    public float chargeRingDuration = 0.25f;
    public Ease chargeRingType = Ease.InOutQuad;

    [Header("Button")]
    public RectTransform buttonLeft;
    public RectTransform buttonRight;
    public CanvasGroup buttonCanGrp;

    public float buttonDuration = 0.25f;
    public Ease buttonType = Ease.InOutQuad;


    public bool isHud;
    [HideInInspector] public bool isCharge;
    List<Tween> tweenList = new List<Tween>();



    public void StartUpJetHud()
    {
        tweenList.KillAllAndClear();
        Sequence seq_startup = DOTween.Sequence();

        seq_startup.Append(chargeRing.DOScale(startupValue, chargeRingDuration).SetEase(chargeRingType));
        seq_startup.Join(chargeRingCanGrp.DOFade(1f, chargeRingDuration).SetEase(chargeRingType));

        buttonCanGrp.blocksRaycasts = true;
        seq_startup.Append(buttonLeft.DOLocalRotate(Vector3.zero, buttonDuration).SetEase(buttonType));
        seq_startup.Join(buttonRight.DOLocalRotate(Vector3.zero, buttonDuration).SetEase(buttonType));
        seq_startup.Join(buttonCanGrp.DOFade(1f, buttonDuration).SetEase(buttonType));

        tweenList.Add(seq_startup);
        isHud = true;
    }
    public void ShutDownJetHud()
    {
        tweenList.KillAllAndClear();
        Sequence seq_shutdown = DOTween.Sequence();

        seq_shutdown.Append(chargeRing.DOScale(1f, chargeRingDuration).SetEase(chargeRingType));
        seq_shutdown.Join(chargeRingCanGrp.DOFade(0f, chargeRingDuration).SetEase(chargeRingType));

        buttonCanGrp.blocksRaycasts = false;
        seq_shutdown.Append(buttonLeft.DOLocalRotate(new Vector3(0f, 0f, 90f), buttonDuration).SetEase(buttonType));
        seq_shutdown.Join(buttonRight.DOLocalRotate(new Vector3(0f, 0f, -90f), buttonDuration).SetEase(buttonType));
        seq_shutdown.Join(buttonCanGrp.DOFade(0f, buttonDuration).SetEase(buttonType));

        tweenList.Add(seq_shutdown);
        isHud = false;
    }




    /*

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



    */
}
