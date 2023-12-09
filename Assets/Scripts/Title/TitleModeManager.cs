using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleModeManager : MonoBehaviour
{
    [Header("Exit")]
    public RectTransform yesRect;
    public RectTransform noRect;
    Button yesButton;
    Button noButton;
    public Text exitText;

    [Range(0.1f, 0.5f)] public float MoveTime;
    public Ease MoveType;
    const int INIPOSX_EXIT = 512;

    [Header("Setting")]
    public RectTransform closeSetRect;
    Button closeSetButton;
    public CanvasGroup settingCanvas;

    [Header("Credit")]
    public RectTransform closeCreditRect;
    Button closeCreditButton;
    public CanvasGroup creditCanvas;

    const int INIPOSX_CLOSE = -280;


    TitleManager titleMg;
    List<Tween> tweenList = new List<Tween>();

    private void Awake()
    {
        titleMg = transform.root.GetComponent<TitleManager>();
        yesButton = yesRect.transform.GetChild(1).GetComponent<Button>();
        noButton = noRect.transform.GetChild(1).GetComponent<Button>();
        closeSetButton = closeSetRect.transform.GetChild(1).GetComponent<Button>();
        closeCreditButton = closeCreditRect.transform.GetChild(1).GetComponent<Button>();
    }


    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        yesRect.anchoredPosition = new Vector2(-INIPOSX_EXIT, 250);
        noRect.anchoredPosition = new Vector2(INIPOSX_EXIT, 250);
        closeSetRect.anchoredPosition = new Vector2(INIPOSX_CLOSE, 900);
        closeCreditRect.anchoredPosition = new Vector2(INIPOSX_CLOSE, 900);

        yesButton.onClick.AddListener(EndGame);
        noButton.onClick.AddListener(CloseExit);
        closeSetButton.onClick.AddListener(CloseSetting);
        closeCreditButton.onClick.AddListener(CloseCredit);

        settingCanvas.alpha = 0f;
        creditCanvas.alpha = 0f;

        exitText.color = new Color(0.894f, 0.894f, 0.894f, 0f);
        exitText.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OpenExit()
    {
        Sequence seq_openExit = DOTween.Sequence();
        //�o���h�{�^���ҋ@
        seq_openExit.Append(titleMg.selectBandMg.HideBandButton(false));

        //�{�^���o��
        seq_openExit.Append(yesRect.DOAnchorPosX(INIPOSX_EXIT, MoveTime).SetEase(MoveType));
        seq_openExit.Join(noRect.DOAnchorPosX(-INIPOSX_EXIT, MoveTime).SetEase(MoveType));

        //�e�L�X�g�o��
        seq_openExit.Join(exitText.rectTransform.DOAnchorPosY(120f, MoveTime).SetEase(MoveType));
        seq_openExit.Join(exitText.DOFade(1f, MoveTime).SetEase(MoveType));
    }

    public void CloseExit()
    {
        Sequence seq_closeExit = DOTween.Sequence();

        //�{�^���ҋ@
        seq_closeExit.Append(yesRect.DOAnchorPosX(-INIPOSX_EXIT, MoveTime).SetEase(MoveType));
        seq_closeExit.Join(noRect.DOAnchorPosX(INIPOSX_EXIT, MoveTime).SetEase(MoveType));

        //�e�L�X�g����
        seq_closeExit.Join(exitText.rectTransform.DOAnchorPosY(0f, MoveTime).SetEase(MoveType));
        seq_closeExit.Join(exitText.DOFade(0f, MoveTime).SetEase(MoveType));

        seq_closeExit.AppendCallback(() => titleMg.selectBandMg.ShowBandButton(false));
    }

    public void EndGame()
    {
        yesButton.enabled = false;
        noButton.enabled = false;

        Sequence seq_end = DOTween.Sequence();

        //�e�L�X�g����
        seq_end.Append(exitText.rectTransform.DOAnchorPosY(0f, MoveTime).SetEase(MoveType));
        seq_end.Join(exitText.DOFade(0f, MoveTime).SetEase(MoveType));

        //�e�L�X�g��������
        seq_end.AppendCallback(() => exitText.text = "�����l�ł���!");

        //�e�L�X�g�o��
        seq_end.Append(exitText.rectTransform.DOAnchorPosY(120f, MoveTime).SetEase(MoveType));
        seq_end.Join(exitText.DOFade(1f, MoveTime).SetEase(MoveType));

        //FadeOut
        seq_end.AppendInterval(0.5f);
        seq_end.Append(titleMg.curtainMg.EndGameCurtain());
        seq_end.AppendInterval(0.5f);
        seq_end.AppendCallback(() => titleMg.EndGame());
    }



    public void OpenSetting()
    {
        settingCanvas.gameObject.SetActive(true);
        //�T�E���h (BGM & SE)
        //�f�[�^������ (�ق�Ƃɂ����́H => ����BOOM)

        Sequence seq_openSet = DOTween.Sequence();
        //�o���h�{�^���ҋ@
        seq_openSet.Append(titleMg.selectBandMg.HideBandButton(true));
        //�ݒ��ʏo��
        seq_openSet.Append(closeSetRect.DOAnchorPosX(-INIPOSX_CLOSE, MoveTime).SetEase(MoveType));
        seq_openSet.Join(settingCanvas.DOFade(1f, 0.25f).SetEase(MoveType));
    }

    public void CloseSetting()
    {
        Sequence seq_closeSet = DOTween.Sequence();
        //�o���h�{�^���ҋ@
        seq_closeSet.Append(closeSetRect.DOAnchorPosX(INIPOSX_CLOSE, MoveTime).SetEase(MoveType));
        seq_closeSet.Join(settingCanvas.DOFade(0f, 0.25f).SetEase(MoveType));
        seq_closeSet.Append(titleMg.selectBandMg.ShowBandButton(true));
        seq_closeSet.AppendCallback(() => settingCanvas.gameObject.SetActive(false));
    }



    public void OpenCredit()
    {
        creditCanvas.gameObject.SetActive(true);
        //��Җ�
        //�����\�L
        Sequence seq_openCredit = DOTween.Sequence();
        //�o���h�{�^���ҋ@
        seq_openCredit.Append(titleMg.selectBandMg.HideBandButton(true));
        //�ݒ��ʏo��
        seq_openCredit.Append(closeCreditRect.DOAnchorPosX(-INIPOSX_CLOSE, MoveTime).SetEase(MoveType));
        seq_openCredit.Join(creditCanvas.DOFade(1f, 0.25f).SetEase(MoveType));
    }

    public void CloseCredit()
    {
        Sequence seq_closeSet = DOTween.Sequence();
        //�o���h�{�^���ҋ@
        seq_closeSet.Append(closeCreditRect.DOAnchorPosX(INIPOSX_CLOSE, MoveTime).SetEase(MoveType));
        seq_closeSet.Join(creditCanvas.DOFade(0f, 0.25f).SetEase(MoveType));
        seq_closeSet.Append(titleMg.selectBandMg.ShowBandButton(true));
        seq_closeSet.AppendCallback(() => creditCanvas.gameObject.SetActive(false));
    }
}
