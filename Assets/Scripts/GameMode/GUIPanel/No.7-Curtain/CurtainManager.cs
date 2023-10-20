using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CurtainManager : MonoBehaviour
{
    [Header("SliderInfo")]
    public RectTransform slider;
    [Range(0.1f, 0.5f)] public float sliderDuration;
    public Ease sliderType;

    [Header("NameInfo")]
    public RectTransform stageName;
    public RectTransform courseName;
    [Range(0.1f, 0.5f)] public float nameDuration;
    public Ease nameType;

    [Header("BackPanel")]
    public Image backPanel;
    [Range(0.1f, 0.5f)] public float fadeDuration;
    public Ease fadeType;

    [Header("Icon")]
    public CanvasGroup icon;
    [Range(0.1f, 0.5f)] public float iconDuration;
    public Ease iconType;
    public Image iconEmi;




    const int SLIDER = 500;
    const int HEIGHT = 100;

    private List<Tween> tweenList = new List<Tween>();


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
        stageName.anchoredPosition = new Vector2(0f, -HEIGHT);
        courseName.anchoredPosition = new Vector2(0f, HEIGHT);
        icon.alpha = 1f;
        iconEmi.color = new Color(iconEmi.color.r, iconEmi.color.g, iconEmi.color.b, 1f);
    }


    public Sequence StartUp()
    {
        Sequence seq_start = DOTween.Sequence().SetUpdate(false);

        seq_start.Append(slider.DOSizeDelta(new Vector2(SLIDER, 2f), sliderDuration).SetEase(sliderType));
        seq_start.AppendInterval(0.1f);
        seq_start.Append(courseName.DOAnchorPosY(0f, nameDuration).SetEase(nameType));
        seq_start.Join(stageName.DOAnchorPosY(0f, nameDuration).SetEase(nameType));
        seq_start.AppendInterval(0.4f);
        seq_start.Append(backPanel.DOFade(0f, fadeDuration).SetEase(fadeType));
        seq_start.Join(icon.DOFade(0f, iconDuration).SetEase(iconType));
        tweenList.Add(seq_start);
        return seq_start;
    }

    public Sequence Hide()
    {
        Sequence seq_hide = DOTween.Sequence().SetUpdate(false);

        seq_hide.AppendInterval(0.65f);
        seq_hide.Append(courseName.DOAnchorPosY(HEIGHT, nameDuration).SetEase(nameType));
        seq_hide.Join(stageName.DOAnchorPosY(-HEIGHT, nameDuration).SetEase(nameType));
        seq_hide.AppendInterval(0.1f);
        seq_hide.Append(slider.DOSizeDelta(new Vector2(0f, 2f), sliderDuration).SetEase(sliderType));
        tweenList.Add(seq_hide);
        return seq_hide;
    }

}
