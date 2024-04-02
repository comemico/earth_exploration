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

    [Header("リール")]
    public RectTransform bgmReel;
    public RectTransform seReel;
    [Range(0.1f, 0.35f)] public float reelTime = 0.175f;
    const int REEL_DISTANCE = 100;


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
                aheadNumber.DOAnchorPosY(-slideDistance, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f).SetRelative(true);
                lateNumber.anchoredPosition = Vector2.zero;
                lateNumber.DOAnchorPosY(-slideDistance, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f).SetRelative(true);
            }
            else
            {
                aheadNumber.anchoredPosition = new Vector2(0f, -slideDistance);
                aheadNumber.DOAnchorPosY(slideDistance, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f).SetRelative(true);
                lateNumber.anchoredPosition = Vector2.zero;
                lateNumber.DOAnchorPosY(slideDistance, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f).SetRelative(true);
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
