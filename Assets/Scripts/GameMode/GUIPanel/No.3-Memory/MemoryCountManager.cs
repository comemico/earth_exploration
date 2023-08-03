using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryCountManager : MonoBehaviour
{
    public RectTransform aheadNumber;
    public RectTransform lateNumber;

    public Image up;
    public Image down;

    [Header("イージング")]
    public float distanceY;
    public float duration;
    public Ease easeType;

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
        aheadNumberText = aheadNumber.GetComponent<Text>();
        aheadNumberText.text = memoryNum.ToString();
        lateNumberText = lateNumber.GetComponent<Text>();
        lateNumberText.text = memoryNum.ToString();
    }

    public void DownStatus(int gearNum)
    {
        down.enabled = (gearNum >= 1);
    }

}
