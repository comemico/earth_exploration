using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryCountManager_Unused : MonoBehaviour
{
    public RectTransform jumpRect;
    public RectTransform slideRect;
    public float jumpPositionY;

    [Space(20)]

    [Header("イージング")]
    public Ease easeType;
    public float duration;

    private RectTransform mid;
    private Text oddNum;
    private Text evenNum;

    public void InitializeMemoryCount(int memoryNum)
    {
        oddNum = jumpRect.gameObject.transform.GetChild(0).GetComponent<Text>();
        evenNum = slideRect.gameObject.transform.GetChild(0).GetComponent<Text>();
        evenNum.text = memoryNum.ToString();

        int num = memoryNum;
        num--;

        if (num % 2 == 0)//偶数
        {
            evenNum.text = num.ToString();
        }
        else //奇数
        {
            mid = jumpRect;
            jumpRect = slideRect;
            slideRect = mid;
            oddNum.text = num.ToString();
        }

    }

    public void DownButton(int memoryNum)
    {
        //再生途中に呼ばれた場合、開始位置を初期化する
        jumpRect.DOComplete();
        slideRect.DOComplete();

        if (memoryNum % 2 == 0)//偶数
        {
            evenNum.text = memoryNum.ToString();
        }
        else //奇数
        {
            oddNum.text = memoryNum.ToString();
        }

        //上位置に移動
        jumpRect.anchoredPosition = new Vector2(0f, jumpPositionY);

        //等しく下へスライド
        jumpRect.DOAnchorPosY(-100f, duration).SetEase(easeType).SetRelative(true);
        slideRect.DOAnchorPosY(-100f, duration).SetEase(easeType).SetRelative(true);

        //参照元を入れ替える
        mid = jumpRect;
        jumpRect = slideRect;
        slideRect = mid;
    }

    public void UpButton(int memoryNum)
    {
        //再生途中に呼ばれた場合、開始位置を初期化する
        jumpRect.DOComplete();
        slideRect.DOComplete();

        if (memoryNum % 2 == 0)//偶数
        {
            evenNum.text = memoryNum.ToString();
        }
        else //奇数
        {
            oddNum.text = memoryNum.ToString();
        }

        //下位置に移動
        jumpRect.anchoredPosition = new Vector2(0f, jumpPositionY * -1);
        //jumpRect.anchoredPosition = rectDown.anchoredPosition;

        //等しく上へスライド
        jumpRect.DOAnchorPosY(100f, duration).SetEase(easeType).SetRelative(true);
        slideRect.DOAnchorPosY(100f, duration).SetEase(easeType).SetRelative(true);

        //参照元を入れ替える
        mid = jumpRect;
        jumpRect = slideRect;
        slideRect = mid;
    }

}
