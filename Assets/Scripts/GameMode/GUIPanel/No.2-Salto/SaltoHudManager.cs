using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SaltoHudManager : MonoBehaviour
{
    [Header("DialRing")]
    public float shutDownValue;
    public float chargeRingDuration = 0.25f;
    public Ease chargeRingType = Ease.InOutQuad;
    /*
    public Transform chargeRing;
    public CanvasGroup chargeRingCanGrp;


    [Header("Button")]
    public RectTransform buttonLeft;
    public RectTransform buttonRight;
    public CanvasGroup buttonCanGrp;

    public float buttonDuration = 0.25f;
    public Ease buttonType = Ease.InOutQuad;
     */


    public bool isHud;
    [HideInInspector] public bool isCharge;
    List<Tween> tweenList = new List<Tween>();



    public void StartUpSaltoHud()
    {
        tweenList.KillAllAndClear();
        Sequence seq_startup = DOTween.Sequence();

        seq_startup.Append(transform.DOLocalRotate(Vector3.zero, 0.3f).SetEase(chargeRingType));
        /*
        seq_startup.Append(chargeRing.DOScale(startupValue, chargeRingDuration).SetEase(chargeRingType));
        seq_startup.Join(chargeRingCanGrp.DOFade(1f, chargeRingDuration).SetEase(chargeRingType));

        buttonCanGrp.blocksRaycasts = true;
        seq_startup.Append(buttonLeft.DOLocalRotate(Vector3.zero, buttonDuration).SetEase(buttonType));
        seq_startup.Join(buttonRight.DOLocalRotate(Vector3.zero, buttonDuration).SetEase(buttonType));
        seq_startup.Join(buttonCanGrp.DOFade(1f, buttonDuration).SetEase(buttonType));
         */

        tweenList.Add(seq_startup);
        isHud = true;
    }

    public void ShutDownSaltoHud()
    {
        tweenList.KillAllAndClear();
        Sequence seq_shutdown = DOTween.Sequence();

        seq_shutdown.Append(transform.DOLocalRotate(new Vector3(0f, 0f, shutDownValue), 0.3f).SetEase(chargeRingType));

        /*
        seq_shutdown.Append(chargeRing.DOScale(1f, chargeRingDuration).SetEase(chargeRingType));
        seq_shutdown.Join(chargeRingCanGrp.DOFade(0f, chargeRingDuration).SetEase(chargeRingType));

        buttonCanGrp.blocksRaycasts = false;
        seq_shutdown.Append(buttonLeft.DOLocalRotate(new Vector3(0f, 0f, 90f), buttonDuration).SetEase(buttonType));
        seq_shutdown.Join(buttonRight.DOLocalRotate(new Vector3(0f, 0f, -90f), buttonDuration).SetEase(buttonType));
        seq_shutdown.Join(buttonCanGrp.DOFade(0f, buttonDuration).SetEase(buttonType));
         */

        tweenList.Add(seq_shutdown);
        isHud = false;
    }
}
