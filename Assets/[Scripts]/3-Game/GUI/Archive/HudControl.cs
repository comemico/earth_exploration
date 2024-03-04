using UnityEngine;

public class HudControl : MonoBehaviour
{
    public Transform targetTrans;
    public Vector3 offset;//= Vector3.zero;

    RectTransform myRectTrans;
    RectTransform parentRectTrans;

    Camera uiCamera;

    private void Awake()
    {
        myRectTrans = GetComponent<RectTransform>();
        parentRectTrans = (RectTransform)myRectTrans.parent;

        var canvasArr = GetComponentsInParent<Canvas>();
        for (int i = 0; i < canvasArr.Length; i++)
        {
            if (canvasArr[i].isRootCanvas)
            {
                uiCamera = canvasArr[i].worldCamera;
            }
        }
    }


    public void UpdateUiLocalPosFromTargetPos(int key)
    {
        if (targetTrans != null && myRectTrans != null && parentRectTrans != null && uiCamera != null)
        {
            var screenPos = Camera.main.WorldToScreenPoint(targetTrans.position + offset);
            var localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTrans, screenPos, uiCamera, out localPos);
            myRectTrans.localPosition = localPos;
        }
    }
}
