using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpeedPowerManager : MonoBehaviour
{
    [Header("矢")]
    [Header("Body : 色")]
    public Color bodyColor;
    [Header("ギア間の幅")]
    public int gearWide;

    [Header("イージング-Release-")]
    [Header("時間")]
    public float fadeoutTime;
    [Header("種類")]
    public Ease easeType;


    [Header("子: RectTransform配列")] [SerializeField] private RectTransform[] gearRect;   //子
    [Header("子: Image配列")] [SerializeField] private Image[] gearImage;          //子
    [Header("孫: Image配列")] [SerializeField] private Image[] gearIssuer;          //孫
    [Header("つまみ")] public RectTransform knob;
    [Header("つまみ : 発行体")] public Image knobIssuer;

    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    //Sequence sequenceA;
    Tween tween_offset;

    void Start()
    {
        //sequenceA = DOTween.Sequence();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        GetAllChildObject();
        Initialize();
    }

    void GetAllChildObject()
    {
        gearRect = new RectTransform[transform.GetChild(0).childCount];
        gearImage = new Image[gearRect.Length];
        gearIssuer = new Image[gearRect.Length];

        for (int i = 0; i < gearRect.Length; i++)
        {
            gearRect[i] = transform.GetChild(0).GetChild(i).GetComponent<RectTransform>();
            gearImage[i] = transform.GetChild(0).GetChild(i).GetComponent<Image>();
            gearIssuer[i] = gearImage[i].transform.GetChild(0).GetComponent<Image>();
        }

    }

    public void Initialize()
    {
        //[1]～[4]位置初期
        for (int i = 1; i < gearRect.Length; i++)
        {
            gearRect[i].anchoredPosition = new Vector2(-1 * (gearWide * (i - 1)), 0f);
        }
        //[0]～[4]ランプOff
        for (int i = 0; i < gearImage.Length; i++)
        {
            gearIssuer[i].enabled = false;
            knobIssuer.enabled = false;
        }
        //[1]～[4]透明
        for (int i = 1; i < gearImage.Length; i++)
        {
            gearImage[i].color = new Color(0.75f, 0.75f, 0.75f, 0f);
        }
    }

    public void DisplaySpeedArrow(int gearNum)
    {
        if (gearNum >= 0)
        {
            Initialize();
            for (int i = 0; i < gearNum; i++)
            {
                if (i >= 1)
                {
                    gearRect[i].anchoredPosition = new Vector2(-1 * (gearWide * i), 0f);
                    gearImage[i].color = bodyColor;
                    gearIssuer[i].enabled = true;
                }

                gearImage[i].color = bodyColor;
                gearIssuer[i].enabled = true;
                knobIssuer.enabled = true;

            }
        }
    }


    public void ChargeGear(int gearNum, float medianValue)
    {

        switch (gearNum)
        {
            case 0:
                canvasGroup.alpha = 0.45f + (medianValue * 0.55f);
                break;

            case 1:
                gearRect[1].anchoredPosition = new Vector2(-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide)), 0f);
                gearImage[1].color = new Color(0.75f, 0.75f, 0.75f, medianValue * 0.8f);
                break;

            case 2:
                gearRect[2].anchoredPosition = new Vector2(-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide)), 0f);
                gearImage[2].color = new Color(0.75f, 0.75f, 0.75f, medianValue * 0.8f);
                break;

            case 3:
                gearRect[3].anchoredPosition = new Vector2(-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide)), 0f);
                gearImage[3].color = new Color(0.75f, 0.75f, 0.75f, medianValue * 0.8f);
                break;

            case 4:
                gearRect[4].anchoredPosition = new Vector2(-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide)), 0f);
                gearImage[4].color = new Color(0.75f, 0.75f, 0.75f, medianValue * 0.8f);
                break;
        }
        if (gearNum >= 1)
        {
            knob.anchoredPosition = new Vector2(-1 * ((gearWide * (gearNum - 1)) + (medianValue * gearWide)), 0f);
        }
    }


    public void Release()
    {
        gearRect[1].DOAnchorPosX(gearRect[0].anchoredPosition.x, fadeoutTime).SetEase(easeType);
        gearRect[2].DOAnchorPosX(gearRect[0].anchoredPosition.x, fadeoutTime).SetEase(easeType);
        gearRect[3].DOAnchorPosX(gearRect[0].anchoredPosition.x, fadeoutTime).SetEase(easeType);
        gearRect[4].DOAnchorPosX(gearRect[0].anchoredPosition.x, fadeoutTime).SetEase(easeType);
        knob.DOAnchorPosX(gearRect[0].anchoredPosition.x, fadeoutTime).SetEase(easeType);
        canvasGroup.DOFade(0f, fadeoutTime);
        /*
        DOTween.To(() => rectTransform.offsetMin.x, value =>
        {
            rectTransform.offsetMin = new Vector2(value, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-value, rectTransform.offsetMax.y);
        }, 500f, fadeoutTime).SetEase(easeType);
         */


        /*
        .OnComplete(() =>
        {
            Initialize();
        });
        sequenceA.Append(gearRect[1].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        .Join(gearRect[2].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        .Join(gearRect[3].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        .Join(gearRect[4].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        .Append(canvasGroup.DOFade(0f, fadeoutTime))
        .AppendCallback(Initialize);
         */
    }
    //強制終了時、完了状態になるようにする
    public void GetReadyCharge(int key)
    {

        gearRect[1].DOComplete();
        gearRect[2].DOComplete();
        gearRect[3].DOComplete();
        gearRect[4].DOComplete();
        knob.DOComplete();
        canvasGroup.DOComplete();


        Initialize();

        canvasGroup.alpha = 0.45f;

        transform.localScale = new Vector3(key, 1f, 1f);

        tween_offset.Complete();
        rectTransform.offsetMin = new Vector2(600f, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(-600f, rectTransform.offsetMax.y);

        tween_offset = DOTween.To(() => rectTransform.offsetMin.x, value =>
        {
            rectTransform.offsetMin = new Vector2(value, rectTransform.offsetMin.y);
            rectTransform.offsetMax = new Vector2(-value, rectTransform.offsetMax.y);
        }
       , 500f, fadeoutTime).SetEase(easeType);


    }


}
