using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public static class List_Tween_Extension
{
    public static void KillAllAndClear(this List<Tween> self)// ���X�g���̂��ׂẴA�j���[�V�������~���܂�
    {
        self.ForEach(tween => tween.Kill(true));//sequence�����Ă���ꍇ�A1��sequence��Kill���Ă��邱�ƂɂȂ�
        self.Clear();
    }
}