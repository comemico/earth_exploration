using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CurtainManager : MonoBehaviour
{
    [Header("SliderInfo")]
    public RectTransform slider;
    [Range(0.1f, 0.5f)] public float sliderDuration;
    public Ease sliderType;

    [Header("NameInfo")]
    public RectTransform course;
    public RectTransform stage;
    [Range(0.1f, 0.5f)] public float nameDuration;
    public Ease nameType;

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

    [Header("StageInfo")]
    public GameObject stageInfo;

    const int SLIDER = 500;
    const int HEIGHT = 100;

    private List<Tween> tweenList = new List<Tween>();

    AsyncOperation async;


    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        backPanel.color = Color.black;
        slider.sizeDelta = new Vector2(0f, 2f);
        course.anchoredPosition = new Vector2(0f, -HEIGHT);
        stage.anchoredPosition = new Vector2(0f, HEIGHT);
        icon.alpha = 1f;
        iconEmi.color = normalColor;
    }


    public Sequence StartUp()
    {
        //    .SetUpdate() : trueを指定した場合、TimeScaleを無視して動作します(デフォルトはfalse)現在、trueにしている
        Sequence seq_start = DOTween.Sequence();//TimeScaleを無視している

        seq_start.Append(slider.DOSizeDelta(new Vector2(SLIDER, 2f), sliderDuration).SetEase(sliderType));
        seq_start.AppendInterval(0.1f);
        seq_start.Append(stage.DOAnchorPosY(0f, nameDuration).SetEase(nameType));
        seq_start.Join(course.DOAnchorPosY(0f, nameDuration).SetEase(nameType));
        seq_start.AppendInterval(0.25f);
        seq_start.Append(backPanel.DOFade(0f, fadeInDuration).SetEase(fadeInType));
        seq_start.Join(icon.DOFade(0f, iconDuration).SetEase(iconType));
        tweenList.Add(seq_start);
        return seq_start;
    }

    public Sequence HideNameInfo()
    {
        Sequence seq_hide = DOTween.Sequence();

        seq_hide.AppendInterval(1.5f);
        seq_hide.Append(stage.DOAnchorPosY(HEIGHT, nameDuration).SetEase(nameType));
        seq_hide.Join(course.DOAnchorPosY(-HEIGHT, nameDuration).SetEase(nameType));
        seq_hide.AppendInterval(0.1f);
        seq_hide.Append(slider.DOSizeDelta(new Vector2(0f, 2f), sliderDuration).SetEase(sliderType));
        seq_hide.AppendCallback(ResetStageInfo);
        tweenList.Add(seq_hide);
        return seq_hide;
    }

    public void ResetStageInfo()
    {
        slider.sizeDelta = new Vector2(SLIDER, 2f);
        course.anchoredPosition = Vector2.zero;
        stage.anchoredPosition = Vector2.zero;
        stageInfo.SetActive(false);
    }

    public void CloseCurtain(string sceneName)//Scene移動処理
    {
        backPanel.raycastTarget = true;
        iconEmi.color = loadColor;
        ResetStageInfo();

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


}
