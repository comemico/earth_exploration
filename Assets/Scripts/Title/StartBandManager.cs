using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartBandManager : MonoBehaviour
{
    [Header("Screen")]
    public Button startUpButton;
    public RectTransform screen;
    [Range(0.15f, 0.5f)] public float screenMoveTime;
    public Ease screenMoveType;
    Image lampImg;

    [Header("Title")]
    public CanvasGroup backPanel;
    public Image tipsEdge;
    [Range(0.1f, 0.5f)] public float tipsTime;
    public Ease tipsType;

    [Header("PressStart")]
    public RectTransform pressStart;
    public CanvasGroup textCanvas;
    Tween tween_pressStart;

    [Header("CheckMark")]
    public RectTransform checkMark;
    public float distance;
    public float easeDuration;
    public Ease easeType;
    Tween tween_check;

    TitleManager titleMg;

    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        GetComponent();
        tween_check = checkMark.DOAnchorPosY(distance, easeDuration).SetEase(easeType).SetLoops(-1, LoopType.Yoyo).SetRelative(true);
        tween_pressStart = textCanvas.DOFade(0.25f, 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

        tweenList.Add(tween_check);
        tweenList.Add(tween_pressStart);
    }

    void GetComponent()
    {
        lampImg = startUpButton.transform.GetChild(0).GetComponent<Image>();
        titleMg = transform.root.GetComponent<TitleManager>();
        startUpButton.onClick.AddListener(WakeUp);
    }


    public void WakeUp() //PressStartボタンで呼ばれる
    {
        startUpButton.image.raycastTarget = false;
        Sequence seq_wakeUp = DOTween.Sequence();

        //PressStart => ランプ点灯
        seq_wakeUp.Append(pressStart.DOAnchorPosY(-150, 0.3f).SetEase(Ease.OutSine));
        seq_wakeUp.Append(lampImg.DOFade(1f, 0.35f).SetEase(Ease.OutSine));
        seq_wakeUp.AppendInterval(0.25f); //この間にグリプスの起動アニメを完了させる
        seq_wakeUp.AppendCallback(() => SoundManager.Instance.PlayBGM(BGMSoundData.BGM.Title));

        //タイトル消失 => カメラの引き
        seq_wakeUp.Append(backPanel.DOFade(0f, tipsTime).SetEase(tipsType));
        seq_wakeUp.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(235, 235), tipsTime).SetEase(tipsType));
        seq_wakeUp.Append(tipsEdge.DOFade(0f, tipsTime).SetEase(tipsType));
        seq_wakeUp.AppendCallback(() => titleMg.cinemachineMg.Zoom(55));

        //パネル待機
        seq_wakeUp.Join(screen.DOAnchorPosY(-350, screenMoveTime).SetEase(screenMoveType));
        seq_wakeUp.Append(titleMg.selectBandMg.StartUp());

        tweenList.Add(seq_wakeUp);
    }

}
