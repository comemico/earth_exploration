using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LongTap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private float time;
    private bool isDown = false;
    public float longTapTime = 1.0f;

    [SerializeField] private Image circleImage;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDown = true;
        time = 0f;
        circleImage.fillAmount = 0f;
        circleImage.color = Color.white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDown = false;
        circleImage.fillAmount = 0f;
        circleImage.color = Color.white;
    }

    private void Update()
    {
        if (isDown)
        {
            time += Time.deltaTime;
            circleImage.fillAmount = time / longTapTime;
            if (time >= longTapTime)
            {
                Debug.Log("LongTap");
                isDown = false;
                circleImage.color = Color.red;
            }
        }
    }

}
