using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpeedPowerManager : MonoBehaviour
{
    public RectTransform feather;
    public RectTransform shaft;
    public RectTransform knob;

    [Header("StartDraw")]
    public int initialWide;
    [Range(0.1f, 0.3f)] public float startTime;
    public Ease startType = Ease.OutQuint;
    [Range(0.25f, 0.5f)] public float startAlpha;

    [Header("Drawing")]
    public int gearWide;

    [Header("Release")]
    [Range(0f, 0.5f)] public float releaseTimeTrigger;
    public Ease releaseTypeTrigger;
    [Range(0f, 0.5f)] public float releaseTime;
    public Ease releaseType;


    Color bodyColor;
    Color bloomColor;
    RectTransform[] featherRect;
    Image[] featherImage, featherChild;
    RectTransform knobSize, shaftSize;
    CanvasGroup canvasGroup;

    List<Tween> tweenList = new List<Tween>();

    const int MAXGEAR = 3;
    float factor;
    float knobDistance;
    bool isPlayingArrow;


    private void Awake()
    {
        GetAllChildObject();
        Initialize();
        factor = 1f / MAXGEAR;
        knobDistance = gearWide * MAXGEAR;
    }


    void GetAllChildObject()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        knobSize = knob.GetChild(0).GetComponent<RectTransform>();
        shaftSize = shaft.GetChild(0).GetComponent<RectTransform>();

        featherRect = new RectTransform[feather.childCount];
        featherImage = new Image[featherRect.Length];
        featherChild = new Image[featherRect.Length];

        for (int i = 0; i < featherRect.Length; i++)
        {
            featherRect[i] = feather.GetChild(i).GetComponent<RectTransform>();
            featherImage[i] = feather.GetChild(i).GetComponent<Image>();
            featherChild[i] = featherImage[i].transform.GetChild(0).GetComponent<Image>();
        }
        bodyColor = featherImage[0].color;
        bloomColor = featherChild[0].color;
    }

    public void Initialize()
    {
        for (int i = 1; i < featherRect.Length; i++)//[1]`[4]ˆÊ’u‰Šú
        {
            featherRect[i].anchoredPosition = new Vector2(-1 * (gearWide * (i - 1)), 0f);
        }
        for (int i = 1; i < featherImage.Length; i++)//[1]`[4]“§–¾
        {
            featherImage[i].color = new Color(bodyColor.r, bodyColor.g, bodyColor.b, 0f);
        }
        for (int i = 0; i < featherImage.Length; i++)//[0]`[4]ƒ‰ƒ“ƒvOff
        {
            featherChild[i].enabled = false;
            featherChild[i].color = new Color(bloomColor.r, bloomColor.g, bloomColor.b, 1f);
        }
        canvasGroup.alpha = 1f;
    }

    public void StartDrawBow(int key)
    {
        tweenList.KillAllAndClear();
        Initialize();
        canvasGroup.alpha = startAlpha;
        transform.localScale = new Vector3(key, 1f, 1f);

        knob.anchoredPosition = Vector2.zero;
        knobSize.sizeDelta = new Vector2(30f, 30f);

        shaftSize.sizeDelta = new Vector2(3f, shaftSize.sizeDelta.y);
        shaft.anchoredPosition = Vector2.zero;

        knob.DOAnchorPosX(-1 * initialWide, startTime).SetEase(startType);
        shaftSize.DOSizeDelta(new Vector2(initialWide + 3f, shaftSize.sizeDelta.y), startTime).SetEase(startType);
        isPlayingArrow = true;
    }


    public void DrawingBow(int gearNum, float medianValue)// medianValue = 0f`1f
    {
        float dragLength = gearNum + medianValue;

        if (isPlayingArrow)
        {
            knob.DOComplete();
            knobSize.DOComplete();
            shaft.DOComplete();
            shaftSize.DOComplete();
            isPlayingArrow = false;
        }

        if (gearNum < 1)
        {
            canvasGroup.alpha = startAlpha + (medianValue * (1f - startAlpha));
            shaftSize.sizeDelta = new Vector2((initialWide + 3f) + (medianValue * gearWide), shaftSize.sizeDelta.y);
        }

        if (1 <= gearNum && gearNum < MAXGEAR)
        {
            featherRect[gearNum].anchoredPosition = new Vector2(-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide)), 0f);
            featherImage[gearNum].color = new Color(bodyColor.r, bodyColor.g, bodyColor.b, medianValue * 0.8f);
            shaft.anchoredPosition = new Vector2((-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide))), 0f);
        }

        knob.anchoredPosition = new Vector2(-initialWide + (-1 * dragLength * factor * knobDistance), 0f);

        knobSize.sizeDelta = new Vector2(30 + (dragLength * factor * 45), 30 + (dragLength * factor * 40));

        // : ¶‚ÌŒvŽZ‚©‚çAgearNum‚É‰ž‚¶‚ÄŒÅ’è’l‚ð—^‚¦‚é : ‰E‚ÌŒvŽZ‚©‚çA0f`(ƒMƒAŠÔ‚Ì•)f‚ª‰ÁŽZ‚³‚ê‚é
        //knob.anchoredPosition = new Vector2(-1 * ((gearWide * gearNum) + (medianValue * gearWide)), 0f);
    }

    public void DisplaySpeedArrow(int gearNum)
    {
        Initialize();

        for (int i = 0; i < gearNum; i++)
        {
            featherRect[i].anchoredPosition = new Vector2(-1 * (gearWide * i), 0f);
            featherImage[i].color = bodyColor;
            featherChild[i].enabled = true;
            featherChild[i].color = new Color(bloomColor.r, bloomColor.g, bloomColor.b, 0.2f + (gearNum * factor * 0.8f));
        }

        shaftSize.sizeDelta = new Vector2(3f + initialWide + gearWide, shaftSize.sizeDelta.y);

        if (gearNum < 1)
        {
            shaft.anchoredPosition = Vector2.zero;
        }
        if (gearNum >= MAXGEAR)
        {
            shaft.anchoredPosition = new Vector2(-1 * (gearNum - 1) * gearWide, 0f);
        }
    }

    public void Release(int gearNum)
    {
        Sequence sequence_Release = DOTween.Sequence();

        if (gearNum < 1)
        {
            sequence_Release.Append(knob.DOAnchorPosX(0f, releaseTimeTrigger).SetEase(releaseTypeTrigger));
        }
        else if (gearNum >= 1)
        {
            sequence_Release.Append(knob.DOAnchorPosX(initialWide + gearWide, releaseTimeTrigger).SetEase(releaseTypeTrigger).SetRelative(true));
        }
        sequence_Release.Join(knobSize.DOSizeDelta(new Vector2(30f, 30f), releaseTimeTrigger).SetEase(releaseTypeTrigger));
        sequence_Release.Join(shaftSize.DOSizeDelta(new Vector2(0f, shaftSize.sizeDelta.y), releaseTimeTrigger).SetEase(releaseTypeTrigger));

        sequence_Release.Append(knob.DOAnchorPosX(featherRect[0].anchoredPosition.x, releaseTime).SetEase(releaseType));
        sequence_Release.Join(featherRect[1].DOAnchorPosX(featherRect[0].anchoredPosition.x, releaseTime).SetEase(releaseType));
        sequence_Release.Join(featherRect[2].DOAnchorPosX(featherRect[0].anchoredPosition.x, releaseTime).SetEase(releaseType));
        sequence_Release.Join(featherRect[3].DOAnchorPosX(featherRect[0].anchoredPosition.x, releaseTime).SetEase(releaseType));
        sequence_Release.Join(featherRect[4].DOAnchorPosX(featherRect[0].anchoredPosition.x, releaseTime).SetEase(releaseType));

        sequence_Release.Join(canvasGroup.DOFade(0f, releaseTime).SetEase(releaseType));

        tweenList.Add(sequence_Release);
    }

    //‹­§I—¹ŽžAŠ®—¹ó‘Ô‚É‚È‚é‚æ‚¤‚É‚·‚é
    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

}

/*
tween_offset.Complete();
rectTransform.offsetMin = new Vector2(TENSION_LOOSE, rectTransform.offsetMin.y);
rectTransform.offsetMax = new Vector2(-TENSION_LOOSE, rectTransform.offsetMax.y);
tween_offset = DOTween.To(() => rectTransform.offsetMin.x, value =>
{
rectTransform.offsetMin = new Vector2(value, rectTransform.offsetMin.y);
rectTransform.offsetMax = new Vector2(-value, rectTransform.offsetMax.y);
}
, TENSION_TIGHT, fadeoutTime).SetEase(easeType);
*/