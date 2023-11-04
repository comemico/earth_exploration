using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class ShutterManager : MonoBehaviour
{
    public RectTransform homePanel;
    public float initialHomeY;

    public RectTransform startPanel;
    public RectTransform startRingIn;
    public RectTransform startRingOut;
    public CanvasGroup startRing;

    public float initialStartY;

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

    [Header("StartRing")]
    [Range(12f, 72f)] public float ringOutDuration;
    [Range(12f, 72f)] public float ringInDuration;
    public float rotateValue;
    public float rotateDuration;
    public Ease rotateType;
    public float fadeDuration;
    public Ease fadeType;
    public float scaleDuration;
    public Ease scaleType;
    public float intervalTime;

    Tween ringOut;
    Tween ringIn;

    [Header("RectMove")]
    [Range(0.1f, 0.25f)] public float firstDuration;
    public Ease firstType;


    [Header("シーンに移れるかのシグナル")]
    public bool isCompleteShutter;
    AsyncOperation async;

    public CourseController courseCtrl;
    private List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        //tweenList.Add(ringOut);
        //tweenList.Add(ringIn);
        tweenList.KillAllAndClear();
    }

    void Start()
    {
        Initialize();
        OpenShutter(false);
    }

    void Initialize()
    {
        homePanel.anchoredPosition = new Vector2(0f, initialHomeY);
        startPanel.anchoredPosition = new Vector2(0f, initialStartY);

        backPanel.color = Color.black;
        icon.alpha = 1f;
        iconEmi.color = normalColor;

        ringOut = startRingOut.DOLocalRotate(new Vector3(0, 0, 360f), ringOutDuration, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true);
        ringIn = startRingIn.DOLocalRotate(new Vector3(0, 0, 360f), ringInDuration, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true);

    }

    public void OpenShutter(bool isComplete)
    {
        isCompleteShutter = false;

        Sequence seq_open = DOTween.Sequence().SetUpdate(false);
        seq_open.AppendInterval(0.35f);
        seq_open.Append(backPanel.DOFade(0f, fadeInDuration).SetEase(fadeInType));
        seq_open.Join(icon.DOFade(0f, iconDuration).SetEase(iconType));

        seq_open.AppendInterval(0.3f);
        seq_open.AppendCallback(() => courseCtrl.MoveCourse(GManager.instance.recentCourseNum, courseCtrl.fadeDuration));

        seq_open.AppendInterval(0.55f);
        seq_open.Append(homePanel.DOAnchorPosY(0f, firstDuration).SetEase(firstType));
        seq_open.Join(startPanel.DOAnchorPosY(-1 * initialStartY, firstDuration).SetEase(firstType));

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

        Tween emi = iconEmi.DOFade(1f, 0.25f).SetEase(Ease.InOutQuad).SetDelay(0.8f)
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
        seq_close.AppendCallback(() =>
        {
            ringOut.Kill(false);
            ringIn.Kill(false);
        });
        seq_close.Append(startRingOut.DOLocalRotate(new Vector3(0, 0, rotateValue), rotateDuration).SetEase(rotateType).SetRelative(true));
        seq_close.Join(startRingIn.DOLocalRotate(new Vector3(0, 0, rotateValue), rotateDuration).SetEase(rotateType).SetRelative(true));
        seq_close.Join(startRingIn.DOScale(1.1f, scaleDuration).SetEase(scaleType));
        seq_close.Join(startRing.DOFade(0f, fadeDuration).SetEase(fadeType).SetDelay(intervalTime));

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
    /*
    public void ShutterClose(bool isComplete)
    {
        Sequence seq_close = DOTween.Sequence();

        seq_close.AppendInterval(0.25f);

        seq_close.AppendCallback(() => courseCtrl.FadeOutItems());
        seq_close.AppendInterval(courseCtrl.fadeDuration);

        seq_close.Append(startPanel.DOAnchorPosY(initialStartY, secondDuration).SetEase(secondType));

        seq_close.Append(homePanel.DOAnchorPosX(initialHomeY, firstDuration).SetEase(firstType));

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
     */

}
