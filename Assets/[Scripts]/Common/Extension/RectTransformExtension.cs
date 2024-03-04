using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension
{

    public static Vector2 GetAnchoredPos(this RectTransform self)
    {
        return self.anchoredPosition;
    }

    // X�̃A���J�[�ʒu���擾����
    public static float GetAnchoredPosX(this RectTransform self)
    {
        return self.anchoredPosition.x;
    }

    // Y�̃A���J�[�ʒu���擾����
    public static float GetAnchoredPosY(this RectTransform self)
    {
        return self.anchoredPosition.y;
    }

    // �I�u�W�F�N�g�̊g�嗦�̐ݒ�
    public static void SetLocalScaleXY(this Transform self, float xy)
    {
        Vector3 scale = self.localScale;
        scale.x = xy;
        scale.y = xy;
        self.localScale = scale;
    }
}
