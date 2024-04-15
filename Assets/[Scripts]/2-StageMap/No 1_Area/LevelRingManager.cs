using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelRingManager : MonoBehaviour
{
    [Header("Lv.Memory")]
    public Image lvImg;
    public Image[] lvLamp;
    [Space(10)]

    [Range(0.1f, 1f)] public float openTime;
    public Ease openType;
    [Range(0.1f, 1f)] public float lampTime;
    public Ease lampType;
    [Space(10)]

    public float INITIALAMOUNT = 0.008f;
    public float PERAMOUNT = 0.0263f;
    public float INITIALANGLE = 4f;
    public float PERANGLE = 9.484f;

    Sequence S_LvRingOpen;
    Tween T_LvRingRevUp;

    private void Start()
    {
        InitializedLvRing();
    }

    private void InitializedLvRing()
    {
        DisplayLevelMemory(0);
        lvImg.fillAmount = 0f;
        lvImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
    }

    public void CompleteLvRing()
    {
        S_LvRingOpen.Kill(true);
        T_LvRingRevUp.Kill(true);
        InitializedLvRing();
    }


    public Sequence OpenLvRing(int levelNum)
    {
        CompleteLvRing();
        S_LvRingOpen = DOTween.Sequence();
        S_LvRingOpen.Append(lvImg.DOFillAmount(INITIALAMOUNT + (PERAMOUNT * levelNum), openTime).SetEase(openType));
        S_LvRingOpen.Join(lvImg.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 90f - (INITIALANGLE + (PERANGLE * levelNum))), openTime, RotateMode.FastBeyond360).SetEase(openType));
        S_LvRingOpen.Append(RevUpLvRing(levelNum));
        return S_LvRingOpen;
    }

    public Tween RevUpLvRing(int levelNum)
    {
        T_LvRingRevUp = DOTween.To(() => 0, x => DisplayLevelMemory(x), levelNum, lampTime).SetEase(lampType);
        return T_LvRingRevUp;
    }

    public void DisplayLevelMemory(int levelNum)
    {
        for (int i = 0; i < lvLamp.Length; i++)
        {

            lvLamp[i].enabled = (levelNum > i);
        };
    }


}
