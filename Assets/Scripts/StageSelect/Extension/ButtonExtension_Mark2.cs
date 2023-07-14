using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonExtension_Mark2 : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler

{
    const float PreHeader = 20f;

    [Space(PreHeader)]
    [Header("ボタン縮小")]
    [Header("-----------------------------")]
    public Ease easeType;
    public float duration;

    [Space(PreHeader)]
    [Header("ボタン縮小")]
    [Header("-----------------------------")]
    public Image bloomImage;
    public Color color_Press;
    public Color color_Onlight;
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic);
        bloomImage.enabled = true;
        Debug.Log(bloomImage.color);
        bloomImage.color = color_Press;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.1f).SetEase(Ease.OutCubic);
        bloomImage.color = color_Onlight;
    }

}
