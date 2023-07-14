using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class List_Tween_Extension
{
    // ���X�g���̂��ׂẴA�j���[�V�������~���܂�
    public static void KillAllAndClear(this List<Tween> self)
    {
        //Debug.Log(self.Count);
        self.ForEach(tween => tween.Kill(true));
        self.Clear();
    }
}