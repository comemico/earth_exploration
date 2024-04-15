using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridDisplayManager : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    MovingMaskManager movingMaskMg;
    Image crlImage;

    Vector2 screenFactor;
    Vector2 startPosition, currentPosition;
    float dragLength; //( 0.0～1.0 )

    private void Start()
    {
        GetComponent();
        screenFactor = new Vector2(1f / Screen.width, 1f / Screen.height);
    }

    private void GetComponent()
    {
        movingMaskMg = transform.parent.GetComponentInChildren<MovingMaskManager>();
        crlImage = GetComponent<Image>();
    }

    public void ChangeControlLimit(StageCtrl.ControlStatus status)//, bool isInterval)空中時、ワープ中、etc...
    {
        crlImage.raycastTarget = (status != StageCtrl.ControlStatus.unControl);
        startPosition.x = currentPosition.x;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPosition = eventData.position * screenFactor;
        movingMaskMg.FadeInMovingMask(startPosition);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        movingMaskMg.FadeOutMovingMask();
    }

    public void OnEndDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData)
    {
        currentPosition = eventData.position * screenFactor;
        movingMaskMg.OnDragMovingMask(currentPosition);
        dragLength = Mathf.Abs(startPosition.x - currentPosition.x);
        //float power = Mathf.Clamp(dragLength * distanceFactor, 0, maxGage);
    }




}
