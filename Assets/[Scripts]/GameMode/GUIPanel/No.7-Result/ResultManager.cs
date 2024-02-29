using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    public CAUSE cause;
    public enum CAUSE
    {
        [InspectorName("ミッション成功！")] clear = 0,
        [InspectorName("落下してしまった！")] missFall = 1,
        [InspectorName("爆発してしまった！")] missBomb = 2,
        [InspectorName("バッテリー切れ！？")] missLack = 3
    }

    [Header("ボタンのパターン")]
    public PATTERN pattern;
    public enum PATTERN
    {
        normal = 0,
        onlyMap = 1,
        onlyNext = 2
    }



    [Header("Panel")]
    public Image panel;
    public Text result;
    public Text causeText;
    public Image[] icon;
    public RectTransform pauseButton;

    const string clear = "ミッション成功";
    const string miss = "ミッション失敗";

    [Header("Button")]
    public RectTransform button;
    public Button push_Retry;
    public Button push_World;
    public Button push_Next;

    public Image[] emissionImg;

    [Header("Clear")]
    public Color clearColor;
    [Range(0.1f, 1f)] public float appDuration;
    public Ease appType;
    [Range(0.1f, 1f)] public float slideDuration;
    public Ease slideType;

    [Header("Miss")]
    public Color missColor;
    [Range(0.1f, 0.5f)] public float fadeDuration;
    public Ease fadeType;
    [Range(0.1f, 0.5f)] public float fallDuration;
    public Ease fallType;
    [Range(0.1f, 1.0f)] public float tumbleDuration;
    public Ease tumbleType;

    [Header("Tips")]
    public Image tipsEdge;
    public CanvasGroup backPanel;
    public Text tipsText;
    public Image rankLamp;
    public float perTextWide;
    public int characterLimit = 10;
    [Range(0.1f, 0.5f)] public float scrollSpeed;

    [Range(0.1f, 0.5f)] public float tipsOpenDuration;
    public Ease tipsOpenType;
    [Range(0.1f, 0.5f)] public float tipsFadeDuration;
    public Ease tipsFadeType;
    [Range(0.1f, 0.5f)] public float lampDuration;
    public Ease lampType;

    StageCtrl stageCrl;
    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        panel.gameObject.SetActive(false);
        stageCrl = GetComponentInParent<StageCtrl>();
        AddListener();
        causeText.enabled = false;
        tipsText.text = stageCrl.tipsText;
        rankLamp.color = stageCrl.rankColor[stageCrl.stageRank - 1];
        for (int i = 0; i < icon.Length; i++)
        {
            icon[i].enabled = false;
        }
    }

    private void AddListener()
    {
        push_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        push_World.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
        push_Next.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("Area[" + stageCrl.areaNum + "]" + stageCrl.stageType + "[" + (stageCrl.stageNum + 1) + "]"));
    }

    public void Result(CAUSE cause)
    {
        switch (cause)
        {

            case CAUSE.clear:
                ButtonPattern(pattern);
                OpenClearPanel();
                break;

            case CAUSE.missFall:
                causeText.text = "落下してしまった！";
                for (int i = 0; i < icon.Length; i++) icon[i].enabled = false;
                icon[0].enabled = true;
                ButtonPattern(PATTERN.normal);
                OpenMissPanel();
                break;

            case CAUSE.missBomb:
                causeText.text = "爆発してしまった！";
                for (int i = 0; i < icon.Length; i++) icon[i].enabled = false;
                icon[1].enabled = true;
                ButtonPattern(PATTERN.normal);
                OpenMissPanel();
                break;

            case CAUSE.missLack:
                causeText.text = "バッテリー切れ！？";
                for (int i = 0; i < icon.Length; i++) icon[i].enabled = false;
                icon[2].enabled = true;
                ButtonPattern(PATTERN.normal);
                OpenMissPanel();
                break;

        }
    }

    public void ButtonPattern(PATTERN setPattern)
    {
        switch (setPattern)
        {
            case PATTERN.normal:
                push_Next.transform.parent.gameObject.SetActive(false);
                break;

            case PATTERN.onlyMap:
                push_World.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -60f);
                push_Retry.transform.parent.gameObject.SetActive(false);
                push_Next.transform.parent.gameObject.SetActive(false);
                break;

            case PATTERN.onlyNext:
                push_Next.transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -60f);
                push_Retry.transform.parent.gameObject.SetActive(false);
                push_World.transform.parent.gameObject.SetActive(false);
                break;
        }
    }

    public void OpenClearPanel()
    {
        panel.gameObject.SetActive(true);

        result.text = clear;
        result.color = new Color(1, 1, 1, 0);
        result.rectTransform.anchoredPosition = new Vector3(-300f, -700f, 0f);

        Sequence seq_clear = DOTween.Sequence();
        seq_clear.Append(result.rectTransform.DOAnchorPosX(0f, slideDuration).SetEase(slideType, 3));
        seq_clear.Join(result.DOFade(1f, appDuration).SetEase(appType));
        seq_clear.Join(pauseButton.DOAnchorPosY(160f, 0.35f));
        seq_clear.AppendCallback(() => stageCrl.JudgeStageData());
        seq_clear.AppendInterval(0.75f);
        seq_clear.Append(button.DOAnchorPosX(-160f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));

        tweenList.Add(seq_clear);
    }

    public void OpenMissPanel()
    {
        panel.gameObject.SetActive(true);
        tipsText.color = new Color(1, 1, 1, 0);

        result.text = miss;
        result.color = missColor;
        result.rectTransform.anchoredPosition = new Vector3(0f, 100f, 0f);

        causeText.enabled = true;

        Sequence seq_miss = DOTween.Sequence();
        seq_miss.Append(panel.DOFade(0.65f, fadeDuration).SetEase(fadeType));
        seq_miss.Append(result.rectTransform.DOAnchorPosY(-225f, fallDuration).SetEase(fallType));
        seq_miss.Append(button.DOAnchorPosX(-160f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));
        seq_miss.AppendInterval(0.15f);

        seq_miss.Append(result.transform.DOLocalRotate(new Vector3(0f, 0f, -7.5f), tumbleDuration).SetEase(tumbleType));
        seq_miss.Join(OpenTips());
        seq_miss.AppendCallback(() =>
        {
            ScrollText();
        });

        tweenList.Add(seq_miss);
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


}
