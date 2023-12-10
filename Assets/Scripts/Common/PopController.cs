using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopController : MonoBehaviour
{
    Image blockImg;

    [Header("バックパネル")]
    public Transform backPanel;
    [Range(0.5f, 1f)] public float startScale = 0.9f;
    public float openDuration = 0.25f;
    public float closeDuration = 0.2f;
    public Ease type_Panel = Ease.OutQuad;
    CanvasGroup canvasGroup;

    [Header("サイドマーク")]
    public RectTransform[] sideMark;
    [Range(0, 100)] public int sideMarkDistance = 25;
    public float sideMarkTime = 0.2f;
    public Ease sideMarkType = Ease.OutQuint;

    [Header("LampBox")]
    public Image[] lampBox;

    List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        InitializeGetComponent();
        ClosePanel(true);
    }

    private void InitializeGetComponent()
    {
        blockImg = GetComponent<Image>();
        canvasGroup = backPanel.GetComponent<CanvasGroup>();
    }

    public void OpenPanel()
    {
        tweenList.KillAllAndClear();
        blockImg.raycastTarget = true;
        canvasGroup.blocksRaycasts = true;//グループ内のUI要素がRayにヒットするかどうか

        Sequence sequence_panel = DOTween.Sequence();
        sequence_panel.Append(canvasGroup.DOFade(1.0f, openDuration).SetEase(type_Panel));
        sequence_panel.Join(backPanel.DOScale(1.0f, openDuration).SetEase(type_Panel));
        sequence_panel.AppendCallback(() => ButtonLamp(true, 0.2f));

        Sequence sequence_arrow = DOTween.Sequence();
        sequence_arrow.Append(sideMark[0].DOAnchorPos(new Vector2(-sideMarkDistance, sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));
        sequence_arrow.Join(sideMark[1].DOAnchorPos(new Vector2(sideMarkDistance, sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));
        sequence_arrow.Join(sideMark[2].DOAnchorPos(new Vector2(sideMarkDistance, -sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));
        sequence_arrow.Join(sideMark[3].DOAnchorPos(new Vector2(-sideMarkDistance, -sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(sequence_panel);
        sequence.Insert(closeDuration, sequence_arrow);

        tweenList.Add(sequence);
    }

    public void ClosePanel(bool isComplete)
    {
        tweenList.KillAllAndClear();
        blockImg.raycastTarget = false;
        canvasGroup.blocksRaycasts = false;
        ButtonLamp(false, 0.25f);

        Sequence sequence_panel = DOTween.Sequence();
        sequence_panel.Append(canvasGroup.DOFade(0, closeDuration).SetEase(type_Panel));
        sequence_panel.Join(backPanel.DOScale(startScale, closeDuration).SetEase(type_Panel));

        Sequence sequence_arrow = DOTween.Sequence();
        sequence_arrow.Append(sideMark[0].DOAnchorPos(new Vector2(sideMarkDistance, -sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));
        sequence_arrow.Join(sideMark[1].DOAnchorPos(new Vector2(-sideMarkDistance, -sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));
        sequence_arrow.Join(sideMark[2].DOAnchorPos(new Vector2(-sideMarkDistance, sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));
        sequence_arrow.Join(sideMark[3].DOAnchorPos(new Vector2(sideMarkDistance, sideMarkDistance), sideMarkTime).SetRelative(true).SetEase(sideMarkType));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(sequence_arrow);
        sequence.Append(sequence_panel);

        tweenList.Add(sequence);

        if (isComplete) sequence.Complete();
    }

    public void ButtonLamp(bool isEnabled, float fadeTime) //FadeInする場合、PanelAnimeで光源が見えるのを防ぐ目的
    {
        foreach (Image img in lampBox)
        {
            img.enabled = isEnabled;
            img.DOKill(true);
            img.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(Ease.OutQuad);
        }
    }


    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

}
