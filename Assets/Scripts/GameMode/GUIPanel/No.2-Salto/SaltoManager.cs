using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SaltoManager : MonoBehaviour
{
    public SaltoHudManager saltoHudMg;
    public Image buttonEmi;
    public Image saltoNumMemory;

    [Header("Disc")]
    public Image saltoTimeGage;
    public Ease saltoTimeType;

    [Header("Disc")]
    public RectTransform disc;
    public float discAngle;
    public float discDuration;
    public Ease discType;

    [Header("TimeScale")]
    [NamedArrayAttribute(new string[] { "Fill", "First", "Secound", "Third" })]
    [Range(0.05f, 1f)] public float[] timeScaleBox;
    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;
    Tween tween_time;

    int saltoNum;
    public bool isSalto;
    StageCtrl stageCrl;
    //GrypsController grypsCrl;

    private void Start()
    {
        stageCrl = transform.root.GetComponent<StageCtrl>();
    }

    public void JugeSaltoMode(float gageTime)
    {
        tween_time.Kill(true);
        tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScaleBox[0], slowDuration).SetEase(slowType);
        saltoHudMg.StartUpSaltoHud();
        StartTimeGage(gageTime);
        isSalto = false;
    }

    public void StartTimeGage(float gageTime)
    {
        saltoTimeGage.fillAmount = 0.5f;
        saltoTimeGage.DOKill(true);
        saltoTimeGage.DOFillAmount(0.1666667f, gageTime).SetEase(saltoTimeType)
            .OnComplete(() =>
            {
                Release();//JetÇÃisHudÇ™falseÇ≈ÅA1Ç¬à»è„JetMemoryÇ™Ç†ÇÍÇŒÅAJetHudÅZ
            });
    }

    public void Release()
    {
        if (saltoHudMg.isHud)
        {
            saltoHudMg.ShutDownSaltoHud();
            saltoNum = 0;
            disc.transform.DOKill(true);
            tween_time.Kill(true);
            tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType)
                .OnComplete(() =>
                {
                    saltoNumMemory.fillAmount = 0f;
                    stageCrl.jetMg.JugeJetMode();
                });
        }
    }


    public void OnButtonUp()
    {
        if (!isSalto && stageCrl.controlStatus == StageCtrl.ControlStatus.unControl && saltoHudMg.isHud)
        {

            buttonEmi.enabled = false;
            stageCrl.grypsCrl.Salto();
            disc.transform.DOLocalRotate(new Vector3(0, 0, discAngle), discDuration, RotateMode.FastBeyond360).SetRelative(true).SetEase(discType);
            isSalto = true;
            saltoNum++;
            tween_time.Kill(true);
            tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScaleBox[saltoNum], slowDuration).SetEase(slowType);

        }

    }

    public void SaltoComplete()
    {
        //memoryUp
        saltoNumMemory.fillAmount = 0.1666f * saltoNum;
        stageCrl.jetMg.DisplayJetLimit(stageCrl.jetMg.limitNumber + 1); //ãÛíÜéû(unControl)Ç»ÇÃÇ≈JetHudÅ~

        buttonEmi.enabled = true;
        if (saltoNum >= 3)
        {
            stageCrl.ChangeControlStatus(StageCtrl.ControlStatus.control);
            Release(); //âÒì]êîè„å¿Ç≈SaltoHudÇã≠êßShutdown
            return;
        }
        isSalto = false;
    }

}
