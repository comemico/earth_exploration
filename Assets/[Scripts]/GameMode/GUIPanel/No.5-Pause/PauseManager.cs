using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    [Header("SetActive")]
    public GameObject panel;
    public GameObject stageInfo;

    [Header("AddListener")]
    public Button push_Pause;
    public Button push_Retry;
    public Button push_World;

    [Header("Tips")]
    public Image tipsEdge;
    public CanvasGroup backPanel;
    public Text tipsText;
    public Image rankLamp;

    public float perTextWide;
    public int characterLimit = 10;
    [Range(0.1f, 0.5f)] public float scrollSpeed;
    [Range(0.1f, 0.5f)] public float tipsFadeDuration;
    public Ease tipsFadeType;
    [Range(0.1f, 0.5f)] public float tipsOpenDuration;
    public Ease tipsOpenType;

    public Image[] emissionImg;
    public float lampDuration;
    public Ease lampType;

    [Header("Button")]
    public RectTransform button;

    public bool isPause = false;

    StageCtrl stageCrl;
    float savedTimeScale;
    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Awake()
    {
        InitializeGetComponent();
        AddListener();
    }

    private void InitializeGetComponent()
    {
        stageCrl = GetComponentInParent<StageCtrl>();
        panel.SetActive(false);
        SwichBloom(false, 0f);
        tipsText.text = stageCrl.tipsText;
    }

    private void AddListener()
    {
        push_Pause.onClick.AddListener(PushPauseButton);
        push_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        push_World.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
    }

    public void PushPauseButton()
    {
        if (!isPause)
        {
            OpenPausePanel();
        }
        else
        {
            ClosePausePanel(false);
        }
    }

    public void OpenPausePanel()
    {
        isPause = true;
        panel.SetActive(true);
        stageInfo.SetActive(true);
        stageCrl.saltoMg.saltoHudMg.gauge.DOPause();
        tipsText.color = new Color(1, 1, 1, 0);
        rankLamp.color = stageCrl.rankColor[stageCrl.stageRank - 1];
        button.anchoredPosition = new Vector2(320, 0);

        tweenList.KillAllAndClear();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(button.DOAnchorPosX(-160f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));
        sequence.Append(OpenTips());
        sequence.AppendCallback(() => ScrollText());

        tweenList.Add(sequence);

        savedTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    public void ClosePausePanel(bool isComplete)
    {
        isPause = false;
        panel.SetActive(false);
        stageInfo.SetActive(false);
        SwichBloom(false, lampDuration);
        stageCrl.saltoMg.saltoHudMg.gauge.DOPlay();

        //スロー状態でポーズ画面に入っても、スロー状態で戻せるようにするため
        if (Mathf.Approximately(Time.timeScale, 1f))//timescale= 1fに近ければ1fに設定
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = savedTimeScale;
        }
    }

    public void SwichBloom(bool isEnabled, float fadeTime) //FadeInする場合、PanelAnimeで光源が見えるのを防ぐ目的
    {
        foreach (Image img in emissionImg)
        {
            img.enabled = isEnabled;
            img.DOKill(true);
            img.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(lampType);
        }
    }
    public Sequence OpenTips()
    {
        backPanel.alpha = 0f;
        tipsEdge.color = new Color(1, 1, 1, 0);
        tipsEdge.rectTransform.sizeDelta = new Vector2(140.8f, 140.8f);

        Sequence sq_tips = DOTween.Sequence();
        sq_tips.AppendInterval(0.1f);
        sq_tips.Append(tipsEdge.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));
        sq_tips.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(875f, 140.8f), tipsOpenDuration).SetEase(tipsOpenType));
        sq_tips.Append(backPanel.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));

        tweenList.Add(sq_tips);
        return sq_tips;
    }

    public void ScrollText()
    {
        tipsText.rectTransform.anchoredPosition = new Vector2(120, 0f);
        Sequence sq_scroll = DOTween.Sequence();
        sq_scroll.Append(tipsText.DOFade(1f, 0.5f));
        sq_scroll.Join(tipsText.rectTransform.DOAnchorPosX(80, 0.5f).SetEase(Ease.Linear));
        if (tipsText.text.Length <= characterLimit) return;

        sq_scroll.AppendInterval(2.5f);
        //・テキスト長さの7.5割を進む、6割からFadeOut ・sequence.Append().SetLoop(-1) ポイントはSetLoops()をsequenceにかけること
        sq_scroll.Append(tipsText.rectTransform.DOAnchorPosX(-1 * tipsText.rectTransform.sizeDelta.x * 0.75f, (tipsText.text.Length * scrollSpeed) * 0.75f).SetRelative(true).SetEase(Ease.Linear));
        sq_scroll.Join(tipsText.DOFade(0f, 0.75f).SetDelay((tipsText.text.Length * scrollSpeed) * 0.6f).SetEase(Ease.Linear));//テキスト長さの半分を超えたあたりから起動
        sq_scroll.Append(tipsText.rectTransform.DOAnchorPosX(120, 0)).SetLoops(-1);
        sq_scroll.AppendInterval(0.25f);
        tweenList.Add(sq_scroll);
    }

    //if (stageCrl.controlStatus == StageCtrl.ControlStatus.unControl) push_Pause.interactable = false;//空中時もポーズを押せなくなるが、変に連打されるより着地後に戻すほうが都合が良いかもしれない
}
