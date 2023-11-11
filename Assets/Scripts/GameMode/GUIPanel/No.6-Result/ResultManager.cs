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
        [InspectorName("クリアおめでとう！")] clear = 0,
        [InspectorName("落下してしまった！")] missFall = 1,
        [InspectorName("爆発してしまった！")] missBomb = 2,
        [InspectorName("バッテリー切れ！？")] missLack = 3
    }

    [Header("Panel")]
    public Image panel;
    public Image icon;
    public Text causeText;
    public GameObject jetPanel;
    public GameObject pausePanel;

    [Header("Button:AddListener")]
    public RectTransform button;
    public Image[] emissionImg;
    public Button push_Retry;
    public Button push_World;

    [Header("miss")]
    public Color clearColor, missColor;
    public Text result;
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
    [Range(0.1f, 0.5f)] public float lampDuration;
    public Ease lampType;

    public float perTextWide;
    public int characterLimit = 10;
    [Range(0.1f, 0.5f)] public float scrollSpeed;
    [Range(0.1f, 0.5f)] public float tipsFadeDuration;
    public Ease tipsFadeType;
    [Range(0.1f, 0.5f)] public float tipsOpenDuration;
    public Ease tipsOpenType;

    const string clear = "ミッション成功";
    const string miss = "ミッション失敗";
    StageCtrl stageCrl;
    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.KillAllAndClear();
    }

    private void Start()
    {
        panel.gameObject.SetActive(false);
        stageCrl = GetComponentInParent<StageCtrl>();
        tipsText.text = stageCrl.tipsText;
        rankLamp.color = stageCrl.rankColor[stageCrl.stageRank - 1];
        AddListener();
    }
    private void AddListener()
    {
        push_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        push_World.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
    }

    public void Result(CAUSE cause)
    {
        switch (cause)
        {
            case CAUSE.clear:
                //panel.color = clearColor;
                //result.text = clear;
                panel.gameObject.SetActive(true);
                pausePanel.SetActive(false);
                jetPanel.SetActive(false);

                icon.enabled = false;
                causeText.text = "クリアおめでとう！";
                OpenClearPanel();
                break;

            case CAUSE.missFall:
                panel.color = missColor;
                result.text = miss;
                causeText.text = "落下してしまった！";
                OpenMissPanel();
                break;

            case CAUSE.missBomb:
                panel.color = missColor;
                result.text = miss;
                causeText.text = "爆発してしまった！";
                OpenMissPanel();
                break;

            case CAUSE.missLack:
                panel.color = missColor;
                result.text = miss;
                causeText.text = "バッテリー切れ！？";
                OpenMissPanel();
                break;
        }

    }

    public void OpenClearPanel()
    {
        //result.rectTransform.anchoredPosition = new Vector3(0f, 100f, 0f);

        Sequence seq_clear = DOTween.Sequence();
        //seq_clear.Append(panel.DOFade(0.65f, fadeDuration).SetEase(fadeType));
        //seq_clear.Append(result.rectTransform.DOAnchorPosY(-225f, fallDuration).SetEase(fallType));
        seq_clear.Append(button.DOAnchorPosX(-160f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));
        //seq_clear.AppendInterval(0.15f);
        //seq_clear.Append(result.transform.DOLocalRotate(new Vector3(0f, 0f, -7.5f), tumbleDuration).SetEase(tumbleType));

        tweenList.Add(seq_clear);
    }

    public void OpenMissPanel()
    {
        panel.gameObject.SetActive(true);
        result.rectTransform.anchoredPosition = new Vector3(0f, 100f, 0f);
        tipsText.color = new Color(1, 1, 1, 0);

        Sequence seq_miss = DOTween.Sequence();
        seq_miss.Append(panel.DOFade(0.65f, fadeDuration).SetEase(fadeType));
        seq_miss.Append(result.rectTransform.DOAnchorPosY(-225f, fallDuration).SetEase(fallType));
        seq_miss.Append(button.DOAnchorPosX(-160f, 0.15f).OnComplete(() => SwichBloom(true, lampDuration)));
        seq_miss.AppendInterval(0.15f);
        seq_miss.Append(result.transform.DOLocalRotate(new Vector3(0f, 0f, -7.5f), tumbleDuration).SetEase(tumbleType));
        seq_miss.Join(OpenTips());
        seq_miss.AppendCallback(() => ScrollText());

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
