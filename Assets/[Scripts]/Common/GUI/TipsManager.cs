using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TipsManager : MonoBehaviour
{
    [Header("Tips")]
    public Image tipsEdge;
    public CanvasGroup backPanel;
    public Text tipsText;
    public Image rankLamp;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float edgeFadeTime = 0.15f;
    public Ease edgeFadeType = Ease.OutQuad;

    [Range(0.1f, 0.5f)] public float edgeWideTime = 0.2f;
    public Ease edgeWideType = Ease.OutQuad;

    [Range(0.1f, 0.5f)] public float panelFadeTime = 0.15f;
    public Ease panelFadeType = Ease.OutQuad;
    [Space(10)]

    public float textWide;
    [Range(0f, 0.1f)] public float scrollSpeed;
    Sequence s_TipsScroll;

    private void Start()
    {
        InitializedTips();
    }

    private void InitializedTips()
    {
        tipsEdge.color = new Color(1, 1, 1, 0);
        tipsEdge.rectTransform.sizeDelta = new Vector2(140.8f, 140.8f);
        backPanel.alpha = 0f;

        tipsText.color = new Color(1, 1, 1, 0);
        tipsText.rectTransform.anchoredPosition = new Vector2(50f, 0f);
    }

    public void ShowTips()
    {
        textWide = tipsText.rectTransform.sizeDelta.x;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(OpenTips());
        sequence.AppendCallback(() => ScrollTips());
    }

    public Sequence OpenTips()
    {
        InitializedTips();

        Sequence s_TipsOpen = DOTween.Sequence(); //Tips_Edge:alpha=1f => Tips_Edge:wide=1080f =同> BackPanel:1f.
        s_TipsOpen.AppendInterval(0.1f);
        s_TipsOpen.Append(tipsEdge.DOFade(1f, edgeFadeTime).SetEase(edgeFadeType));
        s_TipsOpen.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(1080f, 200f), edgeWideTime).SetEase(edgeWideType));
        s_TipsOpen.Append(backPanel.DOFade(1f, panelFadeTime).SetEase(panelFadeType));

        return s_TipsOpen;
    }

    public Sequence CloseTips()
    {
        s_TipsScroll.Kill(false);
        tipsText.color = new Color(1, 1, 1, 0);
        tipsText.rectTransform.anchoredPosition = new Vector2(50f, 0f);

        Sequence s_TipsClose = DOTween.Sequence();
        s_TipsClose.Append(backPanel.DOFade(0f, panelFadeTime).SetEase(panelFadeType));
        s_TipsClose.Join(tipsEdge.rectTransform.DOSizeDelta(new Vector2(200f, 200f), edgeWideTime).SetEase(edgeWideType));
        s_TipsClose.Append(tipsEdge.DOFade(0f, edgeFadeTime).SetEase(edgeFadeType));

        return s_TipsClose;
    }

    public Sequence ScrollTips()
    {
        s_TipsScroll.Kill(false);

        float fadeWide = textWide - 150;//150幅分、早く起動する.

        s_TipsScroll = DOTween.Sequence();
        s_TipsScroll.Append(tipsText.DOFade(1f, 0.5f));
        s_TipsScroll.Join(tipsText.rectTransform.DOAnchorPosX(0f, 0.5f).SetEase(Ease.Linear));
        if (textWide <= 900) return s_TipsScroll;

        s_TipsScroll.AppendInterval(2f);
        s_TipsScroll.Append(tipsText.rectTransform.DOAnchorPosX(-textWide, textWide * scrollSpeed).SetRelative(true).SetEase(Ease.Linear));
        s_TipsScroll.Join(tipsText.DOFade(0f, 0.75f).SetDelay(fadeWide * scrollSpeed).SetEase(Ease.Linear));//150幅分早く起動.
        //sequence.Append().SetLoop(-1) ポイントはSetLoops()をsequenceにかけること
        s_TipsScroll.Append(tipsText.rectTransform.DOAnchorPosX(50, 0)).SetLoops(-1);
        s_TipsScroll.AppendInterval(0.25f);

        return s_TipsScroll;
    }


}
