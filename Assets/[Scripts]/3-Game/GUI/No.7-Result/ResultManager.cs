using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [Header("CLEAR")]
    public RectTransform bandClear;
    public Text textClear;
    public Text textSecret;

    [Header("parameter :OpenClearPanel()")]
    [Range(0.15f, 0.5f)] public float bandTime = 0.35f;
    public Ease bandType = Ease.OutSine;
    [Range(0.1f, 1f)] public float textClearTime;
    public Ease textClearType;

    public PATTERN clearPattern;
    public enum PATTERN
    {
        VER_Normal = 0,
        VER_Miss = 1,
        VER_Guide = 2
    }
    [Space(10)]



    [Header("MISS")]
    public CAUSE cause;
    public CanvasGroup missPanel;
    public Text missText;
    public GameObject[] missIcon;

    [Header("parameter :OpenMissPanel()")]
    [Range(0.1f, 0.5f)] public float fadeDuration;
    public Ease fadeType;
    [Range(0.1f, 0.5f)] public float fallDuration;
    public Ease fallType;
    [Range(0.1f, 1.0f)] public float tumbleDuration;
    public Ease tumbleType;
    public enum CAUSE
    {
        [InspectorName("落下してしまった!")] 落下してしまった = 0,
        [InspectorName("爆発してしまった!")] 爆発してしまった = 1,
        [InspectorName("バッテリー切れ!?")] バッテリー切れ = 2
    }
    [Space(10)]


    [Header("Button : Common")]
    public RectTransform button_Right;
    public Button r_Map;
    public Button r_Next;
    public Button r_Retry;
    public Text r_Text;
    [Space(10)]

    public RectTransform button_Left;
    public Button l_Map;
    public Button l_Retry;
    RectTransform button_Retry;
    [Space(10)]

    [Range(0.1f, 0.5f)] public float buttonTime;
    public Ease buttonType;
    [Range(0.1f, 0.5f)] public float lampDuration;
    public Ease lampType;
    [Space(10)]

    public Image[] bloomImg;
    public Color[] bloomColor;


    StageCtrl stageCrl;
    TipsManager tipsMg;
    List<Tween> tweenList = new List<Tween>();


    private void OnDestroy() => tweenList.KillAllAndClear();

    private void Start()
    {
        stageCrl = GetComponentInParent<StageCtrl>();
        tipsMg = GetComponentInChildren<TipsManager>();
        button_Retry = l_Retry.transform.parent.GetComponent<RectTransform>();

        AddListener();
        tipsMg.SetRankColor(tipsMg.rankColor[stageCrl.stageRank - 1]);
    }

    private void AddListener()
    {
        r_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        r_Map.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
        r_Next.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("Area[" + stageCrl.areaNum + "]" + stageCrl.stageType + "[" + (stageCrl.stageNum + 1) + "]"));

        l_Retry.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain(SceneManager.GetActiveScene().name));
        l_Map.onClick.AddListener(() => stageCrl.curtainMg.CloseCurtain("StageSelect"));
    }

    public void OpenClearPanel()
    {
        ButtonPattern(clearPattern);

        Sequence s_clear = DOTween.Sequence();
        s_clear.Append(bandClear.DOSizeDelta(new Vector2(2160f, 300), bandTime).SetEase(bandType));

        s_clear.AppendInterval(0.5f);
        s_clear.Append(textClear.rectTransform.DOAnchorPosY(0f, textClearTime).SetEase(textClearType));
        s_clear.Join(textClear.DOFade(1f, textClearTime).SetEase(textClearType));
        s_clear.Join(textClear.transform.DOScale(1.5f, textClearTime).SetEase(textClearType));

        s_clear.AppendCallback(() => stageCrl.JudgeStageData());
        s_clear.AppendInterval(1.5f);
        s_clear.Append(button_Right.DOAnchorPosX(0f, buttonTime).SetEase(buttonType));
        s_clear.Join(button_Retry.DOAnchorPosX(355, buttonTime).SetEase(buttonType).OnComplete(() => SwichBloom(true, lampDuration)));

        tweenList.Add(s_clear);
    }


    public void OpenMissPanel(CAUSE setCause)
    {
        ButtonPattern(PATTERN.VER_Miss);

        cause = setCause;
        missText.text = Enum.GetName(typeof(CAUSE), cause) + "!?";
        for (int i = 0; i < missIcon.Length; i++) missIcon[i].SetActive(false);
        missIcon[(int)cause].SetActive(true);

        Sequence seq_miss = DOTween.Sequence();
        seq_miss.Append(missPanel.DOFade(1f, fadeDuration).SetEase(fadeType));
        seq_miss.Append(missText.rectTransform.DOAnchorPosY(-225f, fallDuration).SetEase(fallType));
        seq_miss.Append(button_Right.DOAnchorPosX(0f, buttonTime).SetEase(buttonType));
        seq_miss.Join(button_Left.DOAnchorPosX(0f, buttonTime).SetEase(buttonType).OnComplete(() => SwichBloom(true, lampDuration)));
        seq_miss.AppendInterval(0.15f);
        seq_miss.Append(missText.transform.DOLocalRotate(new Vector3(0f, 0f, -7.5f), tumbleDuration).SetEase(tumbleType));
        seq_miss.AppendCallback(() =>
        {
            tipsMg.ShowTips();
        });

        tweenList.Add(seq_miss);
    }


    public void ButtonPattern(PATTERN setPattern)
    {
        switch (setPattern)
        {
            //通常ステージクリア時.
            case PATTERN.VER_Normal:
                bloomImg[0].color = bloomColor[0];
                r_Text.text = "マップへ";
                r_Map.transform.gameObject.SetActive(true);
                r_Retry.transform.gameObject.SetActive(false);
                r_Next.transform.gameObject.SetActive(false);

                button_Left.gameObject.SetActive(false);
                break;

            //ガイドステージクリア時.
            case PATTERN.VER_Guide:
                bloomImg[0].color = bloomColor[1];
                r_Text.text = "次へ";
                r_Next.transform.gameObject.SetActive(true);
                r_Retry.transform.gameObject.SetActive(false);
                r_Map.transform.gameObject.SetActive(false);

                button_Left.gameObject.SetActive(false);
                break;

            //ステージ失敗時.
            case PATTERN.VER_Miss:
                bloomImg[0].color = bloomColor[2];
                r_Text.text = "リトライ";
                r_Retry.transform.gameObject.SetActive(true);
                r_Map.transform.gameObject.SetActive(false);
                r_Next.transform.gameObject.SetActive(false);

                button_Left.gameObject.SetActive(true);
                break;
        }
    }


    public void SwichBloom(bool isEnabled, float fadeTime) //FadeInする場合、PanelAnimeで光源が見えるのを防ぐ目的
    {
        foreach (Image img in bloomImg)
        {
            img.enabled = isEnabled;
            img.DOKill(true);
            img.DOFade(Convert.ToInt32(isEnabled), fadeTime).SetEase(lampType);
        }
    }




    /*
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
     */


}
