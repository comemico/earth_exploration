using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    [Header("Boost")]
    public RectTransform boostMark;
    CanvasGroup boostCanvas;
    Sequence sequence; //ŠO‚Åkill‚·‚é—p

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        boostCanvas = boostMark.GetComponent<CanvasGroup>();
        boostCanvas.alpha = 0f;
    }
    public void LeadBoost()
    {
        Sequence seq_leadBoost = DOTween.Sequence().SetLoops(-1, LoopType.Restart);
        seq_leadBoost.AppendInterval(0.5f);
        seq_leadBoost.Append(boostMark.DOScale(0.85f, 0.35f));
        seq_leadBoost.Join(boostCanvas.DOFade(1f, 0.35f).SetEase(Ease.OutSine));
        seq_leadBoost.Append(boostMark.DOAnchorPosX(-250f, 0.75f).SetEase(Ease.InOutSine).SetRelative(true));
        seq_leadBoost.AppendInterval(0.25f);
        seq_leadBoost.Append(boostMark.DOScale(1.0f, 0.25f));
        seq_leadBoost.Join(boostCanvas.DOFade(0f, 0.25f).SetEase(Ease.OutSine));
        sequence = seq_leadBoost;
    }

    public void HideLeadMark()
    {
        boostCanvas.alpha = 0f;
        sequence.Kill(true);
        //boostMark.transform.gameObject.SetActive(false);
    }

}
