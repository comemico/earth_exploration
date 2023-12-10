using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartBandManager : MonoBehaviour
{
    [Header("StartButton")]
    public Button startUpButton;
    Image lampImg;

    [Header("Screen")]
    public RectTransform screen;
    [Range(0.15f, 0.5f)] public float screenMoveTime;
    public Ease screenMoveType;


    [Header("Title")]
    public RectTransform title;
    [Range(0.1f, 0.5f)] public float titleFadeInDuration;
    public Ease titleFadeInType;
    [Range(0.1f, 0.5f)] public float titleFadeOutDuration;
    public Ease titleFadeOutType;

    [Header("Tips")]
    public Image tipsEdge;
    public CanvasGroup backPanel;
    public Text textPress;
    public Text textStart;
    [Range(0.1f, 0.5f)] public float tipsFadeDuration;
    public Ease tipsEdgeType;
    public Ease tipsFadeType;
    const int INIDISX_PressStart = 35;
    const int TIPSWIDE = 1200;
    const int TIPSHEIGHT = 215;

    [Header("CheckMark")]
    public RectTransform checkMark;
    public float distance;
    public float easeDuration;
    public Ease easeType;
    Tween tween;

    TitleManager titleMg;

    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Start()
    {
        GetComponent();
        Initialize();
        tween = checkMark.DOAnchorPosY(distance, easeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
        tweenList.Add(tween);
    }

    void GetComponent()
    {
        lampImg = startUpButton.transform.GetChild(0).GetComponent<Image>();
        titleMg = transform.root.GetComponent<TitleManager>();
        startUpButton.onClick.AddListener(WakeUp);
    }

    private void Initialize()
    {
        startUpButton.image.raycastTarget = false;
        backPanel.alpha = 0f;
        tipsEdge.color = new Color(1, 1, 1, 0);
        tipsEdge.rectTransform.sizeDelta = new Vector2(192f, 192f);

        textPress.rectTransform.anchoredPosition = new Vector2(textPress.rectTransform.anchoredPosition.x + INIDISX_PressStart, 0f);
        textStart.rectTransform.anchoredPosition = new Vector2(textStart.rectTransform.anchoredPosition.x - INIDISX_PressStart, 0f);
    }

    public Sequence StartUp() //start()で呼ばれる
    {
        Sequence seq_startUp = DOTween.Sequence();
        seq_startUp.AppendInterval(0.125f);

        //Tips出現
        seq_startUp.Append(tipsEdge.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));
        seq_startUp.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(TIPSWIDE, TIPSHEIGHT), tipsFadeDuration).SetEase(tipsEdgeType));
        seq_startUp.Append(backPanel.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));
        seq_startUp.Join(textPress.rectTransform.DOAnchorPosX(-INIDISX_PressStart, 0.3f).SetEase(Ease.OutSine).SetRelative(true));
        seq_startUp.Join(textStart.rectTransform.DOAnchorPosX(INIDISX_PressStart, 0.3f).SetEase(Ease.OutSine).SetRelative(true));
        seq_startUp.AppendCallback(() => startUpButton.image.raycastTarget = true);
        tweenList.Add(seq_startUp);
        return seq_startUp;
    }

    public void WakeUp() //PressStartボタンで呼ばれる
    {
        startUpButton.image.raycastTarget = false;
        Sequence seq_wakeUp = DOTween.Sequence();

        //ランプ点灯
        seq_wakeUp.Append(lampImg.DOFade(1f, 0.25f));

        seq_wakeUp.AppendInterval(0.15f);

        //タイトル & Tips消失
        seq_wakeUp.Append(backPanel.DOFade(0f, tipsFadeDuration).SetEase(tipsFadeType));
        seq_wakeUp.Join(title.DOAnchorPosY(-100, 0.25f).SetEase(titleFadeOutType));
        seq_wakeUp.AppendCallback(() => titleMg.cinemachineMg.Zoom(55));
        seq_wakeUp.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(TIPSHEIGHT, TIPSHEIGHT), tipsFadeDuration).SetEase(tipsEdgeType));
        seq_wakeUp.Append(tipsEdge.DOFade(0f, tipsFadeDuration).SetEase(tipsEdgeType));

        //パネル消失
        seq_wakeUp.Append(screen.DOAnchorPosY(-182.5f, screenMoveTime).SetEase(screenMoveType));
        seq_wakeUp.Append(titleMg.selectBandMg.StartUp());

        tweenList.Add(seq_wakeUp);
    }

}
