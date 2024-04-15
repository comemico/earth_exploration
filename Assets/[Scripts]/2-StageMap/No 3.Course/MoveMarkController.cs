using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveMarkController : MonoBehaviour
{
    [Header("�C�[�W���O�̎��")]
    public Ease easeType;

    [Header("�C�[�W���O�̎���")]
    public float easeDuration;

    [Header("�Ԋu")]
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
