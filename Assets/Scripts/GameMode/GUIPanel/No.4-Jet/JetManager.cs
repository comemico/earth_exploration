using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;


public class JetManager : MonoBehaviour
{
    //*ボタンを押したときの処理=>JetMg <-ここ

    [Header("LimitRing")]
    public Image limitRingImg;
    public float limitDuration = 0.25f;
    public Ease limitType = Ease.InOutQuad;

    public int limitNumber;
    public float perTime;
    public int consumeNum;
    const int MAXLIMIT = 3;

    [Header("ChargeRing")]
    public RectTransform chargeRing;
    public Image buttonMark_Left;
    public Image buttonMark_Right;
    CanvasGroup chargeRingCanGrp;
    Image chargeRingImg;

    public float fullTime;
    public Color fillingColor;
    public Color firstColor;
    public Color secoundColor;
    public Color completeColor;
    float time;
    bool isDown;


    [Header("TimeScale")]
    [NamedArrayAttribute(new string[] { "Fill", "First", "Secound", "Third" })]
    [Range(0.2f, 1f)] public float[] timeScaleBox;

    public float slowDuration;
    public Ease slowType;
    public float returnDuration;
    public Ease returnType;
    Tween tween_time;


    public GrypsController grypsCrl;
    public JetGUIManager jetGuiMg;


    private void Awake()
    {
        GetComponent();
        perTime = fullTime / (float)MAXLIMIT;
    }

    void GetComponent()
    {
        chargeRingCanGrp = chargeRing.GetComponent<CanvasGroup>();
        chargeRingImg = chargeRing.GetChild(1).GetComponent<Image>();
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
        //JugeJetMode(this.limitNumber);
    }

    void JugeJetMode(int limitNum)
    {
        Debug.Log("JetLevel : " + limitNum);
    }

    public void OnButtonDown()
    {
        isDown = true;
        buttonMark_Left.enabled = true;
        buttonMark_Right.enabled = true;

        tween_time.Kill(true);
        tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScaleBox[0], slowDuration).SetEase(slowType);


    }

    public void OnButtonUp()
    {
        isDown = false;
        buttonMark_Left.enabled = false;
        buttonMark_Right.enabled = false;

        time = 0f;
        chargeRingImg.fillAmount = 0f;
        chargeRingImg.color = fillingColor;
        buttonMark_Left.color = fillingColor;
        buttonMark_Right.color = fillingColor;

        tween_time.Kill(true);
        tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType);

        Release();
    }

    void Release()
    {
        limitNumber -= consumeNum;//limitRingの表示数を合わせる
        DisplayJetLimit(limitNumber);
        consumeNum = 0;

    }

    private void Update()
    {
        if (isDown)
        {
            time += Time.deltaTime;
            chargeRingImg.fillAmount = time / fullTime;

            if (time >= perTime * limitNumber)
            {
                chargeRingImg.fillAmount = (float)limitNumber / (float)MAXLIMIT;
                isDown = false;
            }

            //押した時間によって消費するジェットメモリを決める
            if (time >= perTime && consumeNum < 1)
            {
                consumeNum = 1;

                tween_time.Kill(true);
                tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScaleBox[1], slowDuration).SetEase(slowType);

                chargeRingImg.color = firstColor;
                buttonMark_Left.color = firstColor;
                buttonMark_Right.color = firstColor;
            }
            if (time >= perTime * 2 && consumeNum < 2)
            {
                consumeNum = 2;

                tween_time.Kill(true);
                tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScaleBox[2], slowDuration).SetEase(slowType);

                chargeRingImg.color = secoundColor;
                buttonMark_Left.color = secoundColor;
                buttonMark_Right.color = secoundColor;
            }
            if (time >= fullTime)
            {
                consumeNum = 3;

                tween_time.Kill(true);
                tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScaleBox[3], slowDuration).SetEase(slowType);

                chargeRingImg.color = completeColor;
                buttonMark_Left.color = completeColor;
                buttonMark_Right.color = completeColor;
                chargeRingImg.fillAmount = 1f;
                isDown = false;
            }

        }
    }


    /*
    public void OnButtonDown()
    {
        tween_time.Kill(true);
        tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, timeScale, slowDuration).SetEase(slowType);

        jetCountMg.PushDown();
    }

    public void OnButtonUp()
    {
        tween_time.Kill(true);
        tween_time = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 1f, returnDuration).SetEase(returnType);

        jetCountMg.PushUp();

        if (jetCountMg.isCharge)
        {
            grypsCrl.ForceJet(0);
            jetCountMg.ResetJetRing();
            //if (!isLimitRelease) ShutDownJetHud();
            jetCountMg.isCharge = false;
        }
    }
    */



}
