using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleSoundManager : MonoBehaviour
{
    DataManager dataMg;

    [Header("�{�^��")]
    public Button bgmButton;
    public Button seButton;

    [Header("�����v")]
    public Image bgmLamp;
    public Image seLamp;

    [Header("���[��")]
    public RectTransform bgmReel;
    public RectTransform seReel;
    [Range(0.1f, 0.35f)] public float reelTime = 0.15f;
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

            isBgm = true;
            dataMg.data.isBGM = this.isBgm;
            dataMg.Save(dataMg.data);
        }
        else
        {
            bgmLamp.enabled = false;
            bgmReel.DOComplete();
            bgmReel.anchoredPosition = Vector2.zero;
            bgmReel.DOAnchorPosY(REEL_DISTANCE, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f);

            isBgm = false;
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

            isSe = true;
            dataMg.data.isSE = this.isSe;
            dataMg.Save(dataMg.data);
        }
        else
        {
            seLamp.enabled = false;
            seReel.DOComplete();
            seReel.anchoredPosition = Vector2.zero;
            seReel.DOAnchorPosY(REEL_DISTANCE, reelTime).SetEase(Ease.OutElastic, 0f, 0.225f);

            isSe = false;
            dataMg.data.isSE = this.isSe;
            dataMg.Save(dataMg.data);
        }

    }


}
