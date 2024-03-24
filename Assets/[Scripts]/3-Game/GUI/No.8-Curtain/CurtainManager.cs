using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class CurtainManager : MonoBehaviour
{
    [Header("BackPanel")]
    public Image backPanel;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float fadeInDuration;
    public Ease fadeInType;
    [Range(0.1f, 0.5f)] public float fadeOutDuration;
    public Ease fadeOutType;


    [Header("StageInfo")]
    public GameObject stageInfo;
    public Image check;
    public RectTransform slider;
    public Text area;
    public Text stage;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float sliderDuration;
    public Ease sliderType;
    [Range(0.1f, 0.5f)] public float nameDuration;
    public Ease nameType;


    [Header("Icon")]
    public CanvasGroup icon;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float iconDuration;
    public Ease iconType;
    public Image iconEmi;
    public Color loadingColor;
    public Color completeColor;



    const int SLIDER = 600;
    const int HEIGHT = 100;

    private List<Tween> tweenList = new List<Tween>();

    StageCtrl stageCrl;
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
        stageCrl = GetComponentInParent<StageCtrl>();

        backPanel.color = Color.black;

        check.color = new Color(1, 1, 1, 0);
        slider.sizeDelta = new Vector2(0f, 2f);
        area.rectTransform.anchoredPosition = new Vector2(0f, -HEIGHT);
        stage.rectTransform.anchoredPosition = new Vector2(0f, HEIGHT);
        icon.alpha = 1f;
        iconEmi.color = completeColor;
    }


    public Sequence ShowNameInfo(string areaName, string stageName)
    {
        area.text = areaName;
        stage.text = stageName;

        //    .SetUpdate() : trueを指定した場合、TimeScaleを無視して動作します(デフォルトはfalse)現在、trueにしている
        Sequence seq_start = DOTween.Sequence();//TimeScaleを無視している

        seq_start.Append(check.DOFade(1f, 0f));
        seq_start.AppendInterval(0.15f);
        seq_start.Append(check.DOFade(0f, 0f));
        seq_start.AppendInterval(0.125f);
        seq_start.Append(check.DOFade(1f, 0f));
        seq_start.AppendInterval(0.15f);
        seq_start.Append(check.DOFade(0f, 0f));
        seq_start.AppendInterval(0.2f);

        seq_start.Append(slider.DOSizeDelta(new Vector2(SLIDER, 2f), sliderDuration).SetEase(sliderType));

        seq_start.AppendInterval(0.15f);

        seq_start.Append(stage.rectTransform.DOAnchorPosY(0f, nameDuration).SetEase(nameType));
        seq_start.Join(area.rectTransform.DOAnchorPosY(0f, nameDuration).SetEase(nameType));

        tweenList.Add(seq_start);
        return seq_start;
    }

    public Sequence HideNameInfo()
    {
        Sequence seq_hide = DOTween.Sequence();

        seq_hide.AppendInterval(1.5f);
        seq_hide.Append(stage.rectTransform.DOAnchorPosY(HEIGHT, nameDuration).SetEase(nameType));
        seq_hide.Join(area.rectTransform.DOAnchorPosY(-HEIGHT, nameDuration).SetEase(nameType));
        seq_hide.AppendInterval(0.1f);
        seq_hide.Append(slider.DOSizeDelta(new Vector2(0f, 2f), sliderDuration).SetEase(sliderType));
        seq_hide.AppendCallback(ResetStageInfo);
        tweenList.Add(seq_hide);
        return seq_hide;
    }

    public void ResetStageInfo()
    {
        slider.sizeDelta = new Vector2(SLIDER, 2f);
        area.rectTransform.anchoredPosition = Vector2.zero;
        stage.rectTransform.anchoredPosition = Vector2.zero;
        stageInfo.SetActive(false);
    }

    public Sequence OpenCurtain()
    {
        Sequence s_openC = DOTween.Sequence();//TimeScaleを無視している

        //        s_openC.AppendInterval(0.25f);
        s_openC.Append(backPanel.DOFade(0f, fadeInDuration).SetEase(fadeInType));
        s_openC.Join(icon.DOFade(0f, iconDuration).SetEase(iconType));

        return s_openC;
    }

    public void CloseCurtain(string sceneName)//Scene移動処理
    {
        backPanel.raycastTarget = true;
        iconEmi.color = loadingColor;
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
