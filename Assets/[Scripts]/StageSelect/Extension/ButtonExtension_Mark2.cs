using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonExtension_Mark2 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Button button;

    [Header("É{É^Éìèkè¨")]
    [Range(0.75f, 1f)] public float pressSize;
    [Range(0.05f, 0.25f)] public float duration;
    public Ease easeType;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => SoundManager.Instance.PlaySE(SESoundData.SE.Button01));
    }

    /*
    [Space(PreHeader)]
    [Header("É{É^Éìèkè¨")]
    [Header("-----------------------------")]
    public Image bloomImage;
    public Color color_Press;
    public Color color_Onlight;
     */
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(pressSize, duration).SetEase(easeType);
        //bloomImage.enabled = true;
        //Debug.Log(bloomImage.color);
        //bloomImage.color = color_Press;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.DOScale(1f, duration).SetEase(easeType);
        //bloomImage.color = color_Onlight;
    }

}
