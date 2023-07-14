using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopController : MonoBehaviour
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("バックパネル")]
    [Header("-----------------------------")]
    [Header("バックパネルを付けるか")]
    public bool isBackPanel;

    [Space(PreHeader)]
    [Header("パネル")]
    [Header("-----------------------------")]
    [Header("イージング")]
    public Ease easeType_Panel;
    public float openDuration;
    public float closeDuration;
    [Range(0.5f, 1f)] public float scale_Panel;


    [Space(PreHeader)]
    [Header("マーク")]
    [Header("-----------------------------")]
    public GameObject mark;
    public List<RectTransform> markList;
    public Ease easeType_Mark;
    public float easeDuration_Mark;
    public float distance_Mark;


    Image backPanel;
    CanvasGroup canvasGroup;
    Transform popTransform;

    List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        InitializeGetComponent();
    }

    void Start()
    {
        if (!isBackPanel)
        {
            backPanel.color = new Vector4(0f, 0f, 0f, 0f);
        }
        ClosePanel(true);
    }

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void InitializeGetComponent()
    {
        backPanel = transform.GetChild(0).GetComponent<Image>();
        canvasGroup = transform.GetChild(1).GetComponent<CanvasGroup>();
        popTransform = transform.GetChild(1).GetComponent<Transform>();
        for (int i = 0; i < mark.transform.childCount; i++)
        {
            markList.Add(mark.transform.GetChild(i).GetComponent<RectTransform>());
        }

    }

    public void OpenPanel()
    {
        tweenList.KillAllAndClear();

        backPanel.enabled = true;
        canvasGroup.blocksRaycasts = true;

        Sequence sequence = DOTween.Sequence();
        Sequence sequence_panel = DOTween.Sequence();
        Sequence sequence_mark = DOTween.Sequence();

        sequence_panel.Append(canvasGroup.DOFade(1.0f, openDuration).SetEase(easeType_Panel));
        sequence_panel.Join(popTransform.DOScale(1.0f, openDuration).SetEase(easeType_Panel));
        tweenList.Add(sequence_panel);

        sequence_mark.Append(markList[0].DOAnchorPos(new Vector2(-distance_Mark, distance_Mark), easeDuration_Mark).SetEase(easeType_Mark));
        sequence_mark.Join(markList[1].DOAnchorPos(new Vector2(distance_Mark, distance_Mark), easeDuration_Mark).SetEase(easeType_Mark));
        sequence_mark.Join(markList[2].DOAnchorPos(new Vector2(distance_Mark, -distance_Mark), easeDuration_Mark).SetEase(easeType_Mark));
        sequence_mark.Join(markList[3].DOAnchorPos(new Vector2(-distance_Mark, -distance_Mark), easeDuration_Mark).SetEase(easeType_Mark));
        tweenList.Add(sequence_mark);

        sequence.Append(sequence_panel);
        sequence.Insert(closeDuration, sequence_mark);
        tweenList.Add(sequence);


    }


    public void ClosePanel(bool isComplete)
    {
        tweenList.KillAllAndClear();

        backPanel.enabled = false;
        canvasGroup.blocksRaycasts = false;

        Sequence sequence = DOTween.Sequence();
        Sequence sequence_panel = DOTween.Sequence();
        Sequence sequence_mark = DOTween.Sequence();

        sequence_panel.Append(canvasGroup.DOFade(0, closeDuration).SetEase(easeType_Panel));
        sequence_panel.Join(popTransform.DOScale(scale_Panel, closeDuration).SetEase(easeType_Panel));
        tweenList.Add(sequence_panel);

        sequence_mark.Append(markList[0].DOAnchorPos(Vector2.zero, easeDuration_Mark).SetEase(easeType_Mark));
        sequence_mark.Join(markList[1].DOAnchorPos(Vector2.zero, easeDuration_Mark).SetEase(easeType_Mark));
        sequence_mark.Join(markList[2].DOAnchorPos(Vector2.zero, easeDuration_Mark).SetEase(easeType_Mark));
        sequence_mark.Join(markList[3].DOAnchorPos(Vector2.zero, easeDuration_Mark).SetEase(easeType_Mark));
        tweenList.Add(sequence_mark);


        sequence.Append(sequence_mark);
        sequence.Append(sequence_panel);
        tweenList.Add(sequence);

        if (isComplete)
        {
            sequence.Complete();
        }

    }

}
