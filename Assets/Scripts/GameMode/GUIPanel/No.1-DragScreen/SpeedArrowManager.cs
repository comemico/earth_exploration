using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class SpeedArrowManager : MonoBehaviour
{
    [Header("子: RectTransform配列")] [SerializeField] private RectTransform[] gearRect;   //子
    [Header("子: Image配列")] [SerializeField] private Image[] gearImage;          //子
    [Header("孫: Image配列")] [SerializeField] private Image[] gearIssuer;          //孫
    [Header("つまみ")] public RectTransform knob;
    [Header("つまみ : 発行体")] public Image knobIssuer;
    [Header("Body : 色")] public Color bodyColor;
    [Header("Canvas:表示までの時間")] public float fadeinTime;
    [Header("Canvas:消失までの時間")] public float fadeoutTime;
    [Header("ギア間の幅")] public int gearWide;
    [Header("イージングの種類")] public Ease easeType;
    CanvasGroup canvasGroup;
    //Sequence sequenceA;

    void Start()
    {
        //sequenceA = DOTween.Sequence();
        canvasGroup = GetComponentInParent<CanvasGroup>();
        GetAllChildObject();
        Initialize();
    }

    void GetAllChildObject()
    {
        gearRect = new RectTransform[transform.childCount];
        gearImage = new Image[transform.childCount];
        gearIssuer = new Image[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            gearRect[i] = transform.GetChild(i).GetComponent<RectTransform>();
            gearImage[i] = transform.GetChild(i).GetComponent<Image>();
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
            canvasGroup.alpha = 1f;
            for (int i = 0; i < gearNum; i++)
            {
                if (i >= 1)
                {
                    gearRect[i].anchoredPosition = new Vector2(-1 * (gearWide * i), 0f);
                    gearImage[i].color = bodyColor;
                    gearIssuer[i].enabled = true;
                }
                else
                {
                    gearImage[i].color = bodyColor;
                    gearIssuer[i].enabled = true;
                    knobIssuer.enabled = true;
                }
            }
        }
    }


    public void ChargeGear(int gearNum, float medianValue)
    {

        switch (gearNum)
        {
            case 0:
                //canvasGroup.alpha = 0.45f + ((factorDistance * medianValue) * 0.55f);
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
        canvasGroup.DOFade(0f, fadeoutTime).OnComplete(() =>
        {
            Initialize();
        });
        //sequenceA.Append(gearRect[1].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        //.Join(gearRect[2].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        //.Join(gearRect[3].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        //.Join(gearRect[4].DOAnchorPos(gearRect[0].anchoredPosition, fadeoutTime).SetEase(easeType))
        //.Append(canvasGroup.DOFade(0f, fadeoutTime))
        //.AppendCallback(Initialize);
    }

    //強制終了時、完了状態になるようにする
    public void CompleteSequence(int key)
    {
        gearRect[1].DOComplete();
        gearRect[2].DOComplete();
        gearRect[3].DOComplete();
        gearRect[4].DOComplete();
        knob.DOComplete();
        canvasGroup.alpha = 0f;
        //canvasGroup.DOComplete();
        Initialize();
        transform.parent.localScale = new Vector3(key, 1f, 1f);
    }



}
