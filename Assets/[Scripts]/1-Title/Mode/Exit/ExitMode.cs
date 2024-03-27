using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExitMode : MonoBehaviour
{
    //�I���m�F��ʂ̐��������X�N���v�g.

    [Header("Button")]
    public RectTransform yesRect;
    public RectTransform noRect;
    Button yesButton;
    Button noButton;
    [Space(10)]

    public float buttonShowValue = 512;
    public float buttonHideValue = 150;
    [Range(0.1f, 0.5f)] public float buttonTime;
    public Ease buttonType;

    [Header("Text")]
    public Text exitText;
    [Space(10)]

    public float textShowValue = 120;
    public float textHideValue = 0;
    [Range(0.1f, 0.5f)] public float textTime;
    public Ease textType;

    ModeManager modeMg;


    private void Awake()
    {
        modeMg = transform.parent.GetComponent<ModeManager>();

        yesButton = yesRect.transform.GetChild(1).GetComponent<Button>();
        noButton = noRect.transform.GetChild(1).GetComponent<Button>();

        yesButton.onClick.AddListener(EndGame);
        noButton.onClick.AddListener(ShutDownExitMode);
    }

    public void StartUpExitMode()
    {
        Sequence s_startupExit = DOTween.Sequence();

        //�Z���N�g�{�^������.
        s_startupExit.Append(modeMg.selectMenuMg.HideSelectButton(SelectMenuManager.BAND.normal, SelectMenuManager.COLOR.exit));

        //�I���m�F�{�^���o��.
        s_startupExit.Append(yesRect.DOAnchorPosX(buttonShowValue, buttonTime).SetEase(buttonType));
        s_startupExit.Join(noRect.DOAnchorPosX(-buttonShowValue, buttonTime).SetEase(buttonType));

        //�m�F�e�L�X�g�o��.
        s_startupExit.Join(exitText.rectTransform.DOAnchorPosY(textShowValue, textTime).SetEase(textType));
        s_startupExit.Join(exitText.DOFade(1f, textTime).SetEase(textType));
    }

    public void ShutDownExitMode()
    {
        Sequence s_shutdownExit = DOTween.Sequence();

        //�{�^���ҋ@
        s_shutdownExit.Append(yesRect.DOAnchorPosX(-buttonHideValue, buttonTime).SetEase(buttonType));
        s_shutdownExit.Join(noRect.DOAnchorPosX(buttonHideValue, buttonTime).SetEase(buttonType));

        //�m�F�e�L�X�g����.
        s_shutdownExit.Join(exitText.rectTransform.DOAnchorPosY(0f, textTime).SetEase(textType));
        s_shutdownExit.Join(exitText.DOFade(0f, textTime).SetEase(textType));

        s_shutdownExit.AppendCallback(() => modeMg.selectMenuMg.ShowSelectButton());
    }


    public void EndGame()
    {
        yesButton.enabled = false; //��x�����h�~.
        noButton.enabled = false; //��x�����h�~.

        Sequence s_endgame = DOTween.Sequence();

        //�e�L�X�g����.
        s_endgame.Append(exitText.rectTransform.DOAnchorPosY(0f, textTime).SetEase(textType));
        s_endgame.Join(exitText.DOFade(0f, textTime).SetEase(textType));

        //�e�L�X�g��������.
        s_endgame.AppendCallback(() => exitText.text = "�����l�ł���!");

        //�e�L�X�g�o��.
        s_endgame.Append(exitText.rectTransform.DOAnchorPosY(120f, textTime).SetEase(textType));
        s_endgame.Join(exitText.DOFade(1f, textTime).SetEase(textType));

        //FadeOut.
        s_endgame.AppendInterval(1.0f);
        s_endgame.Append(modeMg.curtainMg.EndGameCurtain());
    }


}
