using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageFrameManager : MonoBehaviour
{
    [Header("Scope")]
    public RectTransform scopeOut;
    public RectTransform scopeIn;
    Image scopeImg;
    public float initialScale;
    [Range(0.1f, 0.5f)] public float scaleDuration;
    public Ease scaleType;
    [Range(0.1f, 0.5f)] public float fadeDuration;
    public Ease fadeType;
    [Range(1f, 10f)] public float loopDuration;
    Tween scopeLoop;

    //[Range(0.1f, 0.5f)] public float focusDuration;
    //public Ease focusType;
    //public float focusAmount; //回転量

    [Header("Lv.Memory")]
    public Image lvImg;
    public Image[] lvLamp;
    [Range(0.1f, 0.5f)] public float openDuration;
    public Ease openType;
    [Range(0.1f, 0.5f)] public float lampDuration;
    public Ease lampType;
    const float INITIALAMOUNT = 0.008f;
    const float PERAMOUNT = 0.0263f;
    const float INITIALANGLE = 4f;
    const float PERANGLE = 9.484f;

    [Header("Tips")]
    public Color[] rankColor;
    TipsManager tipsMg;

    /*
    public Image tipsEdge;
    public CanvasGroup backPanel;
    public Text tipsText;
    public Image rankLamp;

    public int characterLimit = 12;
    public float perTextWide;
    [Range(0.1f, 0.5f)] public float scrollSpeed;
    [Range(0.1f, 0.5f)] public float tipsFadeDuration;
    public Ease tipsFadeType;
    [Range(0.1f, 0.5f)] public float tipsOpenDuration;
    public Ease tipsOpenType;
     */



    public RectTransform RectTransform => this.transform as RectTransform;
    List<Tween> tweenList = new List<Tween>();

    private void OnDestroy()
    {
        tweenList.Add(scopeLoop);
        tweenList.KillAllAndClear();
    }

    void Awake()
    {
        GetComponent();
        scopeLoop = scopeIn.DOLocalRotate(new Vector3(0, 0, 360f), loopDuration, RotateMode.LocalAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear).SetRelative(true);
    }

    void GetComponent()
    {
        tipsMg = GetComponentInChildren<TipsManager>();
        scopeImg = scopeOut.transform.GetComponent<Image>();
    }

    private void Update()
    {
        scopeOut.transform.rotation = scopeIn.transform.rotation;
    }

    public Sequence TargetScope()
    {
        scopeImg.color = new Color(1, 1, 1, 0);
        scopeOut.transform.localScale = new Vector3(initialScale, initialScale, 1f);
        //scopeOut.transform.localEulerAngles = new Vector3(0f, 0f, -1 * (Mathf.Abs(scopeIn.localEulerAngles.z) + 65f));

        Sequence seq_scope = DOTween.Sequence();
        seq_scope.Append(scopeOut.transform.DOScale(Vector3.one, scaleDuration).SetEase(scaleType));
        seq_scope.Join(scopeImg.DOFade(1f, fadeDuration).SetEase(fadeType));
        //seq_scope.Append(scopeOut.transform.DOLocalRotate(new Vector3(0f, 0f, focusAmount), focusDuration, RotateMode.LocalAxisAdd).SetEase(focusType).SetRelative(true));
        tweenList.Add(seq_scope);
        return seq_scope;
    }


    public Sequence TargetLevel(int levelNum)
    {
        DisplayLevelMemory(0);
        lvImg.fillAmount = 0f;
        lvImg.transform.rotation = Quaternion.Euler(0f, 0f, 90f);

        Sequence seq_lv = DOTween.Sequence();//TimeScaleを無視している
        seq_lv.Append(lvImg.DOFillAmount(INITIALAMOUNT + (PERAMOUNT * levelNum), openDuration).SetEase(openType));
        seq_lv.Join(lvImg.transform.DOLocalRotate(new Vector3(0.0f, 0.0f, 90f - (INITIALANGLE + (PERANGLE * levelNum))), openDuration, RotateMode.FastBeyond360).SetEase(openType));
        seq_lv.AppendInterval(0.15f);
        seq_lv.Append(DOTween.To(() => 0, x => DisplayLevelMemory(x), levelNum, lampDuration).SetEase(lampType));
        tweenList.Add(seq_lv);
        return seq_lv;
    }

    public void DisplayLevelMemory(int levelNum)
    {
        for (int i = 0; i < lvLamp.Length; i++)
        {
            lvLamp[i].enabled = (levelNum > i);
        };
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
        sq_tips.Append(tipsEdge.rectTransform.DOSizeDelta(new Vector2(660f, 140.8f), tipsOpenDuration).SetEase(tipsOpenType));
        sq_tips.Append(backPanel.DOFade(1f, tipsFadeDuration).SetEase(tipsFadeType));

        tweenList.Add(sq_tips);
        return sq_tips;
    }

    public void ScrollText()
    {
        tipsText.rectTransform.anchoredPosition = new Vector2(120, 0f);
        Sequence sq_scroll = DOTween.Sequence();
        sq_scroll.Append(tipsText.DOFade(1f, 0.5f));
        sq_scroll.Join(tipsText.rectTransform.DOAnchorPosX(90, 0.5f).SetEase(Ease.Linear));
        if (tipsText.text.Length <= characterLimit) return;

        sq_scroll.AppendInterval(2.5f);
        //・テキスト長さの7.5割を進む、6割からFadeOut ・sequence.Append().SetLoop(-1) ポイントはSetLoops()をsequenceにかけること
        sq_scroll.Append(tipsText.rectTransform.DOAnchorPosX(-1 * (tipsText.text.Length * perTextWide) * 0.75f, (tipsText.text.Length * scrollSpeed) * 0.75f).SetRelative(true).SetEase(Ease.Linear));
        sq_scroll.Join(tipsText.DOFade(0f, 0.75f).SetDelay((tipsText.text.Length * scrollSpeed) * 0.6f).SetEase(Ease.Linear));//テキスト長さの半分を超えたあたりから起動
        sq_scroll.Append(tipsText.rectTransform.DOAnchorPosX(120, 0)).SetLoops(-1);
        sq_scroll.AppendInterval(0.25f);
        tweenList.Add(sq_scroll);
    }
     */

    public void ChangeTarget(int levelNum, string tips)
    {
        tweenList.KillAllAndClear();

        float value = levelNum / 8f;

        tipsMg.rankLamp.color = rankColor[(int)Mathf.Ceil(value) - 1];
        tipsMg.tipsText.text = tips;

        Sequence sequence = DOTween.Sequence();
        //  sequence.Append(TargetScope());
        sequence.AppendCallback(() =>
        {
            TargetScope();
            tipsMg.ShowTips();
            TargetLevel(levelNum);
        });

        tweenList.Add(sequence);
        // sequence.Append(TargetLevel(levelNum));
        // sequence.Join(OpenTips());

    }

}
