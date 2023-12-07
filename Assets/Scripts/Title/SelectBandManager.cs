using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectBandManager : MonoBehaviour
{
    [Header("Screen")]
    public RectTransform screenRect;
    [Range(0.15f, 0.5f)] public float screenMoveTime;
    public Ease screenMoveType;
    const int INIPOSY = -255;

    [Header("LaunchButton")]
    public RectTransform launchButton;
    const int INIPOSX_LAUNCH = 375;

    [Header("TrafficButton")]
    public RectTransform exitButton;
    public RectTransform settingButton;
    public RectTransform creditButton;
    [Range(0.15f, 0.5f)] public float trafficMoveTime;
    public Ease trafficMoveType;

    const int INIPOSX_TRAFFIC = -280;
    const int INIPOSY_TRAFFIC = 250;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        screenRect.anchoredPosition = new Vector2(0f, INIPOSY);
        launchButton.anchoredPosition = new Vector2(INIPOSX_LAUNCH, INIPOSY_TRAFFIC);
        exitButton.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        settingButton.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        creditButton.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
    }

    public Sequence StartUp()
    {
        Sequence seq_startUp = DOTween.Sequence();

        //スクリーン出現
        seq_startUp.Append(screenRect.DOAnchorPosY(0f, screenMoveTime).SetEase(screenMoveType));
        seq_startUp.AppendInterval(0.2f);

        //信号ボタン出現
        seq_startUp.Append(exitButton.DOAnchorPosX(280, trafficMoveTime).SetEase(trafficMoveType));
        seq_startUp.Join(settingButton.DOAnchorPosX(680, trafficMoveTime).SetEase(trafficMoveType));
        seq_startUp.Join(creditButton.DOAnchorPosX(1080, trafficMoveTime).SetEase(trafficMoveType));

        //発進ボタン出現
        seq_startUp.Append(launchButton.DOAnchorPosX(-INIPOSX_LAUNCH, trafficMoveTime).SetEase(trafficMoveType));

        return seq_startUp;
    }

}
