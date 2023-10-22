using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShutterManager : MonoBehaviour
{
    [Header("MemoryPanel")]
    public RectTransform memoryPanel;
    public float initialMemoryY;

    [Header("CoursePanel")]
    public RectTransform coursePanel;
    public float initialCourseX;

    [Header("HomePanel")]
    public RectTransform homePanel;
    public float initialHomeX;

    [Header("StartPanel")]
    public RectTransform startPanel;
    public float initialStartY;

    [Header("FirstMove")]
    [Range(0.1f, 0.25f)] public float firstDuration;
    public Ease firstType;

    [Header("SecondMove")]
    [Range(0.1f, 0.25f)] public float secondDuration;
    public Ease secondType;

    [Header("BackPanel")]
    public Image backPanel;
    [Range(0.1f, 0.5f)] public float fadeInDuration;
    public Ease fadeInType;
    [Range(0.1f, 0.5f)] public float fadeOutDuration;
    public Ease fadeOutType;

    [Header("Icon")]
    public CanvasGroup icon;
    [Range(0.1f, 0.5f)] public float iconDuration;
    public Ease iconType;
    public Image iconEmi;
    public Color loadColor;
    public Color normalColor;

    [Header("シーンに移れるかのシグナル")]
    public bool isCompleteShutter;
    AsyncOperation async;

    public CourseController courseCtrl;

    private List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    /*
    private void OnDisable()
    {
        if (DOTween.instance != null)
        {
            tweenList.KillAllAndClear();
            // tween?.Kill();// Tween破棄
        }
    }
     */


    void Start()
    {
        Initialize();
        ShutterOpen(false);
    }

    void Initialize()
    {
        memoryPanel.anchoredPosition = new Vector2(-400f, initialMemoryY);
        coursePanel.anchoredPosition = new Vector2(initialCourseX, 0f);
        homePanel.anchoredPosition = new Vector2(initialHomeX, 0f);
        startPanel.anchoredPosition = new Vector2(0f, initialStartY);

        backPanel.color = Color.black;
        icon.alpha = 1f;
        iconEmi.color = normalColor;

    }

    public void ShutterOpen(bool isComplete)
    {
        isCompleteShutter = false;

        Sequence seq_open = DOTween.Sequence().SetUpdate(false);
        seq_open.AppendInterval(0.25f);
        seq_open.Append(backPanel.DOFade(0f, fadeInDuration).SetEase(fadeInType));
        seq_open.Append(coursePanel.DOAnchorPosX(-1 * initialCourseX, firstDuration).SetEase(firstType));
        seq_open.Join(homePanel.DOAnchorPosX(0f, firstDuration).SetEase(firstType));

        seq_open.Append(memoryPanel.DOAnchorPosY(-1 * initialMemoryY, secondDuration).SetEase(secondType));
        seq_open.Join(startPanel.DOAnchorPosY(-1 * initialStartY, secondDuration).SetEase(secondType));

        seq_open.AppendCallback(() => courseCtrl.MoveCourse(GManager.instance.recentCourseNum, courseCtrl.fadeDuration));

        tweenList.Add(seq_open);

        if (isComplete)
        {
            seq_open.Complete();
        }
    }

    public void CloseShutter(string sceneName)//Scene移動処理
    {
        backPanel.raycastTarget = true;
        iconEmi.color = loadColor;

        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;

        Tween emi = iconEmi.DOFade(1f, 0.25f).SetEase(Ease.InOutQuad)
        .OnComplete(() =>
        {
            Tween emi = iconEmi.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutFlash, 2)
            .OnStepComplete(() =>
            {
                CheckLoad();//ループ一回毎に (progress >= 0.9f) か判定する
            }
            );
            tweenList.Add(emi);//ループをkillことでエラーを出さないようにする
        }
        );

        Sequence seq_close = DOTween.Sequence();
        seq_close.Append(backPanel.DOFade(1f, fadeOutDuration).SetEase(fadeOutType));
        seq_close.Join(icon.DOFade(1f, iconDuration).SetEase(iconType));
        tweenList.Add(seq_close);
    }

    public void CheckLoad()
    {
        if (async.progress >= 0.9f)
        {
            Time.timeScale = 1f;
            async.allowSceneActivation = true;
        }
    }

    public void ShutterClose(bool isComplete)
    {
        Sequence seq_close = DOTween.Sequence();

        seq_close.AppendInterval(0.25f);

        seq_close.AppendCallback(() => courseCtrl.FadeOutItems());
        seq_close.AppendInterval(courseCtrl.fadeDuration);

        seq_close.Append(memoryPanel.DOAnchorPosY(initialMemoryY, secondDuration).SetEase(secondType));
        seq_close.Join(startPanel.DOAnchorPosY(initialStartY, secondDuration).SetEase(secondType));

        seq_close.Append(coursePanel.DOAnchorPosX(initialCourseX, firstDuration).SetEase(firstType));
        seq_close.Join(homePanel.DOAnchorPosX(initialHomeX, firstDuration).SetEase(firstType));

        seq_close.AppendInterval(0.1f);


        seq_close.OnComplete(() =>
        {
            FadeCanvasManager.instance.CheckLoad();
            isCompleteShutter = true;
        });

        tweenList.Add(seq_close);

        if (isComplete)
        {
            seq_close.Complete();
        }

    }

}
