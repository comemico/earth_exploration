using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CourseFrameManager : MonoBehaviour
{
    const float PreHeader = 20f;

    public RectTransform RectTransform => this.transform as RectTransform;

    [Space(PreHeader)]
    [Header("ターゲットスコープ")]
    [Header("-----------------------------")]
    [Header("移動量")]
    public float distance;
    [Header("時間")]
    public float duration_Move;
    public float duration_Fade;
    [Header("タイプ")]
    public Ease easeType_Move;
    public Ease easeType_Fade;

    CanvasGroup canvasGroup;
    RectTransform mark_Rect;
    Vector2 initialVector;

    Tween tween = null;
    private List<Tween> tweenList = new List<Tween>();


    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        mark_Rect = transform.GetChild(0).GetComponent<RectTransform>();
        initialVector.x = -distance;
    }


    public void SelectCourse()
    {
        mark_Rect.DOComplete();
        mark_Rect.anchoredPosition = initialVector;
        tween = mark_Rect.DOAnchorPosX(distance, duration_Move).SetEase(easeType_Move).SetRelative(true);

        canvasGroup.DOComplete();
        canvasGroup.alpha = 0f;
        tween = canvasGroup.DOFade(1f, duration_Fade).SetEase(easeType_Fade);

        tweenList.Add(tween);
    }
}
