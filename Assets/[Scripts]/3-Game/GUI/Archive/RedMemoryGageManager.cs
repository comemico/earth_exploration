using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class RedMemoryGageManager : MonoBehaviour
{
    [Header("Ôƒƒ‚ƒŠ“ü‚ê” ")] public Image[] memoryGageBoxRed;
    public int old;
    public int num;
    public int now;
    public float time = 1.0f;
    private Tween redMemoryTween;


    void Update()
    {
        if (old != now)
        {
            DisplayGage(now);
            old = now;
        }
    }

    void DisplayGage(int gagenum)
    {
        for (int i = 0; i < memoryGageBoxRed.Length; i++)
        {
            memoryGageBoxRed[i].enabled = (gagenum < i);
        }
    }

    public void RedMemoryGageMove(int consumeGage)
    {

        redMemoryTween = DOTween.To(() => old,
x =>
{
    now = x;
},
now,
time
);

    }
}
