using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShutterManager : MonoBehaviour
{
    [Header("StartUp : 開始時の演出")] //開始時の演出
    [Space(10)]
    [Range(0.1f, 0.5f)]
    public float startUp_FadeIn_Time = 0.2f;
    public Ease startUp_FadeIn_Type = Ease.OutSine;
    [Space(10)]
    [Range(0.1f, 0.25f)]
    public float startUp_Button_Time = 0.25f;
    public Ease startUp_Button_Type = Ease.OutQuint;
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
    [Space(10)]
    [Range(0.1f, 0.5f)]
    public float startGame_FadeOut_Time = 0.2f;
    public Ease startGame_FadeOut_Type = Ease.OutQuad;



    [Header("BackHome : ホームボタン押下時の演出")]
    [Space(10)]
    [Range(0.1f, 0.5f)]
    public float backHome_FadeOut_Time = 0.3f;
    public Ease backHome_FadeOut_Type = Ease.OutSine;


    [Header("Icon : ロードマーク")]
    [Space(10)]
    [Range(0.1f, 0.5f)] public float iconDuration;
    public Ease iconType;
    public Color loadColor;
    public Color normalColor;


    [Space(20)]


    [Header("GetCompenent")]
    [Space(10)]
    public Image curtain;

    public Image ringIn;

    public RectTransform home;
    public RectTransform start;
    const float INI_HOME = 220;
    const float INI_START = -110;

    public CanvasGroup icon;
    public Image iconEmi;


    public bool isCompleteShutter;
    Tween t_ringIn;

    InformationManager informationMg;
    CourseController courseCtrl;

    AsyncOperation async;
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

        home.anchoredPosition = new Vector2(0f, INI_HOME);
        start.anchoredPosition = new Vector2(0f, INI_START);
        curtain.color = Color.black;
        icon.alpha = 1f;
        iconEmi.color = normalColor;

        t_ringIn = ringIn.rectTransform.DOLocalRotate(new Vector3(0, 0, 360f), startUp_ringIn_Speed, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true);
    }

    void StartUp()
    {
        Sequence s_startUp = DOTween.Sequence().SetUpdate(false);

        s_startUp.Append(curtain.DOFade(0f, startUp_FadeIn_Time).SetEase(startUp_FadeIn_Type));
        s_startUp.Join(icon.DOFade(0f, iconDuration).SetEase(iconType));

        s_startUp.AppendInterval(0.3f);
        s_startUp.AppendCallback(() => courseCtrl.MoveCourse(informationMg.data.recentCourseAdress, courseCtrl.fadeDuration));

        s_startUp.AppendInterval(0.5f);
        s_startUp.Append(home.DOAnchorPosY(0f, startUp_Button_Time).SetEase(startUp_Button_Type));
        s_startUp.Join(start.DOAnchorPosY(-INI_START, startUp_Button_Time).SetEase(startUp_Button_Type));

        tweenList.Add(s_startUp);
    }

    public void StartGame(string sceneName)//Scene移動処理
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        curtain.raycastTarget = true;

        LoadIcon(0.85f);

        Sequence s_StartGame = DOTween.Sequence();

        s_StartGame.Append(ringIn.rectTransform.DOLocalRotate(new Vector3(0, 0, startGame_Ring_Distance), startGame_Ring_Time).SetEase(startGame_Ring_Type_Rotate).SetRelative(true));
        s_StartGame.Join(ringIn.rectTransform.DOScale(startGame_Ring_Scale, startGame_Ring_Time).SetEase(startGame_Ring_Type_Rotate));
        s_StartGame.Join(ringIn.DOFade(0f, startGame_Ring_FadeTime).SetEase(startGame_Ring_Type_FadeTime).SetDelay(intervalTime));

        s_StartGame.Append(curtain.DOFade(1f, startGame_FadeOut_Time).SetEase(startGame_FadeOut_Type));
        s_StartGame.Join(icon.DOFade(1f, iconDuration).SetEase(iconType));

        tweenList.Add(s_StartGame);
    }

    public void BackHome(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        curtain.raycastTarget = true;

        LoadIcon(0.85f);

        Sequence seq_close = DOTween.Sequence();
        seq_close.Append(curtain.DOFade(1f, backHome_FadeOut_Time).SetEase(backHome_FadeOut_Type));
        seq_close.Join(icon.DOFade(1f, iconDuration).SetEase(iconType));
        seq_close.AppendCallback((TweenCallback)(() =>
        {
            TweenExtensions.Kill(this.t_ringIn, (bool)false);
        }));
        tweenList.Add(seq_close);
    }

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

}
