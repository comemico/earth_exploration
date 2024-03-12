using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class SelectMenuManager : MonoBehaviour
{
    //※セレクトメニューの表示/非表示を制御するスクリプト

    [Header("Band")]
    public RectTransform band;
    [Space(10)]

    [Range(0.15f, 0.5f)] public float bandTime = 0.35f;
    public Ease bandType = Ease.OutSine;
    public enum BAND
    {
        down = 0,
        normal = 260,
        up = 905
    }


    [Header("SelectButton")]
    public RectTransform exit;
    public RectTransform setting;
    public RectTransform credit;
    public RectTransform launch;

    Button exitButton;
    Button settingButton;
    Button creditButton;
    [HideInInspector] public Button launchButton;

    [Space(10)]

    [Range(0.15f, 0.5f)] public float buttonTime = 0.2f;
    public Ease buttonType = Ease.InOutSine;


    const int INIPOSX_TRAFFIC = -150;
    const int INIPOSY_TRAFFIC = 255;
    const int INIPOSX_LAUNCH = 250;


    ModeManager modeMg;

    private void Start()
    {
        modeMg = transform.root.GetComponentInChildren<ModeManager>();
        exitButton = exit.transform.GetChild(1).GetComponent<Button>();
        settingButton = setting.transform.GetChild(1).GetComponent<Button>();
        creditButton = credit.transform.GetChild(1).GetComponent<Button>();
        launchButton = launch.transform.GetChild(1).GetComponent<Button>();
        Initialize();
    }

    void Initialize()
    {
        exit.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        setting.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        credit.anchoredPosition = new Vector2(INIPOSX_TRAFFIC, INIPOSY_TRAFFIC);
        launch.anchoredPosition = new Vector2(INIPOSX_LAUNCH, INIPOSY_TRAFFIC);

        exitButton.onClick.AddListener(() => modeMg.exitMode.StartUpExitMode());
        settingButton.onClick.AddListener(() => modeMg.settingMode.StartUpSettingMode());
        creditButton.onClick.AddListener(() => modeMg.creditMode.StartUpCreditMode());
        launchButton.onClick.AddListener(() => modeMg.launchMode.StartUpLaunchMode());

        launchButton.enabled = false;
    }

    public Sequence ShowSelectButton() //Band.DOSize() => Button.DOMove().
    {
        Sequence s_show = DOTween.Sequence();

        //帯幅 => normal.
        s_show.Append(band.DOSizeDelta(new Vector2(2160f, (int)BAND.normal), bandTime).SetEase(bandType));

        //ボタン出現.
        s_show.Append(exit.DOAnchorPosX(280, buttonTime).SetEase(buttonType));
        s_show.Join(setting.DOAnchorPosX(680, buttonTime).SetEase(buttonType));
        s_show.Join(credit.DOAnchorPosX(1080, buttonTime).SetEase(buttonType));
        s_show.Join(launch.DOAnchorPosX(-450, buttonTime).SetEase(buttonType));

        return s_show;
    }

    public Sequence HideSelectButton(BAND mode) //Button.DOMove() => Band.DOSize().
    {
        Sequence s_hide = DOTween.Sequence();

        //ボタン消失.
        s_hide.Append(exit.DOAnchorPosX(INIPOSX_TRAFFIC, buttonTime).SetEase(buttonType));
        s_hide.Join(setting.DOAnchorPosX(INIPOSX_TRAFFIC, buttonTime).SetEase(buttonType));
        s_hide.Join(credit.DOAnchorPosX(INIPOSX_TRAFFIC, buttonTime).SetEase(buttonType));
        s_hide.Join(launch.DOAnchorPosX(INIPOSX_LAUNCH, buttonTime).SetEase(buttonType));

        //帯幅 => BAND.mode.
        s_hide.Append(band.DOSizeDelta(new Vector2(2160f, (int)mode), bandTime).SetEase(bandType));

        return s_hide;
    }


}
