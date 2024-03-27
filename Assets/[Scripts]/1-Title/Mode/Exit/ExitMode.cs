using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ExitMode : MonoBehaviour
{
    //終了確認画面の制御をするスクリプト.

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

        //セレクトボタン消失.
        s_startupExit.Append(modeMg.selectMenuMg.HideSelectButton(SelectMenuManager.BAND.normal, SelectMenuManager.COLOR.exit));

        //終了確認ボタン出現.
        s_startupExit.Append(yesRect.DOAnchorPosX(buttonShowValue, buttonTime).SetEase(buttonType));
        s_startupExit.Join(noRect.DOAnchorPosX(-buttonShowValue, buttonTime).SetEase(buttonType));

        //確認テキスト出現.
        s_startupExit.Join(exitText.rectTransform.DOAnchorPosY(textShowValue, textTime).SetEase(textType));
        s_startupExit.Join(exitText.DOFade(1f, textTime).SetEase(textType));
    }

    public void ShutDownExitMode()
    {
        Sequence s_shutdownExit = DOTween.Sequence();

        //ボタン待機
        s_shutdownExit.Append(yesRect.DOAnchorPosX(-buttonHideValue, buttonTime).SetEase(buttonType));
        s_shutdownExit.Join(noRect.DOAnchorPosX(buttonHideValue, buttonTime).SetEase(buttonType));

        //確認テキスト消失.
        s_shutdownExit.Join(exitText.rectTransform.DOAnchorPosY(0f, textTime).SetEase(textType));
        s_shutdownExit.Join(exitText.DOFade(0f, textTime).SetEase(textType));

        s_shutdownExit.AppendCallback(() => modeMg.selectMenuMg.ShowSelectButton());
    }


    public void EndGame()
    {
        yesButton.enabled = false; //二度押し防止.
        noButton.enabled = false; //二度押し防止.

        Sequence s_endgame = DOTween.Sequence();

        //テキスト消失.
        s_endgame.Append(exitText.rectTransform.DOAnchorPosY(0f, textTime).SetEase(textType));
        s_endgame.Join(exitText.DOFade(0f, textTime).SetEase(textType));

        //テキスト書き換え.
        s_endgame.AppendCallback(() => exitText.text = "お疲れ様でした!");

        //テキスト出現.
        s_endgame.Append(exitText.rectTransform.DOAnchorPosY(120f, textTime).SetEase(textType));
        s_endgame.Join(exitText.DOFade(1f, textTime).SetEase(textType));

        //FadeOut.
        s_endgame.AppendInterval(1.0f);
        s_endgame.Append(modeMg.curtainMg.EndGameCurtain());
    }


}
