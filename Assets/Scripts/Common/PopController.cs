using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopController : MonoBehaviour
{
    [Header("バックパネル")]
    public Image backPanel;

    [Header("ポップパネル")]
    public Transform pop;
    [Range(0.5f, 1f)] public float startScale;
    public float openDuration;
    public float closeDuration;
    public Ease type_Panel;
    CanvasGroup canvasGroup;

    [Header("サイドマーク")]
    [Range(0, 100)] public int distance_Arrow;
    public float duration_Arrow;
    public Ease type_Arrow;
    RectTransform[] arrow;

    List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        InitializeGetComponent();
        ClosePanel(true);
    }

    private void InitializeGetComponent()
    {
        canvasGroup = pop.GetComponent<CanvasGroup>();
        arrow = new RectTransform[pop.GetChild(0).childCount];

        for (int i = 0; i < arrow.Length; i++)
        {
            arrow[i] = pop.GetChild(0).GetChild(i).GetComponent<RectTransform>();
        }
    }

    public void OpenPanel()
    {
        tweenList.KillAllAndClear();
        backPanel.enabled = true;
        canvasGroup.blocksRaycasts = true;//グループ内のUI要素がRayにヒットするかどうか

        Sequence sequence_panel = DOTween.Sequence();
        sequence_panel.Append(canvasGroup.DOFade(1.0f, openDuration).SetEase(type_Panel));
        sequence_panel.Join(pop.DOScale(1.0f, openDuration).SetEase(type_Panel));

        Sequence sequence_arrow = DOTween.Sequence();
        sequence_arrow.Append(arrow[0].DOAnchorPos(new Vector2(-distance_Arrow, distance_Arrow), duration_Arrow).SetEase(type_Arrow));
        sequence_arrow.Join(arrow[1].DOAnchorPos(new Vector2(distance_Arrow, distance_Arrow), duration_Arrow).SetEase(type_Arrow));
        sequence_arrow.Join(arrow[2].DOAnchorPos(new Vector2(distance_Arrow, -distance_Arrow), duration_Arrow).SetEase(type_Arrow));
        sequence_arrow.Join(arrow[3].DOAnchorPos(new Vector2(-distance_Arrow, -distance_Arrow), duration_Arrow).SetEase(type_Arrow));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(sequence_panel);
        sequence.Insert(closeDuration, sequence_arrow);

        tweenList.Add(sequence);
    }

    public void ClosePanel(bool isComplete)
    {
        tweenList.KillAllAndClear();
        backPanel.enabled = false;
        canvasGroup.blocksRaycasts = false;

        Sequence sequence_panel = DOTween.Sequence();
        sequence_panel.Append(canvasGroup.DOFade(0, closeDuration).SetEase(type_Panel));
        sequence_panel.Join(pop.DOScale(startScale, closeDuration).SetEase(type_Panel));

        Sequence sequence_arrow = DOTween.Sequence();
        sequence_arrow.Append(arrow[0].DOAnchorPos(Vector2.zero, duration_Arrow).SetEase(type_Arrow));
        sequence_arrow.Join(arrow[1].DOAnchorPos(Vector2.zero, duration_Arrow).SetEase(type_Arrow));
        sequence_arrow.Join(arrow[2].DOAnchorPos(Vector2.zero, duration_Arrow).SetEase(type_Arrow));
        sequence_arrow.Join(arrow[3].DOAnchorPos(Vector2.zero, duration_Arrow).SetEase(type_Arrow));

        Sequence sequence = DOTween.Sequence();
        sequence.Append(sequence_arrow);
        sequence.Append(sequence_panel);

        tweenList.Add(sequence);

        if (isComplete) sequence.Complete();
    }

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

}
