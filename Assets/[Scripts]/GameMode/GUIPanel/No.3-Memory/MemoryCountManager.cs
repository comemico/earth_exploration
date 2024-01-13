using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MemoryCountManager : MonoBehaviour
{
    [Header("スライドカウント")]
    public RectTransform aheadNumber;
    public RectTransform lateNumber;
    public float slideDistance;
    [Range(0.1f, 0.2f)] public float slideDuration;
    public Ease slideType = Ease.OutSine;

    [Header("メモリランプ")]
    public Image produceLamp;
    public Image consumeLamp;

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
                aheadNumber.anchoredPosition = new Vector2(0f, slideDistance);
                aheadNumber.DOAnchorPosY(-slideDistance, slideDuration).SetEase(slideType).SetRelative(true);
                lateNumber.anchoredPosition = Vector2.zero;
                lateNumber.DOAnchorPosY(-slideDistance, slideDuration).SetEase(slideType).SetRelative(true);
            }
            else
            {
                aheadNumber.anchoredPosition = new Vector2(0f, -slideDistance);
                aheadNumber.DOAnchorPosY(slideDistance, slideDuration).SetEase(slideType).SetRelative(true);
                lateNumber.anchoredPosition = Vector2.zero;
                lateNumber.DOAnchorPosY(slideDistance, slideDuration).SetEase(slideType).SetRelative(true);
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

    public void ConsumeLamp()
    {
        consumeLamp.DOKill(true);
        consumeLamp.DOFade(1f, 0.15f).SetEase(Ease.OutFlash, 2);
    }

    public void ProduceLamp()
    {
        produceLamp.DOKill(true);
        produceLamp.DOFade(1f, 0.125f).SetEase(Ease.OutFlash, 2);
    }

}
