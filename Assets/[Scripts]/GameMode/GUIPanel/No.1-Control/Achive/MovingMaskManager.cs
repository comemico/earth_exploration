using UnityEngine;
using Coffee.UISoftMask;
using DG.Tweening;


public class MovingMaskManager : MonoBehaviour
{
    [Header("�C�[�W���O-SoftMask: Alpha-")]
    [Header("���")]
    public Ease easeType;
    [Header("����")]
    public float easeDuration;
    [Header("�ŏ��l")]
    public int maskSizeMin;
    [Header("�ő�l")]
    public int maskSizeMax;

    Vector2 screenSize;
    RectTransform maskRect;
    SoftMask soft;
    RectTransform screenRect;

    /* GCAloc�h�~ */
    DG.Tweening.Core.DOGetter<float> softMaskAlphaGetter;
    DG.Tweening.Core.DOSetter<float> softMaskAlphaSetter;
    Tween softMaskAlpha;

    private void Start()
    {
        softMaskAlphaGetter = () => soft.alpha;

        softMaskAlphaSetter = (value) =>
         {
             soft.alpha = value;
         };

        InitializeComponent();
        soft.alpha = 0f;
        maskRect.sizeDelta = new Vector2(maskSizeMin, maskSizeMin);
        screenSize = transform.root.GetComponent<RectTransform>().sizeDelta;
        screenRect.sizeDelta = screenSize;
    }

    void InitializeComponent()
    {
        maskRect = transform.GetComponent<RectTransform>();
        soft = transform.GetComponent<SoftMask>();
        screenRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    public void FadeInMovingMask(Vector2 position)
    {
        maskRect.anchoredPosition = position * screenSize;
        screenRect.anchoredPosition = -(position * screenSize);
        maskRect.DOSizeDelta(new Vector2(maskSizeMax, maskSizeMax), easeDuration).SetEase(easeType);


        softMaskAlpha.Complete();
        //soft.alpha = 0f;
        softMaskAlpha = DOTween.To(softMaskAlphaGetter, softMaskAlphaSetter, 1f, easeDuration).SetEase(easeType);
        /*
        DOTween.To(() => soft.alpha, value =>
        {
            soft.alpha = value;
        }
        , 1f, easeDuration).SetEase(easeType);
         */


    }

    public void FadeOutMovingMask()
    {
        maskRect.DOSizeDelta(new Vector2(maskSizeMin, maskSizeMin), easeDuration).SetEase(easeType);

        //softMaskAlpha.Complete();
        //soft.alpha = 1f;
        softMaskAlpha = DOTween.To(softMaskAlphaGetter, softMaskAlphaSetter, 0f, easeDuration).SetEase(easeType);
        /*
        DOTween.To(() => soft.alpha, value =>
        {
            soft.alpha = value;
        }
        , 0f, easeDuration).SetEase(easeType);
         */


    }

    public void OnDragMovingMask(Vector2 currentPosition)
    {
        maskRect.anchoredPosition = currentPosition * screenSize;
        screenRect.anchoredPosition = -(currentPosition * screenSize);
    }





}
