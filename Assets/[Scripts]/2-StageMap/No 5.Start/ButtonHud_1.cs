using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHud_1 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button button;

    public RectTransform RectTransform => this.transform as RectTransform;

    [Header("StartUp")]
    public int height;
    [Range(0.1f, 1f)] public float heightTime;
    public float overshoot;

    [Header("Click")]
    public int depth;
    [Range(0.1f, 1f)] public float clickTime;
    public Ease clickType;

    [Header("Lamp")]
    public Image lamp_L;
    public Image lamp_R;
    public Color lampOff;
    public Color lampOn;
    [Range(0.1f, 1f)] public float lampTime;
    public Ease lampType;

    void Awake()
    {
        RectTransform.anchoredPosition = new Vector2(0f, -height);
        lamp_L.color = lampOff;
        lamp_R.color = lampOff;

        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlaySE(SESoundData.SE.Button_Choice);
            SoundManager.Instance.seAudioSource.pitch = 1f;
        });

    }

    public void StartUp()
    {
        Sequence s_startUp = DOTween.Sequence();
        s_startUp.Append(RectTransform.DOAnchorPosY(0f, heightTime).SetEase(Ease.OutBack, overshoot));
        s_startUp.Join(lamp_L.DOColor(lampOn, lampTime).SetEase(lampType).SetDelay(0f));
        s_startUp.Join(lamp_R.DOColor(lampOn, lampTime).SetEase(lampType));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransform.DOAnchorPosY(-depth, clickTime).SetEase(clickType).SetRelative(true);

        //transform.DOScale(pressSize, duration).SetEase(easeType);
        //bloomImage.enabled = true;
        //Debug.Log(bloomImage.color);
        //bloomImage.color = color_Press;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        RectTransform.DOAnchorPosY(depth, clickTime).SetEase(clickType).SetRelative(true);

        //transform.DOScale(1f, duration).SetEase(easeType);
        //bloomImage.color = color_Onlight;
    }

}
