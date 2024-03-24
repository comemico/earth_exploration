using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StageFrameManager : MonoBehaviour
{
    [Header("Scope")]
    public ScopeManager scopeMg;

    [Header("Lv.Memory")]
    public LevelRingManager lvMg;

    [Header("Tips")]
    public TipsManager tipsMg;
    public Color[] rankColor;


    public RectTransform RectTransform => this.transform as RectTransform;

    Sequence S_ChangeTarget;

    public void ChangeTarget(int levelNum, string tips)
    {
        lvMg.OpenLvRing(levelNum);

        tipsMg.tipsText.text = tips;
        float value = levelNum / 8f;
        tipsMg.rankLamp.color = rankColor[(int)Mathf.Ceil(value) - 1];
        tipsMg.ShowTips();

        S_ChangeTarget = DOTween.Sequence();
        S_ChangeTarget.AppendCallback(() =>
        {
            scopeMg.CompleteScope();
        });
        S_ChangeTarget.Append(scopeMg.TargetScope());
    }

}
