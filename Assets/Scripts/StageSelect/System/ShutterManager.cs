using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShutterManager : MonoBehaviour
{
    const float PreHeader = 20f;

    [Header("MemoryPanel")]
    public RectTransform memoryPanel;
    public float initialMemoryY;

    [Header("CoursePanel")]
    public RectTransform coursePanel;
    public float initialCourseX;

    [Header("HomePanel")]
    public RectTransform homePanel;
    public float initialHomeX;

    [Header("StartPanel")]
    public RectTransform startPanel;
    public float initialStartY;

    [Header("FirstMove")]
    [Range(0.1f, 0.25f)] public float firstDuration;
    public Ease firstType;

    [Header("SecondMove")]
    [Range(0.1f, 0.25f)] public float secondDuration;
    public Ease secondType;

    [Header("シーンに移れるかのシグナル")]
    public bool isCompleteShutter;

    /*
    [Space(PreHeader)]
    [Header("コースボタン")]
    [Header("-----------------------------")]
    public RectTransform courseButton_Up;
    public RectTransform courseButton_Down;
     */

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
        memoryPanel.anchoredPosition = new Vector2(-400f, initialMemoryY);
        coursePanel.anchoredPosition = new Vector2(initialCourseX, 0f);
        homePanel.anchoredPosition = new Vector2(initialHomeX, 0f);
        startPanel.anchoredPosition = new Vector2(0f, initialStartY);
    }

    public void ShutterOpen(bool isComplete)
    {
        isCompleteShutter = false;

        Sequence seq_open = DOTween.Sequence().SetUpdate(false);

        seq_open.Append(coursePanel.DOAnchorPosX(-1 * initialCourseX, firstDuration).SetEase(firstType));
        seq_open.Join(homePanel.DOAnchorPosX(0f, firstDuration).SetEase(firstType));

        seq_open.Append(memoryPanel.DOAnchorPosY(-1 * initialMemoryY, secondDuration).SetEase(secondType));
        seq_open.Join(startPanel.DOAnchorPosY(-1 * initialStartY, secondDuration).SetEase(secondType));

        seq_open.AppendCallback(() => courseCtrl.MoveCourse(GManager.instance.recentCourseNum, courseCtrl.fadeDuration));

        tweenList.Add(seq_open);

        if (isComplete)
        {
            seq_open.Complete();
        }
    }

    public void ShutterClose(bool isComplete)
    {
        Sequence seq_close = DOTween.Sequence();

        seq_close.AppendInterval(0.25f);

        seq_close.AppendCallback(() => courseCtrl.FadeOutItems());
        seq_close.AppendInterval(courseCtrl.fadeDuration);

        seq_close.Append(memoryPanel.DOAnchorPosY(initialMemoryY, secondDuration).SetEase(secondType));
        seq_close.Join(startPanel.DOAnchorPosY(initialStartY, secondDuration).SetEase(secondType));

        seq_close.Append(coursePanel.DOAnchorPosX(initialCourseX, firstDuration).SetEase(firstType));
        seq_close.Join(homePanel.DOAnchorPosX(initialHomeX, firstDuration).SetEase(firstType));

        seq_close.AppendInterval(0.1f);


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
