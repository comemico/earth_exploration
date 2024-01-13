using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformExtension
{

    public static Vector2 GetAnchoredPos(this RectTransform self)
    {
        return self.anchoredPosition;
    }

    // Xのアンカー位置を取得する
    public static float GetAnchoredPosX(this RectTransform self)
    {
        return self.anchoredPosition.x;
    }

    // Yのアンカー位置を取得する
    public static float GetAnchoredPosY(this RectTransform self)
    {
        return self.anchoredPosition.y;
    }

    // オブジェクトの拡大率の設定
    public static void SetLocalScaleXY(this Transform self, float xy)
    {
        Vector3 scale = self.localScale;
        scale.x = xy;
        scale.y = xy;
        self.localScale = scale;
    }
}
