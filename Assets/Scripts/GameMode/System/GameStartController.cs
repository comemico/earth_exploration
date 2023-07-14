using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameStartController : MonoBehaviour
{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("シャッター")]
    [Header("-----------------------------")]
    public RectTransform shutter;

    [Header("開いた値")]
    public float openvalue_shutterY;
    [Header("閉じた値")]
    public float closevalue_shutterY;
    [Header("時間")]
    public float duration_shutter;
    [Header("タイプ")]
    public Ease easeType_shutter;


    private List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }


    void Start()
    {
        ShutterClose(true);
        ShutterOpen(FadeCanvasManager.instance.isFade);
    }


    public void ShutterOpen(bool isComplete)
    {
        shutter.anchoredPosition = new Vector2(0f, closevalue_shutterY);

        Sequence seq_open = DOTween.Sequence().SetUpdate(false);
        seq_open.Append(shutter.DOAnchorPosY(openvalue_shutterY, duration_shutter).SetEase(easeType_shutter));

        if (isComplete)
        {
            seq_open.Complete();
        }
    }


    public void ShutterClose(bool isComplete)
    {
        Sequence seq_close = DOTween.Sequence();

        seq_close.Append(shutter.DOAnchorPosY(closevalue_shutterY, duration_shutter).SetEase(easeType_shutter));

        if (isComplete)
        {
            seq_close.Complete();
        }

    }
}
