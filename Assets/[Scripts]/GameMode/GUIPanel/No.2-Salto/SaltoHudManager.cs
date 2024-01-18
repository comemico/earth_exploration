using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SaltoHudManager : MonoBehaviour
{
    //主にUIの動きに関わるスクリプト.
    //: SaltoMgからの指令に答える関係.

    [Header("StartUp / ShutDown")]
    public RectTransform bearing;
    public CanvasGroup saltoCanGrp;
    [Space(10)]

    public bool isHud;
    [Space(10)]

    [Range(0.25f, 1.0f)] public float openTime = 0.25f;
    public Ease openType = Ease.InOutQuad;
    [Range(0.25f, 1.0f)] public float closeTime = 0.25f;
    public Ease closeType = Ease.InOutQuad;

    [Header("Button")]
    public Image push;
    [Space(10)]

    public Color pushColor;
    [Range(0.01f, 0.5f)] public float pushTime = 0.125f;
    public Ease pushType = Ease.OutQuint;
    [Range(0.01f, 0.5f)] public float pullTime = 0.175f;
    public Ease pullType = Ease.OutBack;


    [Header("Disc / TimeGauge")]
    public RectTransform disc;
    public Image gauge;
    [Space(10)]

    [Range(0, 60)] public int discValue = 45; //回転量
    [Range(0.1f, 0.3f)] public float discTime = 0.175f;
    public Ease discType = Ease.OutSine;


    [Header("Stock")]
    public Image[] stock;


    SaltoManager saltoMg;
    List<Tween> tweenList = new List<Tween>();


    private void Awake()
    {
        saltoMg = GetComponent<SaltoManager>();
    }

    public void StartUpSaltoHud()
    {
        saltoCanGrp.blocksRaycasts = true;

        tweenList.KillAllAndClear();
        Sequence s_startup = DOTween.Sequence();

        s_startup.Append(bearing.DOLocalRotate(Vector3.zero, openTime).SetEase(openType));
        s_startup.Join(saltoCanGrp.DOFade(1f, openTime).SetEase(openType));

        tweenList.Add(s_startup);
        isHud = true;
    }

    public void ShutDownSaltoHud()
    {
        saltoCanGrp.blocksRaycasts = false;

        tweenList.KillAllAndClear();
        Sequence s_shutdown = DOTween.Sequence();

        s_shutdown.Append(bearing.DOLocalRotate(new Vector3(0, 0, -45), closeTime).SetEase(closeType));
        s_shutdown.Join(saltoCanGrp.DOFade(0f, closeTime).SetEase(closeType));

        tweenList.Add(s_shutdown);
        isHud = false;
    }

    public void SaltoButton(bool isPush)
    {
        //true=>ボタンOnへ 0.9f
        if (isPush)
        {
            push.color = pushColor;
            push.rectTransform.DOKill(true);
            push.rectTransform.DOScale(0.9f, pushTime).SetEase(pushType);
        }
        //false=>ボタンOffへ 1f
        else
        {
            push.color = Color.white;
            push.DOKill(true);
            push.rectTransform.DOScale(1f, pushTime).SetEase(pushType);
        }

    }

    public void RotateDisc()
    {
        disc.DOKill(true);
        disc.DOLocalRotate(new Vector3(0, 0, discValue), discTime, RotateMode.FastBeyond360).SetRelative(true).SetEase(discType);
    }

    public void StartTimeGauge(float flightDuration)
    {
        gauge.DOKill(true);
        gauge.fillAmount = 0.5f;
        gauge.DOFillAmount(0.17f, flightDuration).SetEase(Ease.Linear).SetDelay(openTime)
            .OnComplete(() =>
            {
                saltoMg.SaltoEnd();//JetのisHudがfalseで、1つ以上JetStockがあれば、JetHud〇
            });
    }

}
