using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveMarkController : MonoBehaviour
{
    [Header("イージングの種類")]
    public Ease easeType;

    [Header("イージングの時間")]
    public float easeDuration;

    [Header("間隔")]
    public float distance;

    Tween tween;
    private List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.Add(tween);
        tweenList.KillAllAndClear();
    }


    void Start()
    {
        tween = transform.GetComponent<RectTransform>().DOAnchorPosY(distance, easeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
    }

}
