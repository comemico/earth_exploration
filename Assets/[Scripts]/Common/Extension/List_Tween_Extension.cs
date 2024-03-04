using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class List_Tween_Extension
{
    public static void KillAllAndClear(this List<Tween> self)// リスト内のすべてのアニメーションを停止します
    {
        self.ForEach(tween => tween.Kill(true));//sequenceを入れている場合、1つのsequenceをKillしていることになる
        self.Clear();
    }
}