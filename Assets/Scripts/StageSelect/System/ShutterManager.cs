using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShutterManager : MonoBehaviour
{
    const float PreHeader = 20f;


    [Space(PreHeader)]
    [Header("シーンに移れるかのシグナル")]
    [Header("-----------------------------")]
    public bool isCompleteShutter;


    [Space(PreHeader)]
    [Header("コースボタン")]
    [Header("-----------------------------")]
    public RectTransform courseButton_Up;
    public RectTransform courseButton_Down;

    [Header("開いた値")]
    public float openvalue_courseButton;
    [Header("閉じた値")]
    public float closevalue_courseButton;
    [Header("時間")]
    public float duration_courseButton;
    [Header("タイプ")]
    public Ease easeType_courseButton;


    [Space(PreHeader)]
    [Header("発進パネル")]
    [Header("-----------------------------")]
    public RectTransform gameStartPanel;

    [Header("開いた値")]
    public float openvalue_startPanel;
    [Header("閉じた値")]
    public float closevalue_startPanel;
    [Header("時間")]
    public float duration_startPanel;
    [Header("タイプ")]
    public Ease easeType_startPanel;


    [Space(PreHeader)]
    [Header("シャッター")]
    [Header("-----------------------------")]
    public RectTransform shutter;

    [Header("開いた値")]
    public float openvalue_shutter;
    [Header("閉じた値")]
    public float closevalue_shutter;
    [Header("時間")]
    public float duration_shutter;
    [Header("タイプ")]
    public Ease easeType_shutter;

    public CourseController courseCtrl;

    private List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    /*
    private void OnDisable()
    {
        if (DOTween.instance != null)
        {
            tweenList.KillAllAndClear();
            // tween?.Kill();// Tween破棄
        }
    }
     */


    void Start()
    {
        Initialize();
        ShutterOpen(false);
    }

    void Initialize()
    {
        shutter.anchoredPosition = new Vector2(openvalue_shutter, 0);
        gameStartPanel.anchoredPosition = new Vector2(0f, closevalue_startPanel);
        courseButton_Up.anchoredPosition = new Vector2(0f, closevalue_courseButton);
        courseButton_Down.anchoredPosition = new Vector2(0f, -closevalue_courseButton);
    }

    public void ShutterOpen(bool isComplete)
    {
        isCompleteShutter = false;
        Sequence seq_open = DOTween.Sequence().SetUpdate(false);

        seq_open.Append(gameStartPanel.DOAnchorPosY(openvalue_startPanel, duration_startPanel).SetEase(easeType_startPanel));
        seq_open.Join(courseButton_Up.DOAnchorPosY(openvalue_courseButton, duration_courseButton).SetEase(easeType_courseButton));
        seq_open.Join(courseButton_Down.DOAnchorPosY(-openvalue_courseButton, duration_courseButton).SetEase(easeType_courseButton));

        tweenList.Add(seq_open);

        if (isComplete)
        {
            seq_open.Complete();
        }
    }

    public void ShutterClose(bool isComplete)
    {
        Sequence seq_close = DOTween.Sequence();

        seq_close.AppendInterval(0.2f);
        seq_close.Append(gameStartPanel.DOAnchorPosY(closevalue_startPanel, duration_startPanel).SetEase(easeType_startPanel));
        seq_close.AppendCallback(() => courseCtrl.FadeOutItems(isComplete));
        seq_close.Join(courseButton_Up.DOAnchorPosY(closevalue_courseButton, duration_courseButton).SetEase(easeType_courseButton));
        seq_close.Join(courseButton_Down.DOAnchorPosY(-closevalue_courseButton, duration_courseButton).SetEase(easeType_courseButton));
        seq_close.AppendInterval(0.1f);
        seq_close.Append(shutter.DOAnchorPosX(closevalue_shutter, duration_shutter).SetEase(easeType_shutter));
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

}
