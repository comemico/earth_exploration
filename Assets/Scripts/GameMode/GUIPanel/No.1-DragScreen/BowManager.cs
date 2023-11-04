using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BowManager : MonoBehaviour
{
    public RectTransform shaft;
    public RectTransform handle;
    public Image handleLamp;
    public Image[] dot;
    public Image[] frame;
    public RectTransform[] frameRect;

    [Header("Drawing")]
    public int gearWide;
    public Color[] lampColor;

    [Header("Shot")]
    [Range(0f, 0.5f)] public float shotDuration;
    public Ease shotType;
    [Range(0f, 0.5f)] public float fadeDuration;
    public Ease fadeType;

    CanvasGroup canvasGroup;
    const int MAXGEAR = 3;
    float factor;
    float handleDistance;

    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy() => tweenList.KillAllAndClear();//ã≠êßèIóπéûÅAäÆóπèÛë‘Ç…Ç»ÇÈÇÊÇ§Ç…Ç∑ÇÈ

    private void Start()
    {
        GetComponent();
        factor = 1f / MAXGEAR;
        handleDistance = gearWide * MAXGEAR;
    }

    void GetComponent()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void StartDrawBow(int key)
    {
        tweenList.KillAllAndClear();

        canvasGroup.alpha = 1f;
        transform.localScale = new Vector3(key, 1f, 1f);
        handle.anchoredPosition = Vector2.zero;
        shaft.sizeDelta = new Vector2(0f, shaft.sizeDelta.y);

        for (int i = 0; i < MAXGEAR - 1; i++)
        {
            dot[i].color = new Color(dot[i].color.r, dot[i].color.g, dot[i].color.b, 0f);
            frame[i].color = new Color(frame[i].color.r, frame[i].color.g, frame[i].color.b, 0f);
            frameRect[i].sizeDelta = new Vector2(45f, 75f);
        }
    }

    public void DrawingBow(int gearNum, float medianValue)// medianValue = 0fÅ`1f
    {
        float dragLength = gearNum + medianValue;
        handle.anchoredPosition = new Vector2(-1 * dragLength * factor * handleDistance, 0f);
        shaft.sizeDelta = new Vector2(dragLength * factor * handleDistance, shaft.sizeDelta.y);
    }

    public void DisplaySpeedArrow(int gearNum)
    {
        //Frame (gearNum >= 2)
        for (int i = 0; i < frame.Length; i++)
        {
            if (i < gearNum - 1)
            {
                frame[i].DOFade(1f, 0.2f);
                frameRect[i].DOSizeDelta(new Vector2(45f, 55.5f), 0.2f);
            }
            else
            {
                frame[i].DOFade(0f, 0.2f);
                frameRect[i].DOSizeDelta(new Vector2(45f, 75f), 0.2f);
            }
        }

        //Dot (gearNum >= 1)
        for (int i = 0; i < dot.Length; i++)
        {
            if (i < gearNum)
            {
                dot[i].DOFade(1f, 0.15f);
            }
            else
            {
                dot[i].DOFade(0f, 0.15f);
            }
        }

        //HandleLamp (gearNum >= 1)
        if (gearNum >= 1)
        {
            handleLamp.enabled = true;
            handleLamp.color = lampColor[gearNum - 1];
        }
        else
        {
            handleLamp.enabled = false;
        }

    }

    public void Release()
    {
        for (int i = 0; i < MAXGEAR - 1; i++)
        {
            dot[i].DOFade(0f, 0.2f);
            frame[i].DOFade(0f, 0.2f);
        }

        Sequence seq_shot = DOTween.Sequence();
        seq_shot.Append(handle.DOAnchorPosX(0f, shotDuration).SetEase(shotType));
        seq_shot.Join(shaft.DOSizeDelta(new Vector2(0f, shaft.sizeDelta.y), shotDuration).SetEase(shotType));
        seq_shot.Append(canvasGroup.DOFade(0f, fadeDuration).SetEase(fadeType));
        seq_shot.Join(handleLamp.DOFade(0f, 0.35f));

        tweenList.Add(seq_shot);
    }

}
