using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SoundSetting : MonoBehaviour
{
    DataManager dataMg;

    [Header("ボタン")]
    public Button bgmButton;
    public Button seButton;

    [Header("ランプ")]
    public Image bgmLamp;
    public Image seLamp;

    [Header("リール")]
    public RectTransform bgmReel;
    public RectTransform seReel;
    [Range(0.1f, 0.35f)] public float reelTime = 0.175f;
    const int REEL_DISTANCE = 100;

    public bool isBgm;
    public bool isSe;

    private void Start()
    {
        dataMg = GetComponentInParent<DataManager>();
        bgmButton.onClick.AddListener(() => PushButtonBGM());
        seButton.onClick.AddListener(() => PushButtonSE());

        Initialize();
    }

    void Initialize()
    {
        this.isBgm = dataMg.data.isBGM;
        this.isSe = dataMg.data.isSE;

        SwitchBGM(this.isBgm);
        SwitchSE(this.isSe);

    }

    void PushButtonBGM()
    {
        if (!isBgm)
        {
            SwitchBGM(true);
        }
        else
        {
            SwitchBGM(false);
        }
    }

    void SwitchBGM(bool isOn)
    {
        if (isOn)
        {
            bgmLamp.enabled = true;
            bgmReel.DOComplete();
            bgmReel.anchoredPosition = new Vector2(0, REEL_DISTANCE);
            bgmReel.DOAnchorPosY(0f, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f);

            this.isBgm = true;
            SoundManager.Instance.bgmAudioSource.mute = false;
            dataMg.data.isBGM = this.isBgm;
            dataMg.Save(dataMg.data);
        }
        else
        {
            bgmLamp.enabled = false;
            bgmReel.DOComplete();
            bgmReel.anchoredPosition = Vector2.zero;
            bgmReel.DOAnchorPosY(REEL_DISTANCE, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f);

            this.isBgm = false;
            SoundManager.Instance.bgmAudioSource.mute = true;
            dataMg.data.isBGM = this.isBgm;
            dataMg.Save(dataMg.data);
        }
    }

    void PushButtonSE()
    {
        if (!isSe)
        {
            SwitchSE(true);
        }
        else
        {
            SwitchSE(false);
        }
    }

    void SwitchSE(bool isOn)
    {
        if (isOn)
        {
            seLamp.enabled = true;
            seReel.DOComplete();
            seReel.anchoredPosition = new Vector2(0, REEL_DISTANCE);
            seReel.DOAnchorPosY(0f, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f);

            this.isSe = true;
            SoundManager.Instance.seAudioSource.mute = false;
            dataMg.data.isSE = this.isSe;
            dataMg.Save(dataMg.data);
        }
        else
        {
            seLamp.enabled = false;
            seReel.DOComplete();
            seReel.anchoredPosition = Vector2.zero;
            seReel.DOAnchorPosY(REEL_DISTANCE, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f);

            this.isSe = false;
            SoundManager.Instance.seAudioSource.mute = true;
            dataMg.data.isSE = this.isSe;
            dataMg.Save(dataMg.data);
        }

    }


}
