using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class JetManager : MonoBehaviour
{
    //*ボタンを押したときの処理=>JetMg <-ここ

    [Header("LimitRing")]
    public CanvasGroup limitRingCanGrp;
    public Image limitRingImg;
    public float limitDuration = 0.25f;
    public Ease limitType = Ease.InOutQuad;
    public int limitNumber;
    const int MAXLIMIT = 3;

    [Header("ChargeRing")]
    public RectTransform chargeRing;
    public Image buttonLamp_Left;
    public Image buttonLamp_Right;
    CanvasGroup chargeRingCanGrp;
    Image chargeRingImg;

    [Header("ChargeGauge")]
    public Image gaugeFill;
    public RectTransform pointer;
    public Image pointerLamp;

    [Range(0.25f, 1.0f)] public float chargeTime;
    public Ease chargeType;

    int consumeNum;
    public Color[] chargeColor;
    public bool isDown;

    [Header("TimeScale")]
    [Range(0.05f, 1f)] public float chargeTimeScale;
    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;
    Tween tween_time;

    StageCtrl stageCrl;
    CinemachineManager cinemachineCrl;
    //public GrypsController grypsCrl;
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
        cinemachineCrl = Camera.main.GetComponent<CinemachineManager>();
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

    public void ChargeRing(int limitNum) //長押しで最大3ストック消費する
    {
        consumeNum = 0;

        chargeRingImg.fillAmount = 0f;
        chargeRingImg.DOFillAmount(0.3333333f, chargeTime).SetEase(chargeType).SetLoops(limitNum, LoopType.Incremental)
            .OnStepComplete(() =>
            {
                consumeNum++; //1～3まで増加する (limitNumに応じて)
                chargeRingImg.color = chargeColor[consumeNum];
            });
    }

    public void ChargeEnergy() //長押しで最大1ストック消費する
    {
        consumeNum = 0;

        pointer.DOKill(true);
        gaugeFill.DOKill(true);

        pointer.localRotation = Quaternion.Euler(0, 0, 240);
        gaugeFill.color = chargeColor[0];
        gaugeFill.fillAmount = 0.07f;


        pointer.DOLocalRotate(new Vector3(0, 0, 120), chargeTime, RotateMode.Fast).SetEase(chargeType)
            .OnComplete(() =>
            {
                pointerLamp.enabled = true;
            });

        gaugeFill.DOFillAmount(0.425f, chargeTime).SetEase(chargeType)
            .OnComplete(() =>
            {
                consumeNum++; //1になる
                gaugeFill.color = chargeColor[consumeNum];
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
        isDown = true;
        stageCrl.Regeneration(); //一時的にLackModeからPlayModeへ移る
        stageCrl.grypsCrl.jetAnimator.SetBool("isDown", isDown);

        ChargeEnergy();

        buttonLamp_Left.enabled = true;
        buttonLamp_Right.enabled = true;

        if (Time.timeScale > chargeTimeScale)
        {
            tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, chargeTimeScale, slowDuration).SetEase(slowType);
        }
    }

    public void OnButtonUp()
    {
        isDown = false;

        stageCrl.grypsCrl.jetAnimator.SetBool("isDown", isDown);
        Release(consumeNum);

        pointer.DOKill(false);
        gaugeFill.DOKill(false);
        pointer.DOLocalRotate(new Vector3(0, 0, 240), 0.25f, RotateMode.Fast).SetEase(chargeType);
        gaugeFill.DOFillAmount(0.07f, 0.25f).SetEase(chargeType);


        pointerLamp.enabled = false;
        buttonLamp_Left.enabled = false;
        buttonLamp_Right.enabled = false;

        if (!stageCrl.saltoMg.saltoHudMg.isHud) //Salto側のTimeScale遷移を実行させるためスルーさせる
        {
            tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType);
        }
    }

    void Release(int consumeNum)
    {
        limitNumber -= consumeNum; //limitRingの表示数を合わせる

        DisplayJetLimit(limitNumber);

        if (consumeNum >= 1)
        {
            stageCrl.saltoMg.Release(); //空中でJetした際に、SaltoHudをShutdownさせるために呼ぶ
            stageCrl.grypsCrl.ForceJet(consumeNum - 1);
            if (limitNumber <= 0)
            {
                jetGuiMg.ShutDownJetHud();
            }
        }

        //メモリ切れモード起動
        if (stageCrl.memoryGageMg.memoryGage <= 0)
        {
            stageCrl.Lack();
        }


    }




}
