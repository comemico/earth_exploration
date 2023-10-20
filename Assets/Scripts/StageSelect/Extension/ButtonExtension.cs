using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonExtension : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler

{
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutQuint);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, 0.1f).SetEase(Ease.OutCubic);
    }
}
