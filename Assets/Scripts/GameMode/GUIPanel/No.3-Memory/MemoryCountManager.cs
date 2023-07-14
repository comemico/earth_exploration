using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryCountManager : MonoBehaviour
{
    public RectTransform aheadNumber;
    public RectTransform lateNumber;
    public float distanceY;

    [Space(20)]
    [Header("イージング")]
    public Ease easeType;
    public float duration;

    private int num;
    private Text aheadNumberText;
    private Text lateNumberText;

    public void DisplayMemoryNumber(int memoryNum)
    {
        aheadNumber.DOComplete();
        lateNumber.DOComplete();
        if (num != memoryNum)
        {
            if (num > memoryNum)//メモリを消費する場合
            {
                aheadNumber.anchoredPosition = new Vector2(0f, distanceY);
                aheadNumber.DOAnchorPosY(-distanceY, duration).SetEase(easeType).SetRelative(true);
                lateNumber.anchoredPosition = Vector2.zero;
                lateNumber.DOAnchorPosY(-distanceY, duration).SetEase(easeType).SetRelative(true);
            }
            else
            {
                aheadNumber.anchoredPosition = new Vector2(0f, -distanceY);
                aheadNumber.DOAnchorPosY(distanceY, duration).SetEase(easeType).SetRelative(true);
                lateNumber.anchoredPosition = Vector2.zero;
                lateNumber.DOAnchorPosY(distanceY, duration).SetEase(easeType).SetRelative(true);
            }
            aheadNumberText.text = memoryNum.ToString();
            lateNumberText.text = num.ToString();
            num = memoryNum;
        }
    }

    public void InitializeMemoryCount(int memoryNum)
    {
        num = memoryNum;
        aheadNumberText = aheadNumber.gameObject.transform.GetChild(0).GetComponent<Text>();
        aheadNumberText.text = memoryNum.ToString();
        lateNumberText = lateNumber.gameObject.transform.GetChild(0).GetComponent<Text>();
        lateNumberText.text = memoryNum.ToString();
    }

}
