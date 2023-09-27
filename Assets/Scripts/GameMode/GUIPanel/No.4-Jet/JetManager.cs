using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class JetManager : MonoBehaviour
{
    //*ボタンを押したときの処理=>JetMg <-ここ

    [Header("LimitRing")]
    public Image limitRingImg;
    public float limitDuration = 0.25f;
    public Ease limitType = Ease.InOutQuad;
    public int limitNumber;
    const int MAXLIMIT = 3;

    [Header("ChargeRing")]
    public RectTransform chargeRing;
    public Image buttonMark_Left;
    public Image buttonMark_Right;
    CanvasGroup chargeRingCanGrp;
    Image chargeRingImg;

    [Range(0.25f, 1.0f)] public float chargeTime;
    public Ease chargeType;

    int consumeNum;
    public Color[] chargeColor;
    bool isDown;

    [Header("TimeScale")]
    [Range(0.05f, 1f)] public float chargeTimeScale;
    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;
    Tween tween_time;

    StageCtrl stageCrl;
    public GrypsController grypsCrl;
    public JetGUIManager jetGuiMg;


    private void Awake()
    {
        GetComponent();
    }

    void GetComponent()
    {
        chargeRingCanGrp = chargeRing.GetComponent<CanvasGroup>();
        chargeRingImg = chargeRing.GetChild(1).GetComponent<Image>();
        stageCrl = transform.root.GetComponent<StageCtrl>();
    }

    private void Start()
    {
        DisplayJetLimit(limitNumber);
    }

    public void DisplayJetLimit(int limitNum)
    {
        if (limitNum > MAXLIMIT) return;

        limitRingImg.DOKill(true);
        limitRingImg.DOFillAmount((float)limitNum / (float)MAXLIMIT, limitDuration).SetEase(limitType);

        this.limitNumber = limitNum;
        if (stageCrl.controlStatus != StageCtrl.ControlStatus.unControl)//空中時(Salto)はJetHud起× 着地時(Dash)はJetHud起〇
        {
            JugeJetMode();
        }
    }

    public void ChargeRing(int limitNum)
    {
        consumeNum = 0;
        chargeRingImg.fillAmount = 0f;
        chargeRingImg.DOFillAmount(0.3333333f, chargeTime).SetEase(chargeType).SetLoops(limitNum, LoopType.Incremental)
            .OnStepComplete(() =>
            {
                consumeNum++;
                chargeRingImg.color = chargeColor[consumeNum];
            });
    }

    public void JugeJetMode()
    {
        if (!jetGuiMg.isHud && limitNumber >= 1)
        {
            jetGuiMg.StartUpJetHud();
        }
    }

    public void OnButtonDown()
    {
        //isDown = true;
        ChargeRing(limitNumber);
        buttonMark_Left.enabled = true;
        buttonMark_Right.enabled = true;

        if (Time.timeScale > chargeTimeScale)
        {
            tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, chargeTimeScale, slowDuration).SetEase(slowType);
        }
    }

    public void OnButtonUp()
    {
        //isDown = false;
        buttonMark_Left.enabled = false;
        buttonMark_Right.enabled = false;

        chargeRingImg.DOKill(false);
        chargeRingImg.fillAmount = 0f;
        chargeRingImg.color = chargeColor[0];
        buttonMark_Left.color = chargeColor[0];
        buttonMark_Right.color = chargeColor[0];

        tween_time.Kill(true);
        tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType);

        Release(consumeNum);
    }

    void Release(int consumeNum)
    {
        limitNumber -= consumeNum; //limitRingの表示数を合わせる

        DisplayJetLimit(limitNumber);

        stageCrl.saltoMg.Release(); //空中でJetした際に、SaltoHudをShutdownさせるために呼ぶ

        if (consumeNum >= 1)
        {
            grypsCrl.ForceJet(consumeNum - 1);
            if (limitNumber <= 0)
            {
                jetGuiMg.ShutDownJetHud();
            }
        }
    }




}
