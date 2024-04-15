using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;


public class InformationHudManager : MonoBehaviour
{

    [Header("ButtonHud")]
    public ButtonHud_1 buttonHud_Start;
    public ButtonHud_1 buttonHud_Home;
    public RectTransform home;
    public RectTransform start;
    const float INI_HOME = 185;
    const float INI_START = 150;

    [Space(10)]
    [Range(0.1f, 1f)] public float buttonHud_Show_Time = 0.25f;
    public Ease buttonHud_Show_Type = Ease.OutQuint;


    [Header("StartUp : 開始時の演出")] //開始時の演出
    [Space(10)]
    [Range(0.1f, 0.5f)]
    public float startUp_FadeIn_Time = 0.2f;
    public Ease startUp_FadeIn_Type = Ease.OutSine;
    [Space(10)]
    [Range(12f, 100f)]
    public float startUp_ringIn_Speed = 20;


    [Header("StartGame : スタートボタン押下時の演出")]
    [Space(10)]
    public float startGame_Ring_Distance = 100; //回転量
    public float startGame_Ring_Scale = 0.5f; //拡大・縮小値
    public float startGame_Ring_Time = 0.5f; //回転時間
    public Ease startGame_Ring_Type_Rotate = Ease.OutSine;
    public float intervalTime;
    [Space(10)]
    public float startGame_Ring_FadeTime = 0.2f; //消失時間
    public Ease startGame_Ring_Type_FadeTime = Ease.OutQuad;



    [Header("GetCompenent")]
    [Space(10)]
    public Image ringIn;
    public Button push_Start;
    public Button push_Home;


    Tween t_ringIn;

    InformationManager informationMg;
    CourseController courseCtrl;
    CurtainManagerSelect curtainMg;

    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        t_ringIn.Kill();
        tweenList.KillAllAndClear();
    }

    void Start()
    {
        Initialize();
        StartUp();
    }

    void Initialize()
    {
        informationMg = GetComponent<InformationManager>();
        courseCtrl = GetComponentInChildren<CourseController>();
        curtainMg = GetComponentInChildren<CurtainManagerSelect>();

        home.anchoredPosition = new Vector2(-INI_HOME, INI_HOME);
        start.anchoredPosition = new Vector2(0f, -INI_START);

        t_ringIn = ringIn.rectTransform.DOLocalRotate(new Vector3(0, 0, 360f), startUp_ringIn_Speed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true);
    }

    void StartUp()
    {
        Sequence s_startUp = DOTween.Sequence().SetUpdate(false);
        s_startUp.Append(curtainMg.OpenCurtain());
        s_startUp.AppendInterval(0.35f);
        s_startUp.AppendCallback(() => courseCtrl.MoveCourse(informationMg.data.recentCourseAdress, courseCtrl.fadeDuration));

        s_startUp.AppendInterval(0.5f);
        s_startUp.Append(home.DOAnchorPosY(0f, buttonHud_Show_Time).SetEase(buttonHud_Show_Type));
        s_startUp.Join(start.DOAnchorPosY(0f, buttonHud_Show_Time).SetEase(buttonHud_Show_Type));
        s_startUp.AppendInterval(0.1f);
        s_startUp.AppendCallback(() =>
        {
            buttonHud_Start.StartUp();
            buttonHud_Home.StartUp();
        });

        tweenList.Add(s_startUp);
    }

    public void StartGame(string sceneName)//Scene移動処理
    {
        Sequence s_StartGame = DOTween.Sequence();

        s_StartGame.Append(ringIn.rectTransform.DOLocalRotate(new Vector3(0, 0, startGame_Ring_Distance), startGame_Ring_Time).SetEase(startGame_Ring_Type_Rotate).SetRelative(true));
        s_StartGame.Join(ringIn.rectTransform.DOScale(startGame_Ring_Scale, startGame_Ring_Time).SetEase(startGame_Ring_Type_Rotate));
        s_StartGame.Join(ringIn.DOFade(0f, startGame_Ring_FadeTime).SetEase(startGame_Ring_Type_FadeTime).SetDelay(intervalTime));
        s_StartGame.AppendInterval(0.1f);
        s_StartGame.AppendCallback(() => curtainMg.CloseCurtain(sceneName, curtainMg.fadeOutTime_Start));

        tweenList.Add(s_StartGame);
    }

    public void BackHome(string sceneName)
    {
        curtainMg.CloseCurtain(sceneName, curtainMg.fadeOutTime_Home);
        this.t_ringIn.Kill(false);

        /*
        Sequence seq_close = DOTween.Sequence();
        seq_close.Append(curtain.DOFade(1f, backHome_FadeOut_Time).SetEase(backHome_FadeOut_Type));
        seq_close.Join(icon.DOFade(1f, iconDuration).SetEase(iconType));

        seq_close.AppendCallback((TweenCallback)(() =>
        {
            TweenExtensions.Kill(this.t_ringIn, (bool)false);
        }));
        tweenList.Add(seq_close);
         */
    }



    /*
    void LoadIcon(float delay)
    {
        iconEmi.color = loadColor;

        Tween emi = iconEmi.DOFade(1f, 0.25f).SetEase(Ease.InOutQuad).SetDelay(delay)
       .OnComplete(() =>
       {
           Tween emiLoop = iconEmi.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash, 2)
           .OnStepComplete(() =>
           {
               CheckLoad();//ループ一回毎に (progress >= 0.9f) か判定する
           }
           );
           tweenList.Add(emiLoop);//ループをkillことでエラーを出さないようにする
       }
       );
    }

    public void CheckLoad()
    {
        if (async.progress >= 0.9f)
        {
            Time.timeScale = 1f;
            async.allowSceneActivation = true;
        }
    }
     */

}
