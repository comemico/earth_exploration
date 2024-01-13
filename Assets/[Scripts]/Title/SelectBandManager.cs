using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SelectBandManager : MonoBehaviour
{
    [Header("Screen")]
    public RectTransform screenRect;
    [Range(0.15f, 0.5f)] public float screenMoveTime;
    public Ease screenMoveType;
    const int INIPOSY = -260;

    [Header("LaunchButton")]
    public RectTransform launchButton;
    const int INIPOSX_LAUNCH = 375;

    [Header("TrafficButton")]
    public RectTransform exitButton;
    public RectTransform settingButton;
    public RectTransform creditButton;
    const int INIPOSX_TRAFFIC = -280;
    const int INIPOSY_TRAFFIC = 250;

    [Header("HowToPlayButton")]
    public RectTransform howtoButton;
    const int INIPOSX_HOW = 275;
    const int INIPOSY_HOW = 125;

    [Header("Time/Type")]
    [Range(0.15f, 0.5f)] public float buttonMoveTime;
    public Ease buttonMoveType;



    TitleManager titleMg;

    private void Start()
    {
        Initialize();
        titleMg = transform.root.GetComponent<TitleManager>();
    }

    void Initialize()
    {
        screenRect.anchoredPosition = new Vector2(0f, INIPOSY);
        launchButton.anchoredPosition = new Vector2(INIPOSX_LAUNCH, INIPOSY_TRAFFIC);
        exitButton.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        settingButton.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        creditButton.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        howtoButton.anchoredPosition = new Vector2(INIPOSX_HOW, INIPOSY_HOW);
    }

    public Sequence StartUp()
    {
        Sequence seq_startUp = DOTween.Sequence();

        //スクリーン出現
        seq_startUp.Append(screenRect.DOAnchorPosY(0f, screenMoveTime).SetEase(screenMoveType));
        seq_startUp.AppendInterval(0.2f);

        //信号ボタン出現
        seq_startUp.Append(ShowBandButton(true));
        //seq_startUp.Append(exitButton.DOAnchorPosX(280, buttonMoveTime).SetEase(buttonMoveType));
        //seq_startUp.Join(settingButton.DOAnchorPosX(680, buttonMoveTime).SetEase(buttonMoveType));
        //seq_startUp.Join(creditButton.DOAnchorPosX(1080, buttonMoveTime).SetEase(buttonMoveType));

        //発進ボタン出現
        seq_startUp.Append(launchButton.DOAnchorPosX(-INIPOSX_LAUNCH, buttonMoveTime).SetEase(buttonMoveType));

        return seq_startUp;
    }

    public Sequence HideBandButton(bool isBandup)
    {
        Sequence seq_hide = DOTween.Sequence();

        //信号ボタン待機
        seq_hide.Append(exitButton.DOAnchorPosX(INIPOSX_TRAFFIC, buttonMoveTime).SetEase(buttonMoveType));
        seq_hide.Join(settingButton.DOAnchorPosX(INIPOSX_TRAFFIC, buttonMoveTime).SetEase(buttonMoveType));
        seq_hide.Join(creditButton.DOAnchorPosX(INIPOSX_TRAFFIC, buttonMoveTime).SetEase(buttonMoveType));
        seq_hide.Join(howtoButton.DOAnchorPosY(INIPOSY_HOW, buttonMoveTime).SetEase(buttonMoveType).SetDelay(0.175f));

        //発進ボタン待機
        seq_hide.Join(launchButton.DOAnchorPosX(INIPOSX_LAUNCH, buttonMoveTime).SetEase(buttonMoveType));

        if (isBandup)
        {
            seq_hide.Append(screenRect.DOSizeDelta(new Vector2(2160f, 900f), 0.2f).SetEase(Ease.OutSine));
        }

        return seq_hide;
    }

    public Sequence ShowBandButton(bool isBandup)
    {
        Sequence seq_show = DOTween.Sequence();

        if (isBandup)
        {
            seq_show.Append(screenRect.DOSizeDelta(new Vector2(2160f, 250f), 0.2f).SetEase(Ease.OutSine));
            //seq_show.Append(closeButton.DOAnchorPosX(INIPOSX_TRAFFIC, buttonMoveTime).SetEase(buttonMoveType));
        }

        //信号ボタン待機
        seq_show.Append(exitButton.DOAnchorPosX(280, buttonMoveTime).SetEase(buttonMoveType));
        seq_show.Join(settingButton.DOAnchorPosX(680, buttonMoveTime).SetEase(buttonMoveType));
        seq_show.Join(creditButton.DOAnchorPosX(1080, buttonMoveTime).SetEase(buttonMoveType));
        seq_show.Join(howtoButton.DOAnchorPosY(-INIPOSY_HOW, buttonMoveTime).SetEase(buttonMoveType).SetDelay(0.175f));

        //発進ボタン待機
        seq_show.Join(launchButton.DOAnchorPosX(-INIPOSX_LAUNCH, buttonMoveTime).SetEase(buttonMoveType));

        return seq_show;
    }



}
